
using System;
using UnityEngine;

public abstract class WorldResourceSpawn
{
    public abstract float colliderCurrentRadius { get; }
    public abstract float colliderBaseRadius { get; }
    public abstract float colliderRadiusPerUnit { get; }
    public abstract float maxUnitDisplay { get; }
    public abstract int maxHealth { get; }
    public abstract Sprite icon { get; }
    public abstract string name { get; }
    public abstract Texture miniMapIcon { get; }

    public readonly EventVariable<WorldResourceSpawn, float> lastInteractionTime;
    public readonly EventVariable<WorldResourceSpawn, bool> isRespawning;
    public readonly EventVariable<WorldResourceSpawn, Vector3> position;

    public readonly float respawnTime;
    public readonly int unitCount;
    public readonly ChunkRoom chunkRoom;

    public readonly EventVariable<WorldResourceSpawn, int> healthRemaining;
    public readonly EventVariable<WorldResourceSpawn, int> unitCountRemaining;

    public event Action onRespawn;

    public WorldResourceSpawn(ChunkRoom chunkRoom, int unitCount, float respawnTime)
    {
        this.chunkRoom = chunkRoom;
        this.unitCount = unitCount;
        this.respawnTime = respawnTime;
        healthRemaining = new ControlledEventVariable<WorldResourceSpawn, int>(this, 0, Check_Health);
        isRespawning = new EventVariable<WorldResourceSpawn, bool>(this, true);
        unitCountRemaining = new EventVariable<WorldResourceSpawn, int>(this, 0);
        lastInteractionTime = new EventVariable<WorldResourceSpawn, float>(this, 0);
        position = new EventVariable<WorldResourceSpawn, Vector3>(this, Vector3.zero);
        unitCountRemaining.onValueChange += OnValueChanged_UnitCountRemaining;
    }

    private int Check_Health(int value)
    {
        if (value > 0)
            return value;

        int unitCountDead = Mathf.Min(unitCountRemaining.value, Mathf.Abs(value) / maxHealth + 1);
        unitCountRemaining.value -= unitCountDead;

        value += unitCountDead * maxHealth;
        value = Mathf.Max(0, value);
        return value;
    }

    private void OnValueChanged_UnitCountRemaining(int oldValue, int newValue)
    {
        isRespawning.value = newValue <= 0;
        lastInteractionTime.value = Mathf.Round(Time.time);
    }

    public void CheckRespawn()
    {
        if (lastInteractionTime.value + respawnTime > Time.time)
            return;

        if (unitCountRemaining.value > 0)
            return;

        if(!TryCalculateNewSpawnPosition(out Vector3 newSpawnPosition))
        {
            unitCountRemaining.value = 0;
            healthRemaining.value = 0;
            return;
        }

        lastInteractionTime.value = Mathf.Round(Time.time);
        unitCountRemaining.value = unitCount;
        ResetHealth();

        position.value = newSpawnPosition;

        if (onRespawn != null)
            onRespawn();
    }

    protected virtual bool TryCalculateNewSpawnPosition(out Vector3 position)
    {
        int layerMask = 1 << LayerMask.NameToLayer("World Moving");
        layerMask += 1 << LayerMask.NameToLayer("Environment");
        layerMask += 1 << LayerMask.NameToLayer("World Blocking");
        float spawnRadius = colliderBaseRadius + colliderRadiusPerUnit * Mathf.Min(maxUnitDisplay, unitCount);
        
        for (int i = 0; i < 5; i++)
        {
            Point randomPoint = chunkRoom.chunkPoints.GetRandomItem(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        
            float xMax = (randomPoint.xIndex + 0.5f) * 2;
            float xMin = (randomPoint.xIndex - 0.5f) * 2;
            float yMax = (randomPoint.yIndex + 0.5f) * 2;
            float yMin = (randomPoint.yIndex - 0.5f) * 2;
        
            if (chunkRoom.HasWallOrDoor(randomPoint, CardinalDirection.Top))
                yMax -= spawnRadius;
            if (chunkRoom.HasWallOrDoor(randomPoint, CardinalDirection.Right))
                xMax -= spawnRadius;
            if (chunkRoom.HasWallOrDoor(randomPoint, CardinalDirection.Bottom))
                yMin += spawnRadius;
            if (chunkRoom.HasWallOrDoor(randomPoint, CardinalDirection.Left))
                xMin += spawnRadius;
        
            float xPosition = UnityEngine.Random.Range(xMin, xMax);
            float yPosition = UnityEngine.Random.Range(yMin, yMax);
        
            position = new Vector3(xPosition, 0, yPosition);

            bool anyOverlapping = false;
            foreach(WorldResourceSpawn otherSpawn in chunkRoom.worldResourceSpawns)
            {
                if (otherSpawn == this)
                    continue;

                if (otherSpawn.unitCountRemaining.value == 0)
                    continue;

                float xDifference = position.x - otherSpawn.position.value.x;
                float zDifference = position.z - otherSpawn.position.value.z;

                float difference = Mathf.Sqrt(xDifference * xDifference + zDifference * zDifference);
                float totalRadius = spawnRadius + otherSpawn.colliderCurrentRadius;

                if (difference > totalRadius )
                    continue;

                anyOverlapping = true;
                break;
            }

            if (anyOverlapping)
                continue;

            if (Physics.CheckSphere(position, spawnRadius, layerMask))
                continue;
        
            return true;
        }

        position = default;
        return false;
    }

    protected abstract void ResetHealth();
    public abstract WorldResource CreateInstance(Transform parent);
}