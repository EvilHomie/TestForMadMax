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
        UpgradeCost upgradeCost = GameConfig.Instance.UpgradeCosts.Costs.Find(cost => cost.RaritieType == data.weaponRaritie);


        if (data.hullDmgCurLvl < data.hullDmgMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.hullDmgCurLvl + 1);
            upgradeRows[0].SetData("Hull Damage", data.hullDmgCurLvl, data.hullDmgMaxLvl, data.hullDmgByLvl, lvlCost.ResCost);
        }
        else upgradeRows[0].OnMaxLvlReached("Hull Damage", data.hullDmgCurLvl, data.hullDmgMaxLvl);

        if (data.shieldDmgCurLvl < data.shieldDmgMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.shieldDmgCurLvl + 1);
            upgradeRows[1].SetData("Shield Damage", data.shieldDmgCurLvl, data.shieldDmgMaxLvl, data.shieldDmgByLvl, lvlCost.ResCost);
        }
        else upgradeRows[1].OnMaxLvlReached("Shield Damage", data.shieldDmgCurLvl, data.shieldDmgMaxLvl);



        if (data.rotationSpeedCurLvl < data.rotationSpeedMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.rotationSpeedCurLvl + 1);
            upgradeRows[2].SetData("Rotation Speed", data.rotationSpeedCurLvl, data.rotationSpeedMaxLvl, data.rotationSpeedByLvl, lvlCost.ResCost);
        }
        else upgradeRows[2].OnMaxLvlReached("Rotation Speed", data.rotationSpeedCurLvl, data.rotationSpeedMaxLvl);

        if (data.weaponType == WeaponType.Beam)
        {
            upgradeRows[3].gameObject.SetActive(false);
            return;
        }

        if (data.fireRateCurtLvl < data.fireRateMaxLvl)
        {
            LvlCost lvlCost = upgradeCost.LvlCosts.Find(cost => cost.lvlNumber == data.fireRateCurtLvl + 1);
            upgradeRows[3].SetData("FireRate", data.fireRateCurtLvl, data.fireRateMaxLvl, data.fireRateByLvl, lvlCost.ResCost);
        }
        else upgradeRows[2].OnMaxLvlReached("FireRate", data.fireRateCurtLvl, data.fireRateMaxLvl);
    }

}
