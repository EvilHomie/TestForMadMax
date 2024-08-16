using UnityEngine;

public class PlayerVehicleManager : MonoBehaviour
{
    public static PlayerVehicleManager Instance;

    PlayerVehicle _playerVehicle;
    float _lastMoveSpeed = 0;    
    bool _engineIsStarted = false;
    public PlayerVehicle PlayerVehicle => _playerVehicle;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    private void Start()
    {
        ChangeVehicle(GetComponentInChildren<PlayerVehicle>());
    }
    void ChangeVehicle(PlayerVehicle newVehicle)
    {
        _playerVehicle = newVehicle;
    }

    private void Update()
    {
        if (!_engineIsStarted) return;

        _playerVehicle.OnMove();
        if (_lastMoveSpeed != RaidManager.Instance.PlayerMoveSpeed)
        {
            _lastMoveSpeed = RaidManager.Instance.PlayerMoveSpeed;
            _playerVehicle.ChangeEngineAudioPitch(_lastMoveSpeed);
        }
    }

    public void OnPlayerStartRaid(out float startMoveDelay, out float startSpeed, out float reachStartSpeedDuration)
    {
        _engineIsStarted = true;
        _playerVehicle.StartVehicle();
        _playerVehicle.GetVehicleStartData(out float startDelay, out float speed, out float duration);
        startMoveDelay = startDelay;
        startSpeed = speed;
        reachStartSpeedDuration = duration;
    }

    public void OnPlayerEndRaid()
    {
        _engineIsStarted = false;
        _playerVehicle.StopVehicle();
    }
}
