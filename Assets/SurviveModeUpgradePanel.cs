using System.Collections.Generic;
using UnityEngine;

public class SurviveModeUpgradePanel : MonoBehaviour
{
    public static SurviveModeUpgradePanel Instance;

    [SerializeField] Transform _upgradeCardsContainer;
    [SerializeField] UIUpgradeCard UIUpgradeCardPF;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        gameObject.SetActive(false);
    }


    public void ConfigPanel(List<UpgradeCardData> upgradeCardDatas)
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
        gameObject.SetActive(true);
        Cursor.visible = true;
        GameFlowManager.Instance.SetPause(this);
    }

    public void OnCardSelected()
    {
        Cursor.visible = false;
        GameFlowManager.Instance.Unpause(this);
        gameObject.SetActive(false);
    }
}
