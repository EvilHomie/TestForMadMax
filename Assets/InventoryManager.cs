using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] InventoryItem _inventoryItemPF;
    [SerializeField] Transform _mainInventoryContainer;

    VehicleData _equipedPlayerVehicle;
    List<WeaponData> _equipedWeapons = new();

    IItemData _selectedItem;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void OnLoad()
    {
        OnChangeVehicle(PlayerData.Instance.GetLastVehicle());
    }


    public void OnOpenInventory()
    {
        foreach (Transform child in _mainInventoryContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in PlayerData.Instance.PlayerItemsData)
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

    public void OnBuyUpdate(string charName, List<ResCost> upgradeCost)
    {
        int scrapMetalAmount = upgradeCost.FirstOrDefault(res => res.ResourcesType == ResourcesType.ScrapMetal).Amount;
        int wiresAmount = upgradeCost.FirstOrDefault(res => res.ResourcesType == ResourcesType.Wires).Amount;
        int copperAmount = upgradeCost.FirstOrDefault(res => res.ResourcesType == ResourcesType.Ñopper).Amount;

        bool result = ResourcesManager.Instance.SpendResources(scrapMetalAmount, wiresAmount, copperAmount);

        if (!result) return;

        if (_selectedItem is WeaponData WData)
        {
            UpgradeWeapon(WData, charName);
        }
        else if (_selectedItem is VehicleData VData)
        {
            UpgradeVehicle(VData, charName);
        }

        InventoryInfoPanelManager.Instance.UpdateInfoPanel(_selectedItem);
        InventoryUpgradePanelManager.Instance.UpdateUpgradePanel(_selectedItem);
    }


    void UpgradeWeapon(WeaponData weaponData, string charName)
    {
        if (charName == Constants.HULLDMG) weaponData.hullDmgCurLvl++;
        else if(charName == Constants.SHIELDDMG) weaponData.shieldDmgCurLvl++;
        else if (charName == Constants.ROTATIONSPEED) weaponData.rotationSpeedCurLvl++;
        else if (charName == Constants.FIRERATE) weaponData.fireRateCurtLvl++;
    }

    void UpgradeVehicle(VehicleData vehicleData, string charName)
    {
        if (charName == Constants.HULLHP) vehicleData.hullHPCurLvl++;
        else if (charName == Constants.SHIELDHP) vehicleData.shieldHPCurLvl++;
        else if (charName == Constants.SHIELREGENRATE) vehicleData.shieldRegenCurtLvl++;
        else if (charName == Constants.WEAPONSCOUNT) vehicleData.curWeaponsCount++;
    }


    void OnChangeVehicle(VehicleData vehicleData)
    {
        _equipedPlayerVehicle = vehicleData;
        PlayerData.Instance.SelectedVehicleName = vehicleData.ItemName;
        PlayerVehicleManager.Instance.ChangeVehicle(vehicleData);
    }
}
