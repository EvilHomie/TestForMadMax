using System.Collections;
using UnityEngine;

public class PlayerShootLogic : AbstractShootLogic
{
    public static PlayerShootLogic Instance;
    [SerializeField] AbstractAmmunitionBelt _abstractAmmunitionBelt;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _abstractAmmunitionBelt.Init();
    }

    public void OnChangeWeapon(AbstractWeapon abstractWeapon, SMWeaponData weaponData)
    {
        _abstractWeapon = abstractWeapon;
        _abstractWeapon.Init(weaponData);
        _fireRate = weaponData.fireRate;
        _reloadTime = weaponData.fireRate;
        _magCapacity = weaponData.magCapacity;
    }

    public override void OnWeaponUpgrade(SMWeaponData weaponData)
    {
        _abstractWeapon.UpdateValues(weaponData);
        _fireRate = weaponData.fireRate;
        _reloadTime = weaponData.reloadTime;

        if (_magCapacity != 0 && _magCapacity != weaponData.magCapacity)
        {
            int diff = weaponData.magCapacity - _magCapacity;
            _bulletInMagLeft += diff;
            _abstractAmmunitionBelt.OnChangeMagCapacity(weaponData.magCapacity);
        }

        _magCapacity = weaponData.magCapacity;
    }

    public void OnStartSurviveMode()
    {
        _nextTimeTofire = 0;
        _bulletInMagLeft = _magCapacity;
        _abstractAmmunitionBelt.OnStartSurviveMode(_magCapacity);
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
        if (_bulletInMagLeft == _magCapacity) return;
        _bulletInMagLeft = 0;
        _isReloading = true;
        _abstractAmmunitionBelt.OnReload(_reloadTime, OnFinishReload);
    }

    void OnFinishReload()
    {
        _isReloading = false;
        _bulletInMagLeft = _magCapacity;
    }

    IEnumerator ShootLogic()
    {
        while (_isShooting)
        {
            if (Time.time >= _nextTimeTofire)
            {
                if (_bulletInMagLeft > 0)
                {
                    _abstractWeapon.Shoot(true);
                    _abstractAmmunitionBelt.OnShoot(_magCapacity - _bulletInMagLeft, _fireRate);
                    _bulletInMagLeft--;
                    _nextTimeTofire = Time.time + 1f / _fireRate;
                }
                else if (!_isReloading)
                {
                    Reload();
                }
            }
            yield return null;
        }
    }
}
