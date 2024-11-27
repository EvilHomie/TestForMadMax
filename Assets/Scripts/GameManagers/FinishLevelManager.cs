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
    [SerializeField] InfoYG _infoYG;
    [SerializeField] TextMeshProUGUI _levelStatusText;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    public void Init()
    {
        _blackoutImage.gameObject.SetActive(false);
        _levelStatusText.transform.parent.gameObject.SetActive(false);
        _viewingAdsYG.customEvents.CloseAd.AddListener(OnAdClose);
        _viewingAdsYG.customEvents.OpenAd.AddListener(OnAdOpen);
    }

    public void OnFinishLevel(string levelName, bool isSuccessfully)
    {        
        StartCoroutine(OnLevelFinishLogic(levelName, isSuccessfully));
        ShowLevelStatusPanel(isSuccessfully);        
    }

    void AditionActionsOnFinishLevel(string levelName, bool isSuccessfully)
    {
        if (isSuccessfully)
        {           
            if(levelName == "1-1") TutorialManager.Instance.TryEnableStage(StageName.FirstLevelCompleted);
            else if (levelName == "1-2") TutorialManager.Instance.TryEnableStage(StageName.ShowLevelStatisticPanel);
        }
        else
        {
            //TutorialManager.Instance.TryEnableStage(StageName.FirstLevelFailed);
        }
    }

    IEnumerator OnLevelFinishLogic(string levelName, bool isSuccessfully)
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
        AditionActionsOnFinishLevel(levelName, isSuccessfully);
    }

    public void OnCloseLevelStatisicAndOpenInventory()
    {
        _blackoutImage.gameObject.SetActive(false);
        _levelStatusText.transform.parent.gameObject.SetActive(false);
        TutorialManager.Instance.TryConfirmStage(StageName.SecondLevelCompleted);

        if (YandexGame.timerShowAd >= YandexGame.Instance.infoYG.fullscreenAdInterval)
        {
            _viewingAdsYG.customEvents.CloseAd.AddListener(OpenInventoryOnCloseAD);
            YandexGame.FullscreenShow();
        }
        else
        {            
            InventoryManager.Instance.OnOpenInventory();
        }
    }

    public void OnCloseLevelStatisicAndStartNewRaid()
    {
        _blackoutImage.gameObject.SetActive(false);
        _levelStatusText.transform.parent.gameObject.SetActive(false);
        TutorialManager.Instance.TryConfirmStage(StageName.FirstLevelCompleted);

        if (YandexGame.timerShowAd >= YandexGame.Instance.infoYG.fullscreenAdInterval)
        {
            _viewingAdsYG.customEvents.CloseAd.AddListener(StartNewRaidOnCloseAD);
            YandexGame.FullscreenShow();
        }
        else
        {
            GameManager.Instance.OnStartRaid();
        }
    }

    void StartNewRaidOnCloseAD()
    {
        GameManager.Instance.OnStartRaid();
        //Debug.Log("NEW RAID AFTER AD");
        _viewingAdsYG.customEvents.CloseAd.RemoveListener(StartNewRaidOnCloseAD);
    }

    void OpenInventoryOnCloseAD()
    {
        InventoryManager.Instance.OnOpenInventory();
        //Debug.Log("NEW RAID AFTER AD");
        _viewingAdsYG.customEvents.CloseAd.RemoveListener(OpenInventoryOnCloseAD);
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
