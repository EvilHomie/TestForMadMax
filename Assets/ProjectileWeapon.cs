using System.Collections;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] string _weaponName;
    [SerializeField] WeaponType _weaponType;
    [SerializeField] Raritie _weaponRaritie;

    [SerializeField] AudioClip _shootSound;
    [SerializeField] AudioClip _hitSound;
    [SerializeField] FirePointManager[] _firePoints;
    [SerializeField] float _shakeOnShootIntensity = 0.2f;

    [SerializeField] GameObject _targetMarker;

    [SerializeField] Vector3 _observerPos;

    [Header("HULL DMG")]
    [SerializeField] float _hullDmgByLvl;
    [SerializeField] float _hullDmgCurLvl;
    [SerializeField] float _hullDmgMaxLvl;

    [Header("SHIELD DMG")]
    [SerializeField] float _shieldDmgByLvl;
    [SerializeField] float _shieldDmgCurLvl;
    [SerializeField] float _shieldDmgMaxLvl;

    [Header("FIRERATE")]
    [SerializeField] float _fireRateByLvl;
    [SerializeField] float _fireRateCurtLvl;
    [SerializeField] float _fireRateMaxLvl;

    [Header("ROTATION SPEED")]
    [SerializeField] float _rotationSpeedByLvl;
    [SerializeField] float _rotationSpeedCurLvl;
    [SerializeField] float _rotationSpeedMaxLvl;

    bool _isShooting = false;
    float _nextTimeTofire = 0;
    int _lastShootBarrelNumber = 0;

    float CurHullDmg => _hullDmgByLvl * _hullDmgCurLvl;

    float CurShieldDmg => _shieldDmgByLvl * _shieldDmgCurLvl;

    float CurFireRate => _fireRateByLvl * _fireRateCurtLvl;

    public float RotationSpeed => _rotationSpeedByLvl * _rotationSpeedCurLvl;
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
            //yield return new WaitForSeconds(CurFireRate);
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

            //yield return new WaitForSeconds(CurFireRate / BarrelCount);
        }
    }
}
