using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Button _startRaidBtn;
    [SerializeField] Button _garageBtn;
    [SerializeField] Button _openInventoryBtn;
    [SerializeField] Button _changeLevelsBtn;
    [SerializeField] Button _closeUpgradesBtn;
    [SerializeField] Button _settingsBtn;


    [SerializeField] TextMeshProUGUI _startRaidBtnText;
    [SerializeField] TextMeshProUGUI _garageBtnText;
    [SerializeField] TextMeshProUGUI _openInventoryBtnText;



    float _showControllerDelay = 3; // зависит от звука запуска двигателя, а точнее времени набора стартовой скорости
    bool _playerOnRaid = false;
    bool _settingsIsopened = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

    }

    //private void OnEnable() => YandexGame.GetDataEvent += Init;
    //private void OnDisable() => YandexGame.GetDataEvent -= Init;

    void Start()
    {
        YandexGame.GameReadyAPI();
        //YandexGame.EnvironmentData.isDesktop = false;
        Init();
    }


    void Init()
    {
        Canvas.ForceUpdateCanvases();
        Application.targetFrameRate = 1000;
        TextConstants.SetLanguage();
        SaveLoadManager.Instance.CheckSaveData();

        _startRaidBtnText.text = TextConstants.RAID;
        _garageBtnText.text = TextConstants.GARAGE;
        _openInventoryBtnText.text = TextConstants.INVENTORY;

        InventoryInfoPanelManager.Instance.Init();
        InventoryManager.Instance.Init();
        InventoryUpgradePanelManager.Instance.Init();
        LevelManager.Instance.Init();
        UIResourcesManager.Instance.Init();
        UINewSchemeManager.Instance.Init();
        UILevelStatistic.Instance.Init();
        AddListenersOnBtns();

        _playerOnRaid = false;
        PlayerVehicleManager.Instance.Init();
        PlayerWeaponManager.Instance.Init();

        UIJoystickTouchController.Instance.Init();
        UIWeaponsSwitcher.Instance.Init();
        GarageBoxManager.Instance.OnPlayerEndRaid();
        CameraManager.Instance.OnPlayerEndRaid();
        RaidManager.Instance.OnPlayerEndRaid();
        PlayerHPManager.Instance.OnPlayerEndRaid();
        UIEnemyHpPanel.Instance.OnPlayerEndRaid();

        SwitchMenuElements();

        ToggleMenu();
        _settingsBtn.gameObject.SetActive(false);
    }

    void AddListenersOnBtns()
    {
        _settingsBtn.onClick.AddListener(ToggleMenu);
        _startRaidBtn.onClick.AddListener(OnStartRaid);
        _garageBtn.onClick.AddListener(OnReturnToGarage);
        _openInventoryBtn.onClick.AddListener(delegate { InventoryManager.Instance.OnOpenInventory(); });
        _closeUpgradesBtn.onClick.AddListener(delegate { InventoryManager.Instance.OnCloseInventory(); });
        _changeLevelsBtn.onClick.AddListener(OnOpenLevels);
    }   

    void ToggleMenu()
    {
        _settingsIsopened = !_settingsIsopened;
        if (_settingsIsopened)
        {
            SwitchMenuElements();
            //Time.timeScale = 0;
            //AudioListener.pause = true;
        }
        else
        {
            DisableMenuElements();
            //Time.timeScale = 1;
            //AudioListener.pause = false;
        }
    }

    void OnOpenLevels()
    {
        LevelManager.Instance.SelectLevelsWindow.SetActive(true);
    }

    void OnStartRaid()
    {
        _playerOnRaid = true;
        _settingsBtn.gameObject.SetActive(true);
        SaveLoadManager.Instance.SaveData();
        PlayerVehicleManager.Instance.OnPlayerStartRaid(out float startMoveDelay, out float startSpeed, out float reachStartSpeedDuration);
        PlayerWeaponManager.Instance.OnPlayerStartRaid();

        UIJoystickTouchController.Instance.ShowControllers(_showControllerDelay);
        GarageBoxManager.Instance.OnPlayerStartRaid(startMoveDelay);
        UIWeaponsSwitcher.Instance.OnPlayerStartRaid();
        CameraManager.Instance.OnPlayerStartRaid();
        RaidManager.Instance.OnPlayerStartRaid(startMoveDelay, startSpeed, reachStartSpeedDuration);
        PlayerHPManager.Instance.OnPlayerStartRaid();
        UIEnemyHpPanel.Instance.OnPlayerStartRaid();
        UILevelStatistic.Instance.OnPlayerStartRaid();


        DisableMenuElements();
        YandexGame.GameplayStart();
    }

    public void OnReturnToGarage()
    {
        _playerOnRaid = false;
        _settingsBtn.gameObject.SetActive(false);
        SaveLoadManager.Instance.SaveData();
        PlayerVehicleManager.Instance.OnPlayerEndRaid();
        PlayerWeaponManager.Instance.OnPlayerEndRaid();

        UIJoystickTouchController.Instance.HideInRaidInterface();
        GarageBoxManager.Instance.OnPlayerEndRaid();
        //WeaponsSwitcher.Instance.OnPlayerEndRaid();
        CameraManager.Instance.OnPlayerEndRaid();
        RaidManager.Instance.OnPlayerEndRaid();
        PlayerHPManager.Instance.OnPlayerEndRaid();
        UIEnemyHpPanel.Instance.OnPlayerEndRaid();

        SwitchMenuElements();
        YandexGame.GameplayStop();
    }

    void DisableMenuElements()
    {
        _startRaidBtn.gameObject.SetActive(false);
        _garageBtn.gameObject.SetActive(false);
        _openInventoryBtn.gameObject.SetActive(false);
        _changeLevelsBtn.gameObject.SetActive(false);
        _settingsIsopened = false;
        InventoryManager.Instance.gameObject.SetActive(false);
        LevelManager.Instance.SelectLevelsWindow.SetActive(false);
    }

    void SwitchMenuElements()
    {
        _startRaidBtn.gameObject.SetActive(!_playerOnRaid);
        _garageBtn.gameObject.SetActive(_playerOnRaid);
        _openInventoryBtn.gameObject.SetActive(!_playerOnRaid);
        _changeLevelsBtn.gameObject.SetActive(!_playerOnRaid);
    }
}


