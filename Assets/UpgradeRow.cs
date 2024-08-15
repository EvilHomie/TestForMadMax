using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeRow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _charName;
    [SerializeField] List<Image> _lvlIcons;
    [SerializeField] List<UpgradeCostRow> _costRows;
    [SerializeField] TextMeshProUGUI _upEffectText;
    [SerializeField] Button _upgradeBtn;
    [SerializeField] TextMeshProUGUI _maxLvlText;

    List<ResCost> _upgradeCost;


    private void Awake()
    {
        _upgradeBtn.onClick.AddListener(OnBuyUpgrade);
    }

    public void SetData(string charName, int currentLvl, int maxLvl, float upgradeEffect, List<ResCost> resCosts)
    {
        gameObject.SetActive(true);
        _upgradeBtn.gameObject.SetActive(true);
        _charName.text = charName;
        _upgradeCost = resCosts;
        _upEffectText.text = $"+ {upgradeEffect}";
        _maxLvlText.gameObject.SetActive(false);

        ShowLvl(currentLvl, maxLvl);
        ShowCost(resCosts);

    }

    public void OnMaxLvlReached(string charName, int currentLvl, int maxLvl)
    {
        gameObject.SetActive(true);
        _upgradeBtn.gameObject.SetActive(false);
        _upEffectText.text = string.Empty;
        _maxLvlText.gameObject.SetActive(true);
        _charName.text = charName;
        foreach (var row in _costRows)
        {
            row.gameObject.SetActive(false);
        }
        ShowLvl(currentLvl, maxLvl);
    }

    void ShowLvl(int currentLvl, int maxLvl)
    {
        for (int i = 1; i <= _lvlIcons.Count; i++)
        {
            if (i <= currentLvl) _lvlIcons[i - 1].color = Color.green;
            else if (i > currentLvl && i <= maxLvl) _lvlIcons[i - 1].color = Color.grey;
            else if (i > maxLvl) _lvlIcons[i - 1].color = Color.red;
        }
    }

    void ShowCost(List<ResCost> resCosts)
    {
        for (var i = 0; i < _costRows.Count; i++)
        {
            if (i <= resCosts.Count - 1)
            {
                Sprite sprite = GameConfig.Instance.ResSprites.Find(sprite => sprite.ResourcesType == resCosts[i].ResourcesType).Sprite;
                _costRows[i].SetData(sprite, resCosts[i].Amount);
            }
            else
            {
                _costRows[i].Hide();
            }
        }
    }

    void OnBuyUpgrade()
    {
        InventoryManager.Instance.OnBuyUpdate(_charName.text);
    }


}
