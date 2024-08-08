using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Button _startRaidBtn;
    [SerializeField] Button _garageBtn;
    [SerializeField] Button _menuBtn;

    CanvasGroup _menuCanvasGroup;
    CanvasGroup _startRaidBtnCG;

    float _showControllerDelay = 13f; // зависит от звука запуска двигателя, а точнее времени набора стартовой скорости


    bool _playerOnRaid = false;


    void Awake()
    {
        _menuCanvasGroup = _startRaidBtn.transform.parent.GetComponent<CanvasGroup>();
        _startRaidBtnCG = _startRaidBtn.GetComponent<CanvasGroup>();
    }

    void Start()
    {
        _startRaidBtn.onClick.AddListener(StartRaid);
        _garageBtn.onClick.AddListener(ReturntToGarage);
        _menuBtn.onClick.AddListener(ToggleMenu);
        _menuCanvasGroup.alpha = 1;
        _menuCanvasGroup.interactable = true;
        _startRaidBtnCG.alpha = 1;
        TouchController.Instance.HideControllers();
    }

    void ToggleMenu()
    {
        _menuCanvasGroup.alpha = _menuCanvasGroup.alpha == 1 ? 0 : 1;
        _menuCanvasGroup.interactable = _menuCanvasGroup.alpha == 1;
    }

    void StartRaid()
    {
        PlayerVehicleManager.Instance.OnPlayerStartRaid();
        ShakeCamera.Instance.OnPlayerStartRaid();
        _playerOnRaid = true;
        ToggleMenu();
        _startRaidBtnCG.alpha = 0;
        _startRaidBtnCG.interactable = false;
        TouchController.Instance.ShowControllers(_showControllerDelay);
    }

    void ReturntToGarage()
    {
        _playerOnRaid = false;
        GarageBoxManager.Instance.OnPlayerEndRaid();
        PlayerVehicleManager.Instance.OnPlayerEndRaid();
        RaidObjectsManager.Instance.OnPlayerEndRaid();
        ShakeCamera.Instance.OnPlayerEndRaid();
        _startRaidBtnCG.alpha = 1;
        _startRaidBtnCG.interactable = true;
        TouchController.Instance.HideControllers();
    }
}
