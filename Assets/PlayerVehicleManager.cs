using System.Collections;
using UnityEngine;

public class PlayerVehicleManager : MonoBehaviour
{
    public static PlayerVehicleManager Instance;
    VehicleVisualController _vehicleVisualController;
    AudioSource _audioSource;

    [SerializeField] ParticleSystem[] _wheelsDustPS;
    [SerializeField] float _shakeIntensityOnStart = 0.5f;
    [SerializeField] float _shakedelayOnStart = 0.5f;
    [SerializeField] float _shakedurationOnStart = 0.8f;
    [SerializeField] float _delayBeforeStartMove = 2;
    [SerializeField] float _targetSpeedOnStart = 5;
    [SerializeField] float _reachTargetSpeedDuration = 10;

    float _lastMoveSpeed = 0;
    float _deffAudioPitch = 0.9f;

    bool _isStarted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        _vehicleVisualController = GetComponent<VehicleVisualController>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!_isStarted) return;
        _vehicleVisualController.RotateWheels();
        if (_lastMoveSpeed != RaidObjectsManager.Instance.PlayerMoveSpeed)
        {
            _vehicleVisualController.UpdateVisualEffect();

            if (RaidObjectsManager.Instance.PlayerMoveSpeed <= GameLogicParameters.Instance.MinPlayerSpeed) return;
            _audioSource.pitch = _deffAudioPitch + ((RaidObjectsManager.Instance.PlayerMoveSpeed - GameLogicParameters.Instance.MinPlayerSpeed) / 50f);
            _lastMoveSpeed = RaidObjectsManager.Instance.PlayerMoveSpeed;
        }
    }


    public void OnPlayerStartRaid()
    {
        StartCoroutine(StartMovement());
        _isStarted = true;
    }

    public void OnPlayerEndRaid()
    {
        StopAllCoroutines();
        _isStarted = false;
        _audioSource.Stop();
        foreach (var ps in _wheelsDustPS)
        {
            ps.Stop();
        }
    }


    IEnumerator StartMovement()
    {
        _audioSource.Play();
        yield return new WaitForSeconds(_shakedelayOnStart);
        ShakeCamera.Instance.Shake(_shakedurationOnStart, _shakeIntensityOnStart);
        yield return new WaitForSeconds(_delayBeforeStartMove);
        GarageBoxManager.Instance.OnPlayerStartRaid(); // перенести в GameManager с задержкой запуска двигателя (нужно определиться со звуком)
        RaidObjectsManager.Instance.ChangeSpeedOnStartRaid(_targetSpeedOnStart, _reachTargetSpeedDuration);

        foreach (var ps in _wheelsDustPS)
        {
            ps.Play();
        }
    }

    
}
