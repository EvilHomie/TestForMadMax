using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    Dictionary<ResourcesType, int> _availableResources = new();
    Dictionary<string, IItem> _availableItemsByName = new();

    List<IItemData> _playerWeaponsData = new();    

    public Dictionary<string, IItem> AvailableItemsByName => _availableItemsByName;



    public List<IItemData> PlayerWeaponsData => _playerWeaponsData;
    public Dictionary<ResourcesType, int> AvailableResources => _availableResources;





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

    public void FillPlayerItemsData(GameItems gameItems, string[] ItemsNames)
    {
        foreach (var itemName in ItemsNames) 
        {
            Weapon weapon = gameItems.Weapons.Find(weapon => weapon.name == itemName);
            if(weapon != null) _playerWeaponsData.Add(Instantiate((WeaponData)weapon.GetItemData()));
        }

        





        //PlayerWeapon weapon = gameItems.playerWeapons.Find(weapon => weapon.name == weaponName);
        //WeaponData copyData = Instantiate((WeaponData)weapon.GetItemData());




        ////weapon.SetItemCopyData(copyData);

        //_availableItemsByName.Add(weapon.name, weapon);


        ////WeaponData weaponData = (WeaponData)_availableItemsByName[weapon.name].GetItemData();

        //copyData.fireRateByLvl = 19;

        //Debug.LogWarning(gameItems.test);
        ////Debug.LogWarning(weaponData.fireRateMaxLvl);

        //PlayerWeapon playerWeapon = Instantiate(weapon);
        //playerWeapon.SetItemCopyData(copyData);
        //WeaponData newweaponData = (WeaponData)playerWeapon.GetItemData();
        //Debug.LogWarning(newweaponData.fireRateByLvl);
    }
}
