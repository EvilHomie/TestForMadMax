using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Button _startRaidBtn;
    [SerializeField] Button _garageBtn;
    [SerializeField] Button _openUpgradesBtn;
    [SerializeField] Button _closeUpgradesBtn;
    [SerializeField] Button _settingsBtn;

    GameObject _inRaidBtns;
    GameObject _inGarageBtns;
    GameObject _menuWindow;
    GameObject _upgradeMenu;

    float _showControllerDelay = 13f; // зависит от звука запуска двигателя, а точнее времени набора стартовой скорости


    bool _playerOnRaid = false;


    void Awake()
    {
        _inGarageBtns = _startRaidBtn.transform.parent.gameObject;
        _inRaidBtns = _garageBtn.transform.parent.gameObject;
        _menuWindow = _inGarageBtns.transform.parent.gameObject;
        _upgradeMenu = _closeUpgradesBtn.transform.parent.gameObject;
    }

    void Start()
    {
        _settingsBtn.onClick.AddListener(ToggleMenu);

        _startRaidBtn.onClick.AddListener(StartRaid);
        _garageBtn.onClick.AddListener(ReturntToGarage);

        _openUpgradesBtn.onClick.AddListener(OpenUpgrades);
        _closeUpgradesBtn.onClick.AddListener(CloseUpgrades);

        TouchController.Instance.HideControllers();
        WeaponsSwitcher.Instance.OnPlayerEndRaid();
        PlayerWeaponPointManager.Instance.OnPlayerEndRaid();

        _playerOnRaid = false;
        SwitchMenuBtns();
    }

    void ToggleMenu()
    {        
        _menuWindow.SetActive(!_menuWindow.activeSelf);

        //Time.timeScale = _menuCanvasGroup.alpha == 1 ? 0 : 1 ;
    }

    void OpenUpgrades()
    {
        _upgradeMenu.SetActive(true);
    }
    void CloseUpgrades()
    {
        _upgradeMenu.SetActive(false);
    }

    void StartRaid()
    {
        _playerOnRaid = true;
        PlayerVehicleManager.Instance.OnPlayerStartRaid();
        CameraManager.Instance.OnPlayerStartRaid();        
        TouchController.Instance.ShowControllers(_showControllerDelay);
        SwitchMenuBtns();
        _menuWindow.SetActive(false);
    }

    void ReturntToGarage()
    {
        _playerOnRaid = false;
        WeaponsSwitcher.Instance.OnPlayerEndRaid();
        PlayerWeaponPointManager.Instance.OnPlayerEndRaid();
        GarageBoxManager.Instance.OnPlayerEndRaid();
        PlayerVehicleManager.Instance.OnPlayerEndRaid();
        RaidObjectsManager.Instance.OnPlayerEndRaid();
        CameraManager.Instance.OnPlayerEndRaid();
        TouchController.Instance.HideControllers();
        SwitchMenuBtns();
    }

    void SwitchMenuBtns()
    {
        _inRaidBtns.SetActive(_playerOnRaid);
        _inGarageBtns.SetActive(!_playerOnRaid);
    }
}


