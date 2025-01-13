using System;
using System.Collections.Generic;
using System.Linq;

public class SurviveModeExpManager
{
    int _killedEnemiesCount;
    List<UpgradeCardData> _upgradeCards;
    List<CharacteristicsName> _characteristicsNames;
    ModeDifficult modeDifficultData;




    public SurviveModeExpManager(List<UpgradeCardData> copyUpgradeCards, ModeDifficult modeDifficult, bool showUpgradeCardsAutomatic)
    {
        _characteristicsNames = Enum.GetValues(typeof(CharacteristicsName)).Cast<CharacteristicsName>().ToList();
        _upgradeCards = copyUpgradeCards;
        modeDifficultData = modeDifficult;
        SurviveModeUpgradePanel.Instance.Init(showUpgradeCardsAutomatic);

    }

    public void OnStartMode()
    {
        _killedEnemiesCount = 0;
        SurviveModeUpgradePanel.Instance.OnStartMode();
    }

    public void OnEnemyKilled()
    {
        _killedEnemiesCount++;

        if (_killedEnemiesCount % modeDifficultData.killAmountForNewWeapon == 0)
        {
            GiveNewWeapon();
            _killedEnemiesCount--;
            return;
        }

        if (_killedEnemiesCount % modeDifficultData.killAmountForLvlUp == 0)
        {
            OnPlayerLvlUp();
        }
    }

    void OnPlayerLvlUp()
    {
        List<UpgradeCardData> randomCardsPack = new();
        List<UpgradeCardData> upgradeCardsCopy = new(_upgradeCards);

        while (randomCardsPack.Count < 3)
        {
            int randomIndex = UnityEngine.Random.Range(0, upgradeCardsCopy.Count);
            randomCardsPack.Add(upgradeCardsCopy[randomIndex]);
            upgradeCardsCopy.RemoveAt(randomIndex);
        }
        SurviveModeUpgradePanel.Instance.AddCardsPack(randomCardsPack);
    }

    public SMWeaponData GetNewWeaponData(UpgradeCardData upgradeCardData, SMWeaponData curentData)
    {
        SMWeaponData newData = curentData;

        switch (upgradeCardData.CharacteristicsName)
        {
            case (CharacteristicsName.WeaponKineticDmg):
                newData.kineticDamage += upgradeCardData.ChangeValue;
                break;
            //case (CharacteristicsName.WeaponEnergyDmg):

            //    break;
            case (CharacteristicsName.WeaponFireRate):
                newData.fireRate += upgradeCardData.ChangeValue;
                break;
            case (CharacteristicsName.WeaponReloadTime):
                newData.reloadTime += upgradeCardData.ChangeValue;
                if (newData.reloadTime <= 0.3f) newData.reloadTime = 0.3f;
                break;
            case (CharacteristicsName.WeaponMagCapacity):
                newData.magCapacity += (int)upgradeCardData.ChangeValue;
                break;
        }
        return newData;
    }

    public SMVehicleData GetNewVehicleData(UpgradeCardData upgradeCardData, SMVehicleData curentData)
    {
        SMVehicleData newData = curentData;

        switch (upgradeCardData.CharacteristicsName)
        {
            case (CharacteristicsName.VehicleHullHP):
                newData.hullHP += upgradeCardData.ChangeValue;
                break;
            case (CharacteristicsName.VehicleShieldHP):
                newData.shieldHP += upgradeCardData.ChangeValue;
                break;
            case (CharacteristicsName.VehicleShieldRegRate):
                newData.shieldRegRate += upgradeCardData.ChangeValue;
                break;

        }
        return newData;
    }

    public void GiveNewWeapon()
    {

    }
}
