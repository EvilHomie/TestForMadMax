using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class EnemyVehicleManager : MonoBehaviour
{
    EnemyVehicleMovementController _vehicleMovementController;
    VehicleEffectsController _vehicleEffectsController;
    EnemyWeaponController _enemyWeaponController;
    ResourcesInVehicle _resourcesInVehicle;
    AudioSource _vehicleAudioSource;

    [SerializeField] ParticleSystem _blowParticleSystem;
    [SerializeField] AudioSource _blowAudioSource;
    [SerializeField] AudioClip _blowAudioClip;
    [SerializeField] int _reservedLineNumber;
    [SerializeField] RotationConstraint _smokeRotationConstraint;

    float _lastMoveSpeed;
    bool _isDead = false;

    ConstraintSource constraintSource;

    public AudioSource VehicleAudioSource => _vehicleAudioSource;
    public int ReservedLineNumber { set => _reservedLineNumber = value; }

    private void Awake()
    {
        _vehicleMovementController = GetComponent<EnemyVehicleMovementController>();
        _vehicleEffectsController = GetComponent<VehicleEffectsController>();
        _enemyWeaponController = GetComponent<EnemyWeaponController>();
        _vehicleAudioSource = GetComponent<AudioSource>();
        _resourcesInVehicle = GetComponent<ResourcesInVehicle>();
    }


    private void Start()
    {
        constraintSource.sourceTransform = RaidManager.Instance.transform;
        constraintSource.weight = 1.0f;
        _smokeRotationConstraint.AddSource(constraintSource);
        _vehicleMovementController.StartMovementLogic(this);
    }

    void Update()
    {
        if (_isDead) return;
        _vehicleEffectsController.PlayMoveEffects();
    }

    private void OnDestroy()
    {
        RaidManager.Instance.OnEnemyDestroyed(this, _reservedLineNumber);
    }


    public void OnReachGameZone()
    {
        _enemyWeaponController.StartShootLogic();
    }

    public void OnBodyDestoyed()
    {
        OnDie();
    }

    public void OnBodyCollisionWithRoad()
    {
        OnDie();
    }

    public void OnWeaponLossHP(GameObject weapon)
    {
        Destroy(weapon);
        if (!_enemyWeaponController.CheckAvailableWeapons())
        {
            _vehicleMovementController.OnTryRunMovementLogic();
        }
    }

    void OnDie()
    {
        _isDead = true;
        _vehicleEffectsController.StopDustEmmiting();
        _vehicleMovementController.StartDieLogic();
        _enemyWeaponController.StopShooting();

        _blowParticleSystem.Play();
        _blowAudioSource.PlayOneShot(_blowAudioClip);

        _resourcesInVehicle.DropResources();
    }
}
