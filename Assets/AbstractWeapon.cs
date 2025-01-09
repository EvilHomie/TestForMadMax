using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
    [SerializeField] protected float _kineticDamage;
    [SerializeField] protected float _energyDamage;
    
    [SerializeField] protected float _timeBeforeReload;
    [SerializeField] protected float _reloadTime;

    public abstract void StartShoot();
    public abstract void StopShoot();
    public abstract void UpdateValues(WeaponData weaponData);
}
