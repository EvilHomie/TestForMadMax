using System.Collections;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    [SerializeField] protected FirePointManager[] _firePointManagers;
    [SerializeField] AudioClip _shootSound;
    [SerializeField] AudioClip _hitSound;


    bool _isShooting = false;
    float _nextTimeTofire = 0;
    int _lastShootBarrelNumber = 0;
    protected virtual float CurHullDmg { get; }

    protected virtual float CurShieldDmg { get; }

    protected virtual float CurFireRate { get; }

    public virtual float RotationSpeed { get; }


    protected void ShootAsPlayer(float shakeOnShootDuration, float shakeOnShootIntensity, WeaponType weaponType, ParticleSystem hitFXEffect)
    {
        _isShooting = true;
        if (weaponType == WeaponType.SingleBarreled)
        {
            StartCoroutine(SimpleSingleBarreledShootAsPlayer(shakeOnShootDuration, shakeOnShootIntensity, hitFXEffect));
        }
        else if (weaponType == WeaponType.MultyBarreled)
        {
            StartCoroutine(SimpleMultyBarreledShootAsPlayer(shakeOnShootDuration, shakeOnShootIntensity, _firePointManagers.Length, hitFXEffect));
        }
        else if (weaponType == WeaponType.RotatingSingle)
        {
            StartCoroutine(RotateSingleBarreledShootAsPlayer(shakeOnShootDuration, shakeOnShootIntensity, hitFXEffect));
        }
        else if (weaponType == WeaponType.RotatingSingle)
        {
            StartCoroutine(RotateMultyBarreledShootAsPlayer(shakeOnShootDuration, shakeOnShootIntensity, _firePointManagers.Length, hitFXEffect));
        }
    }

    protected void ShootAsBot(WeaponType weaponType)
    {
        _isShooting = true;
        if (weaponType == WeaponType.SingleBarreled)
        {
            StartCoroutine(SingleBarreledShootAsBot());
        }
        else if (weaponType == WeaponType.MultyBarreled)
        {
            StartCoroutine(MultyBarreledShootAsBot(_firePointManagers.Length));
        }
        else if (weaponType == WeaponType.Beam)
        {

        }
    }

    IEnumerator SimpleSingleBarreledShootAsPlayer(float shakeOnShootDuration, float shakeOnShootIntensity, ParticleSystem hitFXEffect)
    {
        while (_isShooting)
        {
            if (Time.time >= _nextTimeTofire)
            {
                CameraManager.Instance.Shake(shakeOnShootDuration, shakeOnShootIntensity);
                _firePointManagers[0].SimpleShoot(_shootSound, CurFireRate, true);
                if (Physics.Raycast(_firePointManagers[0].transform.position, _firePointManagers[0].transform.forward, out RaycastHit hitInfo))
                {
                    hitInfo.collider.GetComponent<IDamageable>()?.OnHit(CurHullDmg, CurShieldDmg, _hitSound);
                    Instantiate(hitFXEffect, hitInfo.point, hitFXEffect.transform.rotation);
                    hitInfo.collider.GetComponent<IHitable>()?.OnHit(hitInfo.point, _hitSound);
                }
                _nextTimeTofire = Time.time + 1f / CurFireRate;
            }
            yield return null;
        }
    }

    IEnumerator SimpleMultyBarreledShootAsPlayer(float shakeOnShootDuration, float shakeOnShootIntensity, float barrelCount, ParticleSystem hitFXEffect)
    {
        while (_isShooting)
        {
            if (Time.time >= _nextTimeTofire)
            {
                CameraManager.Instance.Shake(shakeOnShootDuration, shakeOnShootIntensity);

                if (_lastShootBarrelNumber >= barrelCount) _lastShootBarrelNumber = 0;

                _firePointManagers[_lastShootBarrelNumber].SimpleShoot(_shootSound, CurFireRate, true);
                if (Physics.Raycast(_firePointManagers[_lastShootBarrelNumber].transform.position, _firePointManagers[_lastShootBarrelNumber].transform.forward, out RaycastHit hitInfo))
                {
                    hitInfo.collider.GetComponent<IDamageable>()?.OnHit(CurHullDmg, CurShieldDmg, _hitSound);
                    Instantiate(hitFXEffect, hitInfo.point, hitFXEffect.transform.rotation);
                    hitInfo.collider.GetComponent<IHitable>()?.OnHit(hitInfo.point, _hitSound);
                }
                _lastShootBarrelNumber++;
                _nextTimeTofire = Time.time + 1f / CurFireRate;
            }
            yield return null;
        }
    }

    IEnumerator RotateSingleBarreledShootAsPlayer(float shakeOnShootDuration, float shakeOnShootIntensity, ParticleSystem hitFXEffect)
    {
        while (_isShooting)
        {
            if (Time.time >= _nextTimeTofire)
            {
                CameraManager.Instance.Shake(shakeOnShootDuration, shakeOnShootIntensity);
                _firePointManagers[0].RotateShoot(_shootSound, CurFireRate, true);
                if (Physics.Raycast(_firePointManagers[0].transform.position, _firePointManagers[0].transform.forward, out RaycastHit hitInfo))
                {
                    hitInfo.collider.GetComponent<IDamageable>()?.OnHit(CurHullDmg, CurShieldDmg, _hitSound);
                    Instantiate(hitFXEffect, hitInfo.point, hitFXEffect.transform.rotation);
                    hitInfo.collider.GetComponent<IHitable>()?.OnHit(hitInfo.point, _hitSound);
                }
                _nextTimeTofire = Time.time + 1f / CurFireRate;
            }
            yield return null;
        }
    }

    IEnumerator RotateMultyBarreledShootAsPlayer(float shakeOnShootDuration, float shakeOnShootIntensity, float barrelCount, ParticleSystem hitFXEffect)
    {
        while (_isShooting)
        {
            if (Time.time >= _nextTimeTofire)
            {
                CameraManager.Instance.Shake(shakeOnShootDuration, shakeOnShootIntensity);

                if (_lastShootBarrelNumber >= barrelCount) _lastShootBarrelNumber = 0;

                _firePointManagers[_lastShootBarrelNumber].RotateShoot(_shootSound, CurFireRate, true);
                if (Physics.Raycast(_firePointManagers[_lastShootBarrelNumber].transform.position, _firePointManagers[_lastShootBarrelNumber].transform.forward, out RaycastHit hitInfo))
                {
                    hitInfo.collider.GetComponent<IDamageable>()?.OnHit(CurHullDmg, CurShieldDmg, _hitSound);
                    Instantiate(hitFXEffect, hitInfo.point, hitFXEffect.transform.rotation);
                    hitInfo.collider.GetComponent<IHitable>()?.OnHit(hitInfo.point, _hitSound);
                }
                _lastShootBarrelNumber++;
                _nextTimeTofire = Time.time + 1f / CurFireRate;
            }
            yield return null;
        }
    }

    IEnumerator SingleBarreledShootAsBot()
    {
        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        while (_isShooting && !PlayerHPManager.Instance.IsDead)
        {
            if (Time.time >= _nextTimeTofire)
            {
                _firePointManagers[0].SimpleShoot(_shootSound, CurFireRate);               
                PlayerHPManager.Instance.OnHit(CurHullDmg, CurShieldDmg, _hitSound);


                _nextTimeTofire = Time.time + 1f / CurFireRate;
            }
            yield return null;
        }
    }

    IEnumerator MultyBarreledShootAsBot(float barrelCount)
    {
        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        while (_isShooting && !PlayerHPManager.Instance.IsDead)
        {
            if (Time.time >= _nextTimeTofire)
            {
                if (_lastShootBarrelNumber >= barrelCount) _lastShootBarrelNumber = 0;

                _firePointManagers[_lastShootBarrelNumber].SimpleShoot(_shootSound, CurFireRate);
                PlayerHPManager.Instance.OnHit(CurHullDmg, CurShieldDmg, _hitSound);

                _lastShootBarrelNumber++;
                _nextTimeTofire = Time.time + 1f / CurFireRate;
            }
            yield return null;
        }
    }



    protected void OnStopShooting()
    {
        _isShooting = false;
        foreach (var firePointManager in _firePointManagers)
        {
            firePointManager.StopShooting();
        }
    }
}
