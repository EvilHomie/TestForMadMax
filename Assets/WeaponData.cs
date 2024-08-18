using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon")]
public class WeaponData : ScriptableObject, IItemData
{
    public string weaponName;
    public WeaponType weaponType;
    public Raritie weaponRaritie;

    [Header(Constants.HULLDMG)]
    public float hullDmgByLvl;
    public int hullDmgCurLvl;
    public int hullDmgMaxLvl;

    [Header(Constants.SHIELDDMG)]
    public float shieldDmgByLvl;
    public int shieldDmgCurLvl;
    public int shieldDmgMaxLvl;

    [Header(Constants.FIRERATE)]
    public float fireRateByLvl;
    public int fireRateCurtLvl;
    public int fireRateMaxLvl;

    [Header(Constants.ROTATIONSPEED)]
    public float rotationSpeedByLvl;
    public int rotationSpeedCurLvl;
    public int rotationSpeedMaxLvl;

    public string ItemName => weaponName;
}
