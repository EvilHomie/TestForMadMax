using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;


    Dictionary<ResourcesType, int> _availableResources = new();
    Dictionary<string, IItem> _availableItemsByName = new();

    List<WeaponData> _availableWeaponsByName = new();

    public Dictionary<ResourcesType, int> AvailableResources => _availableResources;

    public Dictionary<string, IItem> AvailableItemsByName => _availableItemsByName;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }



    public void FillResources()
    {

    }

    public void FillItems(GameItems gameItems)
    {





    }

    public void FillFromDeffaultData(GameItems gameItems, string weaponName)
    {
        PlayerWeapon weapon = gameItems.playerWeapons.Find(weapon => weapon.name == weaponName);
        WeaponData copyData = Instantiate((WeaponData)weapon.GetItemData());
        //weapon.SetItemCopyData(copyData);

        _availableItemsByName.Add(weapon.name, weapon);


        //WeaponData weaponData = (WeaponData)_availableItemsByName[weapon.name].GetItemData();

        copyData.fireRateByLvl = 19;

        Debug.LogWarning(gameItems.test);
        //Debug.LogWarning(weaponData.fireRateMaxLvl);

        PlayerWeapon playerWeapon = Instantiate(weapon);
        playerWeapon.SetItemCopyData(copyData);
        WeaponData newweaponData = (WeaponData)playerWeapon.GetItemData();
        Debug.LogWarning(newweaponData.fireRateByLvl);
    }
}
