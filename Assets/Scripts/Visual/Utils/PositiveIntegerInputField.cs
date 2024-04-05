using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class PositiveIntegerInputField : InputField
{
    public readonly ControlledEventVariable<PositiveIntegerInputField, int> intValue;

    private PositiveIntegerInputField()
    {
        intValue = new ControlledEventVariable<PositiveIntegerInputField, int>(this, 1, Check_TextValue);
    }

    private int Check_TextValue(int value)
    {
        return Mathf.Max(value, 1);
    }

    protected override void Awake()
    {
        onValueChanged.AddListener(OnValueChanged_Text);
        base.Awake();
    }

    private void OnValueChanged_Text(string newValue)
    {
        string adjustedValue = newValue;

        adjustedValue = Regex.Replace(adjustedValue, @"[^a-zA-Z0-9 ]", "");

        int intValue;
        if (int.TryParse(adjustedValue, out intValue))
        {
            if (intValue < 1)
                adjustedValue = 1.ToString();
            else
                adjustedValue = intValue.ToString();

            this.intValue.value = intValue;
        }
        else
            adjustedValue = string.Empty;

        if (adjustedValue != newValue)
        {
            text = adjustedValue;
            return;
        }
    }

    protected override void OnDestroy()
    {
        onValueChanged.RemoveListener(OnValueChanged_Text);
        base.OnDestroy();
    }
}