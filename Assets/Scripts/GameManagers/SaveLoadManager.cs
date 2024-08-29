using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    string[] _deffaultItemsNames = new string[] { "Simple_Cannon", "Simple_Truck", "Dual_Cannon_Scheme" };
    string[] _TESTdeffaultItemsNames = new string[] { "Simple_Cannon", "Dual_Cannon", "Simple_Truck", "Energy_Dual_Cannon", "Energy_Simple_Cannon", "Dual_Cannon_Scheme" };
    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void ResetProgress(string[] ItemsNames)
    {
        PlayerData.Instance.PlayerItemsData.Clear();
        UIResourcesManager.Instance.RemoveAllResources();
        foreach (var itemName in ItemsNames)
        {
            PlayerWeapon weapon = GameAssets.Instance.GameItems.Weapons.Find(weapon => weapon.name == itemName);
            if (weapon == null) continue;
            PlayerData.Instance.PlayerItemsData.Add(Instantiate((WeaponData)weapon.GetItemData()));
            CustomLogDebuger.Log($"ADD {weapon.name} AS DEFFAULT ITEM");
        }
        foreach (var itemName in ItemsNames)
        {
            PlayerVehicle playerVehicle = GameAssets.Instance.GameItems.PlayerVehicles.Find(weapon => weapon.name == itemName);
            if (playerVehicle == null) continue;
            PlayerData.Instance.PlayerItemsData.Add(Instantiate((VehicleData)playerVehicle.GetItemData()));
            CustomLogDebuger.Log($"ADD {playerVehicle.name} AS DEFFAULT ITEM");
        }
        foreach (var itemName in ItemsNames)
        {
            WeaponSchemeData weaponSchemeData = GameAssets.Instance.GameItems.WeaponSchemeData.Find(scheme => scheme.name == itemName);
            if (weaponSchemeData == null) continue;
            PlayerData.Instance.PlayerItemsData.Add(Instantiate(weaponSchemeData));
            CustomLogDebuger.Log($"ADD {weaponSchemeData.name} AS DEFFAULT ITEM");
        }
    }

    public void SaveData()
    {
        List<VehicleData> vehiclesData = new();
        List<WeaponData> weaponsData = new();
        List<string> weaponsSchemesNames = new();

        foreach (var item in PlayerData.Instance.PlayerItemsData)
        {
            if (item is WeaponData WData) weaponsData.Add(WData);
            else if (item is VehicleData VData) vehiclesData.Add(VData);
            else if (item is WeaponSchemeData WSData) weaponsSchemesNames.Add(WSData.DeffItemName);
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
       
        PlayerPrefs.SetString("SavedWeaponSchemeNames", JsonConvert.SerializeObject(weaponsSchemesNames, Formatting.Indented));

        string savedResources = JsonConvert.SerializeObject(PlayerData.Instance.AvailableResources);
        PlayerPrefs.SetString("SavedResourcesData", savedResources);

        string savedEquipedItems = JsonConvert.SerializeObject(PlayerData.Instance.EquipedItems);
        PlayerPrefs.SetString("SavedEquipedItems", savedEquipedItems);

        string lastselectedLevelName = PlayerData.Instance.LastSelectedLevelName;
        PlayerPrefs.SetString("LastselectedLevelName", lastselectedLevelName);

        string unlockedLevelsNames = JsonConvert.SerializeObject(PlayerData.Instance.UnlockedLevelsNames);
        PlayerPrefs.SetString("UnlockedLevelsNames", unlockedLevelsNames);

        CustomLogDebuger.Log("DATA SAVED");
    }

    public void LoadSaveData()
    {
        if (!PlayerPrefs.HasKey("SavedWeaponsData") || !PlayerPrefs.HasKey("SavedVehiclesData") || !PlayerPrefs.HasKey("SavedResourcesData") || !PlayerPrefs.HasKey("SavedEquipedItems"))
        {
            PlayerData.Instance.AvailableResources = new();
            ResetProgress(_deffaultItemsNames);
            PlayerPrefs.DeleteAll();

            PlayerData.Instance.EquipedItems = new Dictionary<int, string>()
            {
                {0, "Simple_Truck" },
                {1, "Simple_Cannon" }
            };

            PlayerData.Instance.LastSelectedLevelName = "1-1";
            PlayerData.Instance.UnlockedLevelsNames.Add("1-1");

            CustomLogDebuger.Log("LOADED DEFFAULT ITEMS");
            return;
        }

        List<string> weaponsDataAsStrings = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString("SavedWeaponsData"));
        foreach (var weaponStringData in weaponsDataAsStrings)
        {
            WeaponData weaponData = ScriptableObject.CreateInstance<WeaponData>();
            JsonUtility.FromJsonOverwrite(weaponStringData, weaponData);
            PlayerData.Instance.PlayerItemsData.Add(weaponData);
        }
        CustomLogDebuger.Log("WEAPONS LOADED");

        List<string> vehiclesDataAsStrings = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString("SavedVehiclesData"));
        foreach (var vehicleStringData in vehiclesDataAsStrings)
        {
            VehicleData vehicleData = ScriptableObject.CreateInstance<VehicleData>();
            JsonUtility.FromJsonOverwrite(vehicleStringData, vehicleData);
            PlayerData.Instance.PlayerItemsData.Add(vehicleData);
        }
        CustomLogDebuger.Log("VEHICLES LOADED");



        List<string> weaponsSchemeNames = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString("SavedWeaponSchemeNames"));
        foreach (var weaponsSchemeName in weaponsSchemeNames)
        {
            WeaponSchemeData weaponSD = GameAssets.Instance.GameItems.WeaponSchemeData.Find(scheme => scheme.DeffItemName == weaponsSchemeName);
            PlayerData.Instance.PlayerItemsData.Add(weaponSD);
        }
        CustomLogDebuger.Log("WEAPON SCHEMES LOADED");










        PlayerData.Instance.AvailableResources = JsonConvert.DeserializeObject<Dictionary<ResourcesType, int>>(PlayerPrefs.GetString("SavedResourcesData"));
        CustomLogDebuger.Log("RESOURCES LOADED");

        PlayerData.Instance.EquipedItems = JsonConvert.DeserializeObject<Dictionary<int,string>>(PlayerPrefs.GetString("SavedEquipedItems"));
        CustomLogDebuger.Log("EquipedItems LOADED");

        PlayerData.Instance.LastSelectedLevelName = PlayerPrefs.GetString("LastselectedLevelName");

        PlayerData.Instance.UnlockedLevelsNames = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString("UnlockedLevelsNames"));

        CustomLogDebuger.Log("LevelsData LOADED");
    }
}