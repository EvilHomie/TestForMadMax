using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    string[] _deffaultItemsNames = new string[] { "Simple_Cannon", "Simple_Truck", "Energy_Simple_Cannon_Scheme", "Advanced_Truck_Scheme" };
    string[] _TESTdeffaultItemsNames = new string[] { "Simple_Cannon", "Dual_Cannon", "Simple_Truck", "Energy_Dual_Cannon", "Energy_Simple_Cannon", "Dual_Cannon_Scheme" };
    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void ResetProgress(string[] ItemsNames)
    {
        PlayerPrefs.DeleteAll();
        PlayerData.Instance.PlayerItemsData = new();
        UIResourcesManager.Instance.RemoveAllResources();
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
        foreach (var itemName in ItemsNames)
        {
            SchemeData schemeData = GameAssets.Instance.GameItems.SchemeData.Find(scheme => scheme.SchemeName == itemName);
            if (schemeData == null) continue;
            PlayerData.Instance.PlayerItemsData.Add((IItemData)schemeData);
        }

        PlayerData.Instance.EquipedItems = new Dictionary<int, string>()
            {
                {0, "Simple_Truck" },
                {1, "Simple_Cannon" }
            };

        PlayerData.Instance.LastSelectedLevelName = "1-1";
        PlayerData.Instance.UnlockedLevelsNames = new() { "1-1" };



    }

    public void SaveData()
    {
        List<VehicleData> vehiclesData = new();
        List<WeaponData> weaponsData = new();
        List<string> schemesNames = new();

        foreach (var item in PlayerData.Instance.PlayerItemsData)
        {
            if (item is WeaponData WData) weaponsData.Add(WData);
            else if (item is VehicleData VData) vehiclesData.Add(VData);
            else if (item is SchemeData WSData) schemesNames.Add(WSData.SchemeName);
        }

        List<string> weaponsDataAsStrings = new();
        List<string> vehiclesDataAsStrings = new();

        foreach (var item in weaponsData)
        {
            string stringData = JsonConvert.SerializeObject(item, Formatting.Indented);
            weaponsDataAsStrings.Add(stringData);
        }
        YandexGame.savesData.SavedWeaponsData = JsonConvert.SerializeObject(weaponsDataAsStrings, Formatting.Indented);

        foreach (var item in vehiclesData)
        {
            string stringData = JsonConvert.SerializeObject(item, Formatting.Indented);
            vehiclesDataAsStrings.Add(stringData);
        }
        YandexGame.savesData.SavedVehiclesData = JsonConvert.SerializeObject(vehiclesDataAsStrings, Formatting.Indented);

        YandexGame.savesData.SavedSchemeNames = JsonConvert.SerializeObject(schemesNames, Formatting.Indented);

        string savedResources = JsonConvert.SerializeObject(PlayerData.Instance.AvailableResources);
        YandexGame.savesData.SavedResourcesData = savedResources;

        string savedEquipedItems = JsonConvert.SerializeObject(PlayerData.Instance.EquipedItems);
        YandexGame.savesData.SavedEquipedItems = savedEquipedItems;

        string lastselectedLevelName = PlayerData.Instance.LastSelectedLevelName;
        YandexGame.savesData.LastselectedLevelName = lastselectedLevelName;

        string unlockedLevelsNames = JsonConvert.SerializeObject(PlayerData.Instance.UnlockedLevelsNames);
        YandexGame.savesData.UnlockedLevelsNames = unlockedLevelsNames;

        YandexGame.SaveProgress();
    }

    public void LoadSaveData()
    {
        if (
            YandexGame.savesData.SavedWeaponsData == null ||
            YandexGame.savesData.SavedVehiclesData == null ||
            YandexGame.savesData.SavedSchemeNames == null ||
            YandexGame.savesData.SavedResourcesData == null ||
            YandexGame.savesData.SavedEquipedItems == null ||
            YandexGame.savesData.LastselectedLevelName == null ||
            YandexGame.savesData.UnlockedLevelsNames == null
            )
        {
            ResetProgress(_deffaultItemsNames);
            return;
        }

        PlayerData.Instance.PlayerItemsData = new();



        List<string> weaponsDataAsStrings = JsonConvert.DeserializeObject<List<string>>(YandexGame.savesData.SavedWeaponsData);
        foreach (var weaponStringData in weaponsDataAsStrings)
        {
            WeaponData weaponData = ScriptableObject.CreateInstance<WeaponData>();
            JsonUtility.FromJsonOverwrite(weaponStringData, weaponData);
            PlayerData.Instance.PlayerItemsData.Add(weaponData);
        }

        List<string> vehiclesDataAsStrings = JsonConvert.DeserializeObject<List<string>>(YandexGame.savesData.SavedVehiclesData);
        foreach (var vehicleStringData in vehiclesDataAsStrings)
        {
            VehicleData vehicleData = ScriptableObject.CreateInstance<VehicleData>();
            JsonUtility.FromJsonOverwrite(vehicleStringData, vehicleData);
            PlayerData.Instance.PlayerItemsData.Add(vehicleData);
        }



        List<string> schemeNames = JsonConvert.DeserializeObject<List<string>>(YandexGame.savesData.SavedSchemeNames);
        foreach (var schemeName in schemeNames)
        {
            SchemeData schemeData = GameAssets.Instance.GameItems.SchemeData.Find(scheme => scheme.SchemeName == schemeName);
            PlayerData.Instance.PlayerItemsData.Add((IItemData)schemeData);
        }

        PlayerData.Instance.AvailableResources = JsonConvert.DeserializeObject<Dictionary<ResourcesType, int>>(YandexGame.savesData.SavedResourcesData);

        PlayerData.Instance.EquipedItems = JsonConvert.DeserializeObject<Dictionary<int, string>>(YandexGame.savesData.SavedEquipedItems);

        PlayerData.Instance.LastSelectedLevelName = YandexGame.savesData.LastselectedLevelName;

        PlayerData.Instance.UnlockedLevelsNames = JsonConvert.DeserializeObject<List<string>>(YandexGame.savesData.UnlockedLevelsNames);
    }
}




//  Версия через PlayerPrefs
//public void SaveData()
//    {
//        List<VehicleData> vehiclesData = new();
//        List<WeaponData> weaponsData = new();
//        List<string> schemesNames = new();

//        foreach (var item in PlayerData.Instance.PlayerItemsData)
//        {
//            if (item is WeaponData WData) weaponsData.Add(WData);
//            else if (item is VehicleData VData) vehiclesData.Add(VData);
//            else if (item is SchemeData WSData) schemesNames.Add(WSData.SchemeName);
//        }

//        List<string> weaponsDataAsStrings = new();
//        List<string> vehiclesDataAsStrings = new();

//        foreach (var item in weaponsData)
//        {
//            string stringData = JsonConvert.SerializeObject(item, Formatting.Indented);
//            weaponsDataAsStrings.Add(stringData);
//        }
//        PlayerPrefs.SetString("SavedWeaponsData", JsonConvert.SerializeObject(weaponsDataAsStrings, Formatting.Indented));

//        foreach (var item in vehiclesData)
//        {
//            string stringData = JsonConvert.SerializeObject(item, Formatting.Indented);
//            vehiclesDataAsStrings.Add(stringData);
//        }
//        PlayerPrefs.SetString("SavedVehiclesData", JsonConvert.SerializeObject(vehiclesDataAsStrings, Formatting.Indented));

//        PlayerPrefs.SetString("SavedSchemeNames", JsonConvert.SerializeObject(schemesNames, Formatting.Indented));

//        string savedResources = JsonConvert.SerializeObject(PlayerData.Instance.AvailableResources);
//        PlayerPrefs.SetString("SavedResourcesData", savedResources);

//        string savedEquipedItems = JsonConvert.SerializeObject(PlayerData.Instance.EquipedItems);
//        PlayerPrefs.SetString("SavedEquipedItems", savedEquipedItems);

//        string lastselectedLevelName = PlayerData.Instance.LastSelectedLevelName;
//        PlayerPrefs.SetString("LastselectedLevelName", lastselectedLevelName);

//        string unlockedLevelsNames = JsonConvert.SerializeObject(PlayerData.Instance.UnlockedLevelsNames);
//        PlayerPrefs.SetString("UnlockedLevelsNames", unlockedLevelsNames);

//        CustomLogDebuger.Log("DATA SAVED");
//    }

//    public void LoadSaveData()
//    {
//        if (!PlayerPrefs.HasKey("SavedWeaponsData") || !PlayerPrefs.HasKey("SavedVehiclesData") || !PlayerPrefs.HasKey("SavedResourcesData") || !PlayerPrefs.HasKey("SavedEquipedItems"))
//        {
//            ResetProgress(_deffaultItemsNames);
//            return;
//        }

//        PlayerData.Instance.PlayerItemsData = new();



//        List<string> weaponsDataAsStrings = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString("SavedWeaponsData"));
//        foreach (var weaponStringData in weaponsDataAsStrings)
//        {            
//            WeaponData weaponData = ScriptableObject.CreateInstance<WeaponData>();
//            JsonUtility.FromJsonOverwrite(weaponStringData, weaponData);
//            PlayerData.Instance.PlayerItemsData.Add(weaponData);
//        }
//        CustomLogDebuger.Log("WEAPONS LOADED");

//        List<string> vehiclesDataAsStrings = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString("SavedVehiclesData"));
//        foreach (var vehicleStringData in vehiclesDataAsStrings)
//        {
//            VehicleData vehicleData = ScriptableObject.CreateInstance<VehicleData>();
//            JsonUtility.FromJsonOverwrite(vehicleStringData, vehicleData);
//            PlayerData.Instance.PlayerItemsData.Add(vehicleData);
//        }
//        CustomLogDebuger.Log("VEHICLES LOADED");



//        List<string> schemeNames = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString("SavedSchemeNames"));
//        foreach (var schemeName in schemeNames)
//        {
//            SchemeData schemeData = GameAssets.Instance.GameItems.SchemeData.Find(scheme => scheme.SchemeName == schemeName);
//            PlayerData.Instance.PlayerItemsData.Add((IItemData)schemeData);
//        }
//        CustomLogDebuger.Log("WEAPON SCHEMES LOADED");

//        PlayerData.Instance.AvailableResources = JsonConvert.DeserializeObject<Dictionary<ResourcesType, int>>(PlayerPrefs.GetString("SavedResourcesData"));
//        CustomLogDebuger.Log("RESOURCES LOADED");

//        PlayerData.Instance.EquipedItems = JsonConvert.DeserializeObject<Dictionary<int, string>>(PlayerPrefs.GetString("SavedEquipedItems"));
//        CustomLogDebuger.Log("EquipedItems LOADED");

//        PlayerData.Instance.LastSelectedLevelName = PlayerPrefs.GetString("LastselectedLevelName");

//        PlayerData.Instance.UnlockedLevelsNames = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString("UnlockedLevelsNames"));

//        CustomLogDebuger.Log("LevelsData LOADED");
//    }
