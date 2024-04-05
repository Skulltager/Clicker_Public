using UnityEngine;
using SheetCodes;
using System.Collections.Generic;

public class WorldResource_StaticObject : WorldResource
{
    [SerializeField] private Clickable[] clickables;
    [SerializeField] private Transform clickablesContainer;
    [SerializeField] private Transform collidersContainer;

    private new WorldResourceSpawn_StaticObject worldResourceSpawn;
    public WorldResourceRecord record => worldResourceSpawn.record;

    public override string resourceName => record.Name;

    private WorldResource_StaticObject()
        : base() { }

    public void Initialize(WorldResourceSpawn_StaticObject worldResourceSpawn)
    {
        this.worldResourceSpawn = worldResourceSpawn;
        Initialize(worldResourceSpawn, record.Material);
        foreach (Clickable clickable in clickables)
            clickable.Initialize(material);
    }

    protected override void OnValueChanged_CurrentUnitCount(int oldValue, int newValue)
    {
        int clampedNewValue = Mathf.Min(newValue, record.MaxUnitScaling);

        float newScale = (record.ColliderBaseRadius + clampedNewValue * record.ColliderRadiusPerUnit) * 2;
        collidersContainer.localScale = new Vector3(newScale, 1, newScale);

        float horizontalScale = record.BaseHorizontalScale + record.HorizontalScalePerUnit * clampedNewValue;
        float verticalScale = record.BaseVerticalScale + record.VerticalScalePerUnit * clampedNewValue;

        clickablesContainer.localScale = new Vector3(horizontalScale, verticalScale, horizontalScale);
        
        int difference = oldValue - newValue;
        Dictionary<ItemDropRecord, int> itemDropAmount = new Dictionary<ItemDropRecord, int>();

        for (int i = 0; i < difference; i++)
        {
            foreach (ItemDropRecord itemDropRecord in record.ItemDrops)
            {
                if (Random.value > itemDropRecord.DropChance)
                    continue;

                int dropAmount = Random.Range(itemDropRecord.MinAmount, itemDropRecord.MaxAmount);
                if (itemDropAmount.ContainsKey(itemDropRecord))
                    itemDropAmount[itemDropRecord] += dropAmount;
                else
                    itemDropAmount.Add(itemDropRecord, dropAmount);
            }

            float horizontalOffset = (Random.value * 0.70f - 0.35f) * horizontalScale;
            float verticalOffset = Random.value * 0.70f * verticalScale;
            Vector3 offset = transform.rotation * new Vector3(horizontalOffset, verticalOffset, 0);
            if(Random.value < 0.5f)
                offset = Quaternion.Euler(0, 90, 0) * offset;
            
            Vector3 randomWorldPosition = transform.position + offset;
            ParticleController.PlayParticleInstance(ParticleIdentifier.RockHitParticle, randomWorldPosition);

            foreach (KeyValuePair<ItemDropRecord, int> keyValuePair in itemDropAmount)
            {
                Clickable instance = clickables[0];
                ItemDrop itemDropInstance = GameObject.Instantiate(itemDropPrefab);
                itemDropInstance.Initialize(keyValuePair.Key.Item, randomWorldPosition, lastHitItemPickupLocation, keyValuePair.Value);
            }

            itemDropAmount.Clear();
        }

        base.OnValueChanged_CurrentUnitCount(oldValue, newValue);
    }

    public override void Interact(PlayerInventory playerInventory, ItemPickupLocation itemPickupLocation)
    {
        int damage;
        if (!playerInventory.selectedToolBag.value.TryEquipItemCategory(record.ResourceSpawn.ResourceType.InteractableTools.Identifier))
        {
            if (record.ResourceSpawn.ToolRequired)
                return;

            damage = 1;
        }
        else
        {
            QualityInventoryItem equippedItem = playerInventory.selectedToolBag.value.equippedItemCategory.value.equippedItem.value;
            damage = equippedItem.statsRecord.Damage;
            equippedItem.Use();
        }

        TakeDamage(damage, itemPickupLocation);
    }

    protected override void OnEvent_Respawn()
    {
        transform.localRotation = worldResourceSpawn.currentRotation;
        base.OnEvent_Respawn();
    }
}