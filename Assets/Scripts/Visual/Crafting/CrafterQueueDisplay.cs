using System.Collections.Generic;
using UnityEngine;

public class CrafterQueueDisplay : DataDrivenUI<Crafter>
{
    [SerializeField] private CrafterQueueItemDisplay crafterQueueItemDisplayPrefab;
    [SerializeField] private RectTransform crafterQueueItemContainer;

    private readonly List<CrafterQueueItemDisplay> crafterQueueItemDisplayInstances;

    private CrafterQueueDisplay()
        :base()
    {
        crafterQueueItemDisplayInstances = new List<CrafterQueueItemDisplay>();
    }

    protected override void OnValueChanged_Data(Crafter oldValue, Crafter newValue)
    {
        if (oldValue != null)
        {
            foreach (CrafterQueueItemDisplay instance in crafterQueueItemDisplayInstances)
                GameObject.Destroy(instance.gameObject);

            crafterQueueItemDisplayInstances.Clear();
            oldValue.craftingQueue.onAdd -= OnAdd_CraftingItem;
            oldValue.craftingQueue.onRemove -= OnRemove_CraftingItem;
        }

        if (newValue != null)
        {
            for (int i = 0; i < newValue.craftingQueue.Count; i++)
            {
                CrafterQueueItemDisplay instance = GameObject.Instantiate(crafterQueueItemDisplayPrefab, crafterQueueItemContainer);
                instance.data = new CrafterQueueItemDisplayData(data, i);
                crafterQueueItemDisplayInstances.Add(instance);
            }
            newValue.craftingQueue.onAdd += OnAdd_CraftingItem;
            newValue.craftingQueue.onRemove += OnRemove_CraftingItem;
        }
    }

    private void OnAdd_CraftingItem(CraftingItem item)
    {
        CrafterQueueItemDisplay instance = GameObject.Instantiate(crafterQueueItemDisplayPrefab, crafterQueueItemContainer);
        instance.data = new CrafterQueueItemDisplayData(data, item.index);
        crafterQueueItemDisplayInstances.Add(instance);
    }

    private void OnRemove_CraftingItem(CraftingItem item)
    {
        CrafterQueueItemDisplay instance = crafterQueueItemDisplayInstances[crafterQueueItemDisplayInstances.Count - 1];
        crafterQueueItemDisplayInstances.Remove(instance);
        GameObject.Destroy(instance.gameObject);
    }
}