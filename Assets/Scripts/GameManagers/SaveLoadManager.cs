using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    string[] _deffaultItemsNames = new string[] { "Simple_Cannon_V_1", "Dual_Cannon_V_1", "Simple_Truck_V_1", "Advanced_Truck_V_1_Scheme" };
    string[] _TESTdeffaultItemsNames = new string[] { "Simple_Cannon_V_1", "Simple_Truck_V_1", "Advanced_Truck_V_1" };

    [SerializeField] bool resetProgressRequired = false;


    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    //private void Start()
    //{
    //    CompareVersions();
    //}

    void ResetProgress(string[] ItemsNames)
    {
        Debug.LogWarning("RESETPROGRESS");
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
                {0, "Simple_Truck_V_1" },
                {1, "Simple_Cannon_V_1" }
            };

        PlayerData.Instance.LastSelectedLevelName = "1-1";
        PlayerData.Instance.UnlockedLevelsNames = new() { "1-1" };

        SaveData();

    }

    void UnlockAllData()
    {
        ResetProgress(_TESTdeffaultItemsNames);
        UIResourcesManager.Instance.AddResources(3000, 3000, 3000);
        PlayerData.Instance.UnlockedLevelsNames = new() { "1-1", "1-2", "1-3", "1-4", "1-5", "1-6", "1-7", "1-8", "1-9", "1-10", "2-1", "2-2", "2-3", "2-4", "2-5", "2-6", "2-7", "2-8", "2-9", "2-10" };
    }

    public void SaveData()
    {
        List<WeaponData> weaponsData = new();
        List<VehicleData> vehiclesData = new();
        List<string> schemesNames = new();

        foreach (var item in PlayerData.Instance.PlayerItemsData)
        {
            if (item is WeaponData WData) weaponsData.Add(WData);
            else if (item is VehicleData VData) vehiclesData.Add(VData);
            else if (item is SchemeData WSData) schemesNames.Add(WSData.SchemeName);
        }

        List<WeaponDataForSave> weaponDataForSaves = new();
        foreach (var item in weaponsData)
        {
            WeaponDataForSave dataForSave = new(item);
            weaponDataForSaves.Add(dataForSave);
        }
        YandexGame.savesData.weaponsData = weaponDataForSaves;

        List<VehicleDataForSave> vehicleDataForSaves = new();
        foreach (var item in vehiclesData)
        {
            VehicleDataForSave dataForSave = new(item);
            vehicleDataForSaves.Add(dataForSave);
        }
        YandexGame.savesData.vehiclesData = vehicleDataForSaves;

        YandexGame.savesData.schemesNames = schemesNames;

        YandexGame.savesData.availableResources = PlayerData.Instance.AvailableResources;

        YandexGame.savesData.equipedItems = PlayerData.Instance.EquipedItems;

        YandexGame.savesData.lastSelectedLevelName = PlayerData.Instance.LastSelectedLevelName;

        YandexGame.savesData.unlockedLevelsNames = PlayerData.Instance.UnlockedLevelsNames;

        YandexGame.savesData.savesIsClear = false;
        YandexGame.savesData.savedVerion = Application.version;
        YandexGame.SaveProgress();
    }

    public void CheckSaveData()
    {
        if (YandexGame.savesData.savesIsClear || YandexGame.savesData.savedVerion == null)
        {
            ResetProgress(_deffaultItemsNames);
            return;
        }

        if (CheckVersionsCompatibility())
        {
            LoadSaveData();
        }
        else
        {
            ResetProgress(_deffaultItemsNames);
        }
    }

    public void LoadSaveData()
    {
        PlayerData.Instance.PlayerItemsData = new();

        foreach (var weaponData in YandexGame.savesData.weaponsData)
        {
            WeaponData newWeaponData = ScriptableObject.CreateInstance<WeaponData>();
            newWeaponData.SetData(weaponData);
            PlayerData.Instance.PlayerItemsData.Add(newWeaponData);
        }

        foreach (var vehicleData in YandexGame.savesData.vehiclesData)
        {
            VehicleData newVehicleData = ScriptableObject.CreateInstance<VehicleData>();
            newVehicleData.SetData(vehicleData);
            PlayerData.Instance.PlayerItemsData.Add(newVehicleData);
        }

        foreach (var schemeName in YandexGame.savesData.schemesNames)
        {
            SchemeData schemeData = GameAssets.Instance.GameItems.SchemeData.Find(scheme => scheme.SchemeName == schemeName);
            PlayerData.Instance.PlayerItemsData.Add((IItemData)schemeData);
        }

        PlayerData.Instance.AvailableResources = YandexGame.savesData.availableResources;
        PlayerData.Instance.EquipedItems = YandexGame.savesData.equipedItems;
        PlayerData.Instance.LastSelectedLevelName = YandexGame.savesData.lastSelectedLevelName;
        PlayerData.Instance.UnlockedLevelsNames = YandexGame.savesData.unlockedLevelsNames;
    }

    bool CheckVersionsCompatibility()
    {
        if (resetProgressRequired)
        {
            string currentVersion = Application.version;
            string savedVersion = YandexGame.savesData.savedVerion;

            var version1 = new Version(currentVersion);
            var version2 = new Version(savedVersion);

            var result = version1.CompareTo(version2);
            if (result > 0)
            {
                //Console.WriteLine("version1 is greater");
                return false;
            }
            else if (result < 0)
            {
                //Console.WriteLine("version2 is greater");
                return true;
            }
            else
            {
                //Console.WriteLine("versions are equal");
                return true;
            }
        }
        else return true;        
    }
}


public class WeaponDataForSave
{
    public string deffWeaponName;
    public string weaponNameEN;
    public string weaponNameRU;
    public WeaponType weaponType;
    public Raritie weaponRaritie;

    public float hullDmgByLvl;
    public int hullDmgCurLvl;
    public int hullDmgMaxLvl;

    public float shieldDmgByLvl;
    public int shieldDmgCurLvl;
    public int shieldDmgMaxLvl;

    public float fireRateByLvl;
    public int fireRateCurtLvl;
    public int fireRateMaxLvl;

    public float rotationSpeedByLvl;
    public int rotationSpeedCurLvl;
    public int rotationSpeedMaxLvl;

    public WeaponDataForSave()
    {

    }
    public WeaponDataForSave(WeaponData weaponData)
    {
        deffWeaponName = weaponData.deffWeaponName;
        weaponNameEN = weaponData.weaponNameEN;
        weaponNameRU = weaponData.weaponNameRU;
        weaponType = weaponData.weaponType;
        weaponRaritie = weaponData.weaponRaritie;
        hullDmgByLvl = weaponData.hullDmgByLvl;
        hullDmgCurLvl = weaponData.hullDmgCurLvl;
        hullDmgMaxLvl = weaponData.hullDmgMaxLvl;
        shieldDmgByLvl = weaponData.shieldDmgByLvl;
        shieldDmgCurLvl = weaponData.shieldDmgCurLvl;
        shieldDmgMaxLvl = weaponData.shieldDmgMaxLvl;
        fireRateByLvl = weaponData.fireRateByLvl;
        fireRateCurtLvl = weaponData.fireRateCurtLvl;
        fireRateMaxLvl = weaponData.fireRateMaxLvl;
        rotationSpeedByLvl = weaponData.rotationSpeedByLvl;
        rotationSpeedCurLvl = weaponData.rotationSpeedCurLvl;
        rotationSpeedMaxLvl = weaponData.rotationSpeedMaxLvl;
    }
}

public class VehicleDataForSave
{
    public string deffVehicleName;
    public string vehicleNameEN;
    public string vehicleNameRU;
    public Raritie vehicleRaritie;

    public float hullHPByLvl;
    public int hullHPCurLvl;
    public int hullHPMaxLvl;

    public float shieldHPByLvl;
    public int shieldHPCurLvl;
    public int shieldHPMaxLvl;

    public float shieldRegenRateByLvl;
    public int shieldRegenCurtLvl;
    public int shieldRegenMaxLvl;

    public int curWeaponsCount;
    public int maxWeaponsCount;

    public VehicleDataForSave()
    {

    }
    public VehicleDataForSave(VehicleData vehicleData)
    {
        deffVehicleName = vehicleData.deffVehicleName;
        vehicleNameEN = vehicleData.vehicleNameEN;
        vehicleNameRU = vehicleData.vehicleNameRU;
        vehicleRaritie = vehicleData.vehicleRaritie;
        hullHPByLvl = vehicleData.hullHPByLvl;
        hullHPCurLvl = vehicleData.hullHPCurLvl;
        hullHPMaxLvl = vehicleData.hullHPMaxLvl;
        shieldHPByLvl = vehicleData.shieldHPByLvl;
        shieldHPCurLvl = vehicleData.shieldHPCurLvl;
        shieldHPMaxLvl = vehicleData.shieldHPMaxLvl;
        shieldRegenRateByLvl = vehicleData.shieldRegenRateByLvl;
        shieldRegenCurtLvl = vehicleData.shieldRegenCurtLvl;
        shieldRegenMaxLvl = vehicleData.shieldRegenMaxLvl;
        curWeaponsCount = vehicleData.curWeaponsCount;
        maxWeaponsCount = vehicleData.maxWeaponsCount;
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
