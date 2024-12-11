using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    string[] _deffaultItemsNames = new string[] { "Simple_Cannon_V_1", "Dual_Cannon_V_1_Scheme", "Simple_Truck_V_1", "Advanced_Truck_V_1_Scheme", "MiniGun_V_1_Scheme", "Dual_MiniGun_V_1_Scheme" };

    [SerializeField] bool resetProgressRequired = false;


    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void ResetProgress(string[] ItemsNames)
    {
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
            PlayerVehicle playerVehicle = GameAssets.Instance.GameItems.PlayerVehicles.Find(vehicle => vehicle.name == itemName);
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
        PlayerData.Instance.AvailableResources = new() { { ResourcesType.Ñopper, 0 }, { ResourcesType.Wires, 0 }, { ResourcesType.ScrapMetal, 0 } };
        UIResourcesManager.Instance.AddResources(0, 20, 55);
        YandexGame.savesData.tutorilaIsComplete = false;

        YandexGame.savesData.idSave = 1;
        SaveData();

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
        //ResetProgress(_deffaultItemsNames);
        if (YandexGame.savesData.savesIsClear)
        {
            ResetProgress(_deffaultItemsNames);
        }
        else
        {
            YandexGame.savesData.tutorilaIsComplete = true;
            LoadSaveData();
        }
    }

    public void LoadSaveData()
    {
        PlayerData.Instance.PlayerItemsData = new();


        foreach (var weaponData in YandexGame.savesData.weaponsData)
        {
            if (!GameAssets.Instance.CheckExistings(weaponData.deffWeaponName))
            {
                ResetProgress(_deffaultItemsNames);
                return;
            }
            WeaponData newWeaponData = ScriptableObject.CreateInstance<WeaponData>();
            newWeaponData.SetData(weaponData);
            PlayerData.Instance.PlayerItemsData.Add(newWeaponData);
        }

        foreach (var vehicleData in YandexGame.savesData.vehiclesData)
        {
            if (!GameAssets.Instance.CheckExistings(vehicleData.deffVehicleName))
            {
                ResetProgress(_deffaultItemsNames);
                return;
            }
            VehicleData newVehicleData = ScriptableObject.CreateInstance<VehicleData>();
            newVehicleData.SetData(vehicleData);
            PlayerData.Instance.PlayerItemsData.Add(newVehicleData);
        }

        foreach (var schemeName in YandexGame.savesData.schemesNames)
        {
            if (!GameAssets.Instance.CheckExistings(schemeName))
            {
                ResetProgress(_deffaultItemsNames);
                return;
            }
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
            if (YandexGame.savesData.savedVerion == null) return false;


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