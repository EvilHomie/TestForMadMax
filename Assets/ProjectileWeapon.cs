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

    [SerializeField] Vector3 _cameraPos;

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

    float CurHullDmg => _hullDmgByLvl * _hullDmgCurLvl;

    float CurShieldDmg => _shieldDmgByLvl * _shieldDmgCurLvl;

    float CurFireRate => _fireRateByLvl * _fireRateCurtLvl;

    public float RotationSpeed => _rotationSpeedByLvl * _rotationSpeedCurLvl;

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
                ShakeCamera.Instance.Shake(1f / CurFireRate, _shakeOnShootIntensity);
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
        int lastBarrelNumber = 0;

        while (_isShooting)
        {
            if (Time.time >= _nextTimeTofire)
            {
                if (lastBarrelNumber >= BarrelCount) lastBarrelNumber = 0;

                _firePoints[lastBarrelNumber].OneShoot(_shootSound);
                ShakeCamera.Instance.Shake(1f / CurFireRate, _shakeOnShootIntensity);
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo))
                {
                    hitInfo.collider.GetComponent<IDamageable>()?.OnHit(CurHullDmg, CurShieldDmg, _hitSound);
                }
                lastBarrelNumber++;
                _nextTimeTofire = Time.time + 1f / CurFireRate;
            }
            yield return null;

            //yield return new WaitForSeconds(CurFireRate / BarrelCount);
        }
    }
}
