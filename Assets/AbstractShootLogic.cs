using UnityEngine;

public abstract class AbstractShootLogic : MonoBehaviour
{
    protected bool _isShooting;
    protected bool _isReloading;
    protected float _nextTimeTofire;
    protected float _fireRate;
    protected float _reloadTime;
    protected int _bulletInMagLeft;
    protected int _magCapacity;
    protected AbstractWeapon _abstractWeapon;


    public abstract void OnWeaponUpgrade(SMWeaponData weaponData);

    public abstract void StartShoot();

    public abstract void StopShoot();

    public abstract void Reload();
}
