using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FastUpgradeRow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _charName;
    [SerializeField] List<Image> _lvlIcons;
    [SerializeField] List<UpgradeCostRow> _costRows;
    [SerializeField] TextMeshProUGUI _upEffectValueText;
    [SerializeField] Button _upgradeBtn;
    //[SerializeField] TextMeshProUGUI _maxLvlText;

    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] TextMeshProUGUI _upEffectText;
    //[SerializeField] TextMeshProUGUI _costText;
    [SerializeField] TextMeshProUGUI _upgradeText;

    List<ResCost> _upgradeCost;
    Color _notAvailableColir = new(0.128f, 0.128f, 0.128f, 1);

    bool _withUpdateInventory;
    int _maxLevel;


    public Button UpgradeBtn => _upgradeBtn;


    public void Init()
    {
        _levelText.text = TextConstants.LEVEL;
        _upEffectText.text = TextConstants.UPEFFECT;
        //_costText.text = TextConstants.COST;
        _upgradeText.text = TextConstants.UPGRADE;
        //_maxLvlText.text = TextConstants.MAXLEVEL;
        _upgradeBtn.onClick.AddListener(OnBuyUpgrade);
        gameObject.SetActive(false);
    }

    public void SetData(string charName, int currentLvl, int maxLvl, float upgradeEffect, List<ResCost> resCosts, bool updateInventory = true)
    {
        gameObject.SetActive(true);
        _upgradeBtn.gameObject.SetActive(true);
        _charName.text = charName;
        _upgradeCost = resCosts;
        _upEffectValueText.text = $"+ {upgradeEffect}";
        //_maxLvlText.gameObject.SetActive(false);
        _withUpdateInventory = updateInventory;

        _maxLevel = maxLvl;
        ShowLvl(currentLvl, maxLvl);
        ShowCost(resCosts);

    }

    //public void OnMaxLvlReached(string charName, int currentLvl, int maxLvl)
    //{
    //    gameObject.SetActive(true);
    //    _upgradeBtn.gameObject.SetActive(false);
    //    _upEffectValueText.text = string.Empty;
    //    //_maxLvlText.gameObject.SetActive(true);
    //    _charName.text = charName;
    //    foreach (var row in _costRows)
    //    {
    //        row.gameObject.SetActive(false);
    //    }
    //    ShowLvl(currentLvl, maxLvl);
    //}

    void ShowLvl(int currentLvl, int maxLvl)
    {
        for (int i = 1; i <= _lvlIcons.Count; i++)
        {
            if (i <= currentLvl) _lvlIcons[i - 1].color = Color.green;
            else if (i > currentLvl && i <= maxLvl) _lvlIcons[i - 1].color = Color.grey;
            else if (i > maxLvl) _lvlIcons[i - 1].color = _notAvailableColir;
        }
    }

    void ShowCost(List<ResCost> resCosts)
    {
        for (var i = 0; i < _costRows.Count; i++)
        {
            if (i <= resCosts.Count - 1)
            {
                ResSprite resData = GameAssets.Instance.ResSprites.Find(sprite => sprite.ResourcesType == resCosts[i].ResourcesType);
                _costRows[i].SetData(resData.Sprite, resCosts[i].Amount, resData.TextColor);
            }
            else
            {
                _costRows[i].Hide();
            }
        }
    }

    void OnBuyUpgrade()
    {
        InventoryManager.Instance.OnBuyUpgrade(_charName.text, _upgradeCost, _withUpdateInventory);
    }

    public void MaxLevelImitation()
    {
        ShowLvl(_maxLevel, _maxLevel);
    }

    //public void DisableBuyUpgradeOption()
    //{
    //    _upgradeBtn.gameObject.SetActive(false);
    //    _maxLvlText.gameObject.SetActive(false);
    //}
}
