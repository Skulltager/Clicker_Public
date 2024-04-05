using SheetCodes;
using UnityEngine;

public class WorldResourceSpawn_Monster : WorldResourceSpawn
{
    public override float colliderCurrentRadius => record.ColliderBaseRadius + record.ColliderRadiusPerUnit * Mathf.Min(maxUnitDisplay, unitCountRemaining.eventStackValue);
    public override float colliderBaseRadius => record.ColliderBaseRadius;
    public override float colliderRadiusPerUnit => record.ColliderRadiusPerUnit;
    public override int maxHealth => record.HealthPerUnit;
    public override float maxUnitDisplay => 100;
    public override Sprite icon => record.Icon;
    public override string name => record.Name;
    public override Texture miniMapIcon => record.MinimapIcon;

    public readonly EnemyRecord record;

    public WorldResourceSpawn_Monster(ChunkRoom chunkRoom, EnemyRecord record, int unitCount, float respawnTime)
        :base(chunkRoom, unitCount, respawnTime)
    {
        this.record = record;
        healthRemaining.value = record.HealthPerUnit;
        lastInteractionTime.value = float.MinValue;
    }

    protected override void ResetHealth()
    {
        healthRemaining.value = record.HealthPerUnit;
    }

    public override WorldResource CreateInstance(Transform parent)
    {
        CheckRespawn(); 
        WorldResource_Monster instance = GameObject.Instantiate(PrefabCollection.instance.MonsterPrefab, position.value, Quaternion.identity);
        instance.transform.parent = parent;
        instance.Initialize(this);
        return instance;
    }
}