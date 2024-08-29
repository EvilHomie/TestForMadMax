using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSchemeData", menuName = "ScriptableObjects/WeaponSchemeData")]
public class WeaponSchemeData : ScriptableObject, IItemData
{
    public WeaponData weaponData;
    public int copperAmountForUnlock;
    public int wiresAmountForUnlock;
    public int scrapMetalAmountForUnlock;

    public int dropChanceIfDuplicate;
    public string TranslatedItemName
    {
        get
        {
            if (TextConstants.Language == Language.ru) return weaponData.weaponNameRU;
            else return weaponData.deffWeaponName;
        }
    }

    public string DeffItemName => $"{weaponData.deffWeaponName}_Scheme";
}
