using SheetCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomDoor
{
    private const int DOOR_OPENING_CELL_DISTANCE = 2;
    private static int[,] CELL_DISTANCES;

    public readonly List<Point> doorOpenCells;
    public ChunkRoom firstRoom;
    public Point firstRoomPoint;

    public ChunkRoom secondRoom;
    public Point secondRoomPoint;

    private DoorVisual visualPrefab;
    private DoorVisual instance;

    public readonly EventVariable<RoomDoor, bool> doorOpen;
    public readonly EventVariable<RoomDoor, int> visualizeCount;

    private CardinalDirection direction;

    static RoomDoor()
    {
        CELL_DISTANCES = new int[DOOR_OPENING_CELL_DISTANCE * 2 + 2, DOOR_OPENING_CELL_DISTANCE * 2 + 2];
        for(int i = 0; i < CELL_DISTANCES.GetLength(0); i++)
        {
            for(int j = 0; j < CELL_DISTANCES.GetLength(1); j++)
            {
                CELL_DISTANCES[i, j] = int.MaxValue;
            }
        }
    }

    public RoomDoor(ChunkRoom firstRoom, Point firstRoomPoint, ChunkRoom secondRoom, Point secondRoomPoint)
    {
        this.firstRoom = firstRoom;
        this.firstRoomPoint = firstRoomPoint;
        this.secondRoom = secondRoom;
        this.secondRoomPoint = secondRoomPoint;
        doorOpenCells = new List<Point>();
        doorOpen = new EventVariable<RoomDoor, bool>(this, false);
        visualizeCount = new EventVariable<RoomDoor, int>(this, 0);
        visualizeCount.onValueChange += OnValueChanged_VisualizeCount;

        visualPrefab = PrefabCollectionIdentifier.Prefabs.GetRecord().DoorVisual;

        if (firstRoomPoint.xIndex < secondRoomPoint.xIndex)
            direction = CardinalDirection.Right;
        else if (firstRoomPoint.xIndex > secondRoomPoint.xIndex)
            direction = CardinalDirection.Left;
        else if (firstRoomPoint.yIndex < secondRoomPoint.yIndex)
            direction = CardinalDirection.Top;
        else
            direction = CardinalDirection.Bottom;
    }

    private void GetCellRange()
    {
        doorOpenCells.Clear();
        Queue<Point> pointsToHandle = new Queue<Point>();
        Point offset = default;

        doorOpenCells.Add(firstRoomPoint);
        doorOpenCells.Add(secondRoomPoint);
        pointsToHandle.Enqueue(new Point(DOOR_OPENING_CELL_DISTANCE, DOOR_OPENING_CELL_DISTANCE));
        CELL_DISTANCES[DOOR_OPENING_CELL_DISTANCE, DOOR_OPENING_CELL_DISTANCE] = 1;
        switch (direction)
        {
            case CardinalDirection.Bottom:
                offset = new Point(firstRoomPoint.xIndex - DOOR_OPENING_CELL_DISTANCE, firstRoomPoint.yIndex - DOOR_OPENING_CELL_DISTANCE - 1);
                pointsToHandle.Enqueue(new Point(DOOR_OPENING_CELL_DISTANCE, DOOR_OPENING_CELL_DISTANCE + 1));
                CELL_DISTANCES[DOOR_OPENING_CELL_DISTANCE, DOOR_OPENING_CELL_DISTANCE + 1] = 1;
                break;

            case CardinalDirection.Top:
                offset = new Point(firstRoomPoint.xIndex - DOOR_OPENING_CELL_DISTANCE, firstRoomPoint.yIndex - DOOR_OPENING_CELL_DISTANCE);
                pointsToHandle.Enqueue(new Point(DOOR_OPENING_CELL_DISTANCE, DOOR_OPENING_CELL_DISTANCE + 1));
                CELL_DISTANCES[DOOR_OPENING_CELL_DISTANCE, DOOR_OPENING_CELL_DISTANCE + 1] = 1;
                break;
            case CardinalDirection.Left:
                offset = new Point(firstRoomPoint.xIndex - DOOR_OPENING_CELL_DISTANCE - 1, firstRoomPoint.yIndex - DOOR_OPENING_CELL_DISTANCE);
                pointsToHandle.Enqueue(new Point(DOOR_OPENING_CELL_DISTANCE + 1, DOOR_OPENING_CELL_DISTANCE));
                CELL_DISTANCES[DOOR_OPENING_CELL_DISTANCE + 1, DOOR_OPENING_CELL_DISTANCE] = 1;
                break;
            case CardinalDirection.Right:
                offset = new Point(firstRoomPoint.xIndex - DOOR_OPENING_CELL_DISTANCE, firstRoomPoint.yIndex - DOOR_OPENING_CELL_DISTANCE);
                pointsToHandle.Enqueue(new Point(DOOR_OPENING_CELL_DISTANCE + 1, DOOR_OPENING_CELL_DISTANCE));
                CELL_DISTANCES[DOOR_OPENING_CELL_DISTANCE + 1, DOOR_OPENING_CELL_DISTANCE] = 1;
                break;
        }

        while (pointsToHandle.Count > 0)
        {
            Point localPointToHandle = pointsToHandle.Dequeue();
            Point globalPointToHandle = localPointToHandle + offset;
            ChunkRoom handleChunkRoom = firstRoom.GetChunkRoom(globalPointToHandle);
            int distance = CELL_DISTANCES[localPointToHandle.xIndex, localPointToHandle.yIndex] + 1;

            HandlePoint(pointsToHandle, doorOpenCells, handleChunkRoom, globalPointToHandle, offset, distance, CardinalDirection.Left);
            HandlePoint(pointsToHandle, doorOpenCells, handleChunkRoom, globalPointToHandle, offset, distance, CardinalDirection.Right);
            HandlePoint(pointsToHandle, doorOpenCells, handleChunkRoom, globalPointToHandle, offset, distance, CardinalDirection.Bottom);
            HandlePoint(pointsToHandle, doorOpenCells, handleChunkRoom, globalPointToHandle, offset, distance, CardinalDirection.Top);
        }

        foreach (Point pointInRange in doorOpenCells)
        {
            Point adjustedPointInRange = pointInRange - offset;
            CELL_DISTANCES[adjustedPointInRange.xIndex, adjustedPointInRange.yIndex] = int.MaxValue;
        }
    }

    private void HandlePoint(Queue<Point> pointsToHandle, List<Point> pointsInRange, ChunkRoom handleRoom, Point globalPoint, Point offset, int distance, CardinalDirection direction)
    {
        Point adjustedGlobalPoint = globalPoint.AddDirection(direction);
        Point adjustedLocalPoint = adjustedGlobalPoint - offset;
        int adjustedDistance = CELL_DISTANCES[adjustedLocalPoint.xIndex, adjustedLocalPoint.yIndex];

        if (adjustedDistance <= distance)
            return;

        ChunkRoom adjustedChunkRoom = handleRoom.GetChunkRoom(adjustedGlobalPoint);
        if (handleRoom != adjustedChunkRoom && !handleRoom.roomDoors.Any(i => i.MatchesDoor(globalPoint, adjustedGlobalPoint)))
            return;

        if (CELL_DISTANCES[adjustedLocalPoint.xIndex, adjustedLocalPoint.yIndex] == int.MaxValue)
            pointsInRange.Add(adjustedGlobalPoint);

        CELL_DISTANCES[adjustedLocalPoint.xIndex, adjustedLocalPoint.yIndex] = distance;
        if (distance <= DOOR_OPENING_CELL_DISTANCE)
            pointsToHandle.Enqueue(adjustedLocalPoint);
    }

    private void OnValueChanged_VisualizeCount(int oldValue, int newValue)
    {
        if (oldValue > 0 && newValue == 0)
        {
            if(!instance.IsDestroyed())
                GameObject.Destroy(instance.gameObject);
            instance = null;
        }

        if (oldValue == 0 && newValue > 0)
        {
            GetCellRange();
            instance = GameObject.Instantiate(visualPrefab);
            switch (direction)
            {
                case CardinalDirection.Bottom:
                case CardinalDirection.Top:
                    instance.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
            }
            Vector3 firstRoomPosition = new Vector3(firstRoomPoint.xIndex, 0, firstRoomPoint.yIndex) * 2;
            Vector3 secondRoomPosition = new Vector3(secondRoomPoint.xIndex, 0, secondRoomPoint.yIndex) * 2;
            instance.transform.localScale = new Vector3(2, 2, 2);
            instance.transform.position = (firstRoomPosition + secondRoomPosition) / 2;
            instance.data = this;
        }
    }

    public CardinalDirection GetRoomDirection(ChunkRoom sourceRoom)
    {
        if (firstRoom == sourceRoom)
            return direction;

        switch (direction)
        {
            case CardinalDirection.Bottom:
                return CardinalDirection.Top;
            case CardinalDirection.Top:
                return CardinalDirection.Bottom;
            case CardinalDirection.Left:
                return CardinalDirection.Right;
            case CardinalDirection.Right:
                return CardinalDirection.Left;
        }

        throw new EntryPointNotFoundException("Missing something weird");
    }

    public Point GetExitPoint(ChunkRoom from)
    {
        return from == firstRoom ? secondRoomPoint : firstRoomPoint;
    }

    public Point GetEntryPoint(ChunkRoom from)
    {
        return from == firstRoom ? firstRoomPoint : secondRoomPoint;
    }

    public ChunkRoom GetExitChunk(ChunkRoom from)
    {
        return from == firstRoom ? secondRoom : firstRoom;
    }

    public ChunkRoom GetEntryChunk(ChunkRoom from)
    {
        return from == firstRoom ? firstRoom : secondRoom;
    }

    public (ChunkRoom, Point) GetDoorEntrance(ChunkRoom from)
    {
        return from == firstRoom ? (secondRoom, secondRoomPoint) : (firstRoom, firstRoomPoint);
    }

    public bool MatchesDoor(Point firstPoint, Point secondPoint)
    {
        if (firstRoomPoint != firstPoint && firstRoomPoint != secondPoint)
            return false;


        if (secondRoomPoint != firstPoint && secondRoomPoint != secondPoint)
            return false;

        return true;
    }
}