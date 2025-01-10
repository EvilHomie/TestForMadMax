using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SurviveModeUpgradePanel : MonoBehaviour
{
    public static SurviveModeUpgradePanel Instance;

    [SerializeField] Transform _upgradeCardsContainer;
    [SerializeField] UIUpgradeCard UIUpgradeCardPF;
    [SerializeField] Button _availableLevelButton;

    bool _showUpgradeCardsAutomatic;
    List<List<UpgradeCardData>> _collectedCardsPacks = new();

    //int _collectedPacksLeftAmount;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init(bool showUpgradeCardsAutomatic)
    {
        _showUpgradeCardsAutomatic = showUpgradeCardsAutomatic;
        _availableLevelButton.onClick.AddListener(OpenLevelUpPanel);
        ResetPanel();
    }

    public void OpenLevelUpPanel()
    {
        _availableLevelButton.gameObject.SetActive(false);
        //_collectedPacksLeftAmount = _collectedCardsPacks.Count;
        ConfigPanel(_collectedCardsPacks.First());
        GameFlowManager.Instance.SetPause(this);

    }

    public void OnStartMode()
    {
        ResetPanel();
    }

    private void ResetPanel()
    {
        _upgradeCardsContainer.parent.gameObject.SetActive(false);
        _availableLevelButton.gameObject.SetActive(false);
        _collectedCardsPacks.Clear();
        TESTSurviveModStatistics.Instance.UpdateCardsPack(_collectedCardsPacks.Count);
    }


    public void AddCardsPack(List<UpgradeCardData> cardsPack)
    {
        if (_showUpgradeCardsAutomatic)
        {
            ConfigPanel(cardsPack);
            GameFlowManager.Instance.SetPause(this);
        }
        else
        {
            _collectedCardsPacks.Add(cardsPack);
            _availableLevelButton.gameObject.SetActive(true);
        }

        TESTSurviveModStatistics.Instance.UpdateCardsPack(_collectedCardsPacks.Count);
    }


    void ConfigPanel(List<UpgradeCardData> upgradeCardDatas)
    {
        foreach (Transform card in _upgradeCardsContainer)
        {
            Destroy(card.gameObject);
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
        }
        else
        {
            //_collectedPacksLeftAmount--;
            _collectedCardsPacks.RemoveAt(0);
            TESTSurviveModStatistics.Instance.UpdateCardsPack(_collectedCardsPacks.Count);
            if (_collectedCardsPacks.Count > 0)
            {
                ConfigPanel(_collectedCardsPacks.First());
            }
            else
            {
                Cursor.visible = false;
                GameFlowManager.Instance.Unpause(this);
                _upgradeCardsContainer.parent.gameObject.SetActive(false);
            }
        }

    }
}
