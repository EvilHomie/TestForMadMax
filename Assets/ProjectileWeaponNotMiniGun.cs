using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileWeaponNotMiniGun : AbstractWeapon
{
    [SerializeField] float _fireRate;
    [SerializeField] int _magCapacity;

    float _nextTimeTofire;
    int _bulletInMagCount;
    bool _isReloading;


    public override void Init()
    {
        WeaponMagazinePresentation.Instance.Init(_magCapacity);
        foreach (var fp in _firePoints)
        {
            fp.Init(_audioSource, _animationCurve, _shootSound);
        }
        _bulletInMagCount = _magCapacity;
        _nextTimeTofire = 0;
    }

    public void TEMPSetValues(SMWeaponData data)
    {
        _kineticDamage = data.kineticDamage;
        _energyDamage = data.energyDamage;
        _fireRate = data.fireRate;
        _magCapacity = data.magCapacity;
        _reloadTime = data.reloadTime;
        _bulletInMagCount = _magCapacity;
        WeaponMagazinePresentation.Instance.OnChangeMagCapacity(_magCapacity);
    }

    public override void UpdateValues(WeaponData weaponData)
    {
        throw new System.NotImplementedException();
    }

    public override void StartShoot()
    {
        _isShooting = true;
        StartCoroutine(ShootAsPlayerLogic());
    }

    public override void StopShoot()
    {
        _isShooting = false;
    }

    public override void Reload()
    {
        if(_isReloading) return;
        if(_bulletInMagCount == _magCapacity) return;
        StartCoroutine(ReloadLogic());
    }

    IEnumerator ShootAsPlayerLogic()
    {
        int barrelCount = _firePoints.Count();
        int lastShootBarrelNumber = 0;

        while (_isShooting)
        {
            if (_bulletInMagCount > 0)
            {
                if (Time.time >= _nextTimeTofire)
                {
                    OnShootEffects(lastShootBarrelNumber);
                    CheckHit(lastShootBarrelNumber);

                    _bulletInMagCount--;
                    WeaponMagazinePresentation.Instance.OnShoot(_bulletInMagCount);
                    _nextTimeTofire = Time.time + 1f / _fireRate;

                    if (barrelCount > 1)
                    {
                        lastShootBarrelNumber++;
                        if (lastShootBarrelNumber >= barrelCount) lastShootBarrelNumber = 0;
                    }
                }
            }
            else if(!_isReloading)
            {
                StartCoroutine(ReloadLogic());
            }

            yield return null;
        }
    }

    IEnumerator ReloadLogic()
    {
        _isReloading = true;
        _bulletInMagCount = 0;
        float t = 0;
        while (t < _reloadTime)
        {
            t += Time.deltaTime;
            WeaponMagazinePresentation.Instance.ReloadAnimation();
            yield return null;
        }
        WeaponMagazinePresentation.Instance.SetFullMagazine();
        _bulletInMagCount = _magCapacity;
        _isReloading = false;
    }

    public override void OnShootEffects(int firepointIndex)
    {
        CameraManager.Instance.Shake(_shakeOnShootDuration, _shakeOnShootIntensity);
        _firePoints[firepointIndex].PlayEffects(_fireRate);
    }
}
