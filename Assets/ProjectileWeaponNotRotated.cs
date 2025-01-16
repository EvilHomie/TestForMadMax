using System.Collections;
using System.Linq;
using UnityEngine;

public class ProjectileWeaponNotRotated : AbstractPlayerWeapon
{
    AbstractAmmunitionBelt _abstractAmmunitionBelt;

    public override void Init(NewWeaponData weaponData, AbstractAmmunitionBelt abstractAmmunitionBelt)
    {
        _weaponData = weaponData;
        foreach (var fp in _firePoints)
        {
            fp.Init(_audioSource, _animationCurve, _shootSound);
        }
        _barrelsCount = _firePoints.Count();
        _shootBarrelNumber = 0;
        _abstractAmmunitionBelt = abstractAmmunitionBelt;
    }


    public override void Shoot()
    {
        if (_weaponData.isShooting) return;
        _weaponData.isShooting = true;
        StartCoroutine(ShootLogic());
    }

    public override void StopShoot()
    {
        StopAllCoroutines();
        _weaponData.isShooting = false;
        OnstopShootActions();
    }

    public override void Reload()
    {
        if (_weaponData.isReloading) return;
        if (_weaponData.bulletInMagLeft == _weaponData.magCapacity) return;
        OnReloadActions();        
    }


    IEnumerator ShootLogic()
    {
        while (_weaponData.isShooting)
        {
            if (Time.time >= _weaponData.nextTimeTofire)
            {
                if (_weaponData.bulletInMagLeft > 0)
                {
                    OnShootActions();
                }
                else if (!_weaponData.isReloading)
                {
                    OnReloadActions();
                }
            }
            yield return null;
        }
    }

    void OnShootActions()
    {
        CameraManager.Instance.Shake(_weaponData.shakeOnShootDuration, _weaponData.shakeOnShootIntensity);
        _abstractAmmunitionBelt.OnShoot();
        _firePoints[_shootBarrelNumber].PlayEffects(_weaponData.fireRate);
        TryHitEnemy(_shootBarrelNumber);

        if (_barrelsCount > 1)
        {
            _shootBarrelNumber++;
            if (_shootBarrelNumber >= _barrelsCount) _shootBarrelNumber = 0;
        }
        _weaponData.bulletInMagLeft--;
        _weaponData.nextTimeTofire = Time.time + 1f / _weaponData.fireRate;
    }

    void OnstopShootActions()
    {
        foreach (var firePoint in _firePoints)
        {
            firePoint.StopEffects();
        }
    }

    void OnReloadActions()
    {
        _abstractAmmunitionBelt.OnReload();
    }

}
