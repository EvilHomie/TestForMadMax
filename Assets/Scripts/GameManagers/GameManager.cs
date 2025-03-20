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
    [SerializeField] Image _lockSurviveModeIcon;
    [SerializeField] Button _openInventoryBtn;
    [SerializeField] Button _changeLevelsBtn;
    [SerializeField] Button _closeInventoryBtn;



    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

    }

    void Start()
    {
        YandexGame.GameReadyAPI();
        Init();
    }


    void Init()
    {
        Canvas.ForceUpdateCanvases();
        TextConstants.SetLanguage();
        GameAssets.Instance.Init();
        SaveLoadManager.Instance.CheckSaveData();

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

        TutorialManager.Instance.Init();

        SurviveModeManager.Instance.Init();

        SurviveModCheckAvailability();
    }

    void AddListenersOnBtns()
    {
        _startRaidBtn.onClick.AddListener(delegate
        {
            OnStartRaid();
            TutorialManager.Instance.TryConfirmStage(StageName.FirstRaidLaunch);
            _startRaidBtn.interactable = false;
        });
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
            _startSurviveModBtn.interactable = false;
            ConfigMainPanel(true);
        });
    }

    void OnOpenLevels()
    {
        LevelManager.Instance.SelectLevelsWindow.SetActive(true);
    }

    public void OnStartRaid(bool immediateStart = false)
    {
        SaveLoadManager.Instance.SaveData();

        PlayerWeaponManager.Instance.OnPlayerStartRaid();

        UIJoystickTouchController.Instance.OnStartRaid();

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
        UIResourcesManager.Instance.EnablePanel();
        SaveLoadManager.Instance.SaveData();
        PlayerVehicleManager.Instance.OnPlayerEndRaid();
        PlayerWeaponManager.Instance.OnPlayerEndRaid();

        UIJoystickTouchController.Instance.OnPlayerEndRaid();

        CameraManager.Instance.OnPlayerEndRaid();
        InRaidManager.Instance.OnPlayerEndRaid();
        PlayerHPManager.Instance.OnPlayerEndRaid();
        UIEnemyHpPanel.Instance.OnPlayerEndRaid();
        //SurviveModeManager.Instance.OnDisableMode();

        ConfigMainPanel(false);
        SurviveModCheckAvailability();


        YandexGame.GameplayStop();
    }

    void ConfigMainPanel(bool inRaidStatus)
    {
        if (inRaidStatus)
        {
            _garageMainPanel.HidePanel();
        }
        else
        {
            _garageMainPanel.ResetPosition();
            _startRaidBtn.interactable = true;
            _openInventoryBtn.interactable = true;
            _changeLevelsBtn.interactable = true;
            _startSurviveModBtn.interactable = true;
        }

    }

    void SurviveModCheckAvailability()
    {
        if (PlayerData.Instance.UnlockedLevelsNames.Contains("1-7"))
        {
            _startSurviveModBtn.interactable = true;
            _lockSurviveModeIcon.gameObject.SetActive(false);
        }
        else
        {
            _startSurviveModBtn.interactable = false;
            _lockSurviveModeIcon.gameObject.SetActive(true);
        }
    }
}


