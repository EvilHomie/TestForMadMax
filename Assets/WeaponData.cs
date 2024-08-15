using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon")]
public class WeaponData : ScriptableObject, IItemData
{
    public string weaponName;
    public WeaponType weaponType;
    public Raritie weaponRaritie;
    public Sprite weaponSprite;

    public AudioClip shootSound;
    public AudioClip hitSound;
    public float shakeOnShootIntensity = 0.2f;

    [Header("HULL DMG")]
    public float hullDmgByLvl;
    public int hullDmgCurLvl;
    public int hullDmgMaxLvl;

    [Header("SHIELD DMG")]
    public float shieldDmgByLvl;
    public int shieldDmgCurLvl;
    public int shieldDmgMaxLvl;

    [Header("FIRERATE")]
    public float fireRateByLvl;
    public int fireRateCurtLvl;
    public int fireRateMaxLvl;

    [Header("ROTATION SPEED")]
    public float rotationSpeedByLvl;
    public int rotationSpeedCurLvl;
    public int rotationSpeedMaxLvl;

    //[Header("UPGRADES")]
    //public float upgradeMod;

    public string ItemName => weaponName;

    public Sprite ItemSprite => weaponSprite;
}
