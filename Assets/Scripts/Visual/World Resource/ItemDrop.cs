
using SheetCodes;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private float flyOffMinDistance;
    [SerializeField] private float flyOffMaxDistance;
    [SerializeField] private float flyOffAngle;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private MeshRenderer meshRenderer;

    private Inventory inventory;
    private ItemRecord record;
    private ItemPickupLocation itemPickupLocation;
    private Vector3 startPosition;
    private Vector3 flyOffPosition;
    private float lifeTime;
    private float lifeTimeRemaining;

    private int itemCount;

    public void Initialize(ItemRecord record, Vector3 startPosition, ItemPickupLocation itemPickupLocation, int itemCount)
    {
        this.record = record;
        this.startPosition = startPosition;
        this.itemPickupLocation = itemPickupLocation;
        this.itemCount = itemCount;

        meshRenderer.sharedMaterial = record.Material;
        float flyOffDistance = Random.Range(flyOffMinDistance, flyOffMaxDistance);
        float flyOffAngleOffset = Random.Range(-flyOffAngle, flyOffAngle);
        float speed = Random.Range(minSpeed, maxSpeed);

        Vector3 directionFromEnd = startPosition - itemPickupLocation.transform.position;
        float horizontalMagnitude = Mathf.Sqrt(directionFromEnd.x * directionFromEnd.x + directionFromEnd.z * directionFromEnd.z);
        directionFromEnd.y = 0;
        directionFromEnd = directionFromEnd.RotateAround(Vector3.up, flyOffAngleOffset);
        directionFromEnd.y = Random.value * horizontalMagnitude * 0.5f + 0.35f;
        directionFromEnd.Normalize();

        flyOffPosition = startPosition + directionFromEnd * flyOffDistance;

        float totalDistance = (startPosition - flyOffPosition).magnitude + (flyOffPosition - itemPickupLocation.transform.position).magnitude;
        lifeTime = totalDistance / speed;
        lifeTimeRemaining = lifeTime;
    }

    private void Update()
    {
        lifeTimeRemaining -= Time.deltaTime;
        if (lifeTimeRemaining <= 0)
        {
            itemPickupLocation.inventory.items[record.Identifier].AddItems(itemCount, true);
            GameObject.Destroy(gameObject);
            return;
        }

        float moveFactor = lifeTimeRemaining / lifeTime;

        Vector3 startToMidPosition = Vector3.Lerp(flyOffPosition, startPosition, moveFactor);
        Vector3 midToEndPosition = Vector3.Lerp(itemPickupLocation.transform.position, flyOffPosition, moveFactor);

        Vector3 worldPosition = startToMidPosition * moveFactor + midToEndPosition * (1 - moveFactor);
        transform.position = worldPosition;
    }
}