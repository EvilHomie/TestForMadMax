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
        //_viewingAdsYG.customEvents.CloseAd.AddListener(OnAdClose);
        //_viewingAdsYG.customEvents.OpenAd.AddListener(OnAdOpen);
    }

    public void OnFinishLevel(bool isSuccessfully)
    {        
        StartCoroutine(OnLevelFinishLogic(isSuccessfully));
        ShowLevelStatusPanel(isSuccessfully);        
    }

    void AditionActionsOnFinishLevel(bool isSuccessfully)
    {
        if (isSuccessfully)
        {
            if (InRaidManager.Instance.SelectedLeveParameters.LevelName == "1-1")
            {
                TutorialManager.Instance.TryEnableStage(StageName.FirstLevelCompleted);
            }
            else if (InRaidManager.Instance.SelectedLeveParameters.LevelName == "1-2")
            {
                TutorialManager.Instance.TryEnableStage(StageName.ShowLevelStatisticPanel);
            }

        }
        else
        {
            //TutorialManager.Instance.TryEnableStage(StageName.FirstLevelFailed);
        }
    }

    IEnumerator OnLevelFinishLogic(bool isSuccessfully)
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
        //UILevelStatistic.Instance.ShowStatistic();
        UpgradesAfterLevel.Instance.ConfigPanel();
        AditionActionsOnFinishLevel(isSuccessfully);
        GameManager.Instance.OnReturnToGarage();
    }

    public void OnCloseLevelStatisicAndOpenInventory()
    {
        _blackoutImage.gameObject.SetActive(false);
        _levelStatusText.transform.parent.gameObject.SetActive(false);
        TutorialManager.Instance.TryConfirmStage(StageName.SecondLevelCompleted);

        if (YandexGame.timerShowAd >= YandexGame.Instance.infoYG.fullscreenAdInterval)
        {
            _viewingAdsYG.customEvents.CloseAd.AddListener(OpenInventoryOnCloseAD);
            MainAudioManager.Instance.DisableMasterSound();
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
            MainAudioManager.Instance.DisableMasterSound();
            YandexGame.FullscreenShow();
        }
        else
        {
            GameManager.Instance.OnStartRaid(true);
        }
    }

    public void OnCloseLevelStatisic()
    {
        _blackoutImage.gameObject.SetActive(false);
        _levelStatusText.transform.parent.gameObject.SetActive(false);
        TutorialManager.Instance.TryConfirmStage(StageName.FirstLevelCompleted);

        if (YandexGame.timerShowAd >= YandexGame.Instance.infoYG.fullscreenAdInterval)
        {
            _viewingAdsYG.customEvents.CloseAd.AddListener(ShowMenuOnCloseAD);
            MainAudioManager.Instance.DisableMasterSound();
            YandexGame.FullscreenShow();
        }
        else
        {
            //GameManager.Instance.OnStartRaid(true);
        }
    }

    void StartNewRaidOnCloseAD()
    {
        MainAudioManager.Instance.EnableMasterSound();
        GameManager.Instance.OnStartRaid(true);
        _viewingAdsYG.customEvents.CloseAd.RemoveListener(StartNewRaidOnCloseAD);
    }

    void OpenInventoryOnCloseAD()
    {
        MainAudioManager.Instance.EnableMasterSound();
        InventoryManager.Instance.OnOpenInventory();
        _viewingAdsYG.customEvents.CloseAd.RemoveListener(OpenInventoryOnCloseAD);
    }

    void ShowMenuOnCloseAD()
    {
        MainAudioManager.Instance.EnableMasterSound();
        //InventoryManager.Instance.OnOpenInventory();
        _viewingAdsYG.customEvents.CloseAd.RemoveListener(ShowMenuOnCloseAD);
    }

    //void OnAdClose()
    //{
    //    MainAudioManager.Instance.EnableMasterSound();
    //}
    //void OnAdOpen()
    //{
    //    MainAudioManager.Instance.DisableMasterSound();
    //}

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
