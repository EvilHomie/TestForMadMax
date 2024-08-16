using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon")]
public class WeaponData : ScriptableObject, IItemData
{
    [SerializeField] string weaponName;
    [SerializeField] WeaponType weaponType;
    [SerializeField] Raritie weaponRaritie;

    [Header(Constants.HULLDMG)]
    public float hullDmgByLvl;
    public int hullDmgCurLvl;
    [SerializeField] int _hullDmgMaxLvl;

    [Header(Constants.SHIELDDMG)]
    public float shieldDmgByLvl;
    public int shieldDmgCurLvl;
    [SerializeField] int _shieldDmgMaxLvl;

    [Header(Constants.FIRERATE)]
    public float fireRateByLvl;
    public int fireRateCurtLvl;
    [SerializeField] int _fireRateMaxLvl;

    [Header(Constants.ROTATIONSPEED)]
    public float rotationSpeedByLvl;
    public int rotationSpeedCurLvl;
    [SerializeField] int _rotationSpeedMaxLvl;

    public string ItemName => weaponName;

    public Raritie Raritie => weaponRaritie;

    public WeaponType Type => weaponType;

    public int HullDmgMaxLvl => _hullDmgMaxLvl;
    public int ShieldDmgMaxLvl => _shieldDmgMaxLvl;
    public int FireRateMaxLvl => _fireRateMaxLvl;
    public int RotationSpeedMaxLvl => _rotationSpeedMaxLvl;

}
