using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesAfterLevel : MonoBehaviour
{
    public static UpgradesAfterLevel Instance;

    [SerializeField] Image _itemImage;
    [SerializeField] TextMeshProUGUI _itemNameText;
    [SerializeField] FastUpgradeRow _fastUpgradeRow;
    [SerializeField] Button _acceptOfferButton;
    [SerializeField] Button _cancelOfferButton;
    [SerializeField] Button _closeButton;
    [SerializeField] TextMeshProUGUI _panelName;
    [SerializeField] TextMeshProUGUI _maxLevelText;
    [SerializeField] TextMeshProUGUI _cancelOfferText;
    [SerializeField] TextMeshProUGUI _upgradeCostText;
    //[SerializeField] TextMeshProUGUI _offerText;

    bool _withUpdateInventory = false;
    IItemData _targetItemData = null;
    string _targetCharName = null;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _fastUpgradeRow.Init();
        _panelName.text = TextConstants.QUICKIMPROVEMENT;
        _maxLevelText.text = TextConstants.MAX;
        _cancelOfferText.text = TextConstants.REFUSEIMPROVEMENT;
        _upgradeCostText.text = TextConstants.UPGRADECOST;
        //_offerText.text = TextConstants._rewardText[RewardName.FreeUpgrade];
        _acceptOfferButton.onClick.AddListener(OnAcceptOffer);
        _cancelOfferButton.onClick.AddListener(OnCancelOffer);
        _closeButton.onClick.AddListener(OnCloseOffer);
        _fastUpgradeRow.UpgradeBtn.onClick.AddListener(OnBuyUpgrade);
        gameObject.SetActive(false);
    }



    public void ConfigPanel()
    {
        RewardedAdManager.Instance.PrepareReward(OnSelectRewardOption, RewardName.FreeUpgrade);
        gameObject.SetActive(true);
        Cursor.visible = true;
        GetLessUpgradedItemAndCharacteristic(out IItemData targetItemData, out string targetCharName);

        if (targetItemData == null || targetCharName == null)
        {
            OnNoUpgradesAvailable();
        }
        else
        {
            _targetItemData = targetItemData;
            _targetCharName = targetCharName;
            InventoryManager.Instance.SelectedItem = targetItemData;

            _itemNameText.text = _targetItemData.TranslatedItemName;
            if (_targetItemData is WeaponData) _itemImage.sprite = GameAssets.Instance.GameItems.ItemsSpritesAtlas.GetSprite(_targetItemData.DeffItemName);
            else if (_targetItemData is VehicleData) _itemImage.sprite = GameAssets.Instance.GameItems.ItemsSpritesAtlas.GetSprite(_targetItemData.DeffItemName);

            SetUpUbgradeRow(targetItemData, targetCharName);
        }
    }

    public void UpdatePanel()
    {
        if (CheckMaxLevelReached(_targetItemData, _targetCharName))
        {
            OnNoUpgradesAvailable();
        }
        else
        {
            SetUpUbgradeRow(_targetItemData, _targetCharName);
        }
    }


    void GetLessUpgradedItemAndCharacteristic(out IItemData targetItemData, out string targetCharName)
    {
        int maxAvailableUpgradesCount = 0;
        string tempTargetCharName = null;
        IItemData tempTargetItemData = null;

        Dictionary<int, string> equipedItems = PlayerData.Instance.EquipedItems;

        if (equipedItems.ContainsKey(1))
        {
            IItemData itemData = PlayerData.Instance.GetItemDataByName(equipedItems[1]);
            GetLessUpgradedWeaponCharacteristic(itemData as WeaponData, out string charName, out int availableUpgradesCount);

            if (availableUpgradesCount > maxAvailableUpgradesCount)
            {
                tempTargetItemData = itemData;
                tempTargetCharName = charName;
                maxAvailableUpgradesCount = availableUpgradesCount;
            }
        }

        if (equipedItems.ContainsKey(2))
        {
            IItemData itemData = PlayerData.Instance.GetItemDataByName(equipedItems[2]);
            GetLessUpgradedWeaponCharacteristic(itemData as WeaponData, out string charName, out int availableUpgradesCount);

            if (availableUpgradesCount > maxAvailableUpgradesCount)
            {
                tempTargetItemData = itemData;
                tempTargetCharName = charName;
                maxAvailableUpgradesCount = availableUpgradesCount;
            }
        }

        if (equipedItems.ContainsKey(3))
        {
            IItemData itemData = PlayerData.Instance.GetItemDataByName(equipedItems[3]);
            GetLessUpgradedWeaponCharacteristic(itemData as WeaponData, out string charName, out int availableUpgradesCount);

            if (availableUpgradesCount > maxAvailableUpgradesCount)
            {
                tempTargetItemData = itemData;
                tempTargetCharName = charName;
                maxAvailableUpgradesCount = availableUpgradesCount;
            }
        }

        if (equipedItems.ContainsKey(0))
        {
            IItemData itemData = PlayerData.Instance.GetItemDataByName(equipedItems[0]);
            GetLessUpgradedVehicleCharacteristic(itemData as VehicleData, out string charName, out int availableUpgradesCount);

            if (availableUpgradesCount > maxAvailableUpgradesCount)
            {
                tempTargetItemData = itemData;
                tempTargetCharName = charName;
                maxAvailableUpgradesCount = availableUpgradesCount;
            }
        }
        targetItemData = tempTargetItemData;
        targetCharName = tempTargetCharName;
    }


    void GetLessUpgradedWeaponCharacteristic(WeaponData weaponData, out string charName, out int availableUpgradesCount)
    {
        int maxAvailableUpgradesCount = 0;
        string targetCharName = null;

        int hullDmgAvailableLvls = weaponData.hullDmgMaxLvl - weaponData.hullDmgCurLvl;
        int shieldDmgAvailableLvls = weaponData.shieldDmgMaxLvl - weaponData.shieldDmgCurLvl;
        int RSAvailableLvls = weaponData.rotationSpeedMaxLvl - weaponData.rotationSpeedCurLvl;
        int FRAvailableLvls = weaponData.fireRateMaxLvl - weaponData.fireRateCurtLvl;

        if (hullDmgAvailableLvls > maxAvailableUpgradesCount)
        {
            targetCharName = TextConstants.HULLDMG;
            maxAvailableUpgradesCount = hullDmgAvailableLvls;
        }
        if (shieldDmgAvailableLvls > maxAvailableUpgradesCount)
        {
            targetCharName = TextConstants.SHIELDDMG;
            maxAvailableUpgradesCount = shieldDmgAvailableLvls;
        }
        if (RSAvailableLvls > maxAvailableUpgradesCount)
        {
            targetCharName = TextConstants.ROTATIONSPEED;
            maxAvailableUpgradesCount = RSAvailableLvls;
        }
        if (FRAvailableLvls > maxAvailableUpgradesCount)
        {
            targetCharName = TextConstants.FIRERATE;
            maxAvailableUpgradesCount = FRAvailableLvls;
        }
        charName = targetCharName;
        availableUpgradesCount = maxAvailableUpgradesCount;
    }

    void GetLessUpgradedVehicleCharacteristic(VehicleData vehicleData, out string charName, out int availableUpgradesCount)
    {
        int maxAvailableUpgradesCount = 0;
        string targetCharName = null;

        int hullHpAvailableLvls = vehicleData.hullHPMaxLvl - vehicleData.hullHPCurLvl;
        int shieldHpAvailableLvls = vehicleData.shieldHPMaxLvl - vehicleData.shieldHPCurLvl;
        int shieldRegRateAvailableLvls = vehicleData.shieldRegenMaxLvl - vehicleData.shieldRegenCurtLvl;

        if (hullHpAvailableLvls > maxAvailableUpgradesCount)
        {
            targetCharName = TextConstants.HULLHP;
            maxAvailableUpgradesCount = hullHpAvailableLvls;
        }
        if (shieldHpAvailableLvls > maxAvailableUpgradesCount)
        {
            targetCharName = TextConstants.SHIELDHP;
            maxAvailableUpgradesCount = shieldHpAvailableLvls;
        }
        if (shieldRegRateAvailableLvls > maxAvailableUpgradesCount)
        {
            targetCharName = TextConstants.SHIELREGENRATE;
            maxAvailableUpgradesCount = shieldRegRateAvailableLvls;
        }
        charName = targetCharName;
        availableUpgradesCount = maxAvailableUpgradesCount;
    }

    void SetUpUbgradeRow(IItemData itemData, string charName)
    {
        if (itemData is WeaponData WData)
        {
            UpgradeCost upgradeCost = GameAssets.Instance.WeaponUpgradeCosts.Costs.Find(cost => cost.RaritieType == WData.weaponRaritie);

            if (charName.Equals(TextConstants.HULLDMG))
            {
                LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == WData.hullDmgCurLvl + 1);
                _fastUpgradeRow.SetData(TextConstants.HULLDMG, WData.hullDmgCurLvl, WData.hullDmgMaxLvl, WData.hullDmgByLvl, lvlCost.ResCost, _withUpdateInventory);
            }
            else if (charName.Equals(TextConstants.SHIELDDMG))
            {
                LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == WData.shieldDmgCurLvl + 1);
                _fastUpgradeRow.SetData(TextConstants.SHIELDDMG, WData.shieldDmgCurLvl, WData.shieldDmgMaxLvl, WData.shieldDmgByLvl, lvlCost.ResCost, _withUpdateInventory);
            }
            else if (charName.Equals(TextConstants.ROTATIONSPEED))
            {
                LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == WData.rotationSpeedCurLvl + 1);
                _fastUpgradeRow.SetData(TextConstants.ROTATIONSPEED, WData.rotationSpeedCurLvl, WData.rotationSpeedMaxLvl, WData.rotationSpeedByLvl, lvlCost.ResCost, _withUpdateInventory);
            }
            else if (charName.Equals(TextConstants.FIRERATE))
            {
                LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == WData.fireRateCurtLvl + 1);
                _fastUpgradeRow.SetData(TextConstants.FIRERATE, WData.fireRateCurtLvl, WData.fireRateMaxLvl, WData.fireRateByLvl, lvlCost.ResCost, _withUpdateInventory);
            }
        }
        else if (itemData is VehicleData VData)
        {
            UpgradeCost upgradeCost = GameAssets.Instance.VehicleUpgradeCosts.Costs.Find(cost => cost.RaritieType == VData.vehicleRaritie);

            if (charName.Equals(TextConstants.HULLHP))
            {
                LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == VData.hullHPCurLvl + 1);
                _fastUpgradeRow.SetData(TextConstants.HULLHP, VData.hullHPCurLvl, VData.hullHPMaxLvl, VData.hullHPByLvl, lvlCost.ResCost, _withUpdateInventory);
            }
            else if (charName.Equals(TextConstants.SHIELDHP))
            {
                LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == VData.shieldHPCurLvl + 1);
                _fastUpgradeRow.SetData(TextConstants.SHIELDHP, VData.shieldHPCurLvl, VData.shieldHPMaxLvl, VData.shieldHPByLvl, lvlCost.ResCost, _withUpdateInventory);
            }
            else if (charName.Equals(TextConstants.SHIELREGENRATE))
            {
                LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == VData.shieldRegenCurtLvl + 1);
                _fastUpgradeRow.SetData(TextConstants.SHIELREGENRATE, VData.shieldRegenCurtLvl, VData.shieldRegenMaxLvl, VData.shieldRegenRateByLvl, lvlCost.ResCost, _withUpdateInventory);
            }
        }
    }


    bool CheckMaxLevelReached(IItemData itemData, string charName)
    {
        if (itemData is WeaponData WData)
        {
            if (charName.Equals(TextConstants.HULLDMG)) return WData.hullDmgMaxLvl == WData.hullDmgCurLvl;
            else if (charName.Equals(TextConstants.SHIELDDMG)) return WData.shieldDmgMaxLvl == WData.shieldDmgCurLvl;
            else if (charName.Equals(TextConstants.ROTATIONSPEED)) return WData.rotationSpeedMaxLvl == WData.rotationSpeedCurLvl;
            else if (charName.Equals(TextConstants.FIRERATE)) return WData.fireRateMaxLvl == WData.fireRateCurtLvl;
            else return true;
        }
        else if (itemData is VehicleData VData)
        {
            if (charName.Equals(TextConstants.HULLHP)) return VData.hullHPMaxLvl == VData.hullHPCurLvl;
            else if (charName.Equals(TextConstants.SHIELDHP)) return VData.shieldHPMaxLvl == VData.shieldHPCurLvl;
            else if (charName.Equals(TextConstants.SHIELREGENRATE)) return VData.shieldRegenMaxLvl == VData.shieldRegenCurtLvl;
            else return true;
        }
        else return true;
    }


    void OnNoUpgradesAvailable()
    {
        RewardedAdManager.Instance.OnNotAvailable();
        OnClosePanel();
    }

    void OnBuyUpgrade()
    {
        MetricaSender.QuickImprovement(InRaidManager.Instance.SelectedLeveParameters.LevelName);
        UpdatePanel();
    }

    void OnClosePanel()
    {
        Cursor.visible = false;
        gameObject.SetActive(false);
        _targetItemData = null;
        _targetCharName = null;
        UILevelStatistic.Instance.ShowStatistic();
    }


    private void OnCancelOffer()
    {
        RewardedAdManager.Instance.OnCancelOffer();
        OnClosePanel();
    }
    private void OnCloseOffer()
    {
        RewardedAdManager.Instance.OnPlayerCloseOffer();
        OnClosePanel();
    }

    private void OnAcceptOffer()
    {
        AudioManager.Instance.PlayInventorySound(UISound.UpgradeSound);
        _fastUpgradeRow.MaxLevelImitation();        
        RewardedAdManager.Instance.OnAcceptOffer();
    }

    void OnSelectRewardOption(bool GetRewardStatus)
    {
        if (GetRewardStatus)
        {
            UpgradeCharacteristicToMax();
        }
        else
        {

        }
        OnClosePanel();
    }

    void UpgradeCharacteristicToMax()
    {
        if (_targetItemData is WeaponData WData)
        {
            if (_targetCharName.Equals(TextConstants.HULLDMG)) WData.hullDmgCurLvl = WData.hullDmgMaxLvl;
            else if (_targetCharName.Equals(TextConstants.SHIELDDMG)) WData.shieldDmgCurLvl = WData.shieldDmgMaxLvl;
            else if (_targetCharName.Equals(TextConstants.ROTATIONSPEED)) WData.rotationSpeedCurLvl = WData.rotationSpeedMaxLvl;
            else if (_targetCharName.Equals(TextConstants.FIRERATE)) WData.fireRateCurtLvl = WData.fireRateMaxLvl;
        }
        else if (_targetItemData is VehicleData VData)
        {
            if (_targetCharName.Equals(TextConstants.HULLHP)) VData.hullHPCurLvl = VData.hullHPMaxLvl;
            else if (_targetCharName.Equals(TextConstants.SHIELDHP)) VData.shieldHPCurLvl = VData.shieldHPMaxLvl;
            else if (_targetCharName.Equals(TextConstants.SHIELREGENRATE)) VData.shieldRegenCurtLvl = VData.shieldRegenMaxLvl;
        }
        SaveLoadManager.Instance.SaveData();
    }

    

}


//Логика где рандомно выбиралась характеристика

/*
 *  public void ConfigPanel()
    {
        gameObject.SetActive(true);
        // без перебора т.к. нужен определенный порядок, но можно подумать позже )
        string targetCharName;
        IItemData targetItemData;

        Dictionary<int, string> equipedItems = PlayerData.Instance.EquipedItems;

        if (equipedItems.ContainsKey(1))
        {
            targetItemData = PlayerData.Instance.GetItemDataByName(equipedItems[1]);
            targetCharName = GetNotFullChar(targetItemData);
            if (targetItemData != null && targetCharName != null)
            {
                SetUpUbgradeRow(targetItemData, targetCharName);
                return;
            }
        }

        if (equipedItems.ContainsKey(2))
        {
            targetItemData = PlayerData.Instance.GetItemDataByName(equipedItems[2]);
            targetCharName = GetNotFullChar(targetItemData);
            if (targetItemData != null && targetCharName != null)
            {
                SetUpUbgradeRow(targetItemData, targetCharName);
                return;
            }
        }

        if (equipedItems.ContainsKey(3))
        {
            targetItemData = PlayerData.Instance.GetItemDataByName(equipedItems[3]);
            targetCharName = GetNotFullChar(targetItemData);
            if (targetItemData != null && targetCharName != null)
            {
                SetUpUbgradeRow(targetItemData, targetCharName);
                return;
            }
        }

        if (equipedItems.ContainsKey(0))
        {
            targetItemData = PlayerData.Instance.GetItemDataByName(equipedItems[0]);
            targetCharName = GetNotFullChar(targetItemData);
            if (targetItemData != null && targetCharName != null)
            {
                SetUpUbgradeRow(targetItemData, targetCharName);
                return;
            }
        }

        OnNoUpgradesAvailable();
    }


    string GetNotFullChar(IItemData itemData)
    {
        if (itemData is WeaponData WData) return GetRandomWeaponCharAvailableForUpgrade(WData);
        else if (itemData is VehicleData VData) return GetRandomVehicleCharAvailableForUpgrade(VData);
        else return null;
    }

    string GetRandomWeaponCharAvailableForUpgrade(WeaponData weaponData)
    {
        List<string> chars = new();
        if (weaponData.hullDmgCurLvl < weaponData.hullDmgMaxLvl) chars.Add(TextConstants.HULLDMG);
        if (weaponData.shieldDmgCurLvl < weaponData.shieldDmgMaxLvl) chars.Add(TextConstants.SHIELDDMG);
        if (weaponData.rotationSpeedCurLvl < weaponData.rotationSpeedMaxLvl) chars.Add(TextConstants.ROTATIONSPEED);
        if (weaponData.fireRateCurtLvl < weaponData.fireRateMaxLvl) chars.Add(TextConstants.FIRERATE);

        if (chars.Count != 0) return chars[Random.Range(0, chars.Count)];
        else return null;
    }

    string GetRandomVehicleCharAvailableForUpgrade(VehicleData vehicleData)
    {
        List<string> chars = new();
        if (vehicleData.hullHPCurLvl < vehicleData.hullHPMaxLvl) chars.Add(TextConstants.HULLHP);
        if (vehicleData.shieldHPCurLvl < vehicleData.shieldHPMaxLvl) chars.Add(TextConstants.SHIELDHP);
        if (vehicleData.shieldRegenCurtLvl < vehicleData.shieldRegenMaxLvl) chars.Add(TextConstants.SHIELREGENRATE);

        //if (vehicleData.curWeaponsCount < vehicleData.maxWeaponsCount) chars.Add(TextConstants.WEAPONSCOUNT);

        if (chars.Count != 0) return chars[Random.Range(0, chars.Count)];
        else return null;
    }

    void SetUpUbgradeRow(IItemData itemData, string charName)
    {
        InventoryManager.Instance.SelectedItem = itemData;

        if (itemData is WeaponData WData)
        {
            UpgradeCost upgradeCost = GameAssets.Instance.WeaponUpgradeCosts.Costs.Find(cost => cost.RaritieType == WData.weaponRaritie);

            if (charName.Equals(TextConstants.HULLDMG))
            {
                LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == WData.hullDmgCurLvl + 1);
                _upgradeRow.SetData(TextConstants.HULLDMG, WData.hullDmgCurLvl, WData.hullDmgMaxLvl, WData.hullDmgByLvl, lvlCost.ResCost, _withUpdateInventory);
            }
            else if (charName.Equals(TextConstants.SHIELDDMG))
            {
                LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == WData.shieldDmgCurLvl + 1);
                _upgradeRow.SetData(TextConstants.SHIELDDMG, WData.shieldDmgCurLvl, WData.shieldDmgMaxLvl, WData.shieldDmgByLvl, lvlCost.ResCost, _withUpdateInventory);
            }
            else if (charName.Equals(TextConstants.ROTATIONSPEED))
            {
                LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == WData.rotationSpeedCurLvl + 1);
                _upgradeRow.SetData(TextConstants.ROTATIONSPEED, WData.rotationSpeedCurLvl, WData.rotationSpeedMaxLvl, WData.rotationSpeedByLvl, lvlCost.ResCost, _withUpdateInventory);
            }
            else if (charName.Equals(TextConstants.FIRERATE))
            {
                LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == WData.fireRateCurtLvl + 1);
                _upgradeRow.SetData(TextConstants.FIRERATE, WData.fireRateCurtLvl, WData.fireRateMaxLvl, WData.fireRateByLvl, lvlCost.ResCost, _withUpdateInventory);
            }
        }
        else if (itemData is VehicleData VData)
        {
            UpgradeCost upgradeCost = GameAssets.Instance.VehicleUpgradeCosts.Costs.Find(cost => cost.RaritieType == VData.vehicleRaritie);
            //UpgradeCost slotCost = GameAssets.Instance.VehicleSlotCosts.Costs.Find(cost => cost.RaritieType == data.vehicleRaritie);

            if (charName.Equals(TextConstants.HULLHP))
            {
                LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == VData.hullHPCurLvl + 1);
                _upgradeRow.SetData(TextConstants.HULLHP, VData.hullHPCurLvl, VData.hullHPMaxLvl, VData.hullHPByLvl, lvlCost.ResCost, _withUpdateInventory);
            }
            else if (charName.Equals(TextConstants.SHIELDHP))
            {
                LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == VData.shieldHPCurLvl + 1);
                _upgradeRow.SetData(TextConstants.SHIELDHP, VData.shieldHPCurLvl, VData.shieldHPMaxLvl, VData.shieldHPByLvl, lvlCost.ResCost, _withUpdateInventory);
            }
            else if (charName.Equals(TextConstants.SHIELREGENRATE))
            {
                LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == VData.shieldRegenCurtLvl + 1);
                _upgradeRow.SetData(TextConstants.SHIELREGENRATE, VData.shieldRegenCurtLvl, VData.shieldRegenMaxLvl, VData.shieldRegenRateByLvl, lvlCost.ResCost, _withUpdateInventory);
            }

            //if (data.curWeaponsCount < data.maxWeaponsCount)
            //{
            //    LvlCost lvlCost = slotCost.LvlCosts.Find(cost => cost.lvlNumber == data.curWeaponsCount + 1);
            //    upgradeRows[3].SetData(TextConstants.WEAPONSCOUNT, data.curWeaponsCount, data.maxWeaponsCount, 1, lvlCost.ResCost);
            //}
            //else upgradeRows[3].OnMaxLvlReached(TextConstants.WEAPONSCOUNT, data.curWeaponsCount, data.maxWeaponsCount);
        }
    }


    void OnNoUpgradesAvailable()
    {
        gameObject.SetActive(false);
    }

    void OnBuyUpgrade()
    {
        ConfigPanel();
    }




    private void OnCancelOffer()
    {
        //throw new NotImplementedException();
    }

    private void OnAcceptOffer()
    {
        //throw new NotImplementedException();
    }
*/
