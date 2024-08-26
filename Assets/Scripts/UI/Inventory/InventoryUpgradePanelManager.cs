using System.Collections.Generic;
using UnityEngine;

public class InventoryUpgradePanelManager : MonoBehaviour
{
    public static InventoryUpgradePanelManager Instance;
    [SerializeField] List<UpgradeRow> upgradeRows;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
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
    }

    private void Start()
    {
        foreach (var row in upgradeRows) 
        {
            row.gameObject.SetActive(false);
        }
    }

    void ShowWeaponUpgradeInfo(WeaponData data)
    {
        UpgradeCost upgradeCost = GameAssets.Instance.WeaponUpgradeCosts.Costs.Find(cost => cost.RaritieType == data.weaponRaritie);


        if (data.hullDmgCurLvl < data.hullDmgMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.hullDmgCurLvl + 1);
            upgradeRows[0].SetData(Constants.HULLDMG, data.hullDmgCurLvl, data.hullDmgMaxLvl, data.hullDmgByLvl, lvlCost.ResCost);
        }
        else upgradeRows[0].OnMaxLvlReached(Constants.HULLDMG, data.hullDmgCurLvl, data.hullDmgMaxLvl);

        if (data.shieldDmgCurLvl < data.shieldDmgMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.shieldDmgCurLvl + 1);
            upgradeRows[1].SetData(Constants.SHIELDDMG, data.shieldDmgCurLvl, data.shieldDmgMaxLvl, data.shieldDmgByLvl, lvlCost.ResCost);
        }
        else upgradeRows[1].OnMaxLvlReached(Constants.SHIELDDMG, data.shieldDmgCurLvl, data.shieldDmgMaxLvl);

        if (data.rotationSpeedCurLvl < data.rotationSpeedMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.rotationSpeedCurLvl + 1);
            upgradeRows[2].SetData(Constants.ROTATIONSPEED, data.rotationSpeedCurLvl, data.rotationSpeedMaxLvl, data.rotationSpeedByLvl, lvlCost.ResCost);
        }
        else upgradeRows[2].OnMaxLvlReached(Constants.ROTATIONSPEED, data.rotationSpeedCurLvl, data.rotationSpeedMaxLvl);

        if (data.weaponType == WeaponType.Beam)
        {
            upgradeRows[3].gameObject.SetActive(false);
            return;
        }

        if (data.fireRateCurtLvl < data.fireRateMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.fireRateCurtLvl + 1);
            upgradeRows[3].SetData(Constants.FIRERATE, data.fireRateCurtLvl, data.fireRateMaxLvl, data.fireRateByLvl, lvlCost.ResCost);
        }
        else upgradeRows[3].OnMaxLvlReached(Constants.FIRERATE, data.fireRateCurtLvl, data.fireRateMaxLvl);
    }

    void ShowVehicleUpgradeInfo(VehicleData data)
    {
        UpgradeCost upgradeCost = GameAssets.Instance.VehicleUpgradeCosts.Costs.Find(cost => cost.RaritieType == data.vehicleRaritie);
        UpgradeCost slotCost = GameAssets.Instance.VehicleSlotCosts.Costs.Find(cost => cost.RaritieType == data.vehicleRaritie);

        if (data.hullHPCurLvl < data.hullHPMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.hullHPCurLvl + 1);
            upgradeRows[0].SetData(Constants.HULLHP, data.hullHPCurLvl, data.hullHPMaxLvl, data.hullHPByLvl, lvlCost.ResCost);
        }
        else upgradeRows[0].OnMaxLvlReached(Constants.HULLHP, data.hullHPCurLvl, data.hullHPMaxLvl);

        if (data.shieldHPCurLvl < data.shieldHPMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.shieldHPCurLvl + 1);
            upgradeRows[1].SetData(Constants.SHIELDHP, data.shieldHPCurLvl, data.shieldHPMaxLvl, data.shieldHPByLvl, lvlCost.ResCost);
        }
        else upgradeRows[1].OnMaxLvlReached(Constants.SHIELDHP, data.shieldHPCurLvl, data.shieldHPMaxLvl);

        if (data.shieldRegenCurtLvl < data.shieldRegenMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.shieldRegenCurtLvl + 1);
            upgradeRows[2].SetData(Constants.SHIELREGENRATE, data.shieldRegenCurtLvl, data.shieldRegenMaxLvl, data.shieldRegenRateByLvl, lvlCost.ResCost);
        }
        else upgradeRows[2].OnMaxLvlReached(Constants.SHIELREGENRATE, data.shieldRegenCurtLvl, data.shieldRegenMaxLvl);

        if (data.curWeaponsCount < data.maxWeaponsCount)
        {
            LvlCost lvlCost = slotCost.LvlCosts.Find(cost => cost.lvlNumber == data.curWeaponsCount + 1);
            upgradeRows[3].SetData(Constants.WEAPONSCOUNT, data.curWeaponsCount, data.maxWeaponsCount, 1, lvlCost.ResCost);
        }
        else upgradeRows[3].OnMaxLvlReached(Constants.WEAPONSCOUNT, data.curWeaponsCount, data.maxWeaponsCount);
    }
}
