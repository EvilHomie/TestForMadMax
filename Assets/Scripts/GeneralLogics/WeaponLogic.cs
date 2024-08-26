using System.Collections;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    [SerializeField] protected FirePointManager[] _firePoints;
    [SerializeField] AudioClip _shootSound;
    [SerializeField] AudioClip _hitSound;


    bool _isShooting = false;
    float _nextTimeTofire = 0;
    int _lastShootBarrelNumber = 0;
    protected virtual float CurHullDmg { get; }

    protected virtual float CurShieldDmg { get; }

    protected virtual float CurFireRate { get; }

    public virtual float RotationSpeed { get; }


    protected void ShootAsPlayer(float shakeOnShootDuration, float shakeOnShootIntensity, WeaponType weaponType)
    {
        _isShooting = true;
        if (weaponType == WeaponType.SingleBarreled)
        {
            StartCoroutine(SingleBarreledShootAsPlayer(shakeOnShootDuration, shakeOnShootIntensity));
        }
        else if (weaponType == WeaponType.MultyBarreled)
        {
            StartCoroutine(MultyBarreledShootAsPlayer(shakeOnShootDuration, shakeOnShootIntensity, _firePoints.Length));
        }
        else if (weaponType == WeaponType.Beam)
        {

        }
    }

    protected void ShootAsBot(WeaponType weaponType, float accuracy)
    {
        _isShooting = true;
        if (weaponType == WeaponType.SingleBarreled)
        {
            StartCoroutine(SingleBarreledShootAsBot(accuracy));
        }
        else if (weaponType == WeaponType.MultyBarreled)
        {
            StartCoroutine(MultyBarreledShootAsBot(_firePoints.Length, accuracy));
        }
        else if (weaponType == WeaponType.Beam)
        {

        }
    }

    IEnumerator SingleBarreledShootAsPlayer(float shakeOnShootDuration, float shakeOnShootIntensity)
    {
        while (_isShooting)
        {
            if (Time.time >= _nextTimeTofire)
            {
                CameraManager.Instance.Shake(shakeOnShootDuration, shakeOnShootIntensity);
                _firePoints[0].OneShoot(_shootSound);
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo))
                {
                    hitInfo.collider.GetComponent<IDamageable>()?.OnHit(CurHullDmg, CurShieldDmg, _hitSound);
                }
                _nextTimeTofire = Time.time + 1f / CurFireRate;
            }
            yield return null;
        }
    }

    IEnumerator MultyBarreledShootAsPlayer(float shakeOnShootDuration, float shakeOnShootIntensity, float barrelCount)
    {
        while (_isShooting)
        {
            if (Time.time >= _nextTimeTofire)
            {
                CameraManager.Instance.Shake(shakeOnShootDuration, shakeOnShootIntensity);

                if (_lastShootBarrelNumber >= barrelCount) _lastShootBarrelNumber = 0;

                _firePoints[_lastShootBarrelNumber].OneShoot(_shootSound);
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

    IEnumerator SingleBarreledShootAsBot(float accuracy = 0)
    {
        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        while (_isShooting)
        {
            if (Time.time >= _nextTimeTofire)
            {
                _firePoints[0].OneShoot(_shootSound);

                if (accuracy != 0)
                {
                    bool randomDirection = Random.value <= accuracy / 100;
                    if (randomDirection) PlayerHPManager.Instance.OnHit(CurHullDmg, CurShieldDmg, _hitSound);
                }
                else PlayerHPManager.Instance.OnHit(CurHullDmg, CurShieldDmg, _hitSound);


                _nextTimeTofire = Time.time + 1f / CurFireRate;
            }
            yield return null;
        }
    }

    IEnumerator MultyBarreledShootAsBot(float barrelCount, float accuracy = 0)
    {
        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        while (_isShooting)
        {
            if (Time.time >= _nextTimeTofire)
            {
                if (_lastShootBarrelNumber >= barrelCount) _lastShootBarrelNumber = 0;

                _firePoints[_lastShootBarrelNumber].OneShoot(_shootSound);
                if (accuracy != 0)
                {
                    bool randomDirection = Random.value <= accuracy / 100;
                    if (randomDirection) PlayerHPManager.Instance.OnHit(CurHullDmg, CurShieldDmg, _hitSound);
                }
                else PlayerHPManager.Instance.OnHit(CurHullDmg, CurShieldDmg, _hitSound);

                _lastShootBarrelNumber++;
                _nextTimeTofire = Time.time + 1f / CurFireRate;
            }
            yield return null;
        }
    }

    protected void OnStopShooting()
    {
        _isShooting = false;
    }
}
