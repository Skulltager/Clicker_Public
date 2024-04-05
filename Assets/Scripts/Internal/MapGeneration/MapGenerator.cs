
using SheetCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private const int THREADS_BIOME_GENERATION = 8;
    private const int THREADS_BIOME_COPY = 32;

    public static MapGenerator instance { private set; get; }

    [SerializeField] private MapVisualizer mapVisualizer;
    [SerializeField] private int spawnRoomSize;
    [SerializeField] private Point spawnPoint;
    [SerializeField] private RoomIdentifier spawnRoomIdentifier;
    [SerializeField] private int spawnBiomeIndex;
    [SerializeField] private int chunkSize;
    [SerializeField] private int seed;
    [SerializeField] private int xMin;
    [SerializeField] private int xMax;
    [SerializeField] private int yMin;
    [SerializeField] private int yMax;

    [SerializeField] private Material chunkRoomMaterial = default;

    public readonly EventList<MapChunk> mapChunkList;

    public System.Random random { private set; get; }
    public Material ChunkRoomMaterial => chunkRoomMaterial;
    public BiomeRecord[] biomeRecords { private set; get; }
    public int ChunkSize => chunkSize;

    private FourDirectionalList<MapChunk> mapChunks;
    private FourDirectionalList<ComputeBuffer> biomeMapBuffers;
    private FourDirectionalList<ComputeBuffer> biomeDistanceBuffers;

    private ComputeBuffer biomeLayersBuffer;
    private ComputeBuffer biomeStepIndicesBuffer;

    private ComputeBuffer biomeDistancesToCenterBuffer;
    private ComputeBuffer biomeScaleStrengthsBuffer;
    private ComputeBuffer biomeClampStrengthsBuffer;
    private ComputeBuffer biomeElevationStrengthsBuffer;

    private ComputeBuffer biomeSeedsBuffer;
    private ComputeBuffer biomeStepSizesBuffer;
    private ComputeBuffer biomeStepRoughnessBuffer;

    private int copyMapsKernel;
    private int generateBiomeMapKernel;
    private int calculateBiomeDistancesKernel;

    private ComputeShader mapCopier;
    private ComputeShader biomeMapGenerator;
    private ComputeShader biomeDistanceCalculator;

    private ChunkRoom spawnRoom;

    private MapGenerator()
    {
        mapChunkList = new EventList<MapChunk>();
    }

    private void Awake()
    {
        instance = this;
        mapCopier = ComputeShaderIdentifier.MapCopier.GetRecord().Shader;
        biomeMapGenerator = ComputeShaderIdentifier.BiomeGenerator.GetRecord().Shader;
        biomeDistanceCalculator = ComputeShaderIdentifier.BiomeMapDistanceCalculator.GetRecord().Shader;
        generateBiomeMapKernel = biomeMapGenerator.FindKernel("GenerateBiomeMap");
        calculateBiomeDistancesKernel = biomeDistanceCalculator.FindKernel("CalculateBiomeDistances");
        copyMapsKernel = mapCopier.FindKernel("CopyMaps");
        mapChunks = new FourDirectionalList<MapChunk>(null);
        biomeMapBuffers = new FourDirectionalList<ComputeBuffer>(null);
        biomeDistanceBuffers = new FourDirectionalList<ComputeBuffer>(null);

        BiomeIdentifier[] biomeIdentifiers = Enum.GetValues(typeof(BiomeIdentifier)) as BiomeIdentifier[];
        biomeRecords = new BiomeRecord[biomeIdentifiers.Length - 1];
        int index = 0;
        foreach (BiomeIdentifier biomeIdentifier in biomeIdentifiers)
        {
            if (biomeIdentifier == BiomeIdentifier.None)
                continue;

            biomeRecords[index] = biomeIdentifier.GetRecord();
            index++;

        }
        CreateBuffers();
        GenerateChunkRange();
    }

    private void Start()
    {
        mapVisualizer.data = this;
        MiniMapVisualizer.instance.data = this;
    }

    public void Regenerate()
    {
        ReleaseBuffers();
        CreateBuffers();
        GenerateChunkRange();
    }

    private void GenerateChunkRange()
    {
        MapChunk mapChunk = GetMapChunk(0, 0);
        List<Point> points = new List<Point>();
        for (int i = -spawnRoomSize; i <= spawnRoomSize; i++)
        {
            int height = spawnRoomSize - Mathf.Abs(i);
            for (int j = -height; j <= height; j++)
                points.Add(new Point(spawnPoint.xIndex + i, spawnPoint.yIndex + j));
        }

        spawnRoom = mapChunk.GenerateFixedRoom(points, spawnRoomIdentifier.GetRecord(), spawnBiomeIndex);
        spawnRoom.unlocked.value = true;
        new InfinityChest(spawnRoom, new Vector3(1, 0, 0), 0);

        for (int i = xMin; i <= xMax; i++)
        {
            for (int j = yMin; j <= yMax; j++)
            {
                GenerateChunk(i, j);
            }
        }
    }

    public void GenerateChunk(int xIndex, int yIndex)
    {
        MapChunk mapChunk = GetMapChunk(xIndex, yIndex);
        if (mapChunk.generated)
            return;

        mapChunk.GenerateChunkRooms();
        mapChunks.AddItemToList(mapChunk, xIndex, yIndex);
        mapChunkList.Add(mapChunk);
    }

    private void CreateBuffers()
    {
        random = new System.Random(seed);

        int[] biomeLayers = new int[biomeRecords.Length];
        int[] biomeStepIndices = new int[biomeRecords.Length];

        int totalBiomeLayers = 0;
        for (int i = 0; i < biomeRecords.Length; i++)
        {
            biomeLayers[i] = biomeRecords[i].Layers.Length;
            biomeStepIndices[i] = totalBiomeLayers;
            totalBiomeLayers += biomeRecords[i].Layers.Length;
        }

        int[] biomeDistancesToCenter = biomeRecords.Select(i => i.DistanceToCenter).ToArray();
        float[] biomeScaleStrength = biomeRecords.Select(i => i.ScaleStrength).ToArray();
        float[] biomeClampStrength = biomeRecords.Select(i => i.ClampStrength).ToArray();
        float[] biomeElevationStrength = biomeRecords.Select(i => i.ElevationStrength).ToArray();

        int[] biomeSeeds = new int[totalBiomeLayers];
        float[] biomeStepSizes = new float[totalBiomeLayers];
        float[] biomeStepRoughness = new float[totalBiomeLayers];

        int biomeIndex = 0;
        for (int i = 0; i < biomeRecords.Length; i++)
        {
            int biomeStartIndex = biomeIndex;
            float biomeTotalRoughness = 0;

            for (int j = 0; j < biomeRecords[i].Layers.Length; j++)
            {
                biomeStepSizes[biomeIndex] = chunkSize * biomeRecords[i].Layers[j].SizeFactor;
                biomeSeeds[biomeIndex] = random.Next();
                biomeTotalRoughness += biomeRecords[i].Layers[j].Roughness;
                biomeIndex++;
            }

            // Make sure the total roughness of an entire biome is set to 1
            biomeIndex = biomeStartIndex;
            for (int j = 0; j < biomeRecords[i].Layers.Length; j++)
            {
                biomeStepRoughness[biomeIndex] = biomeRecords[i].Layers[j].Roughness / biomeTotalRoughness;
                biomeIndex++;
            }
        }

        biomeLayersBuffer = new ComputeBuffer(biomeRecords.Length, Marshal.SizeOf<int>());
        biomeStepIndicesBuffer = new ComputeBuffer(biomeRecords.Length, Marshal.SizeOf<int>());

        biomeDistancesToCenterBuffer = new ComputeBuffer(biomeLayers.Length, Marshal.SizeOf<int>());
        biomeScaleStrengthsBuffer = new ComputeBuffer(biomeLayers.Length, Marshal.SizeOf<float>());
        biomeClampStrengthsBuffer = new ComputeBuffer(biomeLayers.Length, Marshal.SizeOf<float>());
        biomeElevationStrengthsBuffer = new ComputeBuffer(biomeLayers.Length, Marshal.SizeOf<float>());

        biomeSeedsBuffer = new ComputeBuffer(totalBiomeLayers, Marshal.SizeOf<int>());
        biomeStepSizesBuffer = new ComputeBuffer(totalBiomeLayers, Marshal.SizeOf<float>());
        biomeStepRoughnessBuffer = new ComputeBuffer(totalBiomeLayers, Marshal.SizeOf<float>());

        biomeMapGenerator.SetInt("size", chunkSize);
        biomeMapGenerator.SetInt("biomes", biomeRecords.Length);

        biomeMapGenerator.SetBuffer(generateBiomeMapKernel, "biomeLayers", biomeLayersBuffer);
        biomeMapGenerator.SetBuffer(generateBiomeMapKernel, "biomeStepIndices", biomeStepIndicesBuffer);

        biomeMapGenerator.SetBuffer(generateBiomeMapKernel, "biomeDistancesToCenter", biomeDistancesToCenterBuffer);
        biomeMapGenerator.SetBuffer(generateBiomeMapKernel, "biomeScaleStrengths", biomeScaleStrengthsBuffer);
        biomeMapGenerator.SetBuffer(generateBiomeMapKernel, "biomeClampStrengths", biomeClampStrengthsBuffer);
        biomeMapGenerator.SetBuffer(generateBiomeMapKernel, "biomeElevationStrengths", biomeElevationStrengthsBuffer);

        biomeMapGenerator.SetBuffer(generateBiomeMapKernel, "biomeSeeds", biomeSeedsBuffer);
        biomeMapGenerator.SetBuffer(generateBiomeMapKernel, "biomeStepSizes", biomeStepSizesBuffer);
        biomeMapGenerator.SetBuffer(generateBiomeMapKernel, "biomeStepRoughness", biomeStepRoughnessBuffer);

        biomeDistanceCalculator.SetInt("size", chunkSize);

        biomeLayersBuffer.SetData(biomeLayers);
        biomeStepIndicesBuffer.SetData(biomeStepIndices);

        biomeDistancesToCenterBuffer.SetData(biomeDistancesToCenter);
        biomeScaleStrengthsBuffer.SetData(biomeScaleStrength);
        biomeClampStrengthsBuffer.SetData(biomeClampStrength);
        biomeElevationStrengthsBuffer.SetData(biomeElevationStrength);

        biomeSeedsBuffer.SetData(biomeSeeds);
        biomeStepSizesBuffer.SetData(biomeStepSizes);
        biomeStepRoughnessBuffer.SetData(biomeStepRoughness);
    }

    public ComputeBuffer GetBiomeDistances(int xIndex, int yIndex)
    {
        ComputeBuffer biomeDistanceBuffer;
        if (!biomeDistanceBuffers.TryGetListItem(xIndex, yIndex, out biomeDistanceBuffer))
        {
            biomeDistanceBuffer = new ComputeBuffer(chunkSize * chunkSize, Marshal.SizeOf<int>());
            ComputeBuffer bottomLeftBuffer = GetBiomeMap(xIndex - 1, yIndex - 1);
            ComputeBuffer bottomCenterBuffer = GetBiomeMap(xIndex, yIndex - 1);
            ComputeBuffer bottomRightBuffer = GetBiomeMap(xIndex + 1, yIndex - 1);
            ComputeBuffer centerLeftBuffer = GetBiomeMap(xIndex - 1, yIndex);
            ComputeBuffer centerCenterBuffer = GetBiomeMap(xIndex, yIndex);
            ComputeBuffer centerRightBuffer = GetBiomeMap(xIndex + 1, yIndex);
            ComputeBuffer topLeftBuffer = GetBiomeMap(xIndex - 1, yIndex + 1);
            ComputeBuffer topCenterBuffer = GetBiomeMap(xIndex, yIndex + 1);
            ComputeBuffer topRightBuffer = GetBiomeMap(xIndex + 1, yIndex + 1);

            biomeDistanceCalculator.SetBuffer(calculateBiomeDistancesKernel, "bottomLeftBiomeMap", bottomLeftBuffer);
            biomeDistanceCalculator.SetBuffer(calculateBiomeDistancesKernel, "bottomCenterBiomeMap", bottomCenterBuffer);
            biomeDistanceCalculator.SetBuffer(calculateBiomeDistancesKernel, "bottomRightBiomeMap", bottomRightBuffer);
            biomeDistanceCalculator.SetBuffer(calculateBiomeDistancesKernel, "centerLeftBiomeMap", centerLeftBuffer);
            biomeDistanceCalculator.SetBuffer(calculateBiomeDistancesKernel, "centerCenterBiomeMap", centerCenterBuffer);
            biomeDistanceCalculator.SetBuffer(calculateBiomeDistancesKernel, "centerRightBiomeMap", centerRightBuffer);
            biomeDistanceCalculator.SetBuffer(calculateBiomeDistancesKernel, "topLeftBiomeMap", topLeftBuffer);
            biomeDistanceCalculator.SetBuffer(calculateBiomeDistancesKernel, "topCenterBiomeMap", topCenterBuffer);
            biomeDistanceCalculator.SetBuffer(calculateBiomeDistancesKernel, "topRightBiomeMap", topRightBuffer);
            biomeDistanceCalculator.SetBuffer(calculateBiomeDistancesKernel, "biomeDistanceMap", biomeDistanceBuffer);

            int threadSize = Mathf.CeilToInt((float)chunkSize / THREADS_BIOME_GENERATION);
            biomeDistanceCalculator.Dispatch(calculateBiomeDistancesKernel, threadSize, threadSize, 1);

            biomeDistanceBuffers.AddItemToList(biomeDistanceBuffer, xIndex, yIndex);
        }

        return biomeDistanceBuffer;
    }

    public ComputeBuffer GetBiomeMap(int xIndex, int yIndex)
    {
        ComputeBuffer biomeMapBuffer;
        if (!biomeMapBuffers.TryGetListItem(xIndex, yIndex, out biomeMapBuffer))
        {
            int biomeMapSize = chunkSize * chunkSize;
            biomeMapBuffer = new ComputeBuffer(biomeMapSize, Marshal.SizeOf<int>());
            biomeMapGenerator.SetInt("xLocation", xIndex * chunkSize);
            biomeMapGenerator.SetInt("yLocation", yIndex * chunkSize);
            int[] biomeMap = new int[biomeMapSize];

            int threadSize = Mathf.CeilToInt((float)chunkSize / THREADS_BIOME_GENERATION);
            biomeMapGenerator.SetBuffer(generateBiomeMapKernel, "biomeMap", biomeMapBuffer);
            biomeMapGenerator.Dispatch(generateBiomeMapKernel, threadSize, threadSize, 1);

            biomeMapBuffer.GetData(biomeMap);
            biomeMapBuffers.AddItemToList(biomeMapBuffer, xIndex, yIndex);
        }

        return biomeMapBuffer;
    }

    private void ReleaseBuffers()
    {
        biomeLayersBuffer.Release();
        biomeStepIndicesBuffer.Release();

        biomeDistancesToCenterBuffer.Release();
        biomeScaleStrengthsBuffer.Release();
        biomeClampStrengthsBuffer.Release();
        biomeElevationStrengthsBuffer.Release();

        biomeSeedsBuffer.Release();
        biomeStepSizesBuffer.Release();
        biomeStepRoughnessBuffer.Release();

        foreach (ComputeBuffer biomeDistanceBuffer in biomeDistanceBuffers)
            biomeDistanceBuffer.Release();

        foreach (ComputeBuffer biomeMapBuffer in biomeMapBuffers)
            biomeMapBuffer.Release();

        biomeDistanceBuffers.Clear();
        biomeMapBuffers.Clear();

        foreach (MapChunk mapChunk in mapChunks)
            mapChunk.CleanBuffers();

        mapChunks.Clear();
        mapChunkList.Clear();
    }

    public MapChunk GetMapChunk(Point point)
    {
        int xChunkIndex = Mathf.FloorToInt((float)point.xIndex / chunkSize);
        int yChunkIndex = Mathf.FloorToInt((float)point.yIndex / chunkSize);
        return GetMapChunk(xChunkIndex, yChunkIndex);
    }

    public (MapChunk, Point) GetMapChunkPoint(Point point)
    {
        int xChunkIndex = Mathf.FloorToInt((float)point.xIndex / chunkSize);
        int yChunkIndex = Mathf.FloorToInt((float)point.yIndex / chunkSize);
        MapChunk mapChunk = GetMapChunk(xChunkIndex, yChunkIndex);
        return (mapChunk, new Point(point.xIndex - xChunkIndex * chunkSize, point.yIndex - yChunkIndex * chunkSize));
    }

    public MapChunk GetMapChunk(int xIndex, int yIndex)
    {
        MapChunk mapChunk;

        if (!mapChunks.TryGetListItem(xIndex, yIndex, out mapChunk))
        {
            mapChunk = new MapChunk(this, xIndex, yIndex);
            mapChunks.AddItemToList(mapChunk, xIndex, yIndex);
        }

        return mapChunk;
    }

    public void CopyBiomeMaps(MapChunk mapChunk, MinMaxVector2 bounds)
    {
        int xSize = bounds.xMax - bounds.xMin + 1;
        int ySize = bounds.yMax - bounds.yMin + 1;
        mapCopier.SetInt("leftPadding", bounds.xMin - mapChunk.xPointOffset);
        mapCopier.SetInt("bottomPadding", bounds.yMin - mapChunk.yPointOffset);
        mapCopier.SetInt("targetWidth", xSize);
        mapCopier.SetInt("targetHeight", ySize);
        mapCopier.SetInt("size", chunkSize);

        mapCopier.SetBuffer(copyMapsKernel, "bottomLeftBiomeMap", mapChunk.bottomLeftChunk.biomeMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "bottomCenterBiomeMap", mapChunk.bottomCenterChunk.biomeMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "bottomRightBiomeMap", mapChunk.bottomRightChunk.biomeMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "centerLeftBiomeMap", mapChunk.centerLeftChunk.biomeMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "centerCenterBiomeMap", mapChunk.biomeMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "centerRightBiomeMap", mapChunk.centerRightChunk.biomeMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "topLeftBiomeMap", mapChunk.topLeftChunk.biomeMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "topCenterBiomeMap", mapChunk.topCenterChunk.biomeMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "topRightBiomeMap", mapChunk.topRightChunk.biomeMapBuffer);

        mapCopier.SetBuffer(copyMapsKernel, "bottomLeftRoomMap", mapChunk.bottomLeftChunk.roomMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "bottomCenterRoomMap", mapChunk.bottomCenterChunk.roomMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "bottomRightRoomMap", mapChunk.bottomRightChunk.roomMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "centerLeftRoomMap", mapChunk.centerLeftChunk.roomMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "centerCenterRoomMap", mapChunk.roomMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "centerRightRoomMap", mapChunk.centerRightChunk.roomMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "topLeftRoomMap", mapChunk.topLeftChunk.roomMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "topCenterRoomMap", mapChunk.topCenterChunk.roomMapBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "topRightRoomMap", mapChunk.topRightChunk.roomMapBuffer);

        mapCopier.SetBuffer(copyMapsKernel, "biomeMap", mapChunk.biomeMapPaddingBuffer);
        mapCopier.SetBuffer(copyMapsKernel, "roomMap", mapChunk.roomMapPaddingBuffer);

        int xThreads = Mathf.CeilToInt((float)xSize / THREADS_BIOME_COPY);
        int yThreads = Mathf.CeilToInt((float)ySize / THREADS_BIOME_COPY);
        mapCopier.Dispatch(copyMapsKernel, xThreads, yThreads, 1);
    }

    public MapChunk[] GetMapChunkRange(MinMaxVector2 bounds)
    {
        int xChunkIndexMin = Mathf.FloorToInt((float)bounds.xMin / chunkSize);
        int xChunkIndexMax = Mathf.FloorToInt((float)bounds.xMax / chunkSize);
        int yChunkIndexMin = Mathf.FloorToInt((float)bounds.yMin / chunkSize);
        int yChunkIndexMax = Mathf.FloorToInt((float)bounds.yMax / chunkSize);

        int xChunkRange = xChunkIndexMax - xChunkIndexMin + 1;
        int yChunkRange = yChunkIndexMax - yChunkIndexMin + 1;
        MapChunk[] mapChunkRange = new MapChunk[xChunkRange * yChunkRange];

        for (int i = 0; i < xChunkRange; i++)
            for (int j = 0; j < yChunkRange; j++)
                mapChunkRange[i + j * xChunkRange] = GetMapChunk(i + xChunkIndexMin, j + yChunkIndexMin);

        return mapChunkRange;
    }

    private void OnDestroy()
    {
        ReleaseBuffers();
    }
}