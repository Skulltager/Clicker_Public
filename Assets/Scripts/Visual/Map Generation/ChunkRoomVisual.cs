using SheetCodes;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRoomVisual : DataDrivenBehaviour<ChunkRoom>
{
    private const string PROPERTY_ROOMCOLOR = "roomColor";

    [SerializeField] private Material baseWallsColorMaterial;
    [SerializeField] private Transform tilesContainer = default;
    [SerializeField] private GameObject floorTile = default;
    [SerializeField] private GameObject wallTile = default;
    [SerializeField] private GameObject ceilingTile = default;
    [SerializeField] private DoorData doorTile = default;

    [SerializeField] private float roomHeight = default;
    [SerializeField] private float roomWidth = default;

    private readonly List<DoorData> doorInstances;
    private readonly List<GameObject> tileInstances;
    private readonly List<Interactable> interactableInstances;
    private readonly List<WorldResource> worldResourceInstances;
    private bool[,] roomLayout;
    private int xOffset;
    private int yOffset;
    private int renderedChunkRoomsIndex;

    public readonly EventVariable<ChunkRoomVisual, bool> shouldRender;
    public static readonly EventList<ChunkRoomVisual> renderedChunkRooms;
    public readonly EventVariable<ChunkRoomVisual, Material> wallsMaterial;
    public readonly EventVariable<ChunkRoomVisual, Material> wallsColorMaterial;
    public readonly EventVariable<ChunkRoomVisual, Material> ceilingMaterial;
    public readonly EventVariable<ChunkRoomVisual, Material> floorMaterial;

    static ChunkRoomVisual()
    {
        renderedChunkRooms = new EventList<ChunkRoomVisual>();
    }

    private ChunkRoomVisual()
    {
        doorInstances = new List<DoorData>();
        tileInstances = new List<GameObject>();
        interactableInstances = new List<Interactable>();
        worldResourceInstances = new List<WorldResource>();
        shouldRender = new EventVariable<ChunkRoomVisual, bool>(this, false);
        wallsMaterial = new EventVariable<ChunkRoomVisual, Material>(this, null);
        wallsColorMaterial = new EventVariable<ChunkRoomVisual, Material>(this, null);
        ceilingMaterial = new EventVariable<ChunkRoomVisual, Material>(this, null);
        floorMaterial = new EventVariable<ChunkRoomVisual, Material>(this, null);
    }

    protected override void OnEnableSub()
    {
        shouldRender.onValueChangeImmediate += OnValueChanged_ShouldRender;
    }

    protected override void OnValueChanged_Data(ChunkRoom oldValue, ChunkRoom newValue)
    {
        if (oldValue != null)
        {
            oldValue.meshReady.onValueChange -= OnValueChanged_MeshReady;
            oldValue.anyDoorOpen.onValueChange -= OnValueChanged_AnyDoorOpen;
            oldValue.roomColor.value = 0;
        }

        if (newValue != null)
        {
            newValue.meshReady.onValueChange += OnValueChanged_MeshReady;
            newValue.anyDoorOpen.onValueChange += OnValueChanged_AnyDoorOpen;
        }

        Check_ShouldRender();
    }

    private void OnValueChanged_AnyDoorOpen(bool oldValue, bool newValue)
    {
        Check_ShouldRender();
    }

    private void OnValueChanged_MeshReady(bool oldValue, bool newValue)
    {
        Check_ShouldRender();
    }

    private void OnValueChanged_ShouldRender(bool oldValue, bool newValue)
    {
        if (oldValue)
        {
            foreach (GameObject instance in tileInstances)
                GameObject.Destroy(instance);

            foreach (DoorData instance in doorInstances)
                GameObject.Destroy(instance.gameObject);

            foreach (WorldResource instance in worldResourceInstances)
                GameObject.Destroy(instance.gameObject);

            foreach (Interactable instance in interactableInstances)
                GameObject.Destroy(instance.gameObject);

            data.roomContents.onAdd -= OnAdd_RoomContent;
            data.roomContents.onRemove -= OnRemove_RoomContent;
            data.roomColor.onValueChange -= OnValueChanged_RoomColor;

            renderedChunkRooms.onRemove -= OnRemove_RenderedChunkRoom;
            renderedChunkRooms.Remove(this);
            renderedChunkRoomsIndex = 0;
            data.roomColor.value = 0;

            foreach (RoomDoor roomDoor in data.roomDoors)
                roomDoor.visualizeCount.value--;

            doorInstances.Clear();
            interactableInstances.Clear();
            tileInstances.Clear();
            worldResourceInstances.Clear();

            GameObject.Destroy(wallsMaterial.value);
            GameObject.Destroy(ceilingMaterial.value);
            GameObject.Destroy(floorMaterial.value);
            GameObject.Destroy(wallsColorMaterial.value);

            wallsMaterial.value = null;
            ceilingMaterial.value = null;
            floorMaterial.value = null;
            wallsColorMaterial.value = null;
        }

        if (newValue)
        {
            RoomMaterialsRecord roomMaterials = data.roomRecord.RoomMaterials;
            wallsMaterial.value = new Material(roomMaterials.Walls.Material);
            ceilingMaterial.value = new Material(roomMaterials.Ceiling.Material);
            floorMaterial.value = new Material(roomMaterials.Floor.Material);
            wallsColorMaterial.value = new Material(baseWallsColorMaterial);

            CreateRoomTiles();

            foreach (WorldResourceSpawn worldResourceSpawn in data.worldResourceSpawns)
                worldResourceInstances.Add(worldResourceSpawn.CreateInstance(tilesContainer));

            foreach (RoomContent roomContent in data.roomContents)
                OnAdd_RoomContent(roomContent);

            foreach (RoomDoor roomDoor in data.roomDoors)
                roomDoor.visualizeCount.value++;

            renderedChunkRoomsIndex = renderedChunkRooms.Count;
            data.roomColor.value = (renderedChunkRoomsIndex + 1) / 255f;
            renderedChunkRooms.Add(this);
            renderedChunkRooms.onRemove += OnRemove_RenderedChunkRoom;

            data.roomColor.onValueChangeImmediate += OnValueChanged_RoomColor;
            data.roomContents.onAdd += OnAdd_RoomContent;
            data.roomContents.onRemove += OnRemove_RoomContent;
        }
    }

    private void OnValueChanged_RoomColor(float oldValue, float newValue)
    {
        wallsMaterial.value.SetFloat(PROPERTY_ROOMCOLOR, newValue);
        ceilingMaterial.value.SetFloat(PROPERTY_ROOMCOLOR, newValue);
        floorMaterial.value.SetFloat(PROPERTY_ROOMCOLOR, newValue);
        wallsColorMaterial.value.SetFloat(PROPERTY_ROOMCOLOR, newValue);
    }

    private void OnRemove_RenderedChunkRoom(ChunkRoomVisual item)
    {
        if (item.renderedChunkRoomsIndex > renderedChunkRoomsIndex)
            return;

        renderedChunkRoomsIndex--;
        data.roomColor.value = (renderedChunkRoomsIndex + 1) / 255f;
    }

    private void OnAdd_RoomContent(RoomContent item)
    {
        Interactable instance = item.CreateInteractable(tilesContainer);
        instance.chunkRoom.value = data;
        interactableInstances.Add(instance);
    }

    private void OnRemove_RoomContent(RoomContent item)
    {
        Interactable instance = interactableInstances.Find(i => item.EqualsVisual(i));
        interactableInstances.Remove(instance);
        GameObject.Destroy(instance.gameObject);
    }

    private void Check_ShouldRender()
    {
        if (data == null)
        {
            shouldRender.value = false;
            return;
        }

        if (data.playersInRoom.value == 0 && !data.anyDoorOpen.value)
        {
            shouldRender.value = false;
            return;
        }

        if (!data.meshReady.value)
        {
            shouldRender.value = false;
            return;
        }

        shouldRender.value = true;
    }

    private void CreateRoomTiles()
    {
        int xMin = int.MaxValue;
        int xMax = int.MinValue;
        int yMin = int.MaxValue;
        int yMax = int.MinValue;

        foreach (Point point in data.chunkPoints)
        {
            xMin = Mathf.Min(xMin, point.xIndex);
            xMax = Mathf.Max(xMax, point.xIndex);
            yMin = Mathf.Min(yMin, point.yIndex);
            yMax = Mathf.Max(yMax, point.yIndex);
        }

        int xSize = xMax - xMin + 1;
        int ySize = yMax - yMin + 1;

        roomLayout = new bool[xSize, ySize];

        foreach (Point point in data.chunkPoints)
            roomLayout[point.xIndex - xMin, point.yIndex - yMin] = true;

        xOffset = xMin;
        yOffset = yMin;

        foreach (Point point in data.chunkPoints)
        {
            if (HasWall(point, CardinalDirection.Left))
            {
                GameObject wallInstance = GameObject.Instantiate(wallTile, tilesContainer);
                wallInstance.transform.position = new Vector3((point.xIndex - 0.5f) * roomWidth, roomHeight / 2, point.yIndex * roomWidth);
                wallInstance.transform.localScale = new Vector3(roomWidth, roomHeight, 1);
                wallInstance.transform.rotation = Quaternion.Euler(0, 270, 0);
                tileInstances.Add(wallInstance);
            }

            if (HasWall(point, CardinalDirection.Top))
            {
                GameObject wallInstance = GameObject.Instantiate(wallTile, tilesContainer);
                wallInstance.transform.position = new Vector3(point.xIndex * roomWidth, roomHeight / 2, (point.yIndex + 0.5f) * roomWidth);
                wallInstance.transform.localScale = new Vector3(roomWidth, roomHeight, 1);
                wallInstance.transform.rotation = Quaternion.Euler(0, 0, 0);
                tileInstances.Add(wallInstance);
            }

            if (HasWall(point, CardinalDirection.Right))
            {
                GameObject wallInstance = GameObject.Instantiate(wallTile, tilesContainer);
                wallInstance.transform.position = new Vector3((point.xIndex + 0.5f) * roomWidth, roomHeight / 2, point.yIndex * roomWidth);
                wallInstance.transform.localScale = new Vector3(roomWidth, roomHeight, 1);
                wallInstance.transform.rotation = Quaternion.Euler(0, 90, 0);
                tileInstances.Add(wallInstance);
            }

            if (HasWall(point, CardinalDirection.Bottom))
            {
                GameObject wallInstance = GameObject.Instantiate(wallTile, tilesContainer);
                wallInstance.transform.position = new Vector3(point.xIndex * roomWidth, roomHeight / 2, (point.yIndex - 0.5f) * roomWidth);
                wallInstance.transform.localScale = new Vector3(roomWidth, roomHeight, 1);
                wallInstance.transform.rotation = Quaternion.Euler(0, 180, 0);
                tileInstances.Add(wallInstance);
            }

            GameObject floorInstance = GameObject.Instantiate(floorTile, tilesContainer);
            floorInstance.transform.position = new Vector3(point.xIndex * roomWidth, 0, point.yIndex * roomWidth);
            floorInstance.transform.localScale = new Vector3(roomWidth, roomWidth, 1);
            tileInstances.Add(floorInstance);

            GameObject ceilingInstance = GameObject.Instantiate(ceilingTile, tilesContainer);
            ceilingInstance.transform.position = new Vector3(point.xIndex * roomWidth, roomHeight, point.yIndex * roomWidth);
            ceilingInstance.transform.localScale = new Vector3(roomWidth, roomWidth, 1);
            tileInstances.Add(ceilingInstance);
        }

        foreach (RoomDoor roomDoor in data.roomDoors)
        {
            Point roomPoint = roomDoor.GetEntryPoint(data);
            CardinalDirection direction = roomDoor.GetRoomDirection(data);
            DoorData doorInstance = GameObject.Instantiate(doorTile, tilesContainer);
            doorInstance.fromChunkRoom.value = data;
            doorInstance.toChunkRoom.value = roomDoor.GetExitChunk(data);
            switch (direction)
            {
                case CardinalDirection.Bottom:
                    {
                        doorInstance.transform.position = new Vector3(roomPoint.xIndex * roomWidth, 0, (roomPoint.yIndex - 0.5f) * roomWidth);
                        doorInstance.transform.localScale = new Vector3(roomWidth, roomHeight, 1);
                        doorInstance.transform.rotation = Quaternion.Euler(0, 180, 0);
                        break;
                    }
                case CardinalDirection.Left:
                    {
                        doorInstance.transform.position = new Vector3((roomPoint.xIndex - 0.5f) * roomWidth, 0, roomPoint.yIndex * roomWidth);
                        doorInstance.transform.localScale = new Vector3(roomWidth, roomHeight, 1);
                        doorInstance.transform.rotation = Quaternion.Euler(0, 270, 0);
                        break;
                    }
                case CardinalDirection.Right:
                    {
                        doorInstance.transform.position = new Vector3((roomPoint.xIndex + 0.5f) * roomWidth, 0, roomPoint.yIndex * roomWidth);
                        doorInstance.transform.localScale = new Vector3(roomWidth, roomHeight, 1);
                        doorInstance.transform.rotation = Quaternion.Euler(0, 90, 0);
                        break;
                    }
                case CardinalDirection.Top:
                    {
                        doorInstance.transform.position = new Vector3(roomPoint.xIndex * roomWidth, 0, (roomPoint.yIndex + 0.5f) * roomWidth);
                        doorInstance.transform.localScale = new Vector3(roomWidth, roomHeight, 1);
                        doorInstance.transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    }
            }
            doorInstances.Add(doorInstance);
        }
    }

    public bool HasWall(Point point, CardinalDirection direction)
    {
        int xCheck = point.xIndex - xOffset;
        int yCheck = point.yIndex - yOffset;

        switch(direction)
        {
            case CardinalDirection.Top:
                yCheck++;
                break;
            case CardinalDirection.Right:
                xCheck++;
                break;
            case CardinalDirection.Bottom:
                yCheck--;
                break;
            case CardinalDirection.Left:
                xCheck--;
                break;
        }

        Point checkPoint = new Point(xCheck + xOffset, yCheck + yOffset);
        bool anyFound = false;
        foreach(RoomDoor roomDoor in data.roomDoors)
        {
            if(roomDoor.firstRoomPoint.Equals(point) && roomDoor.secondRoomPoint.Equals(checkPoint))
            {
                anyFound = true;
                break;
            }

            if (roomDoor.firstRoomPoint.Equals(checkPoint) && roomDoor.secondRoomPoint.Equals(point))
            {
                anyFound = true;
                break;
            }
        }

        if (anyFound)
            return false;

        if (xCheck < 0)
            return true;

        if (xCheck >= roomLayout.GetLength(0))
            return true;

        if (yCheck < 0)
            return true;

        if (yCheck >= roomLayout.GetLength(1))
            return true;

        return !roomLayout[xCheck, yCheck];
    }

    protected override void OnDisableSub()
    {
        shouldRender.onValueChangeImmediate -= OnValueChanged_ShouldRender;
    }
}