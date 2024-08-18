using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    string _selectedVehicleName;
    List<string> _selectedWeaponsNames = new();

    Dictionary<ResourcesType, int> _availableResources = new();
    List<IItemData> _playerItemsData = new();

    public List<IItemData> PlayerItemsData => _playerItemsData;
    public Dictionary<ResourcesType, int> AvailableResources { get => _availableResources; set => _availableResources = value; }

    public string SelectedVehicleName { get => _selectedVehicleName; set => _selectedVehicleName = value; }

    public List<string> SelectedWeapons { get => _selectedWeaponsNames; set => _selectedWeaponsNames = value; }



    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public IItemData GetItemData(string itemName)
    {
        return PlayerItemsData.Find(item => item.ItemName == itemName);
    }

    public VehicleData GetLastVehicle()
    {
        return (VehicleData)PlayerItemsData.Find(item => item.ItemName == _selectedVehicleName);
    }

    
}
