using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISurviveModePausePanel : MonoBehaviour
{
    public static UISurviveModePausePanel Instance;

    [SerializeField] Transform _blackoutImage;
    [SerializeField] Button _continueButton;
    [SerializeField] Button _abortButton;
    [SerializeField] TextMeshProUGUI _abortText;
    [SerializeField] TextMeshProUGUI _characteristicsText;

    [SerializeField] TextMeshProUGUI _hullHPText;
    [SerializeField] TextMeshProUGUI _hullHPValueText;

    [SerializeField] TextMeshProUGUI _shieldHPText;
    [SerializeField] TextMeshProUGUI _shieldValueHPText;

    [SerializeField] TextMeshProUGUI _shieldRegText;
    [SerializeField] TextMeshProUGUI _shieldRegValueText;

    [SerializeField] TextMeshProUGUI _damageText;
    [SerializeField] TextMeshProUGUI _damageValueText;

    [SerializeField] TextMeshProUGUI _fireRateText;
    [SerializeField] TextMeshProUGUI _fireRateValueText;

    [SerializeField] TextMeshProUGUI _reloadTimeText;
    [SerializeField] TextMeshProUGUI _reloadTimeValueText;

    [SerializeField] TextMeshProUGUI _capacityText;
    [SerializeField] TextMeshProUGUI _capacityValueText;

    bool _blackoutImageEnableStateOnOpen;
    bool _cursorIsVisibleOnOpen;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        gameObject.SetActive(false);

        _abortText.text = TextConstants.ABORT;
        _characteristicsText.text = TextConstants.CHARACTERISTICS;

        _hullHPText.text = TextConstants.HULLHP;
        _shieldHPText.text = TextConstants.SHIELDHP;
        _shieldRegText.text = TextConstants.SHIELREGENRATE;
        _damageText.text = TextConstants.DMG;
        _fireRateText.text = TextConstants.FIRERATE;
        _reloadTimeText.text = TextConstants.RELOADTIME;
        _capacityText.text = TextConstants.CAPACITY;
    }


    public void OnStartSurviveMode()
    {
        
    }

    public void OnStopSurviveMode()
    {
        gameObject.SetActive(false);
        GameFlowManager.Instance.Unpause(this);
        if (!_blackoutImageEnableStateOnOpen)
        {
            _blackoutImage.gameObject.SetActive(false);
        }
    }

    public void OnOpenPausePanel()
    {
        if(gameObject.activeSelf) return;
        _blackoutImageEnableStateOnOpen = _blackoutImage.gameObject.activeSelf;
        _cursorIsVisibleOnOpen = Cursor.visible;
        if (!_blackoutImageEnableStateOnOpen)
        {
            _blackoutImage.gameObject.SetActive(true);
        }
        if (!_cursorIsVisibleOnOpen)
        {
            Cursor.visible = true;
        }
        

        GameFlowManager.Instance.SetPause(this);

         
         _hullHPValueText.text = SurviveModeManager.Instance.ÑurrentVehicleData.hullHP.ToString();
        _shieldValueHPText.text = SurviveModeManager.Instance.ÑurrentVehicleData.shieldHP.ToString();
        _shieldRegValueText.text = SurviveModeManager.Instance.ÑurrentVehicleData.shieldRegRate.ToString();

        _damageValueText.text = SurviveModeManager.Instance.CurrentWeaponData.kineticDamage.ToString();
        _fireRateValueText.text = SurviveModeManager.Instance.CurrentWeaponData.fireRate.ToString();
        _reloadTimeValueText.text = SurviveModeManager.Instance.CurrentWeaponData.reloadTime.ToString();
        _capacityValueText.text = SurviveModeManager.Instance.CurrentWeaponData.magCapacity.ToString();


        gameObject.SetActive(true);
        
    }

    public void OnClosePausePanel()
    {
        if (!_blackoutImageEnableStateOnOpen)
        {
            _blackoutImage.gameObject.SetActive(false);
        }
        if (!_cursorIsVisibleOnOpen)
        {
            Cursor.visible = false;
        }
        gameObject.SetActive(false);
        GameFlowManager.Instance.Unpause(this);
    }
}
