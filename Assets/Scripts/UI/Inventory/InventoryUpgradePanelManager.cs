using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUpgradePanelManager : MonoBehaviour
{
    public static InventoryUpgradePanelManager Instance;
    [SerializeField] List<UpgradeRow> upgradeRows;
    [SerializeField] TextMeshProUGUI _upgradesText;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _upgradesText.text = TextConstants.UPGRADES;
        foreach (var row in upgradeRows)
        {
            row.Init();           
        }
    }

    public void UpdateUpgradePanel(IItemData itemData)
    {
        if (itemData is WeaponData WData)
        {
            ShowWeaponUpgradeInfo(WData);
        }
        else if (itemData is VehicleData VData)
        {
            ShowVehicleUpgradeInfo(VData);
        }
        else if (itemData is WeaponSchemeData WSData)
        {
            ShowWeaponSchemeUpgradeInfo(WSData.weaponData);
        }
        else if (itemData is VehicleSchemeData VSData)
        {
            ShowVehicleSchemeUpgradeInfo(VSData.vehicleData);
        }
    }    

    void ShowWeaponUpgradeInfo(WeaponData data)
    {
        UpgradeCost upgradeCost = GameAssets.Instance.WeaponUpgradeCosts.Costs.Find(cost => cost.RaritieType == data.weaponRaritie);


        if (data.hullDmgCurLvl < data.hullDmgMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.hullDmgCurLvl + 1);
            upgradeRows[0].SetData(TextConstants.HULLDMG, data.hullDmgCurLvl, data.hullDmgMaxLvl, data.hullDmgByLvl, lvlCost.ResCost);
        }
        else upgradeRows[0].OnMaxLvlReached(TextConstants.HULLDMG, data.hullDmgCurLvl, data.hullDmgMaxLvl);

        if (data.shieldDmgCurLvl < data.shieldDmgMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.shieldDmgCurLvl + 1);
            upgradeRows[1].SetData(TextConstants.SHIELDDMG, data.shieldDmgCurLvl, data.shieldDmgMaxLvl, data.shieldDmgByLvl, lvlCost.ResCost);
        }
        else upgradeRows[1].OnMaxLvlReached(TextConstants.SHIELDDMG, data.shieldDmgCurLvl, data.shieldDmgMaxLvl);

        if (data.rotationSpeedCurLvl < data.rotationSpeedMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.rotationSpeedCurLvl + 1);
            upgradeRows[2].SetData(TextConstants.ROTATIONSPEED, data.rotationSpeedCurLvl, data.rotationSpeedMaxLvl, data.rotationSpeedByLvl, lvlCost.ResCost);
        }
        else upgradeRows[2].OnMaxLvlReached(TextConstants.ROTATIONSPEED, data.rotationSpeedCurLvl, data.rotationSpeedMaxLvl);

        if (data.weaponType == WeaponType.Beam)
        {
            upgradeRows[3].gameObject.SetActive(false);
            return;
        }

        if (data.fireRateCurtLvl < data.fireRateMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.fireRateCurtLvl + 1);
            upgradeRows[3].SetData(TextConstants.FIRERATE, data.fireRateCurtLvl, data.fireRateMaxLvl, data.fireRateByLvl, lvlCost.ResCost);
        }
        else upgradeRows[3].OnMaxLvlReached(TextConstants.FIRERATE, data.fireRateCurtLvl, data.fireRateMaxLvl);
    }

    void ShowVehicleUpgradeInfo(VehicleData data)
    {
        UpgradeCost upgradeCost = GameAssets.Instance.VehicleUpgradeCosts.Costs.Find(cost => cost.RaritieType == data.vehicleRaritie);
        UpgradeCost slotCost = GameAssets.Instance.VehicleSlotCosts.Costs.Find(cost => cost.RaritieType == data.vehicleRaritie);

        if (data.hullHPCurLvl < data.hullHPMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.hullHPCurLvl + 1);
            upgradeRows[0].SetData(TextConstants.HULLHP, data.hullHPCurLvl, data.hullHPMaxLvl, data.hullHPByLvl, lvlCost.ResCost);
        }
        else upgradeRows[0].OnMaxLvlReached(TextConstants.HULLHP, data.hullHPCurLvl, data.hullHPMaxLvl);

        if (data.shieldHPCurLvl < data.shieldHPMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.shieldHPCurLvl + 1);
            upgradeRows[1].SetData(TextConstants.SHIELDHP, data.shieldHPCurLvl, data.shieldHPMaxLvl, data.shieldHPByLvl, lvlCost.ResCost);
        }
        else upgradeRows[1].OnMaxLvlReached(TextConstants.SHIELDHP, data.shieldHPCurLvl, data.shieldHPMaxLvl);

        if (data.shieldRegenCurtLvl < data.shieldRegenMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.shieldRegenCurtLvl + 1);
            upgradeRows[2].SetData(TextConstants.SHIELREGENRATE, data.shieldRegenCurtLvl, data.shieldRegenMaxLvl, data.shieldRegenRateByLvl, lvlCost.ResCost);
        }
        else upgradeRows[2].OnMaxLvlReached(TextConstants.SHIELREGENRATE, data.shieldRegenCurtLvl, data.shieldRegenMaxLvl);

        if (data.curWeaponsCount < data.maxWeaponsCount)
        {
            LvlCost lvlCost = slotCost.LvlCosts.Find(cost => cost.lvlNumber == data.curWeaponsCount + 1);
            upgradeRows[3].SetData(TextConstants.WEAPONSCOUNT, data.curWeaponsCount, data.maxWeaponsCount, 1, lvlCost.ResCost);
        }
        else upgradeRows[3].OnMaxLvlReached(TextConstants.WEAPONSCOUNT, data.curWeaponsCount, data.maxWeaponsCount);
    }

    void ShowWeaponSchemeUpgradeInfo(WeaponData data)
    {
        ShowWeaponUpgradeInfo(data);
        foreach (var row in upgradeRows)
        {
            row.DisableBuyUpgradeOption();
        }
    }

    void ShowVehicleSchemeUpgradeInfo(VehicleData data)
    {
        ShowVehicleUpgradeInfo(data);
        foreach (var row in upgradeRows)
        {
            row.DisableBuyUpgradeOption();
        }
    }
}
