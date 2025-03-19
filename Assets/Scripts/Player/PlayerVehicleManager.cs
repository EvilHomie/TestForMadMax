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

    public void Init()
    {
        OnCloseInventory();
    }

    public void OnCloseInventory()
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
        if (_lastMoveSpeed != InRaidManager.Instance.PlayerMoveSpeed)
        {
            _lastMoveSpeed = InRaidManager.Instance.PlayerMoveSpeed;
            _playerVehicle.ChangeEngineAudioPitch(_lastMoveSpeed);
        }
    }

    void CreateVehicleInstance()
    {
        foreach (Transform vehicle in transform)
        {
            Destroy(vehicle.gameObject);
        }
        string equipedVehicleName = PlayerData.Instance.EquipedItems[0];
        VehicleData vehicleData = (VehicleData)PlayerData.Instance.GetItemDataByName(equipedVehicleName);
        PlayerVehicle vehiclePF = GameAssets.Instance.GameItems.PlayerVehicles.Find(vehicle => vehicle.name == equipedVehicleName);

        PlayerVehicle newVehicleInstance = Instantiate(vehiclePF, transform);
        newVehicleInstance.SetItemData(vehicleData);
        _playerVehicle = newVehicleInstance;
    }

    public void CreateSpecificVehicleInstance(VehicleData vehicleData)
    {
        foreach (Transform vehicle in transform)
        {
            Destroy(vehicle.gameObject);
        }
        PlayerVehicle vehiclePF = GameAssets.Instance.GameItems.PlayerVehicles.Find(vehicle => vehicle.name == vehicleData.DeffItemName);

        PlayerVehicle newVehicleInstance = Instantiate(vehiclePF, transform);
        newVehicleInstance.SetItemData(vehicleData);
        _playerVehicle = newVehicleInstance;
    }

    public void OnPlayerStartRaid()
    {
        if (!InRaidManager.Instance.InSurviveMod)
        {
            CreateVehicleInstance();
        }        
        _engineIsStarted = true;
        _playerVehicle.StartVehicle();
    }

    public void GetVehicleMovingData(out float vehicleStartDelay, out float vehicleFullSpeed, out float vehicleReachFullSpeedDuration)
    {
        _playerVehicle.GetVehicleStartData(out float curVehicleStartDelay, out float curVehicleFullSpeed, out float curVehicleReachFullSpeedDuration);
        vehicleStartDelay = curVehicleStartDelay;
        vehicleFullSpeed = curVehicleFullSpeed;
        vehicleReachFullSpeedDuration = curVehicleReachFullSpeedDuration;
    }

    public void OnPlayerEndRaid()
    {
        _engineIsStarted = false;
        _playerVehicle.StopVehicle();
    }
}
