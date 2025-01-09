using UnityEngine;

public class ProjectileWeaponNotMiniGun : AbstractWeapon
{
    [SerializeField] float _fireRate;
    [SerializeField] AbstarctFirePointEffects[] _firePoints;



    public void TEMPSetValues(float kineticDamage, float energyDamage, float fireRate, float timeBeforeReload, float reloadTime)
    {
        _kineticDamage = kineticDamage;
        _energyDamage = energyDamage;
        _fireRate = fireRate;
        _timeBeforeReload = timeBeforeReload;
        _reloadTime = reloadTime;
    }

    public override void UpdateValues(WeaponData weaponData)
    {
        throw new System.NotImplementedException();
    }

    public override void StartShoot()
    {
        Debug.Log("SHOOT");
    }

    public override void StopShoot()
    {
        throw new System.NotImplementedException();
    }

    
}
