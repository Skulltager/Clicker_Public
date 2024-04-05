using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoomContentDisplayItem : DataDrivenUI<WorldResourceSpawn>
{
    [SerializeField] private Image icon;
    [SerializeField] private Text nameText;
    [SerializeField] private Text amountText;
    [SerializeField] private Text respawnTimerText;

    protected override void OnValueChanged_Data(WorldResourceSpawn oldValue, WorldResourceSpawn newValue)
    {
        if(oldValue != null)
        {
            oldValue.unitCountRemaining.onValueChange -= OnValueChanged_UnitCountRemaining;
            oldValue.isRespawning.onValueChange -= OnValueChanged_IsRespawning;
            oldValue.lastInteractionTime.onValueChange -= OnValueChanged_LastInteractionTime;
        }

        if(newValue != null)
        {
            newValue.unitCountRemaining.onValueChangeImmediate += OnValueChanged_UnitCountRemaining;
            newValue.isRespawning.onValueChangeImmediate += OnValueChanged_IsRespawning;
            newValue.lastInteractionTime.onValueChange += OnValueChanged_LastInteractionTime;
            icon.sprite = newValue.icon;
            nameText.text = newValue.name;
        }
    }

    private void OnValueChanged_UnitCountRemaining(int oldValue, int newValue)
    {
        amountText.text = string.Format("{0}/{1}", data.unitCountRemaining.value, data.unitCount);
    }

    private void Update()
    {
        SetText_RespawnTimer();
    }

    private void OnValueChanged_IsRespawning(bool oldValue, bool newValue)
    {
        SetText_RespawnTimer();
    }

    private void OnValueChanged_LastInteractionTime(float oldValue, float newValue)
    {
        SetText_RespawnTimer();
    }

    private void SetText_RespawnTimer()
    {
        if (!data.isRespawning.value)
        {
            respawnTimerText.text = "-";
            return;
        }

        TimeSpan time = TimeSpan.FromSeconds(data.lastInteractionTime.value + data.respawnTime - Time.time);
        respawnTimerText.text = time.ToString("hh':'mm':'ss");
    }
}