using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class RewardedAdManager : MonoBehaviour
{
    public static RewardedAdManager Instance;

    RewardName _proposedRewardName;
    Action<bool> _rewardReceivedAction;

    Dictionary<RewardName, int> _rewardIdByName = new()
    {
        {RewardName.RestoreHP, 1 },
        {RewardName.FreeUpgrade, 2 }
    };


    bool _firstOffer = true;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void Rewarded(int id)
    {
        //YandexGame.timerShowAd = 0;
        //Debug.Log("REWARD SUCCESSFUL");
        OnReceivedRewardResult(true);
        return;

        //if (id == 2)
        //{
        //}
    }

    void OnRewardError()
    {
        //Debug.Log("REWARD ERROR");
        OnReceivedRewardResult(true);
        SendMetrica(RewardStatus.Error);
    }

    public void PrepareReward(Action<bool> action, RewardName rewardName)
    {
        if (_firstOffer)
        {
            _firstOffer = false;
            YandexGame.RewardVideoEvent += Rewarded;
            YandexGame.ErrorVideoEvent += OnRewardError;
        }
        
        GameFlowManager.Instance.SetPause(this);
        _rewardReceivedAction = action;
        _proposedRewardName = rewardName;
    }

    public void OnAcceptOffer()
    {
        //Debug.Log("ACCEPT OFFER");
        YandexGame.RewVideoShow(_rewardIdByName[_proposedRewardName]);
        SendMetrica(RewardStatus.Accept);
    }

    public void OnCancelOffer()
    {
        //Debug.Log("CANCEL OFFER");
        OnReceivedRewardResult(false);
        SendMetrica(RewardStatus.Cancel);
    }

    public void OnPlayerCloseOffer()
    {
        //Debug.Log("CLOSE OFFER");
        OnReceivedRewardResult(false);
        SendMetrica(RewardStatus.PlayerCloseOffer);
    }

    public void OnOfferTimeOut()
    {
        //Debug.Log("CLOSE OFFER");
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
    FreeUpgrade
}

public enum RewardStatus
{
    Error,
    Accept,
    Cancel,
    PlayerCloseOffer,
    TimeOut
}