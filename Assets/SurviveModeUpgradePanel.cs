using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurviveModeUpgradePanel : MonoBehaviour
{
    public static SurviveModeUpgradePanel Instance;

    [SerializeField] Transform _upgradeCardsContainer;
    [SerializeField] UIUpgradeCard UIUpgradeCardPF;
    [SerializeField] Button _availableLevelButton;


    bool _showUpgradeCardsAutomatic;
    int _collectedLvlUps = 0;

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
        if(_collectedLvlUps <= 0) return;
        _availableLevelButton.gameObject.SetActive(false);
        GameFlowManager.Instance.SetPause(this);
        ShowCards();
    }

    public void OnStartMode()
    {
        ResetPanel();
    }

    private void ResetPanel()
    {
        _collectedLvlUps = 0;
        _upgradeCardsContainer.parent.gameObject.SetActive(false);
        _availableLevelButton.gameObject.SetActive(false);
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
    void ShowCards()
    {
        List<UpgradeCardData> upgradeCardDatas = SurviveModeUpgradeService.Instance.GetCardsCollection();

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
            _collectedLvlUps--;
            TESTSurviveModStatistics.Instance.UpdateCardsPack(_collectedLvlUps);
            if (_collectedLvlUps > 0)
            {
                ShowCards();
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
