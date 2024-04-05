using SheetCodes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance { private set; get; }
    private readonly Dictionary<ResourceSpawnIdentifier, WorldResourceRecord> worldResources;
    private readonly Dictionary<ResourceSpawnIdentifier, EnemyRecord> enemyResources;

    private readonly Dictionary<ResourceTypeIdentifier, List<ResourceSpawnRecord>> resourceSpawnCollections;
    private readonly Dictionary<CraftingCategoryIdentifier, List<CraftingRecipeRecord>> craftingRecipes;

    private GameData()
    {
        resourceSpawnCollections = new Dictionary<ResourceTypeIdentifier, List<ResourceSpawnRecord>>();
        craftingRecipes = new Dictionary<CraftingCategoryIdentifier, List<CraftingRecipeRecord>>();
        enemyResources = new Dictionary<ResourceSpawnIdentifier, EnemyRecord>();
        worldResources = new Dictionary<ResourceSpawnIdentifier, WorldResourceRecord>();
    }

    private void Awake()
    {
        instance = this;
        GenerateResourceSpawnData();
        GenerateCraftingCategoryRecipes();
    }

    private void GenerateResourceSpawnData()
    {
        ResourceTypeIdentifier[] resourceTypeIdentifiers = Enum.GetValues(typeof(ResourceTypeIdentifier)) as ResourceTypeIdentifier[];
        foreach (ResourceTypeIdentifier identifier in resourceTypeIdentifiers)
        {
            if (identifier == ResourceTypeIdentifier.None)
                continue;

            ResourceTypeRecord record = identifier.GetRecord();
            resourceSpawnCollections.Add(identifier, new List<ResourceSpawnRecord>());
        }

        ResourceSpawnIdentifier[] resourceSpawnIdentifiers = Enum.GetValues(typeof(ResourceSpawnIdentifier)) as ResourceSpawnIdentifier[];
        foreach (ResourceSpawnIdentifier identifier in resourceSpawnIdentifiers)
        {
            if (identifier == ResourceSpawnIdentifier.None)
                continue;

            ResourceSpawnRecord record = identifier.GetRecord();
            resourceSpawnCollections[record.ResourceType.Identifier].Add(record);
        }

        WorldResourceIdentifier[] worldResourceIdentifiers = Enum.GetValues(typeof(WorldResourceIdentifier)) as WorldResourceIdentifier[];
        foreach (WorldResourceIdentifier identifier in worldResourceIdentifiers)
        {
            if (identifier == WorldResourceIdentifier.None)
                continue;

            WorldResourceRecord record = identifier.GetRecord();
            worldResources.Add(record.ResourceSpawn.Identifier, record);
        }

        EnemyIdentifier[] enemyIdentifiers = Enum.GetValues(typeof(EnemyIdentifier)) as EnemyIdentifier[];
        foreach (EnemyIdentifier identifier in enemyIdentifiers)
        {
            if (identifier == EnemyIdentifier.None)
                continue;

            EnemyRecord record = identifier.GetRecord();
            enemyResources.Add(record.ResourceSpawn.Identifier, record);
        }
    }

    private void GenerateCraftingCategoryRecipes()
    {
        CraftingCategoryIdentifier[] craftingCategoryIdentifiers = Enum.GetValues(typeof(CraftingCategoryIdentifier)) as CraftingCategoryIdentifier[];
        foreach (CraftingCategoryIdentifier identifier in craftingCategoryIdentifiers)
        {
            if (identifier == CraftingCategoryIdentifier.None)
                continue;

            craftingRecipes.Add(identifier, new List<CraftingRecipeRecord>());
        }


        CraftingRecipeIdentifier[] craftingRecipeIdentifiers = Enum.GetValues(typeof(CraftingRecipeIdentifier)) as CraftingRecipeIdentifier[];
        foreach (CraftingRecipeIdentifier identifier in craftingRecipeIdentifiers)
        {
            if (identifier == CraftingRecipeIdentifier.None)
                continue;

            CraftingRecipeRecord record = identifier.GetRecord();
            craftingRecipes[record.CategoryCategory.Identifier].Add(record);
        }
    }

    public List<CraftingRecipeRecord> GetCategoryRecipes(CraftingCategoryIdentifier identifier)
    {
        return craftingRecipes[identifier];
    }

    public WorldResourceSpawn GetResourceSpawn(ChunkRoom chunkRoom, ResourceTypeIdentifier resourceType, System.Random random, int distance, float sizeFactor)
    {
        List<ResourceSpawnRecord> targetList = resourceSpawnCollections[resourceType];

        WeightedRandomizer<ResourceSpawnRecord> randomizer = new WeightedRandomizer<ResourceSpawnRecord>(random.Next());
        foreach (ResourceSpawnRecord record in targetList)
        {
            if (record.MinDistance > distance)
                continue;

            float distanceFactor = Mathf.Min(1, (float)(record.MaxDistance - record.MinDistance) / (distance - record.MinDistance));
            int weight = (int)Mathf.Lerp(record.MinDistanceSpawnWeight, record.MaxDistanceSpawnWeight, distanceFactor);
            randomizer.AddItem(record, weight);
        }

        ResourceSpawnRecord resourceSpawnRecord = randomizer.GetRandomItem();
        float distanceFact = Mathf.Pow(resourceSpawnRecord.DistanceAmountFactor, distance);
        int minAmount = (int)Mathf.Lerp(resourceSpawnRecord.MinDistanceMinAmount, resourceSpawnRecord.MinDistanceMaxAmount, distance);
        int maxAmount = (int)Mathf.Lerp(resourceSpawnRecord.MaxDistanceMinAmount, resourceSpawnRecord.MaxDistanceMaxAmount, distance);
        int size = (int)(random.Next(minAmount, maxAmount) * sizeFactor * distanceFact);

        if (enemyResources.TryGetValue(resourceSpawnRecord.Identifier, out EnemyRecord enemyRecord))
            return new WorldResourceSpawn_Monster(chunkRoom, enemyRecord, size, resourceSpawnRecord.RepawnTimer);
        else if (worldResources.TryGetValue(resourceSpawnRecord.Identifier, out WorldResourceRecord worldResourceRecord))
            return new WorldResourceSpawn_StaticObject(chunkRoom, worldResourceRecord, size, resourceSpawnRecord.RepawnTimer);

        throw new Exception("Missing an implementation for a world resource");
    }
}