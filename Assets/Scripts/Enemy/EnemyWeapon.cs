using UnityEngine;

public class EnemyWeapon : WeaponLogic
{
    [SerializeField] WeaponType weaponType;
    [SerializeField] float _hullDmg;
    [SerializeField] float _shieldDmg;
    [SerializeField] float _fireRate;
    protected override float CurHullDmg => _hullDmg * LevelManager.Instance.GetSelectedLevelinfo().EnemyDmgMod;
    protected override float CurShieldDmg => _shieldDmg * LevelManager.Instance.GetSelectedLevelinfo().EnemyDmgMod;
    protected override float CurFireRate => _fireRate;

    public void StartShooting(float accuracy)
    {
        ShootAsBot(weaponType, accuracy);
    }

    public void StopShooting()
    {
        OnStopShooting();
    }
}
