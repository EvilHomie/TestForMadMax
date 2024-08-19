using System.Collections;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    [SerializeField] protected FirePointManager[] _firePoints;
    [SerializeField] AudioClip _shootSound;
    [SerializeField] AudioClip _hitSound;

    protected bool _isShooting = false;
    float _nextTimeTofire = 0;
    int _lastShootBarrelNumber = 0;
    protected virtual float CurHullDmg {get; }

    protected virtual float CurShieldDmg {get; }

    protected virtual float CurFireRate { get; }

    public virtual float RotationSpeed { get; }

    protected IEnumerator SingleBarreledShoot(float _shakeOnShootIntensity = 0)
    {
        while (_isShooting)
        {
            if (Time.time >= _nextTimeTofire)
            {
                if (_shakeOnShootIntensity != 0) CameraManager.Instance.Shake(1f / CurFireRate, _shakeOnShootIntensity);
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

    protected IEnumerator MultyBarreledShoot(float BarrelCount, float _shakeOnShootIntensity = 0)
    {
        while (_isShooting)
        {
            if (Time.time >= _nextTimeTofire)
            {
                if (_shakeOnShootIntensity != 0) CameraManager.Instance.Shake(1f / CurFireRate, _shakeOnShootIntensity);

                if (_lastShootBarrelNumber >= BarrelCount) _lastShootBarrelNumber = 0;

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
}
