using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class UIExpPresentationManager : MonoBehaviour
{
    public static UIExpPresentationManager Instance;

    [SerializeField] Image _expFillImageBack;
    [SerializeField] Image _expFillImageFront;
    [SerializeField] GameObject _tipPCGO;

    [SerializeField] TextMeshProUGUI _pressSpaceText;
    [SerializeField] TextMeshProUGUI _collectedCardsCounterText;
    [SerializeField] Animator _glowAnimator;
    [SerializeField] float _fillSpeed;

    int _fillSpeedMod;
    bool _wasShown = false;
    int _forLevelUpAmount;
    int _totalKilledCount;
    int _collectedCardCount;
    float _fillValue;
    Coroutine _fillExpImagesCoroutine;

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
        StopAllCoroutines();
        _fillExpImagesCoroutine = null;
        _fillSpeedMod = 1;
        _collectedCardCount = 0;
        _expFillImageBack.fillAmount = 0;
        _expFillImageFront.fillAmount = 0;
        _totalKilledCount = 0;
        _fillValue = 0;
        _collectedCardsCounterText.text = _collectedCardCount.ToString();
        //_tipPCGO.SetActive(false);
        _pressSpaceText.gameObject.SetActive(false);
        _glowAnimator.SetBool("Enabled", false);
        gameObject.SetActive(true);
    }

    public void OnStopSurviveMode()
    {
        gameObject.SetActive(false);
    }

    public void AddExpOnKillEnemy()
    {
        if (_fillValue >= 1)
        {
            if (_fillExpImagesCoroutine != null)
            {
                StopCoroutine(_fillExpImagesCoroutine);
                _fillExpImagesCoroutine = null;
            }
            _fillSpeedMod = 1;
            _fillValue = 0;
            _expFillImageFront.fillAmount = _fillValue;

        }
        _fillValue += 1f / (float)_forLevelUpAmount;
        _expFillImageBack.fillAmount = _fillValue;


        if (_fillExpImagesCoroutine == null)
        {
            _fillExpImagesCoroutine = StartCoroutine(FillExpImages());
        }
        else
        {
            _fillSpeedMod++;
        }

        if (_fillValue >= 1)
        {
            _collectedCardCount++;
            _collectedCardsCounterText.text = _collectedCardCount.ToString();
            _glowAnimator.SetBool("Enabled", true);

            if (!_wasShown && YandexGame.EnvironmentData.isDesktop)
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
        _collectedCardCount = 0;
        _collectedCardsCounterText.text = _collectedCardCount.ToString();
        if (_fillValue >= 1)
        {
            if (_fillExpImagesCoroutine != null)
            {
                StopCoroutine(_fillExpImagesCoroutine);
                _fillExpImagesCoroutine = null;
            }
            _fillValue = 0;
            _expFillImageBack.fillAmount = _fillValue;
            _expFillImageFront.fillAmount = _fillValue;
        }
        _pressSpaceText.gameObject.SetActive(false);
        _glowAnimator.SetBool("Enabled", false);
        //_tipPCGO.SetActive(false);
    }

    IEnumerator FillExpImages()
    {
        while (_expFillImageFront.fillAmount < _expFillImageBack.fillAmount)
        {
            _expFillImageFront.fillAmount += _fillSpeed / (float)_forLevelUpAmount * Time.deltaTime * _fillSpeedMod;
            yield return null;
        }
        _expFillImageFront.fillAmount = _expFillImageBack.fillAmount;
        _fillExpImagesCoroutine = null;
    }
}
