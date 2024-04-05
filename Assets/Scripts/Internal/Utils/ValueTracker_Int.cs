using UnityEngine;

public class ValueTracker_Int : ValueTracker
{
    public readonly EventVariable<ValueTracker_Int, int> finalValue;

    public ValueTracker_Int(float baseValue)
        : base(baseValue)
    {
        finalValue = new EventVariable<ValueTracker_Int, int>(this, 0);
    }

    protected override void SetFinalValue(float value)
    {
        finalValue.value = Mathf.FloorToInt(value);
    }
}