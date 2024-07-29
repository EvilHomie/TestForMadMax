using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : ScriptableObject, IItemData
{
    public string deffWeaponName;
    public string weaponNameEN;
    public string weaponNameRU;
    public WeaponType weaponType;
    public Raritie weaponRaritie;

    [Header("HULLDMG")]
    public float hullDmgByLvl;
    public int hullDmgCurLvl;
    public int hullDmgMaxLvl;

    [Header("SHIELDDMG")]
    public float shieldDmgByLvl;
    public int shieldDmgCurLvl;
    public int shieldDmgMaxLvl;

    [Header("FIRERATE")]
    public float fireRateByLvl;
    public int fireRateCurtLvl;
    public int fireRateMaxLvl;

    [Header("ROTATIONSPEED")]
    public float rotationSpeedByLvl;
    public int rotationSpeedCurLvl;
    public int rotationSpeedMaxLvl;

    public string TranslatedItemName
    {
        get
        {
            if (TextConstants.Language == Language.ru) return weaponNameRU;
            else return weaponNameEN;
        }
    }

    public string DeffItemName => deffWeaponName;

    public void SetData(WeaponDataForSave weaponData)
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
