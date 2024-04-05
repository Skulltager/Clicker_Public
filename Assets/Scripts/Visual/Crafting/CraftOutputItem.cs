using UnityEngine;
using SheetCodes;
using UnityEngine.UI;

public class CraftOutputItem : DataDrivenUI<CraftingOutputRecord>
{
    [SerializeField] private Text chanceText;
    [SerializeField] private Text amountText;
    [SerializeField] private Image itemIcon;

    protected override void OnValueChanged_Data(CraftingOutputRecord oldValue, CraftingOutputRecord newValue)
    {
        if (newValue != null)
        {
            if (newValue.MinAmount == newValue.MaxAmount)
                amountText.text = newValue.MinAmount.ToString();
            else
                amountText.text = string.Format("{0} - {1}", newValue.MinAmount, newValue.MaxAmount);

            if (newValue.Chance < 1)
            {
                chanceText.gameObject.SetActive(true);
                chanceText.text = ((int)newValue.Chance * 100).ToString() + "%";
            }
            else
            {
                chanceText.gameObject.SetActive(false);
            }

            itemIcon.sprite = newValue.Item.Icon;
        }
    }
}