using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LowHpRewardOffer : MonoBehaviour
{
    public static LowHpRewardOffer Instance;

    [SerializeField] Button _acceptOfferButton;
    [SerializeField] Button _cancelOfferButton;
    [SerializeField] TextMeshProUGUI _acceptOfferButtonText;
    [SerializeField] TextMeshProUGUI _cancelOfferButtonText;
    [SerializeField] TextMeshProUGUI _offerText;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _acceptOfferButtonText.text = TextConstants.ACCEPT;
        _cancelOfferButtonText.text = TextConstants.CANCEL;
        _offerText.text = TextConstants._rewardText[RewardName.RestoreHP];
        _acceptOfferButton.onClick.AddListener(OnAcceptOffer);
        _cancelOfferButton.onClick.AddListener(OnCancelOffer);
        gameObject.SetActive(false);
    }

    public void ShowRewardOffer(Action<bool> action, RewardName rewardName)
    {
        Cursor.visible = true;
        RewardedAdManager.Instance.PrepareReward(action, rewardName);
        gameObject.SetActive(true);
    }

    void OnCancelOffer()
    {
        Cursor.visible = false;
        RewardedAdManager.Instance.OnCancelOffer();
        gameObject.SetActive(false);
    }

    void OnAcceptOffer()
    {        
        RewardedAdManager.Instance.OnAcceptOffer();
        gameObject.SetActive(false);
    }
}
