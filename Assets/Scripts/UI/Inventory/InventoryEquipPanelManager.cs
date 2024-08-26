using System.Collections.Generic;
using UnityEngine;

public class InventoryEquipPanelManager : MonoBehaviour
{
    public static InventoryEquipPanelManager Instance;

    [SerializeField] RectTransform _equipPanelRT;
    [SerializeField] InventoryItem _equipedVehicleSlot;
    [SerializeField] List<WeaponEquipSlot> _equipeWeaponsSlots;
    public RectTransform EquipPanelRT => _equipPanelRT;
    public InventoryItem EquipedVehicleSlot => _equipedVehicleSlot;
    public List<WeaponEquipSlot> EquipeWeaponsSlots => _equipeWeaponsSlots;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void ResetData()
    {
        DisableWeaponEquipOption();
        _equipedVehicleSlot.SetitemData(null);
        foreach (var weaponSlot in EquipeWeaponsSlots)
        {
            weaponSlot.InventoryItem.SetitemData(null);
            weaponSlot.ActiveStatus = false;
        }
        FillSlotsFromPlayerData();
    }

    void FillSlotsFromPlayerData()
    {
        VehicleData vehicleData = (VehicleData)PlayerData.Instance.GetItemDataByName(PlayerData.Instance.EquipedItems[0]);
        _equipedVehicleSlot.SetitemData(vehicleData);


        for (int i = 0; i < vehicleData.curWeaponsCount; i++)
        {
            _equipeWeaponsSlots[i].ActiveStatus = true;
        }

        for (int i = 1; i < PlayerData.Instance.EquipedItems.Count; i++)
        {
            IItemData weaponData = PlayerData.Instance.GetItemDataByName(PlayerData.Instance.EquipedItems[i]);            

            WeaponEquipSlot wSlot = _equipeWeaponsSlots.Find(slot => slot.SlotIndex == i);
            wSlot.InventoryItem.SetitemData(weaponData);
        }
    }

    public void EnableWeaponEquipOption(IItemData newWeapon)
    {
        _equipedVehicleSlot.GetComponent<CanvasGroup>().blocksRaycasts = false;
        _equipPanelRT.localScale = Vector3.one * 2;
        foreach (var weaponSlot in _equipeWeaponsSlots)
        {
            weaponSlot.SelectBtn.gameObject.SetActive(true);
            weaponSlot.SelectBtn.onClick.AddListener(delegate { OnEquipWeapon(weaponSlot, newWeapon); });
        }
    }

    public void OnEquipNewVehicle(IItemData newVehicle)
    {
        PlayerData.Instance.EquipedItems[0] = newVehicle.ItemName;
        IItemData previousItem = _equipedVehicleSlot.GetitemData();

        InventoryManager.Instance.OnSelectedItemEquiped(newVehicle, previousItem);
        _equipedVehicleSlot.SetitemData(newVehicle);
        CheckWeaponsSlotsCount();
    }

    void DisableWeaponEquipOption()
    {
        _equipedVehicleSlot.GetComponent<CanvasGroup>().blocksRaycasts = true;
        _equipPanelRT.localScale = Vector3.one;
        foreach (var weaponSlot in _equipeWeaponsSlots)
        {
            weaponSlot.SelectBtn.gameObject.SetActive(false);
            weaponSlot.SelectBtn.onClick.RemoveAllListeners();
        }
    }

    void OnEquipWeapon(WeaponEquipSlot slot, IItemData newWeapon)
    {
        PlayerData.Instance.EquipedItems[slot.SlotIndex] = newWeapon.ItemName;
        IItemData previousItem = slot.InventoryItem.GetitemData();

        slot.InventoryItem.SetitemData(newWeapon);
        InventoryManager.Instance.OnSelectedItemEquiped(newWeapon, previousItem);
        DisableWeaponEquipOption();  
    }

    public void CheckWeaponsSlotsCount()
    {
        VehicleData currentVehicle = (VehicleData)_equipedVehicleSlot.GetitemData();
        int newSlotsCount = currentVehicle.curWeaponsCount;

        foreach (var weaponSlot in EquipeWeaponsSlots)
        {
            if(weaponSlot.SlotIndex <= newSlotsCount) 
            weaponSlot.ActiveStatus = true;
            else
            {
                IItemData previousItem = weaponSlot.InventoryItem.GetitemData();
                InventoryManager.Instance.OnSelectedItemEquiped(null, previousItem);
                weaponSlot.InventoryItem.SetitemData(null);
                weaponSlot.ActiveStatus = false;
                PlayerData.Instance.EquipedItems.Remove(weaponSlot.SlotIndex);
            }
        }
    }
}
