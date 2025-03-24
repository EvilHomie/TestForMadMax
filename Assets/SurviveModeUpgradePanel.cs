
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SurviveModeUpgradePanel : MonoBehaviour
{
    public static SurviveModeUpgradePanel Instance;

    [SerializeField] Transform _upgradeCardsContainer;
    [SerializeField] UIUpgradeCard UIUpgradeCardPF;
    [SerializeField] UINewWeaponCard UINewWeaponCardPF;
    [SerializeField] Button _availableLevelButton;
    [SerializeField] GameObject _darkBG;

    [SerializeField] Transform _ADSPanel;
    [SerializeField] Button _accepOfferBTN;
    [SerializeField] Button _cancelOfferBTN;
    [SerializeField] int _showADSCloseAmount;
    [SerializeField] float _autoCloseDelay = 10f;

    [SerializeField] TextMeshProUGUI _cancelOfferTimerText;
    [SerializeField] TextMeshProUGUI _offerText;
    [SerializeField] TextMeshProUGUI _collectText;
    [SerializeField] TextMeshProUGUI _cancelText;


    int _closeCounter;
    bool _showUpgradeCardsAutomatic;
    int _collectedLvlUps = 0;
    bool _panelIsOpened;
    UpgradeCardData _bonusCardData;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void OnEnable()
    {
        _accepOfferBTN.onClick.AddListener(OnAcceptOffer);
        _cancelOfferBTN.onClick.AddListener(OnCancelOffer);
    }

    private void OnDisable()
    {
        _accepOfferBTN.onClick.RemoveAllListeners();
        _cancelOfferBTN.onClick.RemoveAllListeners();
    }

    public void Init(bool showUpgradeCardsAutomatic)
    {
        _offerText.text = TextConstants.BONUSCARDOFFERTEXT;
        _collectText.text = TextConstants.COLLECT;
        _cancelText.text = TextConstants.BONUSCARDCANCELEXT;


        _showUpgradeCardsAutomatic = showUpgradeCardsAutomatic;
        _availableLevelButton.onClick.AddListener(OpenLevelUpPanel);
        ResetPanel();
    }

    public void OpenLevelUpPanel()
    {
        if (_collectedLvlUps <= 0) return;
        if(_panelIsOpened) return;
        
        UIExpPresentationManager.Instance.OnOpenUpgrades();
        GameFlowManager.Instance.SetPause(this);
        _darkBG.SetActive(true);
        ShowCards();
    }

    public void OnStartMode()
    {
        ResetPanel();
    }

    private void ResetPanel()
    {
        _closeCounter = 0;
        _ADSPanel.gameObject.SetActive(false);
        _darkBG.SetActive(false);
        _panelIsOpened = false;
        _collectedLvlUps = 0;
        _upgradeCardsContainer.parent.gameObject.SetActive(false);
        TESTSurviveModStatistics.Instance.UpdateCardsPack(_collectedLvlUps);
    }

    public void OnPlayerLevelUp()
    {
        if (_showUpgradeCardsAutomatic)
        {
            ShowCards();
            GameFlowManager.Instance.SetPause(this);
        }
        else
        {
            _collectedLvlUps++;
            _availableLevelButton.gameObject.SetActive(true);
        }

        TESTSurviveModStatistics.Instance.UpdateCardsPack(_collectedLvlUps);
    }

    public void OnGiveNewWeapon(string weaponName)
    {
        _darkBG.SetActive(true);
        GameFlowManager.Instance.SetPause(this);
        foreach (Transform t in _upgradeCardsContainer)
        {
            if(t == _ADSPanel) continue;
            Destroy(t.gameObject);
        }
        UINewWeaponCard UINewWeaponCard = Instantiate(UINewWeaponCardPF, _upgradeCardsContainer);
        UINewWeaponCard.ConfigCard(weaponName);
        _upgradeCardsContainer.parent.gameObject.SetActive(true);
        Cursor.visible = true;
    }
    void ShowCards()
    {
        _panelIsOpened = true;
        List<UpgradeCardData> upgradeCardDatas = SurviveModeUpgradeService.Instance.GetCardsCollection();

        foreach (Transform t in _upgradeCardsContainer)
        {
            if (t == _ADSPanel) continue;
            Destroy(t.gameObject);
        }

        foreach (var cardData in upgradeCardDatas)
        {
            UIUpgradeCard uIUpgradeCard = Instantiate(UIUpgradeCardPF, _upgradeCardsContainer);
            uIUpgradeCard.ConfigCard(cardData);
        }
        _upgradeCardsContainer.parent.gameObject.SetActive(true);
        Cursor.visible = true;
    }

   

    public void OnCardSelected()
    {
        if (_showUpgradeCardsAutomatic)
        {
            Cursor.visible = false;
            GameFlowManager.Instance.Unpause(this);
            _upgradeCardsContainer.parent.gameObject.SetActive(false);
            _panelIsOpened = false;
        }
        else
        {
            _collectedLvlUps--;
            TESTSurviveModStatistics.Instance.UpdateCardsPack(_collectedLvlUps);
            if (_collectedLvlUps > 0)
            {
                ShowCards();
            }
            else
            {
                OnNoCardsLeft();
            }
        }        
    }

    public void OnWeaponSelectConfirm()
    {
        GameFlowManager.Instance.Unpause(this);
        _upgradeCardsContainer.parent.gameObject.SetActive(false);
        Cursor.visible = false;
        _darkBG.SetActive(false);
    }

    void OnNoCardsLeft()
    {
        _closeCounter++;
        MetricaSender.SendSurviveModeGoal(SurviveModeGoal.OpenUpgrades, _closeCounter.ToString());
        if (_closeCounter % _showADSCloseAmount == 0)
        {
            ShowBonusCardOffer();
        }
        else
        {
            CloseUpgradePanel();
        }
    }

    void CloseUpgradePanel()
    {
        GameFlowManager.Instance.Unpause(this);
        _upgradeCardsContainer.parent.gameObject.SetActive(false);
        _panelIsOpened = false;

        _darkBG.SetActive(false);
        Cursor.visible = false;
    }

    public void OnLeaveSurviveMode()
    {
        if (_upgradeCardsContainer.parent.gameObject.activeSelf)
        {
            CloseUpgradePanel();
        }
    }

    void OnGetRewardResult(bool GetRewardStatus)
    {
        if (GetRewardStatus)
        {
            if (_bonusCardData.UpgradeItemType == UpgradeItemType.Weapon)
            {
                SurviveModeUpgradeService.Instance.OnWeaponUpgrade(_bonusCardData);
            }
            else
            {
                SurviveModeUpgradeService.Instance.OnVehicleUpgrade(_bonusCardData);
            }
        }
        CloseUpgradePanel();
    }

    public void ShowBonusCardOffer()
    {
        ShowBonusCard();
        RewardedAddManager.Instance.PrepareReward(OnGetRewardResult, RewardName.BonusCard);
        ToggleRewardPanel(true);
    }

    void ShowBonusCard()
    {
        _panelIsOpened = true;
        List<UpgradeCardData> upgradeCardDatas = SurviveModeUpgradeService.Instance.GetCardsCollection();

        int randomIndex = Random.Range(0, upgradeCardDatas.Count);
        _bonusCardData = upgradeCardDatas[randomIndex];

        foreach (Transform t in _upgradeCardsContainer)
        {
            if (t == _ADSPanel) continue;
            Destroy(t.gameObject);
        }
        UIUpgradeCard uIUpgradeCard = Instantiate(UIUpgradeCardPF, _upgradeCardsContainer);
        uIUpgradeCard.ConfigCard(_bonusCardData, true);
        uIUpgradeCard.transform.SetSiblingIndex(0);
        _upgradeCardsContainer.parent.gameObject.SetActive(true);
        Cursor.visible = true;
    }
    void OnAcceptOffer()
    {
        MetricaSender.SendSurviveModeGoal(SurviveModeGoal.GetBonusCard);
        RewardedAddManager.Instance.OnAcceptOffer();
        ToggleRewardPanel(false);
    }

    void OnCancelOffer()
    {
        RewardedAddManager.Instance.OnCancelOffer();
        ToggleRewardPanel(false);
    }

    void ToggleRewardPanel(bool activeStatus)
    {
        //Cursor.visible = activeStatus;
        _ADSPanel.gameObject.SetActive(activeStatus);
        _upgradeCardsContainer.parent.gameObject.SetActive(activeStatus);

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
        RewardedAddManager.Instance.OnOfferTimeOut();
        ToggleRewardPanel(false);
    }
}
