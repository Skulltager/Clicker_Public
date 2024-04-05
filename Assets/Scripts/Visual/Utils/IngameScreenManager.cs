using UnityEngine;

public class IngameScreenManager : MonoBehaviour
{
    public static IngameScreenManager instance { private set; get; }

    [SerializeField] private PlayerInventoryDisplay playerInventoryDisplay;
    [SerializeField] private CraftingMenu craftingMenu;
    [SerializeField] private WorldMapDisplay worldMapDisplay;
    [SerializeField] private InventoryTransferDisplay inventoryTransferDisplay;
    [SerializeField] private Hud hud;

    private readonly EventVariable<IngameScreenManager, MonoBehaviour> visibleScreen;

    private IngameScreenManager()
    {
        visibleScreen = new EventVariable<IngameScreenManager, MonoBehaviour>(this, null);
    }

    private void Awake()
    {
        instance = this;
        playerInventoryDisplay.gameObject.SetActive(false);
        craftingMenu.gameObject.SetActive(false);
        worldMapDisplay.gameObject.SetActive(false);
        hud.gameObject.SetActive(false);
        inventoryTransferDisplay.gameObject.SetActive(false);
        visibleScreen.value = hud;
        visibleScreen.onValueChangeImmediate += OnValueChanged_VisibleScreen;
    }

    private void OnValueChanged_VisibleScreen(MonoBehaviour oldValue, MonoBehaviour newValue)
    {
        if (oldValue != null)
            oldValue.gameObject.SetActive(false);

        if (newValue != null)
            newValue.gameObject.SetActive(true);
    }

    public void SetHudData(PlayerInventory playerInventory)
    {
        hud.data = playerInventory;
    }

    public void ShowHideScreen_PlayerInventoryDisplay(PlayerInventory playerInventory)
    {
        if (visibleScreen.value == playerInventoryDisplay)
        {
            ShowScreen_Hud();
            return;
        }

        playerInventoryDisplay.data = playerInventory;
        visibleScreen.value = playerInventoryDisplay;
    }

    public void ShowHideScreen_CraftingMenu(Crafter crafter)
    {
        if (visibleScreen.value == craftingMenu)
        {
            ShowScreen_Hud();
            return;
        }

        craftingMenu.data = crafter;
        visibleScreen.value = craftingMenu;
    }

    public void ShowHideScreen_WorldMapDisplay()
    {
        if (visibleScreen.value == worldMapDisplay)
        {
            ShowScreen_Hud();
            return;
        }

        visibleScreen.value = worldMapDisplay;
    }

    public void ShowScreen_InventoryTransferDisplay(Inventory roomInventory, Inventory playerInventory)
    {
        if (visibleScreen.value == inventoryTransferDisplay)
        {
            ShowScreen_Hud();
            return;
        }

        inventoryTransferDisplay.data = new InventoryTransferDisplayData(roomInventory, playerInventory);
        visibleScreen.value = inventoryTransferDisplay;
    }

    public void ShowScreen_Hud()
    {
        visibleScreen.value = hud;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        ShowScreen_Hud();
    }

    private void OnDestroy()
    {
        visibleScreen.onValueChange -= OnValueChanged_VisibleScreen;
    }
}