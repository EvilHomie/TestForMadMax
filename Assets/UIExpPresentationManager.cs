using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class UIExpPresentationManager : MonoBehaviour
{
    public static UIExpPresentationManager Instance;

    [SerializeField] Image _fillingImage;
    [SerializeField] GameObject _tipPCGO;

    [SerializeField] TextMeshProUGUI _pressSpaceText;
    [SerializeField] Animator _glowAnimator;

    bool _wasShown = false;
    int _forLevelUpAmount;
    int _totalKilledCount;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init(int forLevelUpAmount)
    {
        _pressSpaceText.text = TextConstants.PRESSSPACE;
        _wasShown = false;
        _forLevelUpAmount = forLevelUpAmount;
        gameObject.SetActive(false);
    }

    public void OnStartSurviveMode()
    {
        _fillingImage.fillAmount = 0;
        //_tipPCGO.SetActive(false);
        _pressSpaceText.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void OnStopSurviveMode()
    {
        _glowAnimator.SetTrigger("Disable");
        gameObject.SetActive(false);
    }

    public void OnKillEnemy(int killedCount)
    {
        _totalKilledCount = killedCount;
        if (_fillingImage.fillAmount >= 1) return;
        _fillingImage.fillAmount += 1f / _forLevelUpAmount;
        if (_fillingImage.fillAmount >= 1)
        {
            _glowAnimator.SetTrigger("Enable");
            if (!_wasShown)
            {
                _wasShown = true;
                _pressSpaceText.gameObject.SetActive(true);
            }
            

            //if (YandexGame.EnvironmentData.isDesktop)
            //{
            //    _tipPCGO.SetActive(true);
            //}
        }
    }

    public void OnOpenUpgrades()
    {
        int remainder = _totalKilledCount % _forLevelUpAmount;

        _fillingImage.fillAmount = (float)remainder / _forLevelUpAmount;
        _pressSpaceText.gameObject.SetActive(false);
        _glowAnimator.SetTrigger("Disable");
        //_tipPCGO.SetActive(false);
    }
}
