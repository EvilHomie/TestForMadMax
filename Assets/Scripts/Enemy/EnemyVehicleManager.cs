using UnityEngine;
using UnityEngine.Animations;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(EnemyVehicleMovementController))]
[RequireComponent(typeof(EnemyWeaponController))]
[RequireComponent(typeof(CollisionWithRoadLogic))]
[RequireComponent(typeof(PartHPManager))]
[RequireComponent(typeof(VehicleVisualEffectsLogic))]
[RequireComponent(typeof(ResourcesInVehicle))]
public class EnemyVehicleManager : MonoBehaviour
{
    EnemyVehicleMovementController _vehicleMovementController;
    VehicleVisualEffectsLogic _vehicleEffectsController;
    EnemyWeaponController _enemyWeaponController;
    ResourcesInVehicle _resourcesInVehicle;
    SchemesInVehicle _schemesInVehicle;
    AudioSource _vehicleAudioSource;

    [SerializeField] ParticleSystem _blowParticleSystem;
    [SerializeField] AudioSource _blowAudioSource;
    [SerializeField] AudioClip _blowAudioClip;
    [SerializeField] int _reservedLineNumber;
    [SerializeField] RotationConstraint _smokeRotationConstraint;

    float _lastMoveSpeed;
    bool _isDead = false;

    ConstraintSource constraintSource;

    PartHPManager _bodyPartHPManager;

    public AudioSource VehicleAudioSource => _vehicleAudioSource;
    public int ReservedLineNumber { set => _reservedLineNumber = value; }

    private void Awake()
    {
        _vehicleMovementController = GetComponent<EnemyVehicleMovementController>();
        _vehicleEffectsController = GetComponent<VehicleVisualEffectsLogic>();
        _enemyWeaponController = GetComponent<EnemyWeaponController>();
        _vehicleAudioSource = GetComponent<AudioSource>();
        _resourcesInVehicle = GetComponent<ResourcesInVehicle>();
        _schemesInVehicle = GetComponent<SchemesInVehicle>();
        _bodyPartHPManager = GetComponent<PartHPManager>();
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
        if (!Application.isPlaying) return;

        RaidManager.Instance.OnEnemyDestroyed(this, _reservedLineNumber);
        if (UIEnemyHpPanel.Instance.LastEnemyVehicleManager == this)
        {
            UIEnemyHpPanel.Instance.DisableHPBars();
        }
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
        if (!_enemyWeaponController.CheckAvailableWeapons(weapon))
        {
            _vehicleMovementController.OnTryRunMovementLogic();
        }
    }

    public void OnExplosivePartLooseHP()
    {
        _bodyPartHPManager.ExplosionDamage();
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
        if (_schemesInVehicle != null)
        {
            _schemesInVehicle.DropScheme();
        }

        if (UIEnemyHpPanel.Instance.LastEnemyVehicleManager == this)
        {
            UIEnemyHpPanel.Instance.DisableHPBars();
        }
    }

    public void OnHitPart()
    {
        _bodyPartHPManager.GetBodyHPRelativeValues(out float hullHPRelativeValue, out float shieldHPRelativeValue);
        UIEnemyHpPanel.Instance.UpdateHPBars(hullHPRelativeValue, shieldHPRelativeValue, this);
    }
}
