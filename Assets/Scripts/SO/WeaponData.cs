using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon")]
public class WeaponData : ScriptableObject, IItemData
{
    public string weaponName;
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

    public string ItemName => weaponName;
}
