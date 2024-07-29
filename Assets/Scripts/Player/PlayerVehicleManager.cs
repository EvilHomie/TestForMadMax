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

    public void OnChangeVehicle()
    {
        if (_playerVehicle != null)
        {
            VehicleData existVehicleData = (VehicleData)_playerVehicle.GetItemData();
            if (PlayerData.Instance.EquipedItems[0] == existVehicleData.deffVehicleName)
            {
                return;
            }
            else
            {
                Destroy(_playerVehicle.gameObject);
                CreateVehicleInstance();
            }
        }
        else CreateVehicleInstance();
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

    void CreateVehicleInstance()
    {
        string equipedVehicleName = PlayerData.Instance.EquipedItems[0];
        VehicleData vehicleData = (VehicleData)PlayerData.Instance.GetItemDataByName(equipedVehicleName);
        PlayerVehicle vehiclePF = GameAssets.Instance.GameItems.PlayerVehicles.Find(vehicle => vehicle.name == equipedVehicleName);

        PlayerVehicle newVehicleInstance = Instantiate(vehiclePF, transform);
        newVehicleInstance.SetItemData(vehicleData);
        _playerVehicle = newVehicleInstance;
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
        OnChangeVehicle();
        _engineIsStarted = false;
        _playerVehicle.StopVehicle();
    }
}
