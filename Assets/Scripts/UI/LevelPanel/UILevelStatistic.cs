using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelStatistic : MonoBehaviour
{
    public static UILevelStatistic Instance;

    [SerializeField] TextMeshProUGUI _levelSttisticText;
    [SerializeField] TextMeshProUGUI _resourcesCollectedText;
    [SerializeField] TextMeshProUGUI _scrapMetalCollectedSummText;
    [SerializeField] TextMeshProUGUI _whiresCollectedSummText;
    [SerializeField] TextMeshProUGUI _cooperCollectedSummText;
    [SerializeField] TextMeshProUGUI _damageReceivedText;
    [SerializeField] TextMeshProUGUI _damageDoneText;
    [SerializeField] List<TextMeshProUGUI> _toHullText;
    [SerializeField] List<TextMeshProUGUI> _toShieldText;
    [SerializeField] TextMeshProUGUI _toHullDMGRecivedSummText;
    [SerializeField] TextMeshProUGUI _toShieldDMGRecivedSummText;
    [SerializeField] TextMeshProUGUI _toHullDMGDoneSummText;
    [SerializeField] TextMeshProUGUI _toShieldDMGDoneSummText;
    [SerializeField] TextMeshProUGUI _elementDestroyedText;
    [SerializeField] TextMeshProUGUI _wheelsCaterpillarsText;
    [SerializeField] TextMeshProUGUI _wheelsCaterpillarsDestroyedCountText;
    [SerializeField] TextMeshProUGUI _bodiesText;
    [SerializeField] TextMeshProUGUI _bodiesDestroyedCountText;
    [SerializeField] TextMeshProUGUI _othersText;
    [SerializeField] TextMeshProUGUI _othersDestroyedCountText;
    [SerializeField] TextMeshProUGUI _explosivePartsText;
    [SerializeField] TextMeshProUGUI _explosivePartsDestroyedCountText;
    [SerializeField] TextMeshProUGUI _tipText;
    //[SerializeField] TextMeshProUGUI _previousText;
    //[SerializeField] TextMeshProUGUI _nextTipText;
    [SerializeField] Button _nextTipBtn;
    [SerializeField] Button _prevTipBtn;
    [SerializeField] Button _raidBtn;
    [SerializeField] Button _confirmBtn;
    [SerializeField] TextMeshProUGUI _raidBtnText;
    [SerializeField] Button _inventoryBtn;
    [SerializeField] TextMeshProUGUI _inventoryBtnText;
    [SerializeField] TextMeshProUGUI _tipBodyText;

    int _lastTipIndex;
    int _scrapMetalCollectedSumm;
    int _whiresCollectedSumm;
    int _cooperCollectedSumm;
    float _hullDMGRecivedSumm;
    float _shieldDMGRecivedSumm;
    float _hullDMGDoneSumm;
    float _shieldDMGDoneSumm;
    int _wheelsCaterpillarsDestroyedSumm;
    int _bodiesDestroyedSumm;
    int _othersDestroyedSumm;
    int _explosivePartsDestroyedSumm;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _levelSttisticText.text = TextConstants.LEVELSTATISTIC;

        _resourcesCollectedText.text = TextConstants.RESOURCESCOLLECTED;
        _damageReceivedText.text = TextConstants.DAMAGERECEIVED;
        _damageDoneText.text = TextConstants.DAMAGEDONE;
        _toHullText.ForEach(Text => Text.text = TextConstants.TOHULL);
        _toShieldText.ForEach(Text => Text.text = TextConstants.TOSHIELD);
        _elementDestroyedText.text = TextConstants.ELEMENTSDESTROYED;
        _wheelsCaterpillarsText.text = $"{TextConstants.WHEELS}/{TextConstants.CATERPILLARS}";
        _bodiesText.text = TextConstants.BODIES;
        _othersText.text = TextConstants.OTHERS;
        _explosivePartsText.text = TextConstants.EXPLOSIVEPARTS;
        _tipText.text = TextConstants.TIP;
        _raidBtnText.text = TextConstants.RAID;
        _inventoryBtnText.text = TextConstants.INVENTORY;
        //_previousText.text = TextConstants.PREVIOUS;
        //_nextTipText.text = TextConstants.NEXTTIP;

        _lastTipIndex = -1;
        ResetCounters();
        gameObject.SetActive(false);

        _nextTipBtn.onClick.AddListener(ShowNewTip);
        _prevTipBtn.onClick.AddListener(ShowPreviousTip);
        //_raidBtn.onClick.AddListener(StartNewRaid);
        //_inventoryBtn.onClick.AddListener(ShowInventory);
        _confirmBtn.onClick.AddListener(CloseStatistic);
    }

    public void OnPlayerStartRaid()
    {
        ResetCounters();        
    }

    void ResetCounters()
    {
        _scrapMetalCollectedSumm = 0;
        _whiresCollectedSumm = 0;
        _cooperCollectedSumm = 0;
        _hullDMGRecivedSumm = 0;
        _shieldDMGRecivedSumm = 0;
        _hullDMGDoneSumm = 0;
        _shieldDMGDoneSumm = 0;
        _wheelsCaterpillarsDestroyedSumm = 0;
        _bodiesDestroyedSumm = 0;
        _othersDestroyedSumm = 0;
        _explosivePartsDestroyedSumm = 0;
    }

    void UpdateStatistic()
    {
        _scrapMetalCollectedSummText.text = $"{_scrapMetalCollectedSumm}";
        _whiresCollectedSummText.text = $"{_whiresCollectedSumm}";
        _cooperCollectedSummText.text = $"{_cooperCollectedSumm}";
        _toHullDMGRecivedSummText.text = $"{_hullDMGRecivedSumm}";
        _toShieldDMGRecivedSummText.text = $"{_shieldDMGRecivedSumm}";
        _toHullDMGDoneSummText.text = $"{_hullDMGDoneSumm}";
        _toShieldDMGDoneSummText.text = $"{_shieldDMGDoneSumm}";
        _wheelsCaterpillarsDestroyedCountText.text = $"{_wheelsCaterpillarsDestroyedSumm}";
        _bodiesDestroyedCountText.text = $"{_bodiesDestroyedSumm}";
        _othersDestroyedCountText.text = $"{_othersDestroyedSumm}";
        _explosivePartsDestroyedCountText.text = $"{_explosivePartsDestroyedSumm}";
    }

    public void OnCollectResources(int scrapMAmount, int whiresAmount, int cooperAmount)
    {
        _scrapMetalCollectedSumm += scrapMAmount;
        _whiresCollectedSumm += whiresAmount;
        _cooperCollectedSumm += cooperAmount;
    }
    public void OnDamageRecieved(float dmgToHull, float dmgToSHield)
    {
        _hullDMGRecivedSumm += dmgToHull;
        _shieldDMGRecivedSumm += dmgToSHield;
    }

    public void OnDamageDone(float dmgToHull, float dmgToSHield)
    {
        _hullDMGDoneSumm += dmgToHull;
        _shieldDMGDoneSumm += dmgToSHield;
    }

    public void OnPartDestroyed(EnumVehiclePart part)
    {
        if (part == EnumVehiclePart.Wheel)
        {
            _wheelsCaterpillarsDestroyedSumm++;
        }
        else if (part == EnumVehiclePart.Body)
        {
            _bodiesDestroyedSumm++;
        }
        else if (part == EnumVehiclePart.OtherPart)
        {
            _othersDestroyedSumm++;
        }
        else if (part == EnumVehiclePart.ExplosivePart)
        {
            _explosivePartsDestroyedSumm++;
        }
    }

    public void ShowStatistic()
    {
        UpdateStatistic();
        ShowNewTip();
        gameObject.SetActive(true);
        Cursor.visible = true;
    }

    void ShowNewTip()
    {
        _lastTipIndex++;
        if (_lastTipIndex >= TextConstants.TIPSCOLLECTION.Length - 1)
        {
            _lastTipIndex = 0;
        }
        _tipBodyText.text = TextConstants.TIPSCOLLECTION[_lastTipIndex];
    }

    void ShowPreviousTip()
    {
        _lastTipIndex--;
        if (_lastTipIndex <= -1)
        {
            _lastTipIndex = TextConstants.TIPSCOLLECTION.Length - 1;
        }
        _tipBodyText.text = TextConstants.TIPSCOLLECTION[_lastTipIndex];
    }

    void ShowInventory()
    {
        gameObject.SetActive(false);
        FinishLevelManager.Instance.OnCloseLevelStatisicAndOpenInventory();
    }
    void StartNewRaid()
    {
        gameObject.SetActive(false);
        FinishLevelManager.Instance.OnCloseLevelStatisicAndStartNewRaid();
    }

    void CloseStatistic()
    {
        gameObject.SetActive(false);
        FinishLevelManager.Instance.OnCloseLevelStatisic();
    }

    void ConfigPanelForSurviveMod()
    {

    }

    void ConfigPanelForNormalMod()
    {

    }
}
