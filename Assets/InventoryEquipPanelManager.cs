using System.Collections.Generic;
using UnityEngine;

public class InventoryEquipPanelManager : MonoBehaviour
{
    public static InventoryEquipPanelManager Instance;

    [SerializeField] RectTransform _equipPanelRT;
    [SerializeField] InventoryItem _equipedVehicleSlot;
    [SerializeField] List<WeaponSlot> _equipeWeaponsSlots;
    public RectTransform EquipPanelRT => _equipPanelRT;
    public InventoryItem EquipedVehicleSlot => _equipedVehicleSlot;
    public List<WeaponSlot> EquipeWeaponsSlots => _equipeWeaponsSlots;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void ResetData()
    {
        _equipedVehicleSlot.SetitemData(null);
        foreach (var weaponSlot in EquipeWeaponsSlots)
        {
            weaponSlot.InventoryItem.SetitemData(null);
        }
    }
}
