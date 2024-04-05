using SheetCodes;
using UnityEngine;

public class WorldResourceSpawn_StaticObject : WorldResourceSpawn
{
    public override float colliderCurrentRadius => record.ColliderBaseRadius + record.ColliderRadiusPerUnit * Mathf.Min(maxUnitDisplay, unitCountRemaining.eventStackValue);
    public override float colliderBaseRadius => record.ColliderBaseRadius;
    public override float colliderRadiusPerUnit => record.ColliderRadiusPerUnit;
    public override int maxHealth => record.HealthPerUnit;
    public override float maxUnitDisplay => 100;
    public override Sprite icon => record.Icon;
    public override string name => record.Name;
    public override Texture miniMapIcon => record.MinimapIcon;

    public Quaternion currentRotation { private set; get; }
    public readonly WorldResourceRecord record;

    public WorldResourceSpawn_StaticObject(ChunkRoom chunkRoom, WorldResourceRecord worldResourceRecord, int unitCount, float respawnTime)
        : base(chunkRoom, unitCount, respawnTime)
    {
        this.record = worldResourceRecord;
        healthRemaining.value = worldResourceRecord.HealthPerUnit;
        lastInteractionTime.value = float.MinValue;
    }

    protected override void ResetHealth()
    {
        healthRemaining.value = record.HealthPerUnit;
    }

    protected override bool TryCalculateNewSpawnPosition(out Vector3 position)
    {
        currentRotation = Quaternion.Euler(0, Random.value * 360, 0);
        return base.TryCalculateNewSpawnPosition(out position);
    }

    public override WorldResource CreateInstance(Transform parent)
    {
        CheckRespawn();
        WorldResource_StaticObject instance = GameObject.Instantiate(PrefabCollection.instance.StaticObjectPrefab, position.value, currentRotation);
        instance.transform.parent = parent;
        instance.Initialize(this);
        return instance;
    }
}