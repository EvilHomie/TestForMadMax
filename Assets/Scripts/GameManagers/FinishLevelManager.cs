using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class FinishLevelManager : MonoBehaviour
{
    public static FinishLevelManager Instance;

    [SerializeField] Image _blackoutImage;
    [SerializeField] float _blackoutDuration;
    [SerializeField] float _blackoutDelay = 2;
    [SerializeField] ViewingAdsYG _viewingAdsYG;
    [SerializeField] TextMeshProUGUI _levelStatusText;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    private void Start()
    {
        _blackoutImage.gameObject.SetActive(false);
        _levelStatusText.transform.parent.gameObject.SetActive(false);
        _viewingAdsYG.customEvents.CloseAd.AddListener(OnAdClose);
        _viewingAdsYG.customEvents.OpenAd.AddListener(OnAdOpen);
    }

    public void OnFinishLevel(bool isSuccessfully)
    {
        StartCoroutine(OnLevelFinishLogic());
        ShowLevelStatusPanel(isSuccessfully);
    }

    IEnumerator OnLevelFinishLogic()
    {
        yield return new WaitForSeconds(_blackoutDelay);
        float t = 0;
        _blackoutImage.gameObject.SetActive(true);
        while (t <= 1)
        {
            t += Time.deltaTime / _blackoutDuration;
            Color color = Color.Lerp(Color.clear, Color.black, t);
            //Debug.LogWarning(t);
            _blackoutImage.color = color;
            yield return null;
        }
        UILevelStatistic.Instance.ShowStatistic();
        GameManager.Instance.OnReturnToGarage();
    }

    public void OnCloseLevelStatisic()
    {
        _blackoutImage.gameObject.SetActive(false);
        _levelStatusText.transform.parent.gameObject.SetActive(false);
        YandexGame.FullscreenShow();
    }

    void OnAdClose()
    {
        AudioManager.Instance.EnableMasterSound();
    }
    void OnAdOpen()
    {
        AudioManager.Instance.DisableMasterSound();
    }

    void ShowLevelStatusPanel(bool isSuccessfully)
    {
        if (isSuccessfully)
        {
            _levelStatusText.color = Color.green;
            _levelStatusText.text = TextConstants.RAIDDONE;
        }
        else
        {
            _levelStatusText.color = Color.red;
            _levelStatusText.text = TextConstants.RAIDFAILED;
        }
        _levelStatusText.transform.parent.gameObject.SetActive(true);
    }
}
