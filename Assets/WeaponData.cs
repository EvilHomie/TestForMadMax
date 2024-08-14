using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon")]
public class WeaponData : ScriptableObject
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
    public float hullDmgCurLvl;
    public float hullDmgMaxLvl;

    [Header("SHIELD DMG")]
    public float shieldDmgByLvl;
    public float shieldDmgCurLvl;
    public float shieldDmgMaxLvl;

    [Header("FIRERATE")]
    public float fireRateByLvl;
    public float fireRateCurtLvl;
    public float fireRateMaxLvl;

    [Header("ROTATION SPEED")]
    public float rotationSpeedByLvl;
    public float rotationSpeedCurLvl;
    public float rotationSpeedMaxLvl;
}
