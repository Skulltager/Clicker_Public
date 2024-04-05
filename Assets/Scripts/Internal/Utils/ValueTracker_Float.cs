public class ValueTracker_Float : ValueTracker
{
    public readonly EventVariable<ValueTracker_Float, float> finalValue;

    public ValueTracker_Float(float baseValue)
        : base(baseValue)
    {
        finalValue = new EventVariable<ValueTracker_Float, float>(this, 0);
    }

    protected override void SetFinalValue(float value)
    {
        finalValue.value = value;
    }
}