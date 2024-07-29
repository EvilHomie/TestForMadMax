using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSchemeData", menuName = "ScriptableObjects/WeaponSchemeData")]
public class WeaponSchemeData : SchemeData, IItemData 
{
    public WeaponData weaponData;

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

    public override string SchemeName => DeffItemName;
    public override string ItemNameInScheme => weaponData.deffWeaponName;
}
