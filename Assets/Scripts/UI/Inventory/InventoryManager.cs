using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] InventoryItem _inventoryItemPF;
    [SerializeField] InventoryItem _schemeItemPF;
    [SerializeField] Transform _mainInventoryContainer;
    [SerializeField] Transform _schemesContainer;
    [SerializeField] Button _equipBtn;
    [SerializeField] Button _unlockBtn;
    [SerializeField] AudioSource _inventoryAS;
    [SerializeField] AudioClip _upgradeSound;
    [SerializeField] AudioClip _equipSound;

    [SerializeField] CanvasGroup _vehicleSlotCG;

    [SerializeField] TextMeshProUGUI _newSchemesText;
    [SerializeField] TextMeshProUGUI _InventoryText;

    List<InventoryItem> _inventoryItems = new();
    //List<InventoryItem> _inventorySchemes = new();
    IItemData _selectedItem;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _newSchemesText.text = TextConstants.NEWSCHEMES;
        _InventoryText.text = TextConstants.INVENTORY;
        _equipBtn.onClick.AddListener(OnTryEquipItem);
        _equipBtn.gameObject.SetActive(false);

        _unlockBtn.onClick.AddListener(OnTryUnlock);
        _unlockBtn.gameObject.SetActive(false); // ��������... ���� ��� ������ � ���������
        OnCloseInventory();
    }

    public void OnOpenInventory()
    {
        gameObject.SetActive(true);
        foreach (Transform child in _mainInventoryContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in _schemesContainer)
        {
            Destroy(child.gameObject);
        }

        _inventoryItems.Clear();
        foreach (var item in PlayerData.Instance.PlayerItemsData)
        {
            if (PlayerData.Instance.EquipedItems.ContainsValue(item.DeffItemName)) continue;
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
        InventoryEquipPanelManager.Instance.OnSelectItem();

        if (_selectedItem is SchemeData)
        {
            _equipBtn.gameObject.SetActive(false);
            _unlockBtn.gameObject.SetActive(true);
        }
        else
        {
            _unlockBtn.gameObject.SetActive(false);
            _equipBtn.gameObject.SetActive(!PlayerData.Instance.EquipedItems.ContainsValue(itemData.DeffItemName));
        }
    }

    public void OnBuyUpgrade(string charName, List<ResCost> upgradeCost)
    {
        int scrapMetalAmount = upgradeCost.FirstOrDefault(res => res.ResourcesType == ResourcesType.ScrapMetal).Amount;
        int wiresAmount = upgradeCost.FirstOrDefault(res => res.ResourcesType == ResourcesType.Wires).Amount;
        int copperAmount = upgradeCost.FirstOrDefault(res => res.ResourcesType == ResourcesType.�opper).Amount;

        bool enoughResources = UIResourcesManager.Instance.TrySpendResources(scrapMetalAmount, wiresAmount, copperAmount);

        if (!enoughResources) return;

        if (_selectedItem is WeaponData WData)
        {
            UpgradeWeapon(WData, charName);
        }
        else if (_selectedItem is VehicleData VData)
        {
            UpgradeVehicle(VData, charName);
        }
        _inventoryAS.PlayOneShot(_upgradeSound);

        InventoryInfoPanelManager.Instance.UpdateInfoPanel(_selectedItem);
        InventoryUpgradePanelManager.Instance.UpdateUpgradePanel(_selectedItem);
    }


    void UpgradeWeapon(WeaponData weaponData, string charName)
    {
        if (charName == TextConstants.HULLDMG) weaponData.hullDmgCurLvl++;
        else if (charName == TextConstants.SHIELDDMG) weaponData.shieldDmgCurLvl++;
        else if (charName == TextConstants.ROTATIONSPEED) weaponData.rotationSpeedCurLvl++;
        else if (charName == TextConstants.FIRERATE) weaponData.fireRateCurtLvl++;
    }

    void UpgradeVehicle(VehicleData vehicleData, string charName)
    {
        if (charName == TextConstants.HULLHP) vehicleData.hullHPCurLvl++;
        else if (charName == TextConstants.SHIELDHP) vehicleData.shieldHPCurLvl++;
        else if (charName == TextConstants.SHIELREGENRATE) vehicleData.shieldRegenCurtLvl++;
        else if (charName == TextConstants.WEAPONSCOUNT)
        {
            vehicleData.curWeaponsCount++;
            InventoryEquipPanelManager.Instance.CheckWeaponsSlotsCount();
        }
    }

    void OnTryEquipItem()
    {
        if (_selectedItem is WeaponData) InventoryEquipPanelManager.Instance.EnableWeaponEquipOption(_selectedItem);
        else if (_selectedItem is VehicleData) InventoryEquipPanelManager.Instance.OnEquipNewVehicle(_selectedItem);
    }

    void ADDItemToInventory(IItemData item)
    {
        InventoryItem InventoryItem;

        if (item is SchemeData)
        {
            InventoryItem = Instantiate(_schemeItemPF, _schemesContainer);
        }
        else
        {
            InventoryItem = Instantiate(_inventoryItemPF, _mainInventoryContainer);
        }
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
        _inventoryAS.PlayOneShot(_equipSound);
        _equipBtn.gameObject.SetActive(false);

        if (newItem != null && previousItem != null)
        {
            InventoryItem inventoryItem = _inventoryItems.Find(invItem => invItem.GetitemData() == newItem);
            inventoryItem.SetitemData(previousItem);
            return;
        }

        if (newItem != null) RemoveItemFromInventory(newItem);
        if (previousItem != null) ADDItemToInventory(previousItem);
    }

    void OnTryUnlock()
    {
        //bool enoughResources = false;
        //IItemData newItem;
        if (_selectedItem is SchemeData SData)
        {
            bool enoughResources = UIResourcesManager.Instance.TrySpendResources(SData.scrapMetalAmountForUnlock, SData.wiresAmountForUnlock, SData.copperAmountForUnlock);
            if (!enoughResources) return;

            IItemData newItem = null;
            if (SData is WeaponSchemeData WSData)
            {
                newItem = Instantiate(WSData.weaponData);
            }
            else if (SData is VehicleSchemeData VSData)
            {
                newItem = Instantiate(VSData.vehicleData);
            }

            UnlockScheme((IItemData)SData, newItem);
            InventoryInfoPanelManager.Instance.UpdateInfoPanel(newItem);
            InventoryUpgradePanelManager.Instance.UpdateUpgradePanel(newItem);
        }
    }

    void UnlockScheme(IItemData scheme, IItemData newItem)
    {
        _equipBtn.gameObject.SetActive(true);
        _unlockBtn.gameObject.SetActive(false);
        _selectedItem = newItem;
        PlayerData.Instance.PlayerItemsData.Remove(scheme);
        PlayerData.Instance.PlayerItemsData.Add(newItem);
        RemoveItemFromInventory(scheme);
        ADDItemToInventory(newItem);
    }
}
