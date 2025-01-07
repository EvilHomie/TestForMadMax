using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LowHpRewardOffer : MonoBehaviour
{
    public static LowHpRewardOffer Instance;

    [SerializeField] Button _acceptOfferButton;
    [SerializeField] Button _cancelOfferButton;
    [SerializeField] Button _closeOfferButton;
    [SerializeField] Slider _HPSlider;
    //[SerializeField] Slider _RestoredSlider;
    [SerializeField] TextMeshProUGUI _panelNameText;
    [SerializeField] TextMeshProUGUI _hullHPText;
    [SerializeField] TextMeshProUGUI _offerText;
    [SerializeField] TextMeshProUGUI _acceptOfferButtonText;
    [SerializeField] TextMeshProUGUI _cancelOfferButtonText;
    [SerializeField] TextMeshProUGUI _cancelOfferTimerText;
    [SerializeField] float _autoCloseDelay = 10f;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        SetTexts();
        AddListeners();
        gameObject.SetActive(false);
    }

    void SetTexts()
    {
        _panelNameText.text = TextConstants.EMERGENCYREPAIR;
        _hullHPText.text = TextConstants.HULLHP;
        _offerText.text = TextConstants._rewardText[RewardName.RestoreHP];
        _acceptOfferButtonText.text = TextConstants.REPAIR;
        _cancelOfferButtonText.text = $"{TextConstants.CONTINUE} {TextConstants.WITHOUT} {TextConstants.REPAIR}";
    }
    void AddListeners()
    {
        _acceptOfferButton.onClick.AddListener(OnAcceptOffer);
        _cancelOfferButton.onClick.AddListener(OnCancelOffer);
        _closeOfferButton.onClick.AddListener(OnPlayerCloseOffer);
    }

    public void ShowRewardOffer(Action<bool> action, RewardName rewardName, float currentHPValue)
    {
        RewardedAdManager.Instance.PrepareReward(action, rewardName);
        _HPSlider.value = currentHPValue;
        TogglePanel(true);
    }
    void OnAcceptOffer()
    {
        RewardedAdManager.Instance.OnAcceptOffer();
        TogglePanel(false);
    }

    void OnCancelOffer()
    {
        RewardedAdManager.Instance.OnCancelOffer();
        TogglePanel(false);
    }


    void OnPlayerCloseOffer()
    {
        RewardedAdManager.Instance.OnPlayerCloseOffer();
        TogglePanel(false);
    }

    void TogglePanel(bool activeStatus)
    {
        Cursor.visible = activeStatus;
        gameObject.SetActive(activeStatus);

        if (activeStatus)
        {
            StartCoroutine(TimeOutCoroutine());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    IEnumerator TimeOutCoroutine()
    {
        float time = _autoCloseDelay;

        while (time > 0)
        {
            time -= Time.unscaledDeltaTime;
            _cancelOfferTimerText.text = $"{string.Format("{0:f1}", time)} {TextConstants.SEC}";
            yield return null;
        }
        RewardedAdManager.Instance.OnOfferTimeOut();
        TogglePanel(false);
    }
}
