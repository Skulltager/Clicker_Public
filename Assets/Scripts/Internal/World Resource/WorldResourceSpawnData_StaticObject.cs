using System;
using SheetCodes;

[Serializable]
public struct WorldResourceSpawnData_StaticObject
{
    public WorldResourceIdentifier identifier;
    public int minAmount;
    public int maxAmount;
    public float respawnTime;
}