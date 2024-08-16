using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon, IItem
{
    [SerializeField] WeaponData _weaponData;
    [SerializeField] FirePointManager[] _firePoints;
    [SerializeField] GameObject _targetMarker;
    [SerializeField] Vector3 _observerPos;
    [SerializeField] AudioClip _shootSound;
    [SerializeField] AudioClip _hitSound;
    [SerializeField] float _shakeOnShootIntensity = 0.2f;

    bool _isShooting = false;
    float _nextTimeTofire = 0;
    int _lastShootBarrelNumber = 0;

    float CurHullDmg => _weaponData.hullDmgByLvl * _weaponData.hullDmgCurLvl;

    float CurShieldDmg => _weaponData.shieldDmgByLvl * _weaponData.shieldDmgCurLvl;

    float CurFireRate => _weaponData.fireRateByLvl * _weaponData.fireRateCurtLvl;

    public float RotationSpeed => _weaponData.rotationSpeedByLvl * _weaponData.rotationSpeedCurLvl;
    public Vector3 ObserverPos => _observerPos;

    public GameObject TargetMarker => _targetMarker;

    public void StartShooting()
    {
        _isShooting = true;
        
        if (_firePoints.Length == 1)
        {
            StartCoroutine(SingleBarreledShoot());
        }
        else
        {
            StartCoroutine(MultyBarreledShoot(_firePoints.Length));
        }
    }

    public void StopShooting()
    {
        _isShooting = false;
    }


    IEnumerator SingleBarreledShoot()
    {
        while (_isShooting) 
        {
            if (Time.time >= _nextTimeTofire)
            {                
                _firePoints[0].OneShoot(_shootSound);
                CameraManager.Instance.Shake(1f / CurFireRate, _shakeOnShootIntensity);
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo))
                {
                    hitInfo.collider.GetComponent<IDamageable>()?.OnHit(CurHullDmg, CurShieldDmg, _hitSound);
                }
                _nextTimeTofire = Time.time + 1f / CurFireRate;
            }            
            yield return null;
        }
    }

    IEnumerator MultyBarreledShoot(float BarrelCount)
    {        
        while (_isShooting)
        {
            if (Time.time >= _nextTimeTofire)
            {
                if (_lastShootBarrelNumber >= BarrelCount) _lastShootBarrelNumber = 0;

                _firePoints[_lastShootBarrelNumber].OneShoot(_shootSound);
                CameraManager.Instance.Shake(1f / CurFireRate, _shakeOnShootIntensity);
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo))
                {
                    hitInfo.collider.GetComponent<IDamageable>()?.OnHit(CurHullDmg, CurShieldDmg, _hitSound);
                }
                _lastShootBarrelNumber++;
                _nextTimeTofire = Time.time + 1f / CurFireRate;
            }
            yield return null;
        }
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
