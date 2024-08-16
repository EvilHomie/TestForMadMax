using UnityEngine;

public class PlayerWeapon : WeaponLogic, IItem
{
    [SerializeField] WeaponData _weaponData;    
    [SerializeField] GameObject _targetMarker;
    [SerializeField] Vector3 _observerPos;    
    [SerializeField] float _shakeOnShootIntensity = 0.2f;

    protected override float CurHullDmg => _weaponData.hullDmgByLvl * _weaponData.hullDmgCurLvl;
    protected override float CurShieldDmg => _weaponData.shieldDmgByLvl * _weaponData.shieldDmgCurLvl;
    protected override float CurFireRate => _weaponData.fireRateByLvl * _weaponData.fireRateCurtLvl;

    public override float RotationSpeed => _weaponData.rotationSpeedByLvl * _weaponData.rotationSpeedCurLvl;
    public Vector3 ObserverPos => _observerPos;
    public GameObject TargetMarker => _targetMarker;

    public void StartShooting()
    {
        _isShooting = true;
        
        if (_firePoints.Length == 1)
        {
            StartCoroutine(SingleBarreledShoot(_shakeOnShootIntensity));
        }
        else
        {
            StartCoroutine(MultyBarreledShoot(_firePoints.Length, _shakeOnShootIntensity));
        }
    }

    public void StopShooting()
    {
        _isShooting = false;
    }

    public object GetItemData()
    {
        return _weaponData;
    }

    public void SetItemData(object obj)
    {
        _weaponData = obj as WeaponData;
    }
}
