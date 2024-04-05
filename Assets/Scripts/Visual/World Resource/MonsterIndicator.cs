using UnityEngine;
using UnityEngine.UI;

public class MonsterIndicator : DataDrivenUI<WorldResource>
{
    public static MonsterIndicator instance { private set; get; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image unitCountFillImage;
    [SerializeField] private Text healthText;
    [SerializeField] private Text unitCountText;

    private void Awake()
    {
        instance = this;
    }

    protected override void OnValueChanged_Data(WorldResource oldValue, WorldResource newValue)
    {
        if (oldValue != null)
        {
            oldValue.worldResourceSpawn.healthRemaining.onValueChange -= OnValueChanged_CurrentHealth;
            oldValue.worldResourceSpawn.unitCountRemaining.onValueChange -= OnValueChanged_CurrentUnitCount;
            oldValue.onDestroyed -= OnEvent_WorldResourceDestroyed;
        }

        if (newValue != null)
        {
            newValue.worldResourceSpawn.healthRemaining.onValueChangeImmediate += OnValueChanged_CurrentHealth;
            newValue.worldResourceSpawn.unitCountRemaining.onValueChangeImmediate += OnValueChanged_CurrentUnitCount;
            newValue.onDestroyed += OnEvent_WorldResourceDestroyed;
            canvasGroup.Show();
        }
        else
        {
            canvasGroup.Hide();
        }
    }

    private void OnEvent_WorldResourceDestroyed()
    {
        data = null;
    }

    private void OnValueChanged_CurrentUnitCount(int oldValue, int newValue)
    {
        if (newValue <= 0)
        {
            data = null;
            return;
        }

        unitCountFillImage.fillAmount = (float)newValue / data.worldResourceSpawn.unitCount;
        unitCountText.text = string.Format("{0}/{1}", newValue, data.worldResourceSpawn.unitCount);
    }

    private void OnValueChanged_CurrentHealth(int oldValue, int newValue)
    {
        healthBarFillImage.fillAmount = (float)newValue / data.worldResourceSpawn.maxHealth;
        healthText.text = string.Format("{0}/{1}", newValue, data.worldResourceSpawn.maxHealth);
    }
}