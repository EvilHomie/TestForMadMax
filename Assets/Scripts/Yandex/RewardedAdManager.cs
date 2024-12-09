using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class RewardedAdManager : MonoBehaviour
{
    public static RewardedAdManager Instance;

    [SerializeField] Button _acceptOfferButton;
    [SerializeField] Button _cancelOfferButton;
    [SerializeField] TextMeshProUGUI _acceptOfferButtonText;
    [SerializeField] TextMeshProUGUI _cancelOfferButtonText;
    [SerializeField] TextMeshProUGUI _offerText;

    RewardName _proposedRewardName;
    Action<bool> _rewardAction;

    Dictionary<RewardName, int> _rewardIdByName = new()
    {
        {RewardName.RestoreHP, 1 }
    };

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _acceptOfferButtonText.text = TextConstants.ACCEPT;
        _cancelOfferButtonText.text = TextConstants.CANCEL;
        _acceptOfferButton.onClick.AddListener(OnAcceptOffer);
        _cancelOfferButton.onClick.AddListener(OnCancelOffer);
        YandexGame.RewardVideoEvent += Rewarded;
        gameObject.SetActive(false);
        //YandexGame.ErrorVideoEvent += OnRewardError;
    }

    //// Подписываемся на событие открытия рекламы в OnEnable
    //private void OnEnable()
    //{

    //}
    //// Отписываемся от события открытия рекламы в OnDisable
    //private void OnDisable()
    //{
    //    _acceptOfferButton.onClick.RemoveListener(OnAcceptOffer);
    //    _cancelOfferButton.onClick.RemoveListener(OnCancelOffer);
    //    YandexGame.RewardVideoEvent -= Rewarded;
    //}
    // Подписанный метод получения награды
    void Rewarded(int id)
    {
        // Если ID = 1, то востанавливаем прочность
        if (id == 1)
            _rewardAction.Invoke(true);

        YandexGame.ErrorVideoEvent -= OnRewardError;
        GameFlowManager.Instance.Unpause(this);
        gameObject.SetActive(false);
    }

    void OnRewardError()
    {
        YandexGame.ErrorVideoEvent -= OnRewardError;
        _rewardAction.Invoke(false);
        Cursor.visible = false;
        GameFlowManager.Instance.Unpause(this);
        gameObject.SetActive(false);
    }

    public void ShowRewardOffer(Action<bool> action, RewardName rewardName)
    {
        YandexGame.ErrorVideoEvent += OnRewardError;
        GameFlowManager.Instance.SetPause(this);
        Cursor.visible = true;
        _rewardAction = action;
        _proposedRewardName = rewardName;
        _offerText.text = TextConstants._rewardText[rewardName];
        gameObject.SetActive(true);
    }

    // Метод для вызова видео рекламы
    void OpenRewardAd()
    {
        // Вызываем метод открытия видео рекламы
        YandexGame.RewVideoShow(_rewardIdByName[_proposedRewardName]);
    }

    void OnAcceptOffer()
    {
        Cursor.visible = false;
        OpenRewardAd();
    }
    void OnCancelOffer()
    {
        YandexGame.ErrorVideoEvent -= OnRewardError;
        _rewardAction.Invoke(false);
        Cursor.visible = false;
        GameFlowManager.Instance.Unpause(this);
        gameObject.SetActive(false);
    }
}

public enum RewardName
{
    RestoreHP
}