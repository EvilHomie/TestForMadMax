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

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void Rewarded(int id)
    {
        YandexGame.timerShowAd = 0;
        //Debug.Log("REWARD SUCCESSFUL");
        OnReceivedRewardResult(true);
        return;

        //if (id == 1)
        //{
        //}
    }

    void OnRewardError()
    {
        //Debug.Log("REWARD ERROR");
        OnReceivedRewardResult(false);
    }

    public void PrepareReward(Action<bool> action, RewardName rewardName)
    {
        YandexGame.RewardVideoEvent += Rewarded;
        YandexGame.ErrorVideoEvent += OnRewardError;
        GameFlowManager.Instance.SetPause(this);
        _rewardReceivedAction = action;
        _proposedRewardName = rewardName;
    }

    public void OnAcceptOffer()
    {
        //Debug.Log("ACCEPT OFFER");
        YandexGame.RewVideoShow(_rewardIdByName[_proposedRewardName]);
    }

    public void OnCancelOffer()
    {
        //Debug.Log("CANCEL OFFER");
        OnReceivedRewardResult(false);
    }


    void OnReceivedRewardResult(bool result)
    {
        YandexGame.RewardVideoEvent -= Rewarded;
        YandexGame.ErrorVideoEvent -= OnRewardError;
        GameFlowManager.Instance.Unpause(this);
        if (_rewardReceivedAction != null)
        {
            _rewardReceivedAction.Invoke(result);
            _rewardReceivedAction = null;
        }
    }
}

public enum RewardName
{
    RestoreHP,
    FreeUpgrade
}