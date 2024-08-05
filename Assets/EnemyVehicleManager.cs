using System.Collections;
using UnityEngine;

public class EnemyVehicleManager : MonoBehaviour
{
    EnemyVehicleMovementController _vehicleMovementController;
    EnemyVehicleVisualController _enemyVehicleVisualController;
    EnemyWeaponController _enemyWeaponController;
    ResourcesInVehicle _resourcesInVehicle;
    AudioSource _vehicleAudioSource;

    [SerializeField] ParticleSystem _blowParticleSystem;
    [SerializeField] AudioClip _blowAudioClip;

    float _lastMoveSpeed;
    bool _isDead = false;

    private void Awake()
    {
        _vehicleMovementController = GetComponent<EnemyVehicleMovementController>();
        _enemyVehicleVisualController = GetComponent<EnemyVehicleVisualController>();
        _enemyWeaponController = GetComponent<EnemyWeaponController>();
        _vehicleAudioSource = GetComponent<AudioSource>();
        _resourcesInVehicle = GetComponent<ResourcesInVehicle>();
    }


    private void Start()
    {
        _vehicleMovementController.StartTranslateToPlayer();
        _enemyVehicleVisualController.UpdateVisualEffect();
        _enemyWeaponController.StartShooting();
    }

    void FixedUpdate()
    {
        _enemyVehicleVisualController.RotateWheels();

        if (_isDead) return;

        _enemyWeaponController.RotateToPlayer();
        _vehicleMovementController.MotionSimulation();

        if (_lastMoveSpeed != RaidManager.Instance.PlayerMoveSpeed)
        {
            _enemyVehicleVisualController.UpdateVisualEffect();
            _lastMoveSpeed = RaidManager.Instance.PlayerMoveSpeed;
        }
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
        _enemyWeaponController.StopShooting();
        _vehicleMovementController.OnRunMovementLogic();
    }

    void OnDie()
    {
        _isDead = true;
        _enemyWeaponController.StopShooting();
        _vehicleMovementController.OnDieMovementLogic();
        _enemyVehicleVisualController.StopVisualEffects();
        _blowParticleSystem.Play();
        _vehicleAudioSource.PlayOneShot(_blowAudioClip);
        _resourcesInVehicle.DropResources();
    }

    
}
