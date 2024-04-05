using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickupNotification : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Vector2 spawnPosition;
    [SerializeField] private Vector2 spawnOffsetPerNotification;
    [SerializeField] private Vector2 topPosition;
    [SerializeField] private Vector2 offsetPerNotification;
    [SerializeField] private float noChangeDeleteTimer;
    [SerializeField] private float moveFactor;
    [SerializeField] private float minMoveDistance;
    [SerializeField] private Image itemImage;
    [SerializeField] private Text notificationText;
    [SerializeField] private Text itemQualityText;

    [SerializeField] private float fadeOutDuration;
    [SerializeField] private float fadeInDuration;

    public RectTransform rectTransform => transform as RectTransform;
    public InventoryItem inventoryItem { private set; get; }
    public int notificationIndex { private set; get; }

    private long itemsFound;
    private float timeBeforeFading;
    private Vector2 targetPosition;

    public event Action<ItemPickupNotification> onDestroyed;

    public void Initialize(InventoryItem inventoryItem, long difference, int index)
    {
        this.inventoryItem = inventoryItem;
        notificationIndex = index;
        itemsFound = difference;

        inventoryItem.itemCount.onValueChange += OnValueChanged_ItemCount;
        inventoryItem.reservedCount.onValueChange += OnValueChanged_ReservedCount;
        rectTransform.anchoredPosition = spawnPosition + index * spawnOffsetPerNotification;
        timeBeforeFading = noChangeDeleteTimer;
        itemImage.sprite = inventoryItem.itemRecord.Icon;
        targetPosition = topPosition + index * offsetPerNotification;

        if (inventoryItem is QualityInventoryItem qualityInventoryItem)
            itemQualityText.text = qualityInventoryItem.qualityRecord.QualityText;
        else
            itemQualityText.text = "";

        SetNotificationText();
    }

    public void UpdateNotificationIndex(int index)
    {
        notificationIndex = index;
        targetPosition = topPosition + index * offsetPerNotification;
    }

    public void AddNotificationAmount(long value)
    {
        itemsFound += value;
        timeBeforeFading = noChangeDeleteTimer;
        SetNotificationText();
    }

    private void OnValueChanged_ItemCount(long oldValue, long newValue)
    {
        SetNotificationText();
    }

    private void OnValueChanged_ReservedCount(long oldValue, long newValue)
    {
        SetNotificationText();
    }

    private void SetNotificationText()
    {
        notificationText.text = string.Format("{0} ({1}) + {2}", inventoryItem.itemRecord.Name.ToUpper(), inventoryItem.availableCount, itemsFound);
    }

    private void Update()
    {
        timeBeforeFading -= Time.deltaTime;
        if(timeBeforeFading <= 0)
        {
            float timeBeforeDeleted = timeBeforeFading + fadeOutDuration;
            canvasGroup.alpha = timeBeforeDeleted / fadeOutDuration;

            if(timeBeforeDeleted <= 0)
            {
                GameObject.Destroy(gameObject);
                return;
            }
        }
        else
            canvasGroup.alpha = Mathf.Min(1, canvasGroup.alpha + 1 / fadeInDuration * Time.deltaTime);

        Vector2 difference = targetPosition - rectTransform.anchoredPosition;
        float distance = difference.magnitude;
        float moveAmount = Mathf.Min(distance, (distance * moveFactor + minMoveDistance) * Time.deltaTime);
        rectTransform.anchoredPosition += difference.normalized * moveAmount;
    }

    private void OnDestroy()
    {
        if (onDestroyed != null)
            onDestroyed(this);

        inventoryItem.itemCount.onValueChange -= OnValueChanged_ItemCount;
        inventoryItem.reservedCount.onValueChange -= OnValueChanged_ReservedCount;
    }
}