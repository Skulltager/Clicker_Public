using System;
using UnityEngine;

public abstract class WorldResource : Interactable
{
    [SerializeField] protected ItemDrop itemDropPrefab;
    [SerializeField] private GameObject contentContainer;

    public event Action onHit;
    public event Action onDestroyed;
    public abstract string resourceName { get; }
    public WorldResourceSpawn worldResourceSpawn { private set; get; }
    public Material material { private set; get; }

    protected ItemPickupLocation lastHitItemPickupLocation { private set; get; }

    private Texture2D mainTexture;

    protected void Initialize(WorldResourceSpawn worldResourceSpawn, Material material)
    {
        this.worldResourceSpawn = worldResourceSpawn;
        this.material = new Material(material);
        mainTexture = material.GetTexture("mainTex") as Texture2D;
        worldResourceSpawn.unitCountRemaining.onValueChangeImmediate += OnValueChanged_CurrentUnitCount;
        worldResourceSpawn.onRespawn += OnEvent_Respawn;

        transform.position = worldResourceSpawn.position.value;
        chunkRoom.value = worldResourceSpawn.chunkRoom;
        chunkRoom.onValueChangeImmediate += OnValueChanged_ChunkRoom;
    }

    public override bool CheckHitObject(Vector2 texCoord)
    {
        Color color = mainTexture.GetPixel(Mathf.FloorToInt(texCoord.x * mainTexture.width), Mathf.FloorToInt(texCoord.y * mainTexture.height));
        return color.a >= 0.5f;
    }

    public void TakeDamage(int damage, ItemPickupLocation itemPickupLocation)
    {
        lastHitItemPickupLocation = itemPickupLocation;
        worldResourceSpawn.healthRemaining.value -= damage;

        if (onHit != null)
            onHit();
    }

    protected virtual void OnValueChanged_CurrentUnitCount(int oldValue, int newValue)
    {
        contentContainer.SetActive(newValue > 0);
    }

    private void OnValueChanged_ChunkRoom(ChunkRoom oldValue, ChunkRoom newValue)
    {
        if (oldValue != null)
        {
            oldValue.roomColor.onValueChange -= OnValueChanged_ChunkRoom_RoomColor;
        }

        if (newValue != null)
        {
            newValue.roomColor.onValueChangeImmediate += OnValueChanged_ChunkRoom_RoomColor;
        }
    }

    private void OnValueChanged_ChunkRoom_RoomColor(float oldValue, float newValue)
    {
        material.SetFloat("roomColor", newValue);
    }

    protected virtual void OnEvent_Respawn()
    {
        transform.position = worldResourceSpawn.position.value;
    }

    protected virtual void OnDestroy()
    {
        chunkRoom.onValueChangeImmediate -= OnValueChanged_ChunkRoom;
        worldResourceSpawn.unitCountRemaining.onValueChange -= OnValueChanged_CurrentUnitCount;
        worldResourceSpawn.onRespawn -= OnEvent_Respawn;
        if (onDestroyed != null)
            onDestroyed();
    }
}