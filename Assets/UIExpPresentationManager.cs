using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIExpPresentationManager : MonoBehaviour
{
    public static UIExpPresentationManager Instance;

    [SerializeField] Image _expFillImageBack;
    [SerializeField] Image _expFillImageFront;
    [SerializeField] GameObject _tipPCGO;

    [SerializeField] TextMeshProUGUI _pressSpaceText;
    [SerializeField] TextMeshProUGUI _collectedCardsCounterText;
    [SerializeField] Animator _glowAnimator;
    [SerializeField] float _fillDuration;

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
        _collectedCardCount = 0;
        _expFillImageBack.fillAmount = 0;
        _expFillImageFront.fillAmount = 0;
        _totalKilledCount = 0;
        _fillValue = 0;
        _collectedCardsCounterText.text = _collectedCardCount.ToString();
        //_tipPCGO.SetActive(false);
        _pressSpaceText.gameObject.SetActive(false);
        _glowAnimator.SetTrigger("Disable");
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
            _fillValue = 0;
            _expFillImageFront.fillAmount = _fillValue;
            
        }
        _fillValue += 1f / (float)_forLevelUpAmount;
        _expFillImageBack.fillAmount = _fillValue;


        if (_fillExpImagesCoroutine == null)
        {
            _fillExpImagesCoroutine = StartCoroutine(FillExpImages());
        }

        if (_fillValue >= 1)
        {
            _collectedCardCount++;
            _collectedCardsCounterText.text = _collectedCardCount.ToString();
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
        _collectedCardCount = 0;
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
        _glowAnimator.SetTrigger("Disable");
        //_tipPCGO.SetActive(false);
    }

    IEnumerator FillExpImages()
    {        
        float startValue = _expFillImageFront.fillAmount;        
        
        float t = 0;
        while (_expFillImageFront.fillAmount < _expFillImageBack.fillAmount)
        {
            t += Time.deltaTime / _fillDuration;
            _expFillImageFront.fillAmount = Mathf.Lerp(startValue, _expFillImageBack.fillAmount, t);
            yield return null;
        }
        _expFillImageFront.fillAmount = _expFillImageBack.fillAmount;
        _fillExpImagesCoroutine = null;
    }
}
