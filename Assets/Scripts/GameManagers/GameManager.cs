using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GameObject _UIButtons;
    [SerializeField] Button _startRaidBtn;
    [SerializeField] Button _garageBtn;
    [SerializeField] Button _openInventoryBtn;
    [SerializeField] Button _changeLevelsBtn;
    [SerializeField] Button _closeInventoryBtn;
    [SerializeField] Button _settingsBtn;


    [SerializeField] TextMeshProUGUI _startRaidBtnText;
    [SerializeField] TextMeshProUGUI _garageBtnText;
    [SerializeField] TextMeshProUGUI _openInventoryBtnText;



    float _showControllerDelay = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

    }

    void Start()
    {
        YandexGame.GameReadyAPI();
        //YandexGame.EnvironmentData.isDesktop = false;
        Init();
    }


    void Init()
    {
        Canvas.ForceUpdateCanvases();
        TextConstants.SetLanguage();
        SaveLoadManager.Instance.CheckSaveData();

        _startRaidBtnText.text = TextConstants.RAID;
        _garageBtnText.text = TextConstants.GARAGE;
        _openInventoryBtnText.text = TextConstants.INVENTORY;

        InventoryInfoPanelManager.Instance.Init();
        InventoryManager.Instance.Init();
        InventoryUpgradePanelManager.Instance.Init();
        
        UIResourcesManager.Instance.Init();
        UINewSchemeManager.Instance.Init();

        LevelManager.Instance.Init();
        InRaidManager.Instance.Init();
        AddListenersOnBtns();

        PlayerVehicleManager.Instance.Init();
        PlayerWeaponManager.Instance.Init();

        UIJoystickTouchController.Instance.Init();
        UIWeaponsSwitcher.Instance.Init();
        CameraManager.Instance.Init();
        PlayerHPManager.Instance.Init();
        UIEnemyHpPanel.Instance.Init();

        SwitchUIButton(true);
        _settingsBtn.gameObject.SetActive(false);
    }

    void AddListenersOnBtns()
    {
        _startRaidBtn.onClick.AddListener(OnStartRaid);
        _garageBtn.onClick.AddListener(OnReturnToGarage);
        _openInventoryBtn.onClick.AddListener(delegate { InventoryManager.Instance.OnOpenInventory(); });
        _closeInventoryBtn.onClick.AddListener(delegate { InventoryManager.Instance.OnCloseInventory(); });
        _changeLevelsBtn.onClick.AddListener(OnOpenLevels);
    }   

    void SwitchUIButton(bool enableStatus)
    {
       _UIButtons.SetActive(enableStatus);
    }

    

    void OnOpenLevels()
    {
        LevelManager.Instance.SelectLevelsWindow.SetActive(true);
    }

    public void OnStartRaid()
    {
        SwitchUIButton(false);
        _settingsBtn.gameObject.SetActive(true);
        SaveLoadManager.Instance.SaveData();
        
        PlayerWeaponManager.Instance.OnPlayerStartRaid();

        UIJoystickTouchController.Instance.ShowControllers(_showControllerDelay);
        
        UIWeaponsSwitcher.Instance.OnPlayerStartRaid();
        CameraManager.Instance.OnPlayerStartRaid();
        InRaidManager.Instance.OnPlayerStartRaid();
        PlayerHPManager.Instance.OnPlayerStartRaid();
        UIEnemyHpPanel.Instance.OnPlayerStartRaid();
        UILevelStatistic.Instance.OnPlayerStartRaid();


        YandexGame.GameplayStart();
    }

    public void OnReturnToGarage()
    {
        SwitchUIButton(true);
        _settingsBtn.gameObject.SetActive(false);
        SaveLoadManager.Instance.SaveData();
        PlayerVehicleManager.Instance.OnPlayerEndRaid();
        PlayerWeaponManager.Instance.OnPlayerEndRaid();

        UIJoystickTouchController.Instance.HideInRaidInterface();
        
        CameraManager.Instance.OnPlayerEndRaid();
        InRaidManager.Instance.OnPlayerEndRaid();
        PlayerHPManager.Instance.OnPlayerEndRaid();
        UIEnemyHpPanel.Instance.OnPlayerEndRaid();
        

        YandexGame.GameplayStop();
    }
}


