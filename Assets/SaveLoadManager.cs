using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    string[] _deffaultItemsNames = new string[] { "Simple Cannon", "Dual Cannon", "Simple Truck" };
    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void LoadDeffaultItems(string[] ItemsNames)
    {
        foreach (var itemName in ItemsNames)
        {
            PlayerWeapon weapon = GameAssets.Instance.GameItems.Weapons.Find(weapon => weapon.name == itemName);
            if (weapon == null) continue;
            PlayerData.Instance.PlayerItemsData.Add(Instantiate((WeaponData)weapon.GetItemData()));
        }
        foreach (var itemName in ItemsNames)
        {
            PlayerVehicle playerVehicle = GameAssets.Instance.GameItems.PlayerVehicles.Find(weapon => weapon.name == itemName);
            if (playerVehicle == null) continue;
            PlayerData.Instance.PlayerItemsData.Add(Instantiate((VehicleData)playerVehicle.GetItemData()));
        }
    }
    
    public void SaveItemsData()
    {
        List<VehicleData> vehiclesData = new();
        List<WeaponData> weaponsData = new();

        foreach (var item in PlayerData.Instance.PlayerItemsData)
        {
            if (item is WeaponData WData) weaponsData.Add(WData);
            else if (item is VehicleData VData) vehiclesData.Add(VData);
        }

        List<string> weaponsDataAsStrings = new();
        List<string> vehiclesDataAsStrings = new();

        foreach (var item in weaponsData)
        {
            string stringData = JsonConvert.SerializeObject(item, Formatting.Indented);
            weaponsDataAsStrings.Add(stringData);
        }
        PlayerPrefs.SetString("SavedWeaponsData", JsonConvert.SerializeObject(weaponsDataAsStrings, Formatting.Indented));

        foreach (var item in vehiclesData)
        {
            string stringData = JsonConvert.SerializeObject(item, Formatting.Indented);
            vehiclesDataAsStrings.Add(stringData);
        }
        PlayerPrefs.SetString("SavedVehiclesData", JsonConvert.SerializeObject(vehiclesDataAsStrings, Formatting.Indented));

        string savedResources = JsonConvert.SerializeObject(PlayerData.Instance.AvailableResources);
        PlayerPrefs.SetString("SavedResourcesData", savedResources);

        PlayerPrefs.SetString("SelectedVehicle", PlayerData.Instance.SelectedVehicleName);

        string selectedWeapons = JsonConvert.SerializeObject(PlayerData.Instance.SelectedWeapons);
        PlayerPrefs.SetString("SelectedWeapons", selectedWeapons);

        Debug.LogWarning("DATA SAVED");
    }

    public void LoadItemsData()
    {
        if (!PlayerPrefs.HasKey("SavedWeaponsData") || !PlayerPrefs.HasKey("SavedVehiclesData") || !PlayerPrefs.HasKey("SavedResourcesData") || !PlayerPrefs.HasKey("SelectedVehicle") || !PlayerPrefs.HasKey("SelectedWeapons"))
        {
            PlayerData.Instance.AvailableResources = new();
            LoadDeffaultItems(_deffaultItemsNames);
            Debug.LogWarning("LOADED DEFFAULT ITEMS");
            return;
        }

        List<string> weaponsDataAsStrings = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString("SavedWeaponsData"));
        foreach (var weaponStringData in weaponsDataAsStrings)
        {
            WeaponData weaponData = ScriptableObject.CreateInstance<WeaponData>();
            JsonUtility.FromJsonOverwrite(weaponStringData, weaponData);
            PlayerData.Instance.PlayerItemsData.Add(weaponData);
        }
        Debug.LogWarning("WEAPONS LOADED");


        List<string> vehiclesDataAsStrings = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString("SavedVehiclesData"));
        foreach (var vehicleStringData in vehiclesDataAsStrings)
        {
            VehicleData vehicleData = ScriptableObject.CreateInstance<VehicleData>();
            JsonUtility.FromJsonOverwrite(vehicleStringData, vehicleData);
            PlayerData.Instance.PlayerItemsData.Add(vehicleData);
        }
        Debug.LogWarning("VEHICLES LOADED");

        PlayerData.Instance.AvailableResources = JsonConvert.DeserializeObject<Dictionary<ResourcesType, int>>(PlayerPrefs.GetString("SavedResourcesData"));
        Debug.LogWarning("RESOURCES LOADED");

        PlayerData.Instance.SelectedVehicleName = PlayerPrefs.GetString("SelectedVehicle");
        Debug.LogWarning("SelectedVehicle LOADED");

        PlayerData.Instance.SelectedWeapons = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString("SelectedWeapons"));
        Debug.LogWarning("SelectedWeapons LOADED");
    }
}