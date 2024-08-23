using UnityEngine;

public class EnemyWeapon : WeaponLogic
{
    [SerializeField] WeaponType weaponType;
    [SerializeField] float _hullDmg;
    [SerializeField] float _shieldDmg;
    [SerializeField] float _fireRate;
    protected override float CurHullDmg => _hullDmg * LevelConfig.Instance.EnemyDmgMod;
    protected override float CurShieldDmg => _shieldDmg * LevelConfig.Instance.EnemyDmgMod;
    protected override float CurFireRate => _fireRate * LevelConfig.Instance.EnemyFireRateMod;

    public void StartShooting(float accuracy)
    {
        ShootAsBot(weaponType, accuracy);
    }

    public void StopShooting()
    {
        OnStopShooting();
    }
}
