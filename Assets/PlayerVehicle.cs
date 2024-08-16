using System.Collections;
using UnityEngine;

public class PlayerVehicle : MonoBehaviour
{
    [SerializeField] float _shakeIntensityOnStart = 0.5f;
    [SerializeField] float _shakeDelayOnStart = 0.5f;
    [SerializeField] float _shakeDurationOnStart = 0.8f;
    [SerializeField] float _delayBeforeStartMove = 2;
    [SerializeField] float _targetSpeedOnStart = 5;
    [SerializeField] float _reachTargetSpeedDuration = 10;
    [SerializeField] float _delayForDustAfterMove = 3;

    [SerializeField] Transform[] _weaponPoints;

    AudioSource _engineAudioSource;
    VehicleEffectsController _vehicleEffectsController;
    float _deffAudioPitch = 0.9f;

    public Transform[] WeaponPoints => _weaponPoints;

    private void Awake()
    {
        _vehicleEffectsController = GetComponent<VehicleEffectsController>();
        _engineAudioSource = GetComponent<AudioSource>();
    }
    public void OnMove()
    {
        _vehicleEffectsController.OnMove();
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
        _engineAudioSource.Play();
        yield return new WaitForSeconds(_shakeDelayOnStart);
        CameraManager.Instance.Shake(_shakeDurationOnStart, _shakeIntensityOnStart);
        yield return new WaitForSeconds(_delayBeforeStartMove + _delayForDustAfterMove);
        _vehicleEffectsController.PlayDustPS();
    }
}
