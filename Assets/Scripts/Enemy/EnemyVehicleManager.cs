using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyVehicleMovementController))]
[RequireComponent(typeof(EnemyWeaponController))]
[RequireComponent(typeof(VehicleVisualEffectsLogic))]
[RequireComponent(typeof(ResourcesInVehicle))]
[RequireComponent(typeof(EnemyCharacteristics))]

public class EnemyVehicleManager : MonoBehaviour
{
    [SerializeField] EnemyType _enemyType;
    EnemyDieExplosion _enemyDieExplosion;

    VehiclePartManager _bodyPartHPManager;    
    
    EnemyVehicleMovementController _enemyVehicleMovementController;
    VehicleVisualEffectsLogic _vehicleVisualEffectsController;
    EnemyWeaponController _enemyWeaponController;
    EnemyCharacteristics _enemyCharacteristics;
    ResourcesInVehicle _resourcesInVehicle;
    SchemesInVehicle _schemesInVehicle;

    AudioSource _vehicleAudioSource;
    Rigidbody _rigidbody;
    List<Transform> _wheels = new();
    List<Transform> _frontWheels = new();
    List<EnemyWeapon> _weapons = new();
    List<ParticleSystem> _wheelsDustPS = new();
    NavMeshObstacle _navMeshObstacle;

    List<Vector3> explodeOffsetPositions = new();

    bool _isDead = false;

    public EnemyType EnemyType => _enemyType;
    public AudioSource VehicleAudioSource { get => _vehicleAudioSource; set => _vehicleAudioSource = value; }
    public Rigidbody Rigidbody { get => _rigidbody; set => _rigidbody = value; }
    public bool IsDead => _isDead;
    public EnemyCharacteristics EnemyCharacteristics => _enemyCharacteristics;

    public List<Transform> Wheels => _wheels;
    public List<Transform> FrontWheels => _frontWheels;
    public List<EnemyWeapon> Weapons => _weapons;
    public VehiclePartManager BodyPartHPManager { get => _bodyPartHPManager; set => _bodyPartHPManager = value; }
    public NavMeshObstacle NavMeshObstacle { get => _navMeshObstacle; set => _navMeshObstacle = value; }
    public List<ParticleSystem> WheelsDustPS { get => _wheelsDustPS; set => _wheelsDustPS = value; }
    public EnemyDieExplosion EnemyDieExplosion { get => _enemyDieExplosion; set => _enemyDieExplosion = value; }

    void Awake()
    {
        _enemyVehicleMovementController = GetComponent<EnemyVehicleMovementController>();
        _vehicleVisualEffectsController = GetComponent<VehicleVisualEffectsLogic>();
        _enemyWeaponController = GetComponent<EnemyWeaponController>();
        _resourcesInVehicle = GetComponent<ResourcesInVehicle>();
        _schemesInVehicle = GetComponent<SchemesInVehicle>();
        _enemyCharacteristics = GetComponent<EnemyCharacteristics>();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        _enemyVehicleMovementController.CustomUpdate();
        _vehicleVisualEffectsController.CustomUpdate();
    }

    void Init()
    {
        _enemyDieExplosion.Init();
        _enemyVehicleMovementController.Init(this, _rigidbody, _frontWheels, _navMeshObstacle);
        _vehicleVisualEffectsController.Init(_wheels, _wheelsDustPS);
        _enemyWeaponController.Init(this, _weapons);

        explodeOffsetPositions.Add(Vector3.forward * 100);
        explodeOffsetPositions.Add(Vector3.back * 200);
        explodeOffsetPositions.Add(Vector3.right * 50);
        explodeOffsetPositions.Add(Vector3.left * 50);
    }

    public void OnObjectDestroy()
    {
        if(!Application.isPlaying) return;
        if (UIEnemyHpPanel.Instance.LastEnemyVehicleManager == this)
        {
            UIEnemyHpPanel.Instance.DisableHPBars();
        }
    }

    public void OnReachGameZone()
    {
        _enemyWeaponController.StartShootLogic();
    }

    public void OnPlayerDie()
    {
        if (_isDead) return;
        _enemyVehicleMovementController.OnPlayerDie();
    }

    public void OnBodyDestoyed()
    {
        if (_isDead) return;
        int offsetIndex = Random.Range(0, explodeOffsetPositions.Count);
        _rigidbody.AddForceAtPosition(GameConfig.Instance.TouchRoadImpulse * Vector3.up, transform.position + explodeOffsetPositions[offsetIndex], ForceMode.VelocityChange);
        OnDie();
    }

    public void OnBodyCollision()
    {
        OnDie();
        //_vehicleVisualEffectsController.CutDustPS();
    }

    public void OnWeaponLossHP(GameObject weapon)
    {
        if (!_enemyWeaponController.CheckAvailableWeapons(weapon))
        {
            if (_isDead) return;
            _enemyVehicleMovementController.OnTryRunMovementLogic();
        }
    }

    public void OnExplosivePartLooseHP()
    {
        _bodyPartHPManager.ExplosionDamage();
    }

    void OnDie()
    {
        if (_isDead) return;
        _isDead = true;
        UIEnemyHpPanel.Instance.DisableHPBarsOnVehicleDestroyed(this);
        _vehicleVisualEffectsController.CutDustPS();
        _enemyVehicleMovementController.OnDie();
        _enemyWeaponController.StopShooting();
        _enemyDieExplosion.OnDie();        

        _resourcesInVehicle.DropResources();
        if (_schemesInVehicle != null && _schemesInVehicle.scheme != null)
        {
            _schemesInVehicle.DropScheme();
        }

        //UIEnemyHpPanel.Instance.DisableHPBars();
        //if (UIEnemyHpPanel.Instance.LastEnemyVehicleManager == this)
        //{
        //    UIEnemyHpPanel.Instance.DisableHPBars();
        //}

        InRaidManager.Instance.OnPlayerKillEnemy();
    }

    //public void OnHitPart()
    //{
    //    _bodyPartHPManager.GetBodyHPRelativeValues(out float hullHPRelativeValue, out float shieldHPRelativeValue);
    //    UIEnemyHpPanel.Instance.UpdateHPBars(hullHPRelativeValue, shieldHPRelativeValue, this);
    //}

    public void OnLooseWheel(bool isFrontWheel)
    {
        _rigidbody.automaticCenterOfMass = false;
        if (isFrontWheel)
        {
            _rigidbody.centerOfMass = new Vector3(0, 0, 100);
        }
        else
        {
            _rigidbody.centerOfMass = new Vector3(0, 0, -100);
        }
    }

}
