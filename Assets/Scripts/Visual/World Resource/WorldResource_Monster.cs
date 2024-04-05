using UnityEngine;
using SheetCodes;
using System.Collections.Generic;

public class WorldResource_Monster : WorldResource
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform graphicsTransform;
    [SerializeField] private Transform collidersContainer;

    [SerializeField] private Transform monsterImagesContainer;
    [SerializeField] private Clickable clickablePrefab;

    private readonly List<Clickable> monsterImages;

    private new WorldResourceSpawn_Monster worldResourceSpawn;
    public EnemyRecord record => worldResourceSpawn.record;

    private float lastPhysicsTime;

    public override string resourceName => record.Name;

    private WorldResource_Monster()
        : base()
    {
        monsterImages = new List<Clickable>();
    }

    public void Initialize( WorldResourceSpawn_Monster worldResourceSpawn)
    {
        this.worldResourceSpawn = worldResourceSpawn;
        Initialize(worldResourceSpawn, record.Material);
    }

    protected override void OnValueChanged_CurrentUnitCount(int oldValue, int newValue)
    {
        base.OnValueChanged_CurrentUnitCount(oldValue, newValue);

        if (oldValue < newValue)
            OnValueChanged_CurrentUnitCountIncreased(oldValue, newValue);
        else
            OnValueChanged_CurrentUnitCountDecreased(oldValue, newValue, lastHitItemPickupLocation);

        float newScale = (record.ColliderBaseRadius + monsterImages.Count * worldResourceSpawn.record.ColliderRadiusPerUnit) * 2;
        collidersContainer.localScale = new Vector3(newScale, 1, newScale);
    }

    private void OnValueChanged_CurrentUnitCountIncreased(int oldValue, int newValue)
    {
        int clampedNewValue = Mathf.Min(newValue, record.MaxDisplayCount);

        while (monsterImages.Count < clampedNewValue)
        {
            Clickable instance = GameObject.Instantiate(clickablePrefab, monsterImagesContainer);
            instance.Initialize(material);
            Vector3 offset = Vector3.zero;
            if (monsterImages.Count >= 0)
            {
                float maxXOffset = record.MultiUnitXOffset + monsterImages.Count * record.MultiUnitXOffsetPerUnit;
                float maxZOffset = record.MultiUnitZOffset + monsterImages.Count * record.MultiUnitZOffsetPerUnit;
                offset.x = Random.Range(-maxXOffset, maxXOffset);
                offset.z = Random.Range(-maxZOffset, maxZOffset);
            }

            offset.y += record.HeightOffset;
            instance.transform.localScale = new Vector3(record.SpriteHorizontalScale, record.SpriteVerticalScale, record.SpriteHorizontalScale);
            instance.transform.localPosition = offset;
            monsterImages.Add(instance);
        }
    }

    private void OnValueChanged_CurrentUnitCountDecreased(int oldValue, int newValue, ItemPickupLocation itemPickupLocation)
    {
        int difference = oldValue - newValue;
        Dictionary<ItemDropRecord, int> itemDropAmount = new Dictionary<ItemDropRecord, int>();

        for (int i = 0; i < difference; i++)
        {
            float despawnChance = (float)monsterImages.Count / (oldValue - i);

            if (monsterImages.Count == 1 && oldValue - i > 1)
                despawnChance = 0;

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

            if (Random.value > despawnChance)
                continue;

            Clickable monsterInstance = monsterImages[monsterImages.Count - 1];

            foreach (KeyValuePair<ItemDropRecord, int> keyValuePair in itemDropAmount)
            {
                ItemDrop itemDropInstance = GameObject.Instantiate(itemDropPrefab);
                itemDropInstance.Initialize(keyValuePair.Key.Item, monsterInstance.transform.position, itemPickupLocation, keyValuePair.Value);
            }
            itemDropAmount.Clear();

            Vector3 particlePosition = monsterInstance.transform.position;
            particlePosition.y += record.DeathParticleHeightOffset;
            ParticleController.PlayParticleInstance(ParticleIdentifier.DeathParticle, particlePosition);
            GameObject.Destroy(monsterInstance.gameObject);
            monsterImages.RemoveAt(monsterImages.Count - 1);
        }

        foreach (KeyValuePair<ItemDropRecord, int> keyValuePair in itemDropAmount)
        {
            Clickable instance = monsterImages[monsterImages.Count - 1];
            ItemDrop itemDropInstance = GameObject.Instantiate(itemDropPrefab);
            itemDropInstance.Initialize(keyValuePair.Key.Item, instance.transform.position, itemPickupLocation, keyValuePair.Value);
        }
    }

    private void LateUpdate()
    {
        float physicsTimeDifference = Time.time - lastPhysicsTime;
        graphicsTransform.position = rigidBody.transform.position + (rigidBody.velocity * physicsTimeDifference);
        worldResourceSpawn.position.value = graphicsTransform.position;
    }

    private void FixedUpdate()
    {
        lastPhysicsTime = Time.time;
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
}