using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicle : MonoBehaviour, IItem
{
    [SerializeField] RenderQueue3001 _renderQueue3001;
    [SerializeField] VehicleData _vehicleData;
    [SerializeField] float _shakeIntensityOnStart = 0.5f;
    [SerializeField] float _shakeDelayOnStart = 0.5f;
    [SerializeField] float _shakeDurationOnStart = 0.8f;
    [SerializeField] float _delayBeforeStartMove = 2;
    [SerializeField] float _targetSpeedOnStart = 10;
    [SerializeField] float _reachTargetSpeedDuration = 10;
    [SerializeField] float _delayForDustAfterMove = 3;

    [SerializeField] List<Transform> _wheels;
    [SerializeField] List<ParticleSystem> _wheelsDustPS;

    [SerializeField] List<PlayerVehicleWeaponPoint> _weaponPoints;

    AudioSource _engineAudioSource;
    VehicleVisualEffectsLogic _vehicleEffectsController;
    float _deffAudioPitch = 0.9f;

    public List<PlayerVehicleWeaponPoint> WeaponPoints => _weaponPoints;

    private void Awake()
    {
        _vehicleEffectsController = GetComponent<VehicleVisualEffectsLogic>();
        
    }

    private void Start()
    {
        _renderQueue3001.Init();
        _vehicleEffectsController.Init(_wheels, _wheelsDustPS, false);
        _engineAudioSource = AudioManager.Instance.PlayerEngineAS;
    }

    public void ChangeEngineAudioPitch(float newSpeed)
    {
        if (newSpeed <= GameConfig.Instance.MinPlayerSpeed) return;
        _engineAudioSource.pitch = _deffAudioPitch + ((newSpeed - GameConfig.Instance.MinPlayerSpeed) / 50f);
    }

    public void StopVehicle()
    {
        StopAllCoroutines();
        _engineAudioSource.Stop();
        _vehicleEffectsController.CutDustPS();
    }

    public void StartVehicle()
    {
        StartCoroutine(StartMovement());        
    }

    public void GetVehicleStartData(out float startMoveDelay, out float startSpeed, out float reachStartSpeedDuration)
    {
        startMoveDelay = _shakeDelayOnStart + _delayBeforeStartMove;
        startSpeed = _targetSpeedOnStart;
        reachStartSpeedDuration = _reachTargetSpeedDuration;
    }
    IEnumerator StartMovement()
    {
        //_engineAudioSource.Play();
        yield return new WaitForSeconds(_shakeDelayOnStart);
        CameraManager.Instance.Shake(_shakeDurationOnStart, _shakeIntensityOnStart);
        yield return new WaitForSeconds(_delayBeforeStartMove + _delayForDustAfterMove);
        _vehicleEffectsController.PlayDustPS();
    }

    public object GetItemData()
    {
        return _vehicleData;
    }

    public void SetItemData(object obj)
    {
        _vehicleData = obj as VehicleData;
    }
}
