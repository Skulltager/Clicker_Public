using UnityEngine;

public class WorldResourceMarker : DataDrivenBehaviour<WorldResourceSpawn>
{
    protected override void OnValueChanged_Data(WorldResourceSpawn oldValue, WorldResourceSpawn newValue)
    {
        if (oldValue != null)
            oldValue.unitCountRemaining.onValueChangeImmediate -= OnValueChanged_UnitCount;

        if (newValue != null)
            newValue.unitCountRemaining.onValueChangeImmediate += OnValueChanged_UnitCount;
    }

    private void OnValueChanged_UnitCount(int oldValue, int newValue)
    {
        if(oldValue > 0 && newValue == 0)
        {
            MiniMapVisualizer.instance.RemoveMiniMapMarker(transform);
            data.position.onValueChange -= OnValueChanged_Position;
        }

        if(oldValue == 0 && newValue > 0)
        {
            MiniMapVisualizer.instance.AddMiniMapMarker(transform, data.miniMapIcon);
            data.position.onValueChangeImmediate += OnValueChanged_Position;
        }
    }

    private void OnValueChanged_Position(Vector3 oldValue, Vector3 newValue)
    {
        transform.position = newValue;
    }
}