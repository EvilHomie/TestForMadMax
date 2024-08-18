using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Button _startRaidBtn;
    [SerializeField] Button _garageBtn;
    [SerializeField] Button _openInventoryBtn;
    [SerializeField] Button _changeLevelsBtn;
    [SerializeField] Button _closeUpgradesBtn;
    [SerializeField] Button _settingsBtn;
    [SerializeField] GameObject _inventory;

    float _showControllerDelay = 13f; // зависит от звука запуска двигател€, а точнее времени набора стартовой скорости
    bool _playerOnRaid = false;
    bool _settingsIsopened = false;

    

    void Start()
    {
        _playerOnRaid = false;
        _settingsBtn.onClick.AddListener(ToggleMenu);
        _startRaidBtn.onClick.AddListener(OnStartRaid);
        _garageBtn.onClick.AddListener(OnReturntToGarage);
        _openInventoryBtn.onClick.AddListener(OnOpenInventory);
        _closeUpgradesBtn.onClick.AddListener(OnCloseInventory);
        _changeLevelsBtn.onClick.AddListener(OnOpenLevels);

        //TouchController.Instance.HideControllers();
        //WeaponsSwitcher.Instance.OnPlayerEndRaid();
        //PlayerWeaponManager.Instance.OnPlayerEndRaid();
        SwitchButtonsElements();
        OnCloseInventory();
        ToggleMenu();

        //SaveLoadManager.Instance.LoadItemsData();

        //“≈—“ќ¬јя ќЅЋј—“№
        ResourcesManager.Instance.AddResources(1000, 1000, 1000);
        //SaveLoadManager.Instance.LoadDeffaultItems(_deffaulItemsNames);
        
        PlayerData.Instance.SelectedVehicleName = "Simple Truck";
        PlayerData.Instance.SelectedWeapons.Add("Simple Cannon");
        //PlayerVehicleManager.Instance.ChangeVehicle();
        //PlayerWeaponManager.Instance.OnChangeVehicle();
    }

    void ToggleMenu()
    {
        _settingsIsopened = !_settingsIsopened;
        if (_settingsIsopened) SwitchButtonsElements();       
        else DisableMenuElements();
    }

    void OnOpenInventory()
    {
        _inventory.SetActive(true);
        InventoryManager.Instance.OnOpenInventory();
    }
    void OnCloseInventory()
    {
        _inventory.SetActive(false);
    }
    void OnOpenLevels()
    {

    }

    void OnStartRaid()
    {
        _playerOnRaid = true;
        PlayerVehicleManager.Instance.OnPlayerStartRaid(out float startMoveDelay, out float startSpeed, out float reachStartSpeedDuration);
        CameraManager.Instance.OnPlayerStartRaid();
        TouchController.Instance.ShowControllers(_showControllerDelay);
        GarageBoxManager.Instance.OnPlayerStartRaid(startMoveDelay);
        RaidManager.Instance.ChangeSpeedOnStartRaid(startMoveDelay, startSpeed, reachStartSpeedDuration);
        DisableMenuElements();
    }

    void OnReturntToGarage()
    {
        _playerOnRaid = false;
        WeaponsSwitcher.Instance.OnPlayerEndRaid();
        PlayerWeaponManager.Instance.OnPlayerEndRaid();
        GarageBoxManager.Instance.OnPlayerEndRaid();
        PlayerVehicleManager.Instance.OnPlayerEndRaid();
        RaidManager.Instance.OnPlayerEndRaid();
        CameraManager.Instance.OnPlayerEndRaid();
        TouchController.Instance.HideControllers();
        SwitchButtonsElements();
    }

    void DisableMenuElements()
    {
        _startRaidBtn.gameObject.SetActive(false);
        _garageBtn.gameObject.SetActive(false);
        _openInventoryBtn.gameObject.SetActive(false);
        _changeLevelsBtn.gameObject.SetActive(false);
        _settingsIsopened = false;
        OnCloseInventory();
    }

    void SwitchButtonsElements()
    {
        _startRaidBtn.gameObject.SetActive(!_playerOnRaid);
        _garageBtn.gameObject.SetActive(_playerOnRaid);
        _openInventoryBtn.gameObject.SetActive(!_playerOnRaid);
        _changeLevelsBtn.gameObject.SetActive(!_playerOnRaid);
    }
}


