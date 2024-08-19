using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] InventoryItem _inventoryItemPF;
    [SerializeField] Transform _mainInventoryContainer;
    [SerializeField] Button _equipBtn;
    [SerializeField] Button _unlockBtn;

    [SerializeField] CanvasGroup _vehicleSlotCG;

    List<InventoryItem> _inventoryItems = new();
    IItemData _selectedItem;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    //public void OnLoad()
    //{
    //    OnChangeVehicle(PlayerData.Instance.GetLastVehicle());
    //}
    private void Start()
    {
        _equipBtn.onClick.AddListener(OnTryEquipItem);
        _equipBtn.gameObject.SetActive(false);
        _unlockBtn.gameObject.SetActive(false); // временно... пока нет логики с чертежами
    }

    public void OnOpenInventory()
    {
        gameObject.SetActive(true);
        foreach (Transform child in _mainInventoryContainer)
        {
            Destroy(child.gameObject);
        }

        _inventoryItems.Clear();
        foreach (var item in PlayerData.Instance.PlayerItemsData)
        {
            if (PlayerData.Instance.EquipedItems.ContainsValue(item.ItemName)) continue;
            ADDItemToInventory(item);
        }

        InventoryEquipPanelManager.Instance.ResetData();
    }
    public void OnCloseInventory()
    {
        PlayerVehicleManager.Instance.OnChangeVehicle();
        PlayerWeaponManager.Instance.UpdateWeaponsData();
        gameObject.SetActive(false);
    }

    public void OnSelectInventoryItem(IItemData itemData)
    {
        _selectedItem = itemData;
        InventoryInfoPanelManager.Instance.UpdateInfoPanel(itemData);
        InventoryUpgradePanelManager.Instance.UpdateUpgradePanel(itemData);
        _equipBtn.gameObject.SetActive(!PlayerData.Instance.EquipedItems.ContainsValue(itemData.ItemName));
    }

    public void OnBuyUpgrade(string charName, List<ResCost> upgradeCost)
    {
        int scrapMetalAmount = upgradeCost.FirstOrDefault(res => res.ResourcesType == ResourcesType.ScrapMetal).Amount;
        int wiresAmount = upgradeCost.FirstOrDefault(res => res.ResourcesType == ResourcesType.Wires).Amount;
        int copperAmount = upgradeCost.FirstOrDefault(res => res.ResourcesType == ResourcesType.Сopper).Amount;

        bool enoughResources = ResourcesManager.Instance.SpendResources(scrapMetalAmount, wiresAmount, copperAmount);

        if (!enoughResources) return;

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
        else if (charName == Constants.SHIELDDMG) weaponData.shieldDmgCurLvl++;
        else if (charName == Constants.ROTATIONSPEED) weaponData.rotationSpeedCurLvl++;
        else if (charName == Constants.FIRERATE) weaponData.fireRateCurtLvl++;
    }

    void UpgradeVehicle(VehicleData vehicleData, string charName)
    {
        if (charName == Constants.HULLHP) vehicleData.hullHPCurLvl++;
        else if (charName == Constants.SHIELDHP) vehicleData.shieldHPCurLvl++;
        else if (charName == Constants.SHIELREGENRATE) vehicleData.shieldRegenCurtLvl++;
        else if (charName == Constants.WEAPONSCOUNT)
        {
            vehicleData.curWeaponsCount++;
            InventoryEquipPanelManager.Instance.OnCheckWeaponsSlotsCount();
        }
    }


    void OnTryEquipItem()
    {
        if (_selectedItem is WeaponData) InventoryEquipPanelManager.Instance.EnableWeaponEquipOption(_selectedItem);      
        else if (_selectedItem is VehicleData) InventoryEquipPanelManager.Instance.OnEquipNewVehicle(_selectedItem);
    }

    void ADDItemToInventory(IItemData item)
    {
        InventoryItem InventoryItem = Instantiate(_inventoryItemPF, _mainInventoryContainer);
        _inventoryItems.Add(InventoryItem);
        InventoryItem.SetitemData(item);
    }
    void RemoveItemFromInventory(IItemData item)
    {
        InventoryItem inventoryItem = _inventoryItems.Find(invItem => invItem.GetitemData() == item);
        Destroy(inventoryItem.gameObject);
        _inventoryItems.Remove(inventoryItem);
    }

    public void OnSelectedItemEquiped(IItemData newItem = null, IItemData previousItem = null)
    {
        _equipBtn.gameObject.SetActive(false);

        if(newItem != null && previousItem != null)
        {
            InventoryItem inventoryItem = _inventoryItems.Find(invItem => invItem.GetitemData() == newItem);
            inventoryItem.SetitemData(previousItem);
            return;
        }

        if (newItem != null) RemoveItemFromInventory(newItem);
        if (previousItem != null) ADDItemToInventory(previousItem);
    }

}
