using SheetCodes;
using System.Collections.Generic;
using System.Linq;

public class ChunkRoom
{
    public static int CURRENT_INDEX = 1;
    public System.Random random => centerMapChunk.random;

    public readonly EventVariable<ChunkRoom, bool> meshReady;
    public readonly EventVariable<ChunkRoom, int> playersInRoom;
    public readonly EventVariable<ChunkRoom, bool> anyDoorOpen;
    public readonly EventVariable<ChunkRoom, bool> visited;
    public readonly EventVariable<ChunkRoom, float> roomColor;
    public readonly EventVariable<ChunkRoom, bool> unlocked;

    public readonly MapChunk centerMapChunk;
    public readonly List<Point> chunkPoints;
    public readonly RoomRecord roomRecord;
    public readonly EventList<RoomDoor> roomDoors;
    public readonly int roomIndex;
    public readonly bool fixedRoom;
    public readonly int roomDistance;
    public readonly WorldResourceSpawn[] worldResourceSpawns;
    public readonly EventList<RoomContent> roomContents;
    public Inventory inventory;
    public MinMaxVector2 bounds { private set; get; }
    public Point centerPoint;
    public Point centerCell;

    public ChunkRoomSet chunkSet;

    private readonly int biomeIndex;
    private int targetDoorCount;
    private List<MapChunk> mapChunks;
    
    public ChunkRoom()
    {
        //Use for dummy room
    }

    public ChunkRoom(MapChunk mapChunk, List<Point> chunkPoints, RoomRecord roomRecord, int biomeIndex, int roomDistance, bool fixedRoom)
    {
        roomIndex = CURRENT_INDEX;
        chunkSet = new ChunkRoomSet(this, CURRENT_INDEX);
        CURRENT_INDEX++;
        this.centerMapChunk = mapChunk;
        this.chunkPoints = chunkPoints;
        this.roomRecord = roomRecord;
        this.biomeIndex = biomeIndex;
        this.targetDoorCount = random.Next(this.roomRecord.MinDoors, this.roomRecord.MaxDoors + 1);
        this.fixedRoom = fixedRoom;

        roomDoors = new EventList<RoomDoor>();
        roomContents = new EventList<RoomContent>();
        playersInRoom = new EventVariable<ChunkRoom, int>(this, 0);
        anyDoorOpen = new EventVariable<ChunkRoom, bool>(this, false);
        meshReady = new EventVariable<ChunkRoom, bool>(this, false);
        visited = new EventVariable<ChunkRoom, bool>(this, false);
        roomColor = new EventVariable<ChunkRoom, float>(this, 0);
        unlocked = new EventVariable<ChunkRoom, bool>(this, false);
        roomDoors.onAdd += OnAdd_RoomDoor;
        roomDoors.onRemove += OnRemove_RoomDoor;
        playersInRoom.onValueChange += OnValueChanged_PlayersInRoom;
        visited.onValueChangeImmediate += OnValueChanged_Visited;

        int resourceCount = random.Next(roomRecord.MinResourceSpawns, roomRecord.MaxResourceSpawns + 1);
        worldResourceSpawns = new WorldResourceSpawn[resourceCount];

        WeightedRandomizer<ResourceTypeIdentifier> randomizer = new WeightedRandomizer<ResourceTypeIdentifier>();
        foreach (ResourceSpawnWeightRecord subRecord in roomRecord.ResourceSpawnWeights)
            randomizer.AddItem(subRecord.Type.Identifier, subRecord.Weight);

        for (int i = 0; i < resourceCount; i++)
        {
            ResourceTypeIdentifier resourceType = randomizer.GetRandomItem();
            float sizeFactor = roomRecord.ResourceSpawnCountMultiplier;
            worldResourceSpawns[i] = GameData.instance.GetResourceSpawn(this, resourceType, random, roomDistance, sizeFactor);
        }
    }

    public void CreateInventory()
    {
        if (inventory != null)
            return;

        inventory = new Inventory(new InventoryFilter(), false);
    }

    public ChunkRoom GetChunkRoom(Point point)
    {
        return centerMapChunk.GetChunkRoom(point);
    }

    private void OnValueChanged_Visited(bool oldValue, bool newValue)
    {
        if(newValue)
        {
            centerMapChunk.GenerateAdjacentChunks();
        }
    }

    private void OnAdd_RoomDoor(RoomDoor item)
    {
        item.doorOpen.onValueChangeImmediate += OnValueChanged_RoomDoor_DoorOpen;
    }

    private void OnRemove_RoomDoor(RoomDoor item)
    {
        item.doorOpen.onValueChangeImmediate -= OnValueChanged_RoomDoor_DoorOpen;
    }

    private void OnValueChanged_RoomDoor_DoorOpen(bool oldValue, bool newValue)
    {
        CheckAnyDoorOpen();
    }

    private void CheckAnyDoorOpen()
    {
        anyDoorOpen.value = roomDoors.Any(i => i.doorOpen.value);
    }

    private void OnValueChanged_PlayersInRoom(int oldValue, int newValue)
    {
        if (newValue > 0)
            visited.value = true;
    }

    public bool CheckRoomAdjacencyFinished()
    {
        foreach (Point point in chunkPoints)
        {
            if (centerMapChunk.GetChunkRoom(point.AddDirection(DiagonalDirection.CenterLeft)) == null)
                return false;

            if (centerMapChunk.GetChunkRoom(point.AddDirection(DiagonalDirection.CenterRight)) == null)
                return false;

            if (centerMapChunk.GetChunkRoom(point.AddDirection(DiagonalDirection.BottomCenter)) == null)
                return false;

            if (centerMapChunk.GetChunkRoom(point.AddDirection(DiagonalDirection.TopCenter)) == null)
                return false;
        }


        bounds = new MinMaxVector2(this);
        MapChunk[] mapChunksGrid = centerMapChunk.GetMapChunkRange(bounds);
        if (mapChunksGrid.Any(i => !i.generated))
            return false;

        MapChunk[] mapChunkDoors = roomDoors.Select(i => i.GetExitChunk(this).centerMapChunk).Distinct().ToArray();
        if (mapChunkDoors.Any(i => !i.generated))
            return false;

        List<MapChunk> allMapChunks = new List<MapChunk>(mapChunksGrid);
        allMapChunks.AddRange(mapChunkDoors);
        mapChunks = allMapChunks.Distinct().ToList();
        return true;
    }

    private void AddDoor(RoomDoor roomDoor)
    {
        if (mapChunks != null)
        {
            MapChunk endChunk = roomDoor.GetExitChunk(this).centerMapChunk;
            if (!mapChunks.Contains(endChunk))
                mapChunks.Add(endChunk);
        }
        roomDoors.Add(roomDoor);
    }

    public void GenerateRoomContents()
    {

    }

    public void Update()
    {
        //roomContent.Update();
    }

    public void SetMapIndices()
    {
        bounds = new MinMaxVector2(this);

        MapChunk[] mapChunksGrid = centerMapChunk.GetMapChunkRange(bounds);
        MapChunk[] mapChunkDoors = roomDoors.Select(i => i.GetExitChunk(this).centerMapChunk).Distinct().ToArray();

        List<MapChunk> allMapChunks = new List<MapChunk>(mapChunksGrid);
        allMapChunks.AddRange(mapChunkDoors);
        mapChunks = allMapChunks.Distinct().ToList();

        foreach (Point point in chunkPoints)
            centerMapChunk.SetMapChunkIndex(biomeIndex, roomIndex, point);
    }

    public List<ChunkRoom> GenerateInitialDoors()
    {
        List<ChunkRoom> newConnectedRooms = new List<ChunkRoom>();
        Dictionary<ChunkRoom, List<RoomDoor>> possibleDoors = new Dictionary<ChunkRoom, List<RoomDoor>>();

        foreach (Point point in chunkPoints)
        {
            List<RoomDoor> roomDoorList;
            Point targetPoint = point.AddDirection(DiagonalDirection.CenterLeft);
            ChunkRoom targetRoom = centerMapChunk.GetChunkRoom(targetPoint);

            if (!possibleDoors.TryGetValue(targetRoom, out roomDoorList))
            {
                roomDoorList = new List<RoomDoor>();
                possibleDoors.Add(targetRoom, roomDoorList);
            }
            roomDoorList.Add(new RoomDoor(this, point, targetRoom, targetPoint));

            targetPoint = point.AddDirection(DiagonalDirection.CenterRight);
            targetRoom = centerMapChunk.GetChunkRoom(targetPoint);

            if (!possibleDoors.TryGetValue(targetRoom, out roomDoorList))
            {
                roomDoorList = new List<RoomDoor>();
                possibleDoors.Add(targetRoom, roomDoorList);
            }
            roomDoorList.Add(new RoomDoor(this, point, targetRoom, targetPoint));

            targetPoint = point.AddDirection(DiagonalDirection.TopCenter);
            targetRoom = centerMapChunk.GetChunkRoom(targetPoint);

            if (!possibleDoors.TryGetValue(targetRoom, out roomDoorList))
            {
                roomDoorList = new List<RoomDoor>();
                possibleDoors.Add(targetRoom, roomDoorList);
            }
            roomDoorList.Add(new RoomDoor(this, point, targetRoom, targetPoint));

            targetPoint = point.AddDirection(DiagonalDirection.BottomCenter);
            targetRoom = centerMapChunk.GetChunkRoom(targetPoint);

            if (!possibleDoors.TryGetValue(targetRoom, out roomDoorList))
            {
                roomDoorList = new List<RoomDoor>();
                possibleDoors.Add(targetRoom, roomDoorList);
            }
            roomDoorList.Add(new RoomDoor(this, point, targetRoom, targetPoint));
        }

        possibleDoors.Remove(this);

        List<ChunkRoom> availableRooms = new List<ChunkRoom>();

        foreach(ChunkRoom chunkRoom in possibleDoors.Keys)
        {
            if (chunkRoom.roomDoors.Count >= chunkRoom.targetDoorCount)
                continue;

            if (roomDoors.Any(i => i.firstRoom == chunkRoom || i.secondRoom == chunkRoom))
                continue;

            availableRooms.Add(chunkRoom);
        }

        while (availableRooms.Count > 0 && roomDoors.Count < targetDoorCount)
        {
            int randomIndex = random.Next(availableRooms.Count);
            ChunkRoom adjacentRoom = availableRooms[randomIndex];
            availableRooms.RemoveAt(randomIndex);

            List<RoomDoor> doors = possibleDoors[adjacentRoom];

            randomIndex = random.Next(doors.Count);
            RoomDoor roomDoor = doors[randomIndex];
            AddDoor(roomDoor);
            adjacentRoom.AddDoor(roomDoor);

            newConnectedRooms.Add(adjacentRoom);
            if (chunkSet.setIndex == adjacentRoom.chunkSet.setIndex)
                continue;

            ChunkRoomSet targetSet = chunkSet.setIndex < adjacentRoom.chunkSet.setIndex ? chunkSet : adjacentRoom.chunkSet;
            ChunkRoomSet mergeSet = chunkSet.setIndex > adjacentRoom.chunkSet.setIndex ? chunkSet : adjacentRoom.chunkSet;

            targetSet.chunkRooms.AddRange(mergeSet.chunkRooms);
            foreach (ChunkRoom chunkRoom in mergeSet.chunkRooms)
                chunkRoom.chunkSet = targetSet;
        }

        return newConnectedRooms;
    }

    public bool HasWall(Point point, CardinalDirection direction)
    {
        Point checkPoint = default;
        switch (direction)
        {
            case CardinalDirection.Top:
                checkPoint = point.AddDirection(DiagonalDirection.TopCenter);
                break;
            case CardinalDirection.Right:
                checkPoint = point.AddDirection(DiagonalDirection.CenterRight);
                break;
            case CardinalDirection.Bottom:
                checkPoint = point.AddDirection(DiagonalDirection.BottomCenter);
                break;
            case CardinalDirection.Left:
                checkPoint = point.AddDirection(DiagonalDirection.CenterLeft);
                break;
        }

        bool anyFound = false;
        foreach(RoomDoor roomDoor in roomDoors)
        {
            if (!roomDoor.GetEntryPoint(this).Equals(point) || !roomDoor.GetExitPoint(this).Equals(checkPoint))
                continue;
            
            anyFound = true;
            break;
        }

        if (anyFound)
            return false;

        return centerMapChunk.GetChunkRoom(checkPoint) != this;
    }

    public bool HasWallOrDoor(Point point, CardinalDirection direction)
    {
        Point checkPoint = default;
        switch (direction)
        {
            case CardinalDirection.Top:
                checkPoint = point.AddDirection(DiagonalDirection.TopCenter);
                break;
            case CardinalDirection.Right:
                checkPoint = point.AddDirection(DiagonalDirection.CenterRight);
                break;
            case CardinalDirection.Bottom:
                checkPoint = point.AddDirection(DiagonalDirection.BottomCenter);
                break;
            case CardinalDirection.Left:
                checkPoint = point.AddDirection(DiagonalDirection.CenterLeft);
                break;
        }

        return centerMapChunk.GetChunkRoom(checkPoint) != this;
    }


    public bool TryAddRandomRoomConnection(out ChunkRoom newConnectedRoom)
    {
        // this means it's connected already so no need to add a door
        if (chunkSet.setIndex == 0)
        {
            newConnectedRoom = null;
            return false;
        }

        Dictionary<ChunkRoom, List<RoomDoor>> possibleDoors = new Dictionary<ChunkRoom, List<RoomDoor>>();

        foreach (Point point in chunkPoints)
        {
            List<RoomDoor> roomDoorList;
            Point targetPoint = point.AddDirection(DiagonalDirection.CenterLeft);
            ChunkRoom targetRoom = centerMapChunk.GetChunkRoom(targetPoint);

            if (!possibleDoors.TryGetValue(targetRoom, out roomDoorList))
            {
                roomDoorList = new List<RoomDoor>();
                possibleDoors.Add(targetRoom, roomDoorList);
            }
            roomDoorList.Add(new RoomDoor(this, point, targetRoom, targetPoint));

            targetPoint = point.AddDirection(DiagonalDirection.CenterRight);
            targetRoom = centerMapChunk.GetChunkRoom(targetPoint);

            if (!possibleDoors.TryGetValue(targetRoom, out roomDoorList))
            {
                roomDoorList = new List<RoomDoor>();
                possibleDoors.Add(targetRoom, roomDoorList);
            }
            roomDoorList.Add(new RoomDoor(this, point, targetRoom, targetPoint));

            targetPoint = point.AddDirection(DiagonalDirection.TopCenter);
            targetRoom = centerMapChunk.GetChunkRoom(targetPoint);

            if (!possibleDoors.TryGetValue(targetRoom, out roomDoorList))
            {
                roomDoorList = new List<RoomDoor>();
                possibleDoors.Add(targetRoom, roomDoorList);
            }
            roomDoorList.Add(new RoomDoor(this, point, targetRoom, targetPoint));

            targetPoint = point.AddDirection(DiagonalDirection.BottomCenter);
            targetRoom = centerMapChunk.GetChunkRoom(targetPoint);

            if (!possibleDoors.TryGetValue(targetRoom, out roomDoorList))
            {
                roomDoorList = new List<RoomDoor>();
                possibleDoors.Add(targetRoom, roomDoorList);
            }
            roomDoorList.Add(new RoomDoor(this, point, targetRoom, targetPoint));
        }

        possibleDoors.Remove(this);

        List<ChunkRoom> availableRooms = new List<ChunkRoom>();

        foreach (ChunkRoom chunkRoom in possibleDoors.Keys)
        {
            if (chunkSet.setIndex == chunkRoom.chunkSet.setIndex)
                continue;

            if (roomDoors.Any(i => i.firstRoom == chunkRoom || i.secondRoom == chunkRoom))
                continue;

            availableRooms.Add(chunkRoom);
        }

        if (availableRooms.Count == 0)
        {
            newConnectedRoom = null;
            return false;
        }

        int randomIndex = random.Next(availableRooms.Count);
        ChunkRoom adjacentRoom = availableRooms[randomIndex];
        availableRooms.RemoveAt(randomIndex);

        List<RoomDoor> doors = possibleDoors[adjacentRoom];

        randomIndex = random.Next(doors.Count);
        RoomDoor roomDoor = doors[randomIndex];
        AddDoor(roomDoor);
        adjacentRoom.AddDoor(roomDoor);

        ChunkRoomSet targetSet = chunkSet.setIndex < adjacentRoom.chunkSet.setIndex ? chunkSet : adjacentRoom.chunkSet;
        ChunkRoomSet mergeSet = chunkSet.setIndex > adjacentRoom.chunkSet.setIndex ? chunkSet : adjacentRoom.chunkSet;

        targetSet.chunkRooms.AddRange(mergeSet.chunkRooms);
        foreach (ChunkRoom chunkRoom in mergeSet.chunkRooms)
            chunkRoom.chunkSet = targetSet;

        newConnectedRoom = adjacentRoom;
        return true;
    }
}