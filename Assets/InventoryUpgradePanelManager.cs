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
        if (itemData is WeaponData data)
        {
            ShowUpgradeInfo(data);
        }
    }

    private void Start()
    {
        foreach (var row in upgradeRows) 
        {
            row.gameObject.SetActive(false);
        }
    }

    void ShowUpgradeInfo(WeaponData data)
    {
        UpgradeCost upgradeCost = GameAssets.Instance.UpgradeCosts.Costs.Find(cost => cost.RaritieType == data.Raritie);


        if (data.hullDmgCurLvl < data.HullDmgMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.hullDmgCurLvl + 1);
            upgradeRows[0].SetData(Constants.HULLDMG, data.hullDmgCurLvl, data.HullDmgMaxLvl, data.hullDmgByLvl, lvlCost.ResCost);
        }
        else upgradeRows[0].OnMaxLvlReached(Constants.HULLDMG, data.hullDmgCurLvl, data.HullDmgMaxLvl);

        if (data.shieldDmgCurLvl < data.ShieldDmgMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.shieldDmgCurLvl + 1);
            upgradeRows[1].SetData(Constants.SHIELDDMG, data.shieldDmgCurLvl, data.ShieldDmgMaxLvl, data.shieldDmgByLvl, lvlCost.ResCost);
        }
        else upgradeRows[1].OnMaxLvlReached(Constants.SHIELDDMG, data.shieldDmgCurLvl, data.ShieldDmgMaxLvl);



        if (data.rotationSpeedCurLvl < data.RotationSpeedMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.rotationSpeedCurLvl + 1);
            upgradeRows[2].SetData(Constants.ROTATIONSPEED, data.rotationSpeedCurLvl, data.RotationSpeedMaxLvl, data.rotationSpeedByLvl, lvlCost.ResCost);
        }
        else upgradeRows[2].OnMaxLvlReached(Constants.ROTATIONSPEED, data.rotationSpeedCurLvl, data.RotationSpeedMaxLvl);

        if (data.Type == WeaponType.Beam)
        {
            upgradeRows[3].gameObject.SetActive(false);
            return;
        }

        if (data.fireRateCurtLvl < data.FireRateMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.fireRateCurtLvl + 1);
            upgradeRows[3].SetData(Constants.FIRERATE, data.fireRateCurtLvl, data.FireRateMaxLvl, data.fireRateByLvl, lvlCost.ResCost);
        }
        else upgradeRows[2].OnMaxLvlReached(Constants.FIRERATE, data.fireRateCurtLvl, data.FireRateMaxLvl);
    }

}
