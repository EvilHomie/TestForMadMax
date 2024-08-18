using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

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
            if (weapon != null)
            {
                PlayerData.Instance.PlayerItemsData.Add(Instantiate((WeaponData)weapon.GetItemData()));
                continue;
            }
            else
            {
                PlayerVehicle playerVehicle = GameAssets.Instance.GameItems.PlayerVehicles.Find(weapon => weapon.name == itemName);
                if (playerVehicle != null)
                {
                    PlayerData.Instance.PlayerItemsData.Add(Instantiate((VehicleData)playerVehicle.GetItemData()));
                    continue;
                }
            }
        }
    }
    //private void Start()
    //{
    //    LoadItemsData();
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SaveItemsData();

        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadItemsData();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
            Debug.LogWarning("SAVE CLEAR");
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

        foreach (var item in vehiclesData)
        {
            string stringData = JsonConvert.SerializeObject(item, Formatting.Indented);
            vehiclesDataAsStrings.Add(stringData);
        }

        PlayerPrefs.SetString("SavedWeaponsData", JsonConvert.SerializeObject(weaponsDataAsStrings, Formatting.Indented));
        PlayerPrefs.SetString("SavedVehiclesData", JsonConvert.SerializeObject(vehiclesDataAsStrings, Formatting.Indented));

        string savedResources = JsonConvert.SerializeObject(PlayerData.Instance.AvailableResources);
        PlayerPrefs.SetString("SavedResourcesData", savedResources);
        Debug.LogWarning("DATA SAVED");

        Debug.LogWarning(JsonUtility.ToJson(weaponsData[0], true));
        Debug.LogWarning(JsonConvert.SerializeObject(weaponsData[0], Formatting.Indented));
        //Debug.LogWarning(PlayerPrefs.GetString("SavedWeaponsData"));
        //Debug.LogWarning(PlayerPrefs.GetString("SavedVehiclesData"));
        //Debug.LogWarning(PlayerPrefs.GetString("SavedResourcesData"));

        //List<PlayerWeaponSaveData> weapons = new();
        //foreach (var weapon in weaponsData)
        //{
        //    weapons.Add(new(weapon));
        //}

        //List<PlayerVehicleSaveData> vehicles = new();
        //foreach (var vehicle in vehiclesData)
        //{
        //    vehicles.Add(new(vehicle));
        //}

        //SerializableList<string> weaponsDataAsStrings = new();
        //SerializableList<string> vehiclesDataAsStrings = new();

        //foreach (var weaponData in weaponsData) weaponsDataAsStrings.list.Add(JsonUtility.ToJson(weaponData, true));
        //PlayerPrefs.SetString("SavedWeaponsData", JsonUtility.ToJson(weaponsDataAsStrings, true));

        //foreach (var vehicleData in vehiclesData) vehiclesDataAsStrings.list.Add(JsonUtility.ToJson(vehicleData, true));
        //PlayerPrefs.SetString("SavedVehiclesData", JsonUtility.ToJson(vehiclesDataAsStrings, true));

        //string savedResources = JsonConvert.SerializeObject(PlayerData.Instance.AvailableResources);
        //PlayerPrefs.SetString("SavedResourcesData", savedResources);
        //Debug.LogWarning("DATA SAVED");
    }

    public void LoadItemsData()
    {
        if (!PlayerPrefs.HasKey("SavedWeaponsData") || !PlayerPrefs.HasKey("SavedVehiclesData") || !PlayerPrefs.HasKey("SavedResourcesData"))
        {
            PlayerData.Instance.AvailableResources = new();
            LoadDeffaultItems(_deffaultItemsNames);
            Debug.LogWarning("LOADED DEFFAULT ITEMS");
            return;
        }



        List<string> weaponsDataAsStrings = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString("SavedWeaponsData"));
        foreach (var weaponStringData in weaponsDataAsStrings)
        {
            Debug.LogWarning($"{weaponStringData}");
            WeaponData weaponData = ScriptableObject.CreateInstance<WeaponData>();

            JsonUtility.FromJsonOverwrite(weaponStringData, weaponData);
            PlayerData.Instance.PlayerItemsData.Add(weaponData);
            //JObject.Parse()

            //JsonConvert.DeserializeObject()
            //JsonUtility.FromJsonOverwrite(weaponStringData, weaponData);
            //PlayerData.Instance.PlayerItemsData.Add(weaponData);
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






        //SerializableList<string> weaponsDataAsStrings = JsonUtility.FromJson<SerializableList<string>>(PlayerPrefs.GetString("SavedWeaponsData"));
        //foreach (var weaponStringData in weaponsDataAsStrings.list)
        //{
        //    WeaponData weaponData = ScriptableObject.CreateInstance<WeaponData>();
        //    JsonUtility.FromJsonOverwrite(weaponStringData, weaponData);
        //    PlayerData.Instance.PlayerItemsData.Add(weaponData);
        //}
        //Debug.LogWarning("WEAPONS LOADED");


        //SerializableList<string> vehiclesDataAsStrings = JsonUtility.FromJson<SerializableList<string>>(PlayerPrefs.GetString("SavedVehiclesData"));
        //foreach (var vehicleStringData in vehiclesDataAsStrings.list)
        //{
        //    VehicleData vehicleData = ScriptableObject.CreateInstance<VehicleData>();
        //    JsonUtility.FromJsonOverwrite(vehicleStringData, vehicleData);
        //    PlayerData.Instance.PlayerItemsData.Add(vehicleData);
        //}
        //Debug.LogWarning("VEHICLES LOADED");

        //PlayerData.Instance.AvailableResources = JsonConvert.DeserializeObject<Dictionary<ResourcesType, int>>(PlayerPrefs.GetString("SavedResourcesData"));
        //Debug.LogWarning("RESOURCES LOADED");





    }
}

[Serializable]
public class SerializableList<T>
{
    public List<T> list;
    public SerializableList(List<T> list)
    {
        this.list = list;
    }
    public SerializableList()
    {
        list = new List<T>();
    }
}


//[Serializable]
//public class PlayerWeaponSaveData
//{
//    public string weaponName;
//    public WeaponType weaponType;
//    public Raritie weaponRaritie;

//    public float hullDmgByLvl;
//    public int hullDmgCurLvl;
//    public int _hullDmgMaxLvl;

//    public float shieldDmgByLvl;
//    public int shieldDmgCurLvl;
//    public int _shieldDmgMaxLvl;

//    public float fireRateByLvl;
//    public int fireRateCurtLvl;
//    public int _fireRateMaxLvl;

//    public float rotationSpeedByLvl;
//    public int rotationSpeedCurLvl;
//    public int _rotationSpeedMaxLvl;
//    public PlayerWeaponSaveData(WeaponData weaponData)
//    {
//        weaponName = weaponData.ItemName;
//        weaponType = weaponData.Type;
//        weaponRaritie = weaponData.Raritie;
//        hullDmgByLvl = weaponData.hullDmgByLvl;
//        hullDmgCurLvl = weaponData.hullDmgCurLvl;
//        _hullDmgMaxLvl = weaponData.HullDmgMaxLvl;
//        shieldDmgByLvl = weaponData.hullDmgByLvl;
//        shieldDmgCurLvl = weaponData.shieldDmgCurLvl;
//        _shieldDmgMaxLvl = weaponData.ShieldDmgMaxLvl;
//        fireRateByLvl = weaponData.fireRateByLvl;
//        fireRateCurtLvl = weaponData.fireRateCurtLvl;
//        _fireRateMaxLvl = weaponData.FireRateMaxLvl;
//        rotationSpeedByLvl = weaponData.rotationSpeedByLvl;
//        rotationSpeedCurLvl = weaponData.rotationSpeedCurLvl;
//        _rotationSpeedMaxLvl = weaponData.RotationSpeedMaxLvl;
//    }
//}

//[Serializable]
//public class PlayerVehicleSaveData
//{
//    public string _vehicleName;
//    public Raritie _vehicleRaritie;

//    public float hullHPByLvl;
//    public int hullHPCurLvl;
//    public int _hullHPMaxLvl;

//    public float shieldHPByLvl;
//    public int shieldHPCurLvl;
//    public int _shieldHPMaxLvl;

//    public float shieldRegenRateByLvl;
//    public int shieldRegenCurtLvl;
//    public int _shieldRegenMaxLvl;

//    public int curWeaponsCount;
//    public int maxWeaponsCount;
//    public PlayerVehicleSaveData(VehicleData vehicleData)
//    {
//        _vehicleName = vehicleData.ItemName;
//        _vehicleRaritie = vehicleData.Raritie;
//        hullHPByLvl = vehicleData.hullHPByLvl;
//        hullHPCurLvl = vehicleData.hullHPCurLvl;
//        _hullHPMaxLvl = vehicleData.HullHPMaxLvl;
//        shieldHPByLvl = vehicleData.shieldHPByLvl;
//        shieldHPCurLvl = vehicleData.shieldHPCurLvl;
//        _shieldHPMaxLvl = vehicleData.ShieldHPMaxLvl;
//        shieldRegenRateByLvl = vehicleData.shieldRegenRateByLvl;
//        shieldRegenCurtLvl = vehicleData.shieldRegenCurtLvl;
//        _shieldRegenMaxLvl = vehicleData.ShieldRegenMaxLvl;
//        curWeaponsCount = vehicleData.curWeaponsCount;
//        maxWeaponsCount = vehicleData.MaxWeaponsCount;
//    }
//}