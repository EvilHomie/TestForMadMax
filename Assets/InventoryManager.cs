using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
        _equipBtn.onClick.AddListener(OnEquipItem);
        _equipBtn.gameObject.SetActive(false);
        _unlockBtn.gameObject.SetActive(false); // временно... пока нет логики с чертежами
    }

    public void OnOpenInventory()
    {        
        foreach (Transform child in _mainInventoryContainer)
        {
            Destroy(child.gameObject);
        }
        _inventoryItems.Clear();
        foreach (var item in PlayerData.Instance.PlayerItemsData)
        {
            ADDItemToInventory(item);
        }

        SetUpEquipSlots();

        DisableReplacesWeaponOption();
    }

    void SetUpEquipSlots()
    {
        InventoryEquipPanelManager.Instance.ResetData();
        for (int i = 0; i < PlayerData.Instance.EquipedItems.Count; i++)
        {
            IItemData itemData = PlayerData.Instance.GetItemData(PlayerData.Instance.EquipedItems[i]);
            if (i == 0)
            {
                InventoryEquipPanelManager.Instance.EquipedVehicleSlot.SetitemData(itemData);
            }
            else
            {
                WeaponSlot wSlot = InventoryEquipPanelManager.Instance.EquipeWeaponsSlots.Find(slot => slot.SlotIndex == i);
                wSlot.InventoryItem.SetitemData(itemData);
            }
            RemoveItemFromInventory(itemData);
        }
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


    void OnEquipItem()
    {
        if (_selectedItem is WeaponData)
        {
            EnableReplacesWeaponOption(_selectedItem);
        }
        else if (_selectedItem is VehicleData)
        {
            PlayerData.Instance.EquipedItems[0] = _selectedItem.ItemName;
            InventoryEquipPanelManager.Instance.EquipedVehicleSlot.SetitemData(_selectedItem);
            RemoveItemFromInventory(_selectedItem);
        }
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

    void DisableReplacesWeaponOption()
    {
        _vehicleSlotCG.blocksRaycasts = true;
        InventoryEquipPanelManager.Instance.EquipPanelRT.localScale = Vector3.one;
        foreach (var weaponSlot in InventoryEquipPanelManager.Instance.EquipeWeaponsSlots)
        {
            weaponSlot.SelectBtn.gameObject.SetActive(false);
            weaponSlot.SelectBtn.onClick.RemoveAllListeners();
        }
    }

    void EnableReplacesWeaponOption(IItemData newItemData)
    {
        _vehicleSlotCG.blocksRaycasts = false;
        InventoryEquipPanelManager.Instance.EquipPanelRT.localScale = Vector3.one * 2;
        foreach (var weaponSlot in InventoryEquipPanelManager.Instance.EquipeWeaponsSlots)
        {
            weaponSlot.SelectBtn.gameObject.SetActive(true);
            weaponSlot.SelectBtn.onClick.AddListener(delegate { OnReplaceWeapon(weaponSlot, newItemData); });
        }          
    }
    void OnReplaceWeapon(WeaponSlot slot, IItemData newItemData)
    {
        if (slot.InventoryItem.GetitemData() != null)
        {
            IItemData oldItem = slot.InventoryItem.GetitemData();
            ADDItemToInventory(oldItem);
        }
        _equipBtn.gameObject.SetActive(false);
        slot.InventoryItem.SetitemData(newItemData);
        RemoveItemFromInventory(newItemData);
        PlayerData.Instance.EquipedItems[slot.SlotIndex] = newItemData.ItemName;
        DisableReplacesWeaponOption();
    }
}
