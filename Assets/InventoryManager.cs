using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] InventoryItem _inventoryItemPF;
    [SerializeField] Transform _mainInventoryContainer;

    IItemData _selectedItem;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }


    public void OnOpenInventory()
    {
        foreach (Transform child in _mainInventoryContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in PlayerData.Instance.PlayerWeaponsData)
        {
            InventoryItem InventoryItem = Instantiate(_inventoryItemPF, _mainInventoryContainer);
            InventoryItem.SetitemData(item);
        }
    }

    public void OnSelectInventoryItem(IItemData itemData)
    {
        _selectedItem = itemData;
        InventoryInfoPanelManager.Instance.UpdateInfoPanel(itemData);
        InventoryUpgradePanelManager.Instance.UpdateUpgradePanel(itemData);
    }

    public void OnBuyUpdate(string charName)
    {
        Debug.LogWarning("UPGRADE" + "  " + charName);
    }
}
