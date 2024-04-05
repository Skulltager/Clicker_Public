using System.Collections.Generic;
using UnityEngine;

public class HudCrafterQueueDisplay : DataDrivenUI<Crafter>
{
    [SerializeField] private HudCrafterQueueItemDisplay crafterQueueItemDisplayPrefab;
    [SerializeField] private RectTransform crafterQueueItemContainer;

    private readonly List<HudCrafterQueueItemDisplay> crafterQueueItemDisplayInstances;

    private HudCrafterQueueDisplay()
        : base()
    {
        crafterQueueItemDisplayInstances = new List<HudCrafterQueueItemDisplay>();
    }

    protected override void OnValueChanged_Data(Crafter oldValue, Crafter newValue)
    {
        if (oldValue != null)
        {
            foreach (HudCrafterQueueItemDisplay instance in crafterQueueItemDisplayInstances)
                GameObject.Destroy(instance.gameObject);

            crafterQueueItemDisplayInstances.Clear();
            oldValue.craftingQueue.onAdd -= OnAdd_CraftingItem;
            oldValue.craftingQueue.onRemove -= OnRemove_CraftingItem;
        }

        if (newValue != null)
        {
            for (int i = 0; i < newValue.craftingQueue.Count; i++)
            {
                HudCrafterQueueItemDisplay instance = GameObject.Instantiate(crafterQueueItemDisplayPrefab, crafterQueueItemContainer);
                instance.data = new HudCrafterQueueItemDisplayData(data, i);
                crafterQueueItemDisplayInstances.Add(instance);
            }
            newValue.craftingQueue.onAdd += OnAdd_CraftingItem;
            newValue.craftingQueue.onRemove += OnRemove_CraftingItem;
        }
    }

    private void OnAdd_CraftingItem(CraftingItem item)
    {
        HudCrafterQueueItemDisplay instance = GameObject.Instantiate(crafterQueueItemDisplayPrefab, crafterQueueItemContainer);
        instance.data = new HudCrafterQueueItemDisplayData(data, item.index);
        crafterQueueItemDisplayInstances.Add(instance);
    }

    private void OnRemove_CraftingItem(CraftingItem item)
    {
        HudCrafterQueueItemDisplay instance = crafterQueueItemDisplayInstances[crafterQueueItemDisplayInstances.Count - 1];
        crafterQueueItemDisplayInstances.Remove(instance);
        GameObject.Destroy(instance.gameObject);
    }
}