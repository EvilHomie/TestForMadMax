using UnityEngine;

public abstract class AbstractPlayerWeapon : MonoBehaviour
{
    [SerializeField] protected NewWeaponData _weaponData;

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
    public float RotationSpeed => _weaponData.rotationSpeed;
    public Vector3 ObserverPos => _observerPos;
    public GameObject TargetMarker => _targetMarker.gameObject;
    public Transform BaseTransform => _baseTransform;
    public Transform TurretTransform => _turretTransform;

    public abstract void Init(NewWeaponData weaponData, AbstractAmmunitionBelt abstractAmmunitionBelt);
    public abstract void Shoot();
    public abstract void StopShoot();
    public abstract void Reload();

    protected bool TryHitEnemy(int firepointIndex)
    {
        if (Physics.Raycast(_firePoints[firepointIndex].transform.position, _firePoints[firepointIndex].transform.forward, out RaycastHit hitInfo))
        {
            hitInfo.collider.GetComponent<IDamageable>()?.OnHit(_weaponData.kineticDamage, _weaponData.energyDamage, _hitSound);
            Instantiate(_hitFXEffect, hitInfo.point, _hitFXEffect.transform.rotation);
            hitInfo.collider.GetComponent<IHitable>()?.OnHit(hitInfo.point, _hitSound);
            return true;
        }
        else return false;
    }
}
