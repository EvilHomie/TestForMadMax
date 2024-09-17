using System.Collections;
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


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    private void Start()
    {
        _blackoutImage.gameObject.SetActive(false);
        _viewingAdsYG.customEvents.CloseAd.AddListener(OnAdClose);
        _viewingAdsYG.customEvents.OpenAd.AddListener(OnAdOpen);
    }

    public void OnFinishLevel()
    {
        StartCoroutine(OnLevelClearLogic());
    }

    IEnumerator OnLevelClearLogic()
    {
        yield return new WaitForSeconds(_blackoutDelay);
        float t = 0;
        _blackoutImage.gameObject.SetActive(true);
        while (t <= 1)
        {
            t += Time.deltaTime / _blackoutDuration;
            Color color = Color.Lerp(Color.clear, Color.black, t);

            _blackoutImage.color = color;
            yield return null;
        }
        UILevelStatistic.Instance.ShowStatistic();
        GameManager.Instance.OnReturnToGarage();
    }

    public void OnCloseLevelStatisic()
    {
        _blackoutImage.gameObject.SetActive(false);        
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

}
