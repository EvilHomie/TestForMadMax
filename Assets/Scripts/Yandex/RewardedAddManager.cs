using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class RewardedAddManager : MonoBehaviour
{
    public static RewardedAddManager Instance;

    RewardName _proposedRewardName;
    Action<bool> _rewardReceivedAction;
    RewardAdResult _lastRewardAdResult;

    Dictionary<RewardName, int> _rewardIdByName = new()
    {
        {RewardName.RestoreHP, 1 },
        {RewardName.FreeUpgrade, 2 },
        {RewardName.BonusCard, 3 }
    };


    bool _rewardedResult = true;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        YandexGame.Instance.infoYG.rewardedAfterClosing = false;
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += OnGetRewarded;
        YandexGame.ErrorVideoEvent += OnRewardError;
        YandexGame.CloseVideoEvent += OnCloseVideoEvent;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= OnGetRewarded;
        YandexGame.ErrorVideoEvent -= OnRewardError;
        YandexGame.CloseVideoEvent -= OnCloseVideoEvent;
    }

    void OnCloseVideoEvent()
    {
        bool result = _lastRewardAdResult == RewardAdResult.Success ? true : false;
        OnReceivedRewardResult(result);
    }

    void OnGetRewarded(int id)
    {
        _lastRewardAdResult = RewardAdResult.Success;
    }

    void OnRewardError()
    {
        _lastRewardAdResult = RewardAdResult.Error;
        SendMetrica(RewardStatus.Error);
    }

    public void PrepareReward(Action<bool> action, RewardName rewardName)
    {
        _lastRewardAdResult = RewardAdResult.None;
        GameFlowManager.Instance.SetPause(this);
        _rewardReceivedAction = action;
        _proposedRewardName = rewardName;
    }

    public void OnAcceptOffer()
    {
        YandexGame.RewVideoShow(_rewardIdByName[_proposedRewardName]);
        SendMetrica(RewardStatus.Accept);
    }

    public void OnCancelOffer()
    {
        OnReceivedRewardResult(false);
        SendMetrica(RewardStatus.Cancel);
    }

    public void OnPlayerCloseOffer()
    {
        OnReceivedRewardResult(false);
        SendMetrica(RewardStatus.PlayerCloseOffer);
    }

    public void OnOfferTimeOut()
    {
        OnReceivedRewardResult(false);
        SendMetrica(RewardStatus.TimeOut);
    }


    void OnReceivedRewardResult(bool result)
    {
        if (_rewardReceivedAction != null)
        {
            _rewardReceivedAction.Invoke(result);
            _rewardReceivedAction = null;
        }
        GameFlowManager.Instance.Unpause(this);
    }


    void SendMetrica(RewardStatus rewardStatus)
    {
        if (_proposedRewardName == RewardName.FreeUpgrade) MetricaSender.QuickImprovementAfterLevel(InRaidManager.Instance.SelectedLeveParameters.LevelName, rewardStatus);
        if (_proposedRewardName == RewardName.RestoreHP) MetricaSender.RestoreHPByADD(InRaidManager.Instance.SelectedLeveParameters.LevelName, rewardStatus);
    }

    public void OnNotAvailable()
    {
        GameFlowManager.Instance.Unpause(this);
    }
}

public enum RewardName
{
    RestoreHP,
    FreeUpgrade,
    BonusCard
}

public enum RewardStatus
{
    Error,
    Accept,
    Cancel,
    PlayerCloseOffer,
    TimeOut
}
enum RewardAdResult
{
    None,
    Success,
    Error
};