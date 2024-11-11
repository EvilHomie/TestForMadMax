using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance;

    [Header("GAME Items Data")]
    [SerializeField] GameItems _gameItems;
    [SerializeField] UpgradeCosts _weaponUpgradeCosts;
    [SerializeField] UpgradeCosts _vehicleUpgradeCosts;
    [SerializeField] UpgradeCosts _vehicleSlotCosts;
    [SerializeField] List<ResSprite> _resSprites;
    [SerializeField] List<string> _allItemsNamesCollection;

    public GameItems GameItems => _gameItems;
    public UpgradeCosts WeaponUpgradeCosts => _weaponUpgradeCosts;
    public UpgradeCosts VehicleUpgradeCosts => _vehicleUpgradeCosts;

    public UpgradeCosts VehicleSlotCosts => _vehicleSlotCosts;
    public List<ResSprite> ResSprites => _resSprites;



    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        _allItemsNamesCollection = new();
        
    }
    public void Init()
    {
        foreach (PlayerWeapon item in _gameItems.Weapons)
        {
            WeaponData weaponData = (WeaponData)item.GetItemData();
            _allItemsNamesCollection.Add(weaponData.DeffItemName);
        }
        foreach (PlayerVehicle item in _gameItems.PlayerVehicles)
        {
            VehicleData playerVehicle = (VehicleData)item.GetItemData();
            _allItemsNamesCollection.Add(playerVehicle.DeffItemName);
        }
        foreach (SchemeData item in _gameItems.SchemeData)
        {
            _allItemsNamesCollection.Add(item.SchemeName);
        }
    }

    public bool CheckExistings(string ItemName)
    {
        if(_allItemsNamesCollection.Contains(ItemName)) return true;
        else return false;
    }
}
