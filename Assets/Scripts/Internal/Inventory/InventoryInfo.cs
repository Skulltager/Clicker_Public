using UnityEngine;
using UnityEngine.UI;

public class InventoryInfo : DataDrivenUI<Inventory>
{
    [SerializeField] private GameObject overweightContent;
    [SerializeField] private Text weightText;
    [SerializeField] private Image weightFillImage;
    [SerializeField] private Color weightEmptyColor;
    [SerializeField] private Color weightFullColor;

    protected override void OnValueChanged_Data(Inventory oldValue, Inventory newValue)
    {
        if (oldValue != null)
        {
            oldValue.currentWeight.onValueChange -= OnValueChanged_CurrentWeight;
            oldValue.maxWeight.onValueChange -= OnValueChanged_MaxWeight;
            oldValue.overWeight.onValueChange -= OnValueChanged_OverWeight;
        }

        if (newValue != null)
        {
            newValue.currentWeight.onValueChange += OnValueChanged_CurrentWeight;
            newValue.maxWeight.onValueChange += OnValueChanged_MaxWeight;
            newValue.overWeight.onValueChangeImmediate += OnValueChanged_OverWeight;
            SetWeightInfo();
        }
    }

    private void OnValueChanged_CurrentWeight(long oldValue, long newValue)
    {
        SetWeightInfo();
    }

    private void OnValueChanged_MaxWeight(long oldValue, long newValue)
    {
        SetWeightInfo();
    }

    private void SetWeightInfo()
    {
        if(data.infiniteWeight.value)
        {
            weightFillImage.fillAmount = 0;
            weightFillImage.color = weightEmptyColor;

            string currentWeightString = (data.currentWeight.value / 100f).ToString("0.00");
            weightText.text = string.Format("{0} / inf kg", currentWeightString);
        }
        else
        {
            float weightFactor = Mathf.Min((float)data.currentWeight.value / data.maxWeight.value, 1);
            weightFillImage.fillAmount = weightFactor;
            weightFillImage.color = Color.Lerp(weightEmptyColor, weightFullColor, weightFactor);

            string maxWeightString = (data.maxWeight.value / 100f).ToString("0.00");
            string currentWeightString = (data.currentWeight.value / 100f).ToString("0.00");
            weightText.text = string.Format("{0} / {1}kg", currentWeightString, maxWeightString);
        }
    }

    private void OnValueChanged_OverWeight(bool oldValue, bool newValue)
    {
        overweightContent.SetActive(newValue);
    }
}