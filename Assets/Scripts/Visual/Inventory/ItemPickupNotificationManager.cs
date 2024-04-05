
using SheetCodes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class ItemPickupNotificationManager : MonoBehaviour
{
    [SerializeField] private ItemPickupNotification itemNotificationPrefab = default;
    [SerializeField] private RectTransform itemNotificationContainer = default;

    private readonly List<ItemPickupNotification> itemNotifications;

    public static ItemPickupNotificationManager instance { private set; get; }

    private ItemPickupNotificationManager()
    {
        itemNotifications = new List<ItemPickupNotification>();
    }

    private void Awake()
    {
        instance = this;
    }

    public void ShowItemNotification(InventoryItem item, long difference)
    {
        ItemPickupNotification instance = itemNotifications.Find(i => i.inventoryItem == item);
        if (instance != null)
        {
            instance.AddNotificationAmount(difference);
            return;
        }

        instance = GameObject.Instantiate(itemNotificationPrefab, itemNotificationContainer);
        instance.onDestroyed += OnEvent_NotificationDestroyed;
        instance.Initialize(item, difference, itemNotifications.Count);
        itemNotifications.Add(instance);
    }

    private void OnEvent_NotificationDestroyed(ItemPickupNotification instance)
    {
        itemNotifications.RemoveAt(instance.notificationIndex);
        instance.onDestroyed -= OnEvent_NotificationDestroyed;
        UpdateNotificationIndices();
    }

    private void UpdateNotificationIndices()
    {
        for (int i = 0; i < itemNotifications.Count; i++)
            itemNotifications[i].UpdateNotificationIndex(i);
    }
}