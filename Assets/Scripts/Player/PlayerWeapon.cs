using UnityEngine;

public class PlayerWeapon : WeaponLogic, IItem
{
    [SerializeField] WeaponData _weaponData;    
    [SerializeField] LineRenderer _targetMarkerLine;
    [SerializeField] Vector3 _observerPos;    
    [SerializeField] float _shakeOnShootIntensity = 0.2f;
    [SerializeField] float _shakeOnShootDuration = 0.1f;

    [SerializeField] Transform _baseTransform;
    [SerializeField] Transform _turretTransform;

    protected override float CurHullDmg => _weaponData.hullDmgByLvl * _weaponData.hullDmgCurLvl;
    protected override float CurShieldDmg => _weaponData.shieldDmgByLvl * _weaponData.shieldDmgCurLvl;
    protected override float CurFireRate => _weaponData.fireRateByLvl * _weaponData.fireRateCurtLvl;

    public override float RotationSpeed => _weaponData.rotationSpeedByLvl * _weaponData.rotationSpeedCurLvl;
    public Vector3 ObserverPos => _observerPos;
    public LineRenderer TargetMarkerLine => _targetMarkerLine;

    public FirePointManager[] FirePointsManagers => _firePointManagers;

    public Transform BaseTransform => _baseTransform;
    public Transform TurretTransform => _turretTransform;

    public void StartShooting()
    {
        ShootAsPlayer(_shakeOnShootDuration, _shakeOnShootIntensity, _weaponData.weaponType);
    }

    public void StopShooting()
    {
        OnStopShooting();
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
