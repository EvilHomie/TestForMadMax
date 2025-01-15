using System.Collections;
using System.Linq;
using UnityEngine;

public class ProjectileWeaponNotMiniGun : AbstractWeapon
{
    [SerializeField] float _fireRate;
    [SerializeField] int _magCapacity;

    float _nextTimeTofire;

    bool _isReloading;


    AbstractAmmunitionBelt _abstractAmmunitionBelt;


    public override void InitAsPlayerWeapon(AbstractAmmunitionBelt abstractAmmunitionBelt)
    {
        _abstractAmmunitionBelt = abstractAmmunitionBelt;
        _abstractAmmunitionBelt.Init(OnFinishReload);
        _isReloading = _abstractAmmunitionBelt.IsReloading;
        foreach (var fp in _firePoints)
        {
            fp.Init(_audioSource, _animationCurve, _shootSound);
        }
    }

    public override void OnStartSurviveMode()
    {
        _nextTimeTofire = 0;
        _bulletInMagCount = _magCapacity;
        _abstractAmmunitionBelt.OnStartSurviveMode(_magCapacity);
    }

    public override void SetValues(SMWeaponData data)
    {
        _kineticDamage = data.kineticDamage;
        _energyDamage = data.energyDamage;
        _fireRate = data.fireRate;
        _reloadTime = data.reloadTime;
        _bulletInMagCount = data.leftBullets;

        if (_magCapacity != 0 && _magCapacity != data.magCapacity)
        {
            int diff = data.magCapacity - _magCapacity;
            _bulletInMagCount += diff;
            _abstractAmmunitionBelt.OnChangeMagCapacity(data.magCapacity);
        }

        _magCapacity = data.magCapacity;
    }

    public override void UpdateValues(WeaponData weaponData)
    {
        throw new System.NotImplementedException();
    }

    public override void StartShoot()
    {
        if (_isShooting) return;
        _isShooting = true;
        StartCoroutine(ShootLogic());
    }

    public override void StopShoot()
    {
        _isShooting = false;
        StopAllCoroutines();
    }

    public override void Reload()
    {
        if (_isReloading) return;
        if (_bulletInMagCount == _magCapacity) return;
        _bulletInMagCount = 0;
        _isReloading = true;
        _abstractAmmunitionBelt.OnReload(_reloadTime);
    }

    IEnumerator ShootLogic()
    {
        int barrelCount = _firePoints.Count();
        int lastShootBarrelNumber = 0;

        while (_isShooting)
        {
            if (Time.time >= _nextTimeTofire)
            {
                if (_bulletInMagCount > 0)
                {
                    OnShootEffects(lastShootBarrelNumber);
                    CheckHit(lastShootBarrelNumber);

                    _abstractAmmunitionBelt.OnShoot(_magCapacity - _bulletInMagCount, _fireRate);
                    _bulletInMagCount--;
                    _nextTimeTofire = Time.time + 1f / _fireRate;

                    if (barrelCount > 1)
                    {
                        lastShootBarrelNumber++;
                        if (lastShootBarrelNumber >= barrelCount) lastShootBarrelNumber = 0;
                    }
                }
                else if (!_isReloading)
                {
                    Reload();
                }
            }
            yield return null;
        }
    }

    void OnFinishReload()
    {
        _isReloading = false;
        _bulletInMagCount = _magCapacity;
    }

    public override void OnShootEffects(int firepointIndex)
    {
        CameraManager.Instance.Shake(_shakeOnShootDuration, _shakeOnShootIntensity);
        _firePoints[firepointIndex].PlayEffects(_fireRate);
    }
}
