using System;
using System.Collections.Generic;
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

    float _showControllerDelay = 13f; // ������� �� ����� ������� ���������, � ������ ������� ������ ��������� ��������
    bool _playerOnRaid = false;
    bool _settingsIsopened = false;

    

    void Start()
    {
        _playerOnRaid = false;
        _settingsBtn.onClick.AddListener(ToggleMenu);
        _startRaidBtn.onClick.AddListener(OnStartRaid);
        _garageBtn.onClick.AddListener(OnReturntToGarage);
        _openInventoryBtn.onClick.AddListener(delegate { InventoryManager.Instance.OnOpenInventory(); });
        _closeUpgradesBtn.onClick.AddListener(delegate { InventoryManager.Instance.OnCloseInventory(); });
        _changeLevelsBtn.onClick.AddListener(OnOpenLevels);

        SaveLoadManager.Instance.LoadSaveData();
        ResourcesManager.Instance.UpdateCounters();

        OnReturntToGarage();
        InventoryManager.Instance.OnCloseInventory();
        ToggleMenu();
    }

    //�������� �������
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SaveLoadManager.Instance.SaveData();

        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveLoadManager.Instance.LoadSaveData();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
            Debug.LogWarning("SAVE CLEAR");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            ResourcesManager.Instance.AddResources(1000, 1000, 1000);
        }
    }

    void ToggleMenu()
    {
        _settingsIsopened = !_settingsIsopened;
        if (_settingsIsopened) SwitchMenuElements();       
        else DisableMenuElements();
    }

    void OnOpenLevels()
    {

    }

    void OnStartRaid()
    {        
        _playerOnRaid = true;
        SaveLoadManager.Instance.SaveData();
        PlayerVehicleManager.Instance.OnPlayerStartRaid(out float startMoveDelay, out float startSpeed, out float reachStartSpeedDuration);
        PlayerWeaponManager.Instance.OnPlayerStartRaid();

        TouchController.Instance.ShowControllers(_showControllerDelay);
        GarageBoxManager.Instance.OnPlayerStartRaid(startMoveDelay);
        WeaponsSwitcher.Instance.OnPlayerStartRaid();
        CameraManager.Instance.OnPlayerStartRaid();
        RaidManager.Instance.OnPlayerStartRaid(startMoveDelay, startSpeed, reachStartSpeedDuration);



        DisableMenuElements();
    }

    void OnReturntToGarage()
    {
        _playerOnRaid = false;
        SaveLoadManager.Instance.SaveData();
        PlayerVehicleManager.Instance.OnPlayerEndRaid();
        PlayerWeaponManager.Instance.OnPlayerEndRaid();

        TouchController.Instance.HideControllers();
        GarageBoxManager.Instance.OnPlayerEndRaid();
        //WeaponsSwitcher.Instance.OnPlayerEndRaid();
        CameraManager.Instance.OnPlayerEndRaid();
        RaidManager.Instance.OnPlayerEndRaid();



        
        SwitchMenuElements();
    }

    void DisableMenuElements()
    {
        _startRaidBtn.gameObject.SetActive(false);
        _garageBtn.gameObject.SetActive(false);
        _openInventoryBtn.gameObject.SetActive(false);
        _changeLevelsBtn.gameObject.SetActive(false);
        _settingsIsopened = false;
        InventoryManager.Instance.OnCloseInventory();
    }

    void SwitchMenuElements()
    {
        _startRaidBtn.gameObject.SetActive(!_playerOnRaid);
        _garageBtn.gameObject.SetActive(_playerOnRaid);
        _openInventoryBtn.gameObject.SetActive(!_playerOnRaid);
        _changeLevelsBtn.gameObject.SetActive(!_playerOnRaid);
    }
}


