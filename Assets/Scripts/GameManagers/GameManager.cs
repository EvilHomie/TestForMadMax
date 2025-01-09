using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GarageMainPanleAnimation _garageMainPanel;
    [SerializeField] GameObject _inGarageInterface;
    [SerializeField] Button _startRaidBtn;
    [SerializeField] Button _startSurviveModBtn;
    //[SerializeField] Button _garageBtn;
    [SerializeField] Button _openInventoryBtn;
    [SerializeField] Button _changeLevelsBtn;
    [SerializeField] Button _closeInventoryBtn;
    //[SerializeField] Button _settingsBtn;


    //[SerializeField] TextMeshProUGUI _startRaidBtnText;
    //[SerializeField] TextMeshProUGUI _garageBtnText;
    //[SerializeField] TextMeshProUGUI _openInventoryBtnText;



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
        //Application.targetFrameRate = 60;

    }


    void Init()
    {
        Canvas.ForceUpdateCanvases();
        TextConstants.SetLanguage();
        GameAssets.Instance.Init();
        SaveLoadManager.Instance.CheckSaveData();

        //_startRaidBtnText.text = TextConstants.RAID;
        //_garageBtnText.text = TextConstants.GARAGE;
        //_openInventoryBtnText.text = TextConstants.INVENTORY;

        MainAudioManager.Instance.Init();
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
        LowHpRewardOffer.Instance.Init();
        UpgradesAfterLevel.Instance.Init();

        //SwitchUIButton(true);
        //_settingsBtn.gameObject.SetActive(false);

        TutorialManager.Instance.Init();
    }

    void AddListenersOnBtns()
    {
        _startRaidBtn.onClick.AddListener(delegate
        {
            OnStartRaid();
            TutorialManager.Instance.TryConfirmStage(StageName.FirstRaidLaunch);
        });
        //_garageBtn.onClick.AddListener(OnReturnToGarage);
        _openInventoryBtn.onClick.AddListener(delegate
        {
            InventoryManager.Instance.OnOpenInventory();
        });
        _closeInventoryBtn.onClick.AddListener(delegate
        {
            InventoryManager.Instance.OnCloseInventory();
            SaveLoadManager.Instance.SaveData();
        });
        _changeLevelsBtn.onClick.AddListener(OnOpenLevels);

        _startSurviveModBtn.onClick.AddListener(delegate
        {
            SurviveModeManager.Instance.OnStartMode();
            ConfigMainPanel(true);
        });
    }

    void SwitchUIButton(bool enableStatus)
    {
        _inGarageInterface.SetActive(enableStatus);
    }



    void OnOpenLevels()
    {
        LevelManager.Instance.SelectLevelsWindow.SetActive(true);
    }

    public void OnStartRaid(bool immediateStart = false)
    {
        //SwitchUIButton(false);
        //_settingsBtn.gameObject.SetActive(true);
        SaveLoadManager.Instance.SaveData();

        PlayerWeaponManager.Instance.OnPlayerStartRaid();

        UIJoystickTouchController.Instance.OnStartRaid(_showControllerDelay);

        UIWeaponsSwitcher.Instance.OnPlayerStartRaid();
        CameraManager.Instance.OnPlayerStartRaid();
        InRaidManager.Instance.OnPlayerStartRaid();
        PlayerHPManager.Instance.OnPlayerStartRaid();
        UIEnemyHpPanel.Instance.OnPlayerStartRaid();
        UILevelStatistic.Instance.OnPlayerStartRaid();

        if (!immediateStart)
        {
            ConfigMainPanel(true);
        }
        else
        {
            _garageMainPanel.gameObject.SetActive(false);
        }



        YandexGame.GameplayStart();
    }

    public void OnReturnToGarage()
    {
        //SwitchUIButton(true);
        //_settingsBtn.gameObject.SetActive(false);
        UIResourcesManager.Instance.EnablePanel();
        SaveLoadManager.Instance.SaveData();
        PlayerVehicleManager.Instance.OnPlayerEndRaid();
        PlayerWeaponManager.Instance.OnPlayerEndRaid();

        UIJoystickTouchController.Instance.OnPlayerEndRaid();

        CameraManager.Instance.OnPlayerEndRaid();
        InRaidManager.Instance.OnPlayerEndRaid();
        PlayerHPManager.Instance.OnPlayerEndRaid();
        UIEnemyHpPanel.Instance.OnPlayerEndRaid();
        SurviveModeManager.Instance.Disable();

        ConfigMainPanel(false);


        YandexGame.GameplayStop();
    }

    void ConfigMainPanel(bool inRaidStatus)
    {
        if (inRaidStatus) _garageMainPanel.HidePanel();
        else _garageMainPanel.ResetPosition();
        _startRaidBtn.interactable = !inRaidStatus;
        _openInventoryBtn.interactable = !inRaidStatus;
        _changeLevelsBtn.interactable = !inRaidStatus;
    }
}


