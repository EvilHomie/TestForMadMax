using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
    [SerializeField] protected float _kineticDamage;
    [SerializeField] protected float _energyDamage;
    [SerializeField] protected float _reloadTime;
    [SerializeField] protected float _rotationSpeed;
    [SerializeField] protected float _shakeOnShootIntensity;
    [SerializeField] protected float _shakeOnShootDuration;
    [SerializeField] protected Vector3 _observerPos;
    [SerializeField] protected AbstarctFirePoint[] _firePoints;
    [SerializeField] protected ParticleSystem _hitFXEffect;
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AnimationCurve _animationCurve;
    [SerializeField] protected AudioClip _shootSound;
    [SerializeField] protected AudioClip _hitSound;
    [SerializeField] protected Transform _baseTransform;
    [SerializeField] protected Transform _turretTransform;
    [SerializeField] protected Transform _targetMarker;

    protected int _barrelsCount;
    protected int _shootBarrelNumber;
    public float RotationSpeed => _rotationSpeed;
    public Vector3 ObserverPos => _observerPos;
    public GameObject TargetMarker => _targetMarker.gameObject;
    public Transform BaseTransform => _baseTransform;
    public Transform TurretTransform => _turretTransform;

    public abstract void Init(SMWeaponData weaponData);
    public abstract void UpdateValues(SMWeaponData weaponData);
    public abstract void Shoot(bool asPlayer);
    public abstract void StopShoot();

    protected bool TryHitEnemy(int firepointIndex)
    {
        if (Physics.Raycast(_firePoints[firepointIndex].transform.position, _firePoints[firepointIndex].transform.forward, out RaycastHit hitInfo))
        {
            hitInfo.collider.GetComponent<IDamageable>()?.OnHit(_kineticDamage, _energyDamage, _hitSound);
            Instantiate(_hitFXEffect, hitInfo.point, _hitFXEffect.transform.rotation);
            hitInfo.collider.GetComponent<IHitable>()?.OnHit(hitInfo.point, _hitSound);
            return true;
        }
        else return false;
    }

    protected bool TryHitPlayer()
    {
        return true;
    }
}
