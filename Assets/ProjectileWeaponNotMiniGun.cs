using System.Linq;
using UnityEngine;

public class ProjectileWeaponNotMiniGun : AbstractWeapon
{
    [SerializeField] float _fireRate;

    public override void Init(SMWeaponData weaponData)
    {
        foreach (var fp in _firePoints)
        {
            fp.Init(_audioSource, _animationCurve, _shootSound);
        }
        _barrelsCount = _firePoints.Count();
        _shootBarrelNumber = 0;
        UpdateValues(weaponData);
    }

    public override void UpdateValues(SMWeaponData weaponData)
    {
        _kineticDamage = weaponData.kineticDamage;
        _energyDamage = weaponData.energyDamage;
        _fireRate = weaponData.fireRate;
        _reloadTime = weaponData.reloadTime;
    }

    public override void Shoot(bool asPlayer)
    {
        if (asPlayer)
        {
            CameraManager.Instance.Shake(_shakeOnShootDuration, _shakeOnShootIntensity);
            _firePoints[_shootBarrelNumber].PlayEffects(_fireRate);
            TryHitEnemy(_shootBarrelNumber);
        }
        else
        {
            TryHitPlayer();
        }

        if (_barrelsCount > 1)
        {
            _shootBarrelNumber++;
            if (_shootBarrelNumber >= _barrelsCount) _shootBarrelNumber = 0;
        }
    }

    public override void StopShoot()
    {
        foreach (var firePoint in _firePoints)
        {
            firePoint.StopEffects();
        }
    }
}
