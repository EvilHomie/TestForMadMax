using UnityEngine;

public class EnemyVehicleManager : MonoBehaviour
{
    EnemyVehicleMovementController _vehicleMovementController;
    VehicleEffectsController _vehicleEffectsController;
    //EnemyWeaponController _enemyWeaponController;
    //ResourcesInVehicle _resourcesInVehicle;
    //AudioSource _vehicleAudioSource;

    [SerializeField] ParticleSystem _blowParticleSystem;
    [SerializeField] AudioClip _blowAudioClip;

    float _lastMoveSpeed;
    bool _isDead = false;

    float translateDuration;

    private void Awake()
    {
        _vehicleMovementController = GetComponent<EnemyVehicleMovementController>();
        _vehicleEffectsController = GetComponent<VehicleEffectsController>();
        //_enemyWeaponController = GetComponent<EnemyWeaponController>();
        //_vehicleAudioSource = GetComponent<AudioSource>();
        //_resourcesInVehicle = GetComponent<ResourcesInVehicle>();
    }


    private void Start()
    {
        _vehicleMovementController.StartTranslateToGameZone();        
        //_enemyWeaponController.StartShooting();
    }

    void Update()
    {
        if (_isDead) return;
        _vehicleEffectsController.PlayMoveEffects();
        _vehicleMovementController.MotionSimulation();


        //_vehicleEffectsController.RotateWheels();

        //if (_isDead) return;

        //_enemyWeaponController.RotateToPlayer();
        //_vehicleMovementController.MotionSimulation();

        //if (_lastMoveSpeed != RaidManager.Instance.PlayerMoveSpeed)
        //{
        //    _vehicleEffectsController.UpdateVisualEffect();
        //    _lastMoveSpeed = RaidManager.Instance.PlayerMoveSpeed;
        //}
    }

    public void OnBodyDestoyed()
    {
        OnDie();
    }

    public void OnBodyCollisionWithRoad()
    {
        OnDie();
    }

    public void OnWeaponDestroy()
    {
        //_enemyWeaponController.StopShooting();
        //_vehicleMovementController.OnRunMovementLogic();
    }

    void OnDie()
    {
        //_isDead = true;
        //_enemyWeaponController.StopShooting();
        //_vehicleMovementController.OnDieMovementLogic();
        //_vehicleEffectsController.StopDustEmmiting();
        //_blowParticleSystem.Play();
        //_vehicleAudioSource.PlayOneShot(_blowAudioClip);
        //_resourcesInVehicle.DropResources();
    }

    
}
