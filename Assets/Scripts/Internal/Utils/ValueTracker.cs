using System.Collections;
using UnityEngine;

public abstract class ValueTracker
{
    protected readonly float baseValue;
    public readonly EventList<float> multipliers;
    public readonly EventList<float> additions;
    protected readonly EventVariable<ValueTracker, float> startingValue;
    protected readonly EventVariable<ValueTracker, float> multiplierValue;

    protected ValueTracker(float baseValue)
    {
        this.baseValue = baseValue;
        multipliers = new EventList<float>();
        additions = new EventList<float>();
        startingValue = new EventVariable<ValueTracker, float>(this, baseValue);
        multiplierValue = new EventVariable<ValueTracker, float>(this, 1);

        multipliers.onAdd += OnAdd_Multipliers;
        multipliers.onRemove += OnRemove_Multipliers;
        additions.onAdd += OnAdd_Additions;
        additions.onRemove += OnRemove_Additions;
        startingValue.onValueChange += OnValueChanged_AdditionsValue;
        multiplierValue.onValueChange += OnValueChanged_MultiplierValue;
    }

    private void OnAdd_Multipliers(float value)
    {
        RecalculateMultiplier();
    }

    private void OnRemove_Multipliers(float value)
    {
        RecalculateMultiplier();
    }

    private void RecalculateMultiplier()
    {
        float totalMultiplier = 1;
        foreach (float multiplier in multipliers)
            totalMultiplier *= multiplier;

        multiplierValue.value = totalMultiplier;
    }

    private void OnAdd_Additions(float value)
    {
        RecalculateStartingValue();
    }

    private void OnRemove_Additions(float value)
    {
        RecalculateStartingValue();
    }

    private void RecalculateStartingValue()
    {
        float value = 1;
        foreach (float addition in additions)
            value += addition;

        startingValue.value = value;
    }


    private void OnValueChanged_AdditionsValue(float oldValue, float newValue)
    {
        SetFinalValue(newValue * multiplierValue.value);
    }

    private void OnValueChanged_MultiplierValue(float oldValue, float newValue)
    {
        SetFinalValue(startingValue.value * newValue);
    }

    protected abstract void SetFinalValue(float value);
}