using UnityEngine;

public class EnemyWeapon : WeaponLogic
{
    [SerializeField] WeaponType weaponType;
    [SerializeField] float _hullDmg;
    [SerializeField] float _shieldDmg;
    [SerializeField] float _fireRate;
    protected override float CurHullDmg => _hullDmg;
    protected override float CurShieldDmg => _shieldDmg;
    protected override float CurFireRate => _fireRate;

    public void StartShooting()
    {
        _isShooting = true;

        if (_firePoints.Length == 1)
        {
            StartCoroutine(SingleBarreledShoot());
        }
        else
        {
            StartCoroutine(MultyBarreledShoot(_firePoints.Length));
        }
    }

    public void StopShooting()
    {
        _isShooting = false;
    }
}
