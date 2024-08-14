using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [SerializeField] InventoryItem _inventoryItemPF;
    [SerializeField] Transform _mainInventoryContainer;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }


    public void OnOpenUpgrades()
    {
        foreach (var item in PlayerData.Instance.AvailableItemsByName)
        {
            InventoryItem InventoryItem = Instantiate(_inventoryItemPF, _mainInventoryContainer);
            InventoryItem.SetImage(item.Value);
        }
    }
}
