using TMPro;
using UnityEngine;
using UnityEngine.UI;

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


    float _showControllerDelay = 6; // зависит от звука запуска двигател€, а точнее времени набора стартовой скорости
    bool _playerOnRaid = false;
    bool _settingsIsopened = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;        
    }

    void Start()
    {
        TextConstants.SetLanguage(Language.ru);
        Init();
    }


    void Init()
    {
        Application.targetFrameRate = 1000;
        _startRaidBtnText.text = TextConstants.RAID;
        _garageBtnText.text= TextConstants.GARAGE;
        _openInventoryBtnText.text = TextConstants.INVENTORY;

        InventoryInfoPanelManager.Instance.Init();
        InventoryManager.Instance.Init();
        InventoryUpgradePanelManager.Instance.Init();
        AddListenersOnBtns();

        _playerOnRaid = false;
        SaveLoadManager.Instance.LoadSaveData();
        UIResourcesManager.Instance.UpdateCounters();
        LevelManager.Instance.Init();

        OnReturntToGarage();
        InventoryManager.Instance.OnCloseInventory();
        ToggleMenu();

        UIResourcesManager.Instance.AddResources(1000, 1000, 1000);
    }

    void AddListenersOnBtns()
    {
        _settingsBtn.onClick.AddListener(ToggleMenu);
        _startRaidBtn.onClick.AddListener(OnStartRaid);
        _garageBtn.onClick.AddListener(OnReturntToGarage);
        _openInventoryBtn.onClick.AddListener(delegate { InventoryManager.Instance.OnOpenInventory(); });
        _closeUpgradesBtn.onClick.AddListener(delegate { InventoryManager.Instance.OnCloseInventory(); });
        _changeLevelsBtn.onClick.AddListener(OnOpenLevels);
    }


    

    //“≈—“ќ¬јя ќЅЋј—“№
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
            UIResourcesManager.Instance.AddResources(1000, 1000, 1000);
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
        LevelManager.Instance.SelectLevelsWindow.SetActive(true);
    }

    void OnStartRaid()
    {        
        _playerOnRaid = true;
        SaveLoadManager.Instance.SaveData();
        PlayerVehicleManager.Instance.OnPlayerStartRaid(out float startMoveDelay, out float startSpeed, out float reachStartSpeedDuration);
        PlayerWeaponManager.Instance.OnPlayerStartRaid();

        UIJoystickTouchController.Instance.ShowControllers(_showControllerDelay);
        GarageBoxManager.Instance.OnPlayerStartRaid(startMoveDelay);
        UIWeaponsSwitcher.Instance.OnPlayerStartRaid();
        CameraManager.Instance.OnPlayerStartRaid();
        RaidManager.Instance.OnPlayerStartRaid(startMoveDelay, startSpeed, reachStartSpeedDuration);
        PlayerHPManager.Instance.OnPlayerStartRaid();



        DisableMenuElements();
    }

    void OnReturntToGarage()
    {
        _playerOnRaid = false;
        SaveLoadManager.Instance.SaveData();
        PlayerVehicleManager.Instance.OnPlayerEndRaid();
        PlayerWeaponManager.Instance.OnPlayerEndRaid();

        UIJoystickTouchController.Instance.HideControllers();
        GarageBoxManager.Instance.OnPlayerEndRaid();
        //WeaponsSwitcher.Instance.OnPlayerEndRaid();
        CameraManager.Instance.OnPlayerEndRaid();
        RaidManager.Instance.OnPlayerEndRaid();
        PlayerHPManager.Instance.OnPlayerEndRaid();



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
        LevelManager.Instance.SelectLevelsWindow.SetActive(false);
    }

    void SwitchMenuElements()
    {
        _startRaidBtn.gameObject.SetActive(!_playerOnRaid);
        _garageBtn.gameObject.SetActive(_playerOnRaid);
        _openInventoryBtn.gameObject.SetActive(!_playerOnRaid);
        _changeLevelsBtn.gameObject.SetActive(!_playerOnRaid);
    }

    public void OnPlayerVehicleDestroyed()
    {
        OnReturntToGarage();
    }

    public void OnPlayerKillAllEnemy()
    {
        OnReturntToGarage();
    }
}


