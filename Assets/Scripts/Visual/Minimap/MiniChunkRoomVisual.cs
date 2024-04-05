using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniChunkRoomVisual : DataDrivenBehaviour<ChunkRoom>
{
    [SerializeField] private WorldResourceMarker worldResourceMarkerPrefab;
    [SerializeField] private Transform worldResourceMarkerContainer;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshCollider meshCollider;

    public readonly EventVariable<MiniChunkRoomVisual, bool> shouldRender;
    private readonly List<WorldResourceMarker> worldResourceMarkerInstances;
    private Mesh mesh;

    private MiniChunkRoomVisual()
    {
        worldResourceMarkerInstances = new List<WorldResourceMarker>();
        shouldRender = new EventVariable<MiniChunkRoomVisual, bool>(this, false);
    }

    private void Awake()
    {
        shouldRender.onValueChange += OnValueChanged_ShouldRender;
    }

    protected override void OnValueChanged_Data(ChunkRoom oldValue, ChunkRoom newValue)
    {
        if (oldValue != null)
        {
            oldValue.meshReady.onValueChange -= OnValueChanged_MeshReady;
            oldValue.visited.onValueChange -= OnValueChanged_Visited;
        }

        if (newValue != null)
        {
            newValue.meshReady.onValueChange += OnValueChanged_MeshReady;
            newValue.visited.onValueChange += OnValueChanged_Visited;
            Check_ShouldRender();
        }

    }

    private void OnValueChanged_MeshReady(bool oldValue, bool newValue)
    {
        Check_ShouldRender();
    }

    private void OnValueChanged_Visited(bool oldValue, bool newValue)
    {
        Check_ShouldRender();
    }

    private void Check_ShouldRender()
    {
        if (!data.meshReady.value)
        {
            shouldRender.value = false;
            return;
        }

        if (!data.visited.value)
        {
            shouldRender.value = false;
            return;
        }

        shouldRender.value = true;
    }

    private void OnValueChanged_ShouldRender(bool oldValue, bool newValue)
    {
        if (oldValue)
        {
            GameObject.Destroy(mesh);
            mesh = null;

            foreach (WorldResourceMarker instance in worldResourceMarkerInstances)
                GameObject.Destroy(instance.gameObject);

            worldResourceMarkerInstances.Clear();
        }

        if (newValue)
        {
            GenerateMesh();
            meshRenderer.material = data.centerMapChunk.chunkRoomMaterial;
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;

            foreach (WorldResourceSpawn worldResourceSpawn in data.worldResourceSpawns)
            {
                WorldResourceMarker instance = GameObject.Instantiate(worldResourceMarkerPrefab, worldResourceMarkerContainer);
                instance.data = worldResourceSpawn;
                worldResourceMarkerInstances.Add(instance);
            }

            worldResourceMarkerInstances.Clear();
        }
    }

    public void GenerateMesh()
    {
        List<int> indices = new List<int>();
        List<Vector3> vertices = new List<Vector3>();
        foreach (Point point in data.chunkPoints)
        {
            int bottomLeftWall = data.centerMapChunk.GetChunkRoom(point.AddDirection(DiagonalDirection.BottomLeft)) != data ? 1 : 0;
            int bottomCenterWall = data.centerMapChunk.GetChunkRoom(point.AddDirection(DiagonalDirection.BottomCenter)) != data ? 1 : 0;
            int bottomRightWall = data.centerMapChunk.GetChunkRoom(point.AddDirection(DiagonalDirection.BottomRight)) != data ? 1 : 0;

            int centerLeftWall = data.centerMapChunk.GetChunkRoom(point.AddDirection(DiagonalDirection.CenterLeft)) != data ? 1 : 0;
            int centerRightWall = data.centerMapChunk.GetChunkRoom(point.AddDirection(DiagonalDirection.CenterRight)) != data ? 1 : 0;

            int topLeftWall = data.centerMapChunk.GetChunkRoom(point.AddDirection(DiagonalDirection.TopLeft)) != data ? 1 : 0;
            int topCenterWall = data.centerMapChunk.GetChunkRoom(point.AddDirection(DiagonalDirection.TopCenter)) != data ? 1 : 0;
            int topRightWall = data.centerMapChunk.GetChunkRoom(point.AddDirection(DiagonalDirection.TopRight)) != data ? 1 : 0;

            bool leftDoor = false;
            bool bottomDoor = false;
            bool topDoor = false;
            bool rightDoor = false;
            IEnumerable<RoomDoor> cellDoors = data.roomDoors.Where(i => i.firstRoomPoint.Equals(point) || i.secondRoomPoint.Equals(point));
            foreach (RoomDoor cellDoor in cellDoors)
            {
                switch (cellDoor.GetRoomDirection(data))
                {
                    case CardinalDirection.Bottom:
                        bottomCenterWall = 0;
                        bottomLeftWall = 1;
                        bottomRightWall = 1;
                        bottomDoor = true;
                        break;
                    case CardinalDirection.Top:
                        topCenterWall = 0;
                        topLeftWall = 1;
                        topRightWall = 1;
                        topDoor = true;
                        break;
                    case CardinalDirection.Left:
                        centerLeftWall = 0;
                        topLeftWall = 1;
                        bottomLeftWall = 1;
                        leftDoor = true;
                        break;
                    case CardinalDirection.Right:
                        centerRightWall = 0;
                        topRightWall = 1;
                        bottomRightWall = 1;
                        rightDoor = true;
                        break;
                }
            }

            if (leftDoor)
            {
                int indexCount = vertices.Count;
                for (int i = 0; i < ChunkCell.LEFTDOOR_INDICES.Length; i++)
                    indices.Add(indexCount + ChunkCell.LEFTDOOR_INDICES[i]);

                for (int i = 0; i < ChunkCell.LEFTDOOR_VERTICES.Length; i++)
                    vertices.Add(ChunkCell.LEFTDOOR_VERTICES[i] + new Vector3(point.xIndex - data.centerMapChunk.xPointOffset, point.yIndex - data.centerMapChunk.yPointOffset, 0) * ChunkCell.CELLSIZE);
            }

            if (topDoor)
            {
                int indexCount = vertices.Count;
                for (int i = 0; i < ChunkCell.TOPDOOR_INDICES.Length; i++)
                    indices.Add(indexCount + ChunkCell.TOPDOOR_INDICES[i]);

                for (int i = 0; i < ChunkCell.TOPDOOR_VERTICES.Length; i++)
                    vertices.Add(ChunkCell.TOPDOOR_VERTICES[i] + new Vector3(point.xIndex - data.centerMapChunk.xPointOffset, point.yIndex - data.centerMapChunk.yPointOffset, 0) * ChunkCell.CELLSIZE);
            }

            if (rightDoor)
            {
                int indexCount = vertices.Count;
                for (int i = 0; i < ChunkCell.RIGHTDOOR_INDICES.Length; i++)
                    indices.Add(indexCount + ChunkCell.RIGHTDOOR_INDICES[i]);

                for (int i = 0; i < ChunkCell.RIGHTDOOR_VERTICES.Length; i++)
                    vertices.Add(ChunkCell.RIGHTDOOR_VERTICES[i] + new Vector3(point.xIndex - data.centerMapChunk.xPointOffset, point.yIndex - data.centerMapChunk.yPointOffset, 0) * ChunkCell.CELLSIZE);
            }

            if (bottomDoor)
            {
                int indexCount = vertices.Count;
                for (int i = 0; i < ChunkCell.BOTTOMDOOR_INDICES.Length; i++)
                    indices.Add(indexCount + ChunkCell.BOTTOMDOOR_INDICES[i]);

                for (int i = 0; i < ChunkCell.BOTTOMDOOR_VERTICES.Length; i++)
                    vertices.Add(ChunkCell.BOTTOMDOOR_VERTICES[i] + new Vector3(point.xIndex - data.centerMapChunk.xPointOffset, point.yIndex - data.centerMapChunk.yPointOffset, 0) * ChunkCell.CELLSIZE);
            }

            if (bottomCenterWall == 1)
            {
                bottomLeftWall = 0;
                bottomRightWall = 0;
            }

            if (topCenterWall == 1)
            {
                topLeftWall = 0;
                topRightWall = 0;
            }

            if (centerLeftWall == 1)
            {
                topLeftWall = 0;
                bottomLeftWall = 0;
            }

            if (centerRightWall == 1)
            {
                topRightWall = 0;
                bottomRightWall = 0;
            }

            int index = (bottomLeftWall << 0) +
                (bottomCenterWall << 1) +
                (bottomRightWall << 2) +
                (centerLeftWall << 3) +
                (centerRightWall << 4) +
                (topLeftWall << 5) +
                (topCenterWall << 6) +
                (topRightWall << 7);

            int[] cellIndices = ChunkCell.CELL_INDICES[index];
            Vector3[] cellVertices = ChunkCell.CELL_VERTICES[index];

            int currentIndicesCount = vertices.Count;
            for (int i = 0; i < cellIndices.Length; i++)
                indices.Add(currentIndicesCount + cellIndices[i]);

            for (int i = 0; i < cellVertices.Length; i++)
                vertices.Add(cellVertices[i] + new Vector3(point.xIndex - data.centerMapChunk.xPointOffset, point.yIndex - data.centerMapChunk.yPointOffset, 0) * ChunkCell.CELLSIZE);
        }

        mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
        Vector3[] normals = new Vector3[vertices.Count];

        for (int i = 0; i < normals.Length; i++)
            normals[i] = -Vector3.forward;

        mesh.normals = normals;
        mesh.UploadMeshData(true);
    }

    private void OnDestroy()
    {
        shouldRender.onValueChange -= OnValueChanged_ShouldRender;
    }
}