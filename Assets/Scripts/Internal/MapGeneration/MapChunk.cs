
using SheetCodes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class MapChunk
{
    private static List<ChunkRoom> unconnectedRooms;
    private static List<ChunkRoom> meshReadyRooms;
    private static ChunkRoom dummyRoom;

    public System.Random random => mapGenerator.random;
    public int chunkSize => mapGenerator.ChunkSize;

    public readonly EventList<ChunkRoom> finishedRooms;
    public readonly int xIndex;
    public readonly int yIndex;

    public int xPointOffset => xIndex * chunkSize;
    public int yPointOffset => yIndex * chunkSize;

    public MapChunk bottomLeftChunk { private set; get; }
    public MapChunk bottomCenterChunk { private set; get; }
    public MapChunk bottomRightChunk { private set; get; }
    public MapChunk centerLeftChunk { private set; get; }
    public MapChunk centerRightChunk { private set; get; }
    public MapChunk topLeftChunk { private set; get; }
    public MapChunk topCenterChunk { private set; get; }
    public MapChunk topRightChunk { private set; get; }

    public bool generated { private set; get; }
    public Material chunkRoomMaterial { private set; get; }
    public ComputeBuffer biomeMapBuffer { private set; get; }
    public ComputeBuffer roomMapBuffer { private set; get; }
    public ComputeBuffer biomeMapPaddingBuffer { private set; get; }
    public ComputeBuffer roomMapPaddingBuffer { private set; get; }

    public readonly MapGenerator mapGenerator;
    private readonly List<ChunkRoom> unfinishedRooms;
    private readonly ChunkRoom[,] chunkRoomPoints;
    private int[] biomeMap;
    private int[] biomeDistanceMap;
    private int[] roomMap;

    private MinMaxVector2 chunkBounds;

    private BiomeRecord[] biomeSettings => mapGenerator.biomeRecords;

    static MapChunk()
    {
        dummyRoom = new ChunkRoom();
        unconnectedRooms = new List<ChunkRoom>();
        meshReadyRooms = new List<ChunkRoom>();
    }

    public MapChunk(MapGenerator mapGenerator, int xIndex, int yIndex)
    {
        this.mapGenerator = mapGenerator;
        this.xIndex = xIndex;
        this.yIndex = yIndex;

        chunkRoomMaterial = new Material(mapGenerator.ChunkRoomMaterial);
        chunkRoomMaterial.SetFloat("doorHeight", ChunkCell.DOORHEIGHT);
        chunkRoomMaterial.SetFloat("wallSize", ChunkCell.WALLWIDTH);
        chunkRoomMaterial.SetFloat("wallHeight", ChunkCell.WALLHEIGHT);
        chunkRoomMaterial.SetFloat("cellSize", ChunkCell.CELLSIZE);

        generated = false;
        chunkRoomPoints = new ChunkRoom[chunkSize, chunkSize];
        finishedRooms = new EventList<ChunkRoom>();
        unfinishedRooms = new List<ChunkRoom>();

        biomeMapBuffer = mapGenerator.GetBiomeMap(xIndex, yIndex);
        biomeMap = new int[chunkSize * chunkSize];
        biomeMapBuffer.GetData(biomeMap);

        roomMapBuffer = new ComputeBuffer(chunkSize * chunkSize, Marshal.SizeOf<int>());
        roomMap = new int[chunkSize * chunkSize];
    }

    public ChunkRoom GenerateFixedRoom(List<Point> points, RoomRecord roomSettings, int biomeIndex)
    {
        ChunkRoom room = new ChunkRoom(this, points, roomSettings, biomeIndex, 0, true);
        foreach (Point point in points)
            AssignRoomToPoint(room, point);

        unfinishedRooms.Add(room);
        return room;
    }

    public void GenerateAdjacentChunks()
    {
        mapGenerator.GenerateChunk(xIndex - 1, yIndex - 1);
        mapGenerator.GenerateChunk(xIndex, yIndex - 1);
        mapGenerator.GenerateChunk(xIndex + 1, yIndex - 1);

        mapGenerator.GenerateChunk(xIndex - 1, yIndex);
        mapGenerator.GenerateChunk(xIndex + 1, yIndex);

        mapGenerator.GenerateChunk(xIndex - 1, yIndex + 1);
        mapGenerator.GenerateChunk(xIndex, yIndex + 1);
        mapGenerator.GenerateChunk(xIndex + 1, yIndex + 1);
    }

    public void GenerateChunkRooms()
    {
        if (generated)
            return;

        ComputeBuffer biomeDistanceBuffer = mapGenerator.GetBiomeDistances(xIndex, yIndex);
        biomeDistanceMap = new int[chunkSize * chunkSize];
        biomeDistanceBuffer.GetData(biomeDistanceMap);

        bottomLeftChunk = mapGenerator.GetMapChunk(xIndex - 1, yIndex - 1);
        bottomCenterChunk = mapGenerator.GetMapChunk(xIndex, yIndex - 1);
        bottomRightChunk = mapGenerator.GetMapChunk(xIndex + 1, yIndex - 1);
        centerLeftChunk = mapGenerator.GetMapChunk(xIndex - 1, yIndex);
        centerRightChunk = mapGenerator.GetMapChunk(xIndex + 1, yIndex);
        topLeftChunk = mapGenerator.GetMapChunk(xIndex - 1, yIndex + 1);
        topCenterChunk = mapGenerator.GetMapChunk(xIndex, yIndex + 1);
        topRightChunk = mapGenerator.GetMapChunk(xIndex + 1, yIndex + 1);

        Point[] unUsedCells = new Point[chunkSize * chunkSize];
        int[,] unUsedCellIndices = new int[chunkSize, chunkSize];

        int unUsedCellCount = 0;
        for (int i = 0; i < mapGenerator.ChunkSize; i++)
        {
            for (int j = 0; j < mapGenerator.ChunkSize; j++)
            {
                if (chunkRoomPoints[i, j] != null)
                    continue;

                unUsedCellIndices[i, j] = unUsedCellCount;
                unUsedCells[unUsedCellCount] = new Point(i + xPointOffset, j + yPointOffset);
                unUsedCellCount++;
            }
        }

        List<Point> remainingPoints = new List<Point>();
        while (unUsedCellCount > 0)
        {
            int index = random.Next(unUsedCellCount);
            Point point = unUsedCells[index];
            int distanceFromCenter = Mathf.Abs(point.xIndex) + Mathf.Abs(point.yIndex);
            int biomeIndex = biomeMap[(point.xIndex - xPointOffset) + (point.yIndex - yPointOffset) * chunkSize];
            BiomeRecord biomeSetting = biomeSettings[biomeIndex];
            int distanceInBiome = biomeDistanceMap[(point.xIndex - xPointOffset) + (point.yIndex - yPointOffset) * chunkSize];
            RoomRecord[] possibleRooms = biomeSetting.Rooms.Where(i => i.MinBiomeDistance <= distanceInBiome && i.MaxBiomeDistance >= distanceInBiome && i.MinSpawnDistance <= distanceFromCenter).ToArray();
            int maximumRoomSize = possibleRooms.Max(i => i.MaxRoomSize);
            List<Point> roomPoints = new List<Point>();
            List<Point> possibleRoomPoints = new List<Point>();
            possibleRoomPoints.Add(point);
            AssignRoomToPoint(dummyRoom, point);

            while (roomPoints.Count < maximumRoomSize && possibleRoomPoints.Count > 0)
            {
                int randomIndex = random.Next(possibleRoomPoints.Count);
                Point currentPoint = possibleRoomPoints[randomIndex];
                possibleRoomPoints.RemoveAt(randomIndex);

                roomPoints.Add(currentPoint);
                Point targetPoint = currentPoint.AddDirection(DiagonalDirection.CenterLeft);
                if (!IsPointAssignedToARoom(targetPoint))
                {
                    possibleRoomPoints.Add(targetPoint);
                    AssignRoomToPoint(dummyRoom, targetPoint);
                }

                targetPoint = currentPoint.AddDirection(DiagonalDirection.CenterRight);
                if (!IsPointAssignedToARoom(targetPoint))
                {
                    possibleRoomPoints.Add(targetPoint);
                    AssignRoomToPoint(dummyRoom, targetPoint);
                }

                targetPoint = currentPoint.AddDirection(DiagonalDirection.BottomCenter);
                if (!IsPointAssignedToARoom(targetPoint))
                {
                    possibleRoomPoints.Add(targetPoint);
                    AssignRoomToPoint(dummyRoom, targetPoint);
                }

                targetPoint = currentPoint.AddDirection(DiagonalDirection.TopCenter);
                if (!IsPointAssignedToARoom(targetPoint))
                {
                    possibleRoomPoints.Add(targetPoint);
                    AssignRoomToPoint(dummyRoom, targetPoint);
                }
            }

            foreach (Point roomPoint in possibleRoomPoints)
                AssignRoomToPoint(null, roomPoint);

            possibleRooms = possibleRooms.Where(i => i.MinRoomSize <= roomPoints.Count).ToArray();
            if (possibleRooms.Length == 0)
            {
                remainingPoints.AddRange(roomPoints);
                foreach (Point roomPoint in roomPoints)
                {
                    AssignRoomToPoint(null, roomPoint);
                    if (roomPoint.xIndex < xPointOffset || roomPoint.xIndex >= chunkSize + xPointOffset || roomPoint.yIndex < yPointOffset || roomPoint.yIndex >= chunkSize + yPointOffset)
                        continue;

                    int pointIndex = unUsedCellIndices[roomPoint.xIndex - xPointOffset, roomPoint.yIndex - yPointOffset];
                    unUsedCellCount--;
                    if (pointIndex == unUsedCellCount)
                        continue;

                    Point temp = unUsedCells[unUsedCellCount];
                    unUsedCellIndices[temp.xIndex - xPointOffset, temp.yIndex - yPointOffset] = pointIndex;
                    unUsedCells[pointIndex] = temp;
                }
                continue;
            }

            WeightedRandomizer<RoomRecord> weightedRandomizer = new WeightedRandomizer<RoomRecord>(random.Next());
            foreach (RoomRecord possibleRoom in possibleRooms)
            {
                float lerpFactor = Mathf.Min(1f, (float)(possibleRoom.MaxSpawnDistance - possibleRoom.MinSpawnDistance) / (distanceFromCenter - possibleRoom.MinSpawnDistance));
                int weight = (int)Mathf.Lerp(possibleRoom.MinSpawnDistanceWeight, possibleRoom.MaxSpawnDistanceWeight, lerpFactor);
                weightedRandomizer.AddItem(possibleRoom, weight);
            }

            RoomRecord targetRoom = weightedRandomizer.GetRandomItem();
            int targetRoomSize = Mathf.Min(random.Next(targetRoom.MinRoomSize, targetRoom.MaxRoomSize + 1), roomPoints.Count);

            List<Point> chunkRoomPoints = roomPoints.GetRange(0, targetRoomSize);
            ChunkRoom chunkRoom = new ChunkRoom(this, chunkRoomPoints, targetRoom, biomeIndex, distanceFromCenter, false);
            for (int i = 0; i < targetRoomSize; i++)
            {
                Point roomPoint = roomPoints[i];
                AssignRoomToPoint(chunkRoom, roomPoint);

                if (roomPoint.xIndex < xPointOffset || roomPoint.xIndex >= chunkSize + xPointOffset || roomPoint.yIndex < yPointOffset || roomPoint.yIndex >= chunkSize + yPointOffset)
                    continue;

                int pointIndex = unUsedCellIndices[roomPoint.xIndex - xPointOffset, roomPoint.yIndex - yPointOffset];
                unUsedCellCount--;
                if (pointIndex == unUsedCellCount)
                    continue;

                Point temp = unUsedCells[unUsedCellCount];
                unUsedCellIndices[temp.xIndex - xPointOffset, temp.yIndex - yPointOffset] = pointIndex;
                unUsedCells[pointIndex] = temp;
            }
            for (int i = targetRoomSize; i < roomPoints.Count; i++)
            {
                Point roomPoint = roomPoints[i];
                AssignRoomToPoint(null, roomPoint);
            }
            unfinishedRooms.Add(chunkRoom);
        }

        WeightedRandomizer<ChunkRoom> randomizer = new WeightedRandomizer<ChunkRoom>(random.Next());
        while (true)
        {
            bool anyFound = false;
            for (int i = remainingPoints.Count - 1; i >= 0; i--)
            {
                Point point = remainingPoints[i];
                ChunkRoom leftRoom = GetChunkRoom(point.AddDirection(DiagonalDirection.CenterLeft));
                ChunkRoom rightRoom = GetChunkRoom(point.AddDirection(DiagonalDirection.CenterRight));
                ChunkRoom bottomRoom = GetChunkRoom(point.AddDirection(DiagonalDirection.BottomCenter));
                ChunkRoom topRoom = GetChunkRoom(point.AddDirection(DiagonalDirection.TopCenter));

                if (leftRoom == null || rightRoom == null || topRoom == null || bottomRoom == null)
                    continue;

                if (!leftRoom.fixedRoom)
                    randomizer.AddItem(leftRoom, 1);
                if (!rightRoom.fixedRoom)
                    randomizer.AddItem(rightRoom, 1);
                if (!bottomRoom.fixedRoom)
                    randomizer.AddItem(bottomRoom, 1);
                if (!topRoom.fixedRoom)
                    randomizer.AddItem(topRoom, 1);

                ChunkRoom room = randomizer.GetRandomItem();
                room.chunkPoints.Add(point);
                AssignRoomToPoint(room, point);
                randomizer.Clear();
                anyFound = true;
                remainingPoints.RemoveAt(i);
            }

            if (anyFound)
                continue;

            for (int i = remainingPoints.Count - 1; i >= 0; i--)
            {
                Point point = remainingPoints[i];
                ChunkRoom leftRoom = GetChunkRoom(point.AddDirection(DiagonalDirection.CenterLeft));
                ChunkRoom rightRoom = GetChunkRoom(point.AddDirection(DiagonalDirection.CenterRight));
                ChunkRoom bottomRoom = GetChunkRoom(point.AddDirection(DiagonalDirection.BottomCenter));
                ChunkRoom topRoom = GetChunkRoom(point.AddDirection(DiagonalDirection.TopCenter));

                int count = 0;
                if (leftRoom != null)
                    count++;
                if (rightRoom != null)
                    count++;
                if (topRoom != null)
                    count++;
                if (bottomRoom != null)
                    count++;

                if (count != 3)
                    continue;

                if (leftRoom != null && !leftRoom.fixedRoom)
                    randomizer.AddItem(leftRoom, 1);
                if (rightRoom != null && !rightRoom.fixedRoom)
                    randomizer.AddItem(rightRoom, 1);
                if (bottomRoom != null && !bottomRoom.fixedRoom)
                    randomizer.AddItem(bottomRoom, 1);
                if (topRoom != null && !topRoom.fixedRoom)
                    randomizer.AddItem(topRoom, 1);

                ChunkRoom room = randomizer.GetRandomItem();
                room.chunkPoints.Add(point);
                AssignRoomToPoint(room, point);
                randomizer.Clear();

                anyFound = true;
                remainingPoints.RemoveAt(i);
            }

            if (anyFound)
                continue;

            for (int i = remainingPoints.Count - 1; i >= 0; i--)
            {
                Point point = remainingPoints[i];
                ChunkRoom leftRoom = GetChunkRoom(point.AddDirection(DiagonalDirection.CenterLeft));
                ChunkRoom rightRoom = GetChunkRoom(point.AddDirection(DiagonalDirection.CenterRight));
                ChunkRoom bottomRoom = GetChunkRoom(point.AddDirection(DiagonalDirection.BottomCenter));
                ChunkRoom topRoom = GetChunkRoom(point.AddDirection(DiagonalDirection.TopCenter));

                int count = 0;
                if (leftRoom != null)
                    count++;
                if (rightRoom != null)
                    count++;
                if (topRoom != null)
                    count++;
                if (bottomRoom != null)
                    count++;

                if (count != 2)
                    continue;

                if (leftRoom != null && !leftRoom.fixedRoom)
                    randomizer.AddItem(leftRoom, 1);
                if (rightRoom != null && !rightRoom.fixedRoom)
                    randomizer.AddItem(rightRoom, 1);
                if (bottomRoom != null && !bottomRoom.fixedRoom)
                    randomizer.AddItem(bottomRoom, 1);
                if (topRoom != null && !topRoom.fixedRoom)
                    randomizer.AddItem(topRoom, 1);

                if (randomizer.count > 0)
                {
                    ChunkRoom room = randomizer.GetRandomItem();
                    room.chunkPoints.Add(point);
                    AssignRoomToPoint(room, point);

                    anyFound = true;
                    remainingPoints.RemoveAt(i);
                }
                randomizer.Clear();
            }

            if (anyFound)
                continue;

            break;
        }

        generated = true;

        CheckUnfinishedRooms();
        bottomLeftChunk.CheckUnfinishedRooms();
        bottomCenterChunk.CheckUnfinishedRooms();
        bottomRightChunk.CheckUnfinishedRooms();
        centerLeftChunk.CheckUnfinishedRooms();
        centerRightChunk.CheckUnfinishedRooms();
        topLeftChunk.CheckUnfinishedRooms();
        topCenterChunk.CheckUnfinishedRooms();
        topRightChunk.CheckUnfinishedRooms();

        FixUnconnectedRooms();

        AssignBufferData();
        bottomLeftChunk.AssignBufferData();
        bottomCenterChunk.AssignBufferData();
        bottomRightChunk.AssignBufferData();
        centerLeftChunk.AssignBufferData();
        centerRightChunk.AssignBufferData();
        topLeftChunk.AssignBufferData();
        topCenterChunk.AssignBufferData();
        topRightChunk.AssignBufferData();

        SetMaterialProperties();
        bottomLeftChunk.SetMaterialProperties();
        bottomCenterChunk.SetMaterialProperties();
        bottomRightChunk.SetMaterialProperties();
        centerLeftChunk.SetMaterialProperties();
        centerRightChunk.SetMaterialProperties();
        topLeftChunk.SetMaterialProperties();
        topCenterChunk.SetMaterialProperties();
        topRightChunk.SetMaterialProperties();

        MarkMeshReadyRooms();
    }

    private void FixUnconnectedRooms()
    {
        while (unconnectedRooms.Count > 0)
        {
            int randomIndex = random.Next(unconnectedRooms.Count);
            ChunkRoom randomRoom = unconnectedRooms[randomIndex];
            ChunkRoom newConnectedRoom;
            if (randomRoom.TryAddRandomRoomConnection(out newConnectedRoom))
            {
                if (!newConnectedRoom.meshReady.value)
                    continue;

                newConnectedRoom.meshReady.value = false;
                meshReadyRooms.Add(newConnectedRoom);
                continue;
            }

            unconnectedRooms.RemoveAt(randomIndex);
        }
    }

    private void MarkMeshReadyRooms()
    {
        foreach (ChunkRoom room in meshReadyRooms)
        {
            room.GenerateRoomContents();
            room.meshReady.value = true;
        }

        meshReadyRooms.Clear();
    }

    private void CheckUnfinishedRooms()
    {
        for (int i = 0; i < unfinishedRooms.Count; i++)
        {
            ChunkRoom chunkRoom = unfinishedRooms[i];
            if (!chunkRoom.CheckRoomAdjacencyFinished())
                continue;

            unfinishedRooms.RemoveAt(i);
            i--;
            finishedRooms.Add(chunkRoom);
            chunkRoom.SetMapIndices();
            List<ChunkRoom> newConnectedRooms = chunkRoom.GenerateInitialDoors();

            foreach (ChunkRoom newConnectedRoom in newConnectedRooms)
            {
                if (!newConnectedRoom.meshReady.value)
                    continue;

                newConnectedRoom.meshReady.value = false;
                meshReadyRooms.Add(newConnectedRoom);
            }

            if (chunkRoom.chunkSet.setIndex > 0)
                unconnectedRooms.Add(chunkRoom);

            meshReadyRooms.Add(chunkRoom);
        }
    }

    public MapChunk[] GetMapChunkRange(MinMaxVector2 bounds)
    {
        return mapGenerator.GetMapChunkRange(bounds);
    }

    private void AssignBufferData()
    {
        biomeMapBuffer.SetData(biomeMap);
        roomMapBuffer.SetData(roomMap);
    }

    public (MapChunk, Point) GetMapChunkPoint(Point point)
    {
        return mapGenerator.GetMapChunkPoint(point);
    }

    private bool IsPointAssignedToARoom(Point point)
    {
        (MapChunk, Point) chunkPoint = GetMapChunkPoint(point);
        return chunkPoint.Item1.chunkRoomPoints[chunkPoint.Item2.xIndex, chunkPoint.Item2.yIndex] != null;
    }

    private void AssignRoomToPoint(ChunkRoom chunkRoom, Point point)
    {
        (MapChunk, Point) chunkPoint = GetMapChunkPoint(point);
        chunkPoint.Item1.chunkRoomPoints[chunkPoint.Item2.xIndex, chunkPoint.Item2.yIndex] = chunkRoom;
    }

    public ChunkRoom GetChunkRoom(Point point)
    {
        (MapChunk, Point) chunkPoint = GetMapChunkPoint(point);
        return chunkPoint.Item1.chunkRoomPoints[chunkPoint.Item2.xIndex, chunkPoint.Item2.yIndex];
    }

    public void SetMapChunkIndex(int biomeIndex, int roomIndex, Point point)
    {
        (MapChunk, Point) chunkPoint = GetMapChunkPoint(point);
        chunkPoint.Item1.biomeMap[chunkPoint.Item2.xIndex + chunkPoint.Item2.yIndex * chunkSize] = biomeIndex;
        chunkPoint.Item1.roomMap[chunkPoint.Item2.xIndex + chunkPoint.Item2.yIndex * chunkSize] = roomIndex;
    }

    private void SetMaterialProperties()
    {
        if (!generated)
            return;

        if (biomeMapPaddingBuffer != null)
        {
            biomeMapPaddingBuffer.Release();
            roomMapPaddingBuffer.Release();
        }

        chunkBounds = MinMaxVector2.CreateEmpty();

        foreach (ChunkRoom chunkRoom in finishedRooms)
            foreach (Point point in chunkRoom.chunkPoints)
                chunkBounds = MinMaxVector2.AssignHighestOffsets(chunkBounds, chunkRoom.bounds);

        int xSize = chunkBounds.xMax - chunkBounds.xMin + 1;
        int ySize = chunkBounds.yMax - chunkBounds.yMin + 1;
        biomeMapPaddingBuffer = new ComputeBuffer(xSize * ySize, Marshal.SizeOf<int>());
        roomMapPaddingBuffer = new ComputeBuffer(xSize * ySize, Marshal.SizeOf<int>());
        chunkRoomMaterial.SetBuffer("biomeMap", biomeMapPaddingBuffer);
        chunkRoomMaterial.SetBuffer("roomMap", roomMapPaddingBuffer);
        chunkRoomMaterial.SetInt("biomeMapSize", xSize);
        chunkRoomMaterial.SetInt("leftPadding", chunkBounds.xMin - xPointOffset);
        chunkRoomMaterial.SetInt("bottomPadding", chunkBounds.yMin - yPointOffset);

        mapGenerator.CopyBiomeMaps(this, chunkBounds);
    }

    public void CleanBuffers()
    {
        roomMapBuffer.Release();

        if (biomeMapPaddingBuffer != null)
        {
            biomeMapPaddingBuffer.Release();
            roomMapPaddingBuffer.Release();
        }
    }
}