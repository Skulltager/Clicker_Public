using System;
using SheetCodes;

[Serializable]
public struct WorldResourceSpawnData_Monster
{
    public EnemyIdentifier identifier;
    public int minAmount;
    public int maxAmount;
    public float respawnTime;
}