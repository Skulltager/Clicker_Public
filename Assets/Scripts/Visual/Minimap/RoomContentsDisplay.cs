using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomContentsDisplay : DataDrivenUI<ChunkRoom>
{
    [SerializeField] private RoomContentDisplayItem romContentDisplayItemPrefab;
    [SerializeField] private RectTransform roomContentDisplayItemsContainer;

    private readonly List<RoomContentDisplayItem> roomContentDisplayItemInstances;

    private RoomContentsDisplay()
        : base()
    {
        roomContentDisplayItemInstances = new List<RoomContentDisplayItem>();
    }

    protected override void OnValueChanged_Data(ChunkRoom oldValue, ChunkRoom newValue)
    {
        if (oldValue != null)
        {
            foreach (RoomContentDisplayItem instance in roomContentDisplayItemInstances)
                GameObject.Destroy(instance.gameObject);

            roomContentDisplayItemInstances.Clear();
        }

        if (newValue != null)
        {
            foreach(WorldResourceSpawn worldResourceSpawn in newValue.worldResourceSpawns)
            {
                RoomContentDisplayItem instance = GameObject.Instantiate(romContentDisplayItemPrefab, roomContentDisplayItemsContainer);
                instance.data = worldResourceSpawn;
                roomContentDisplayItemInstances.Add(instance);
            }
        }
    }
}