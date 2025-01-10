using System;
using System.Collections.Generic;
using System.Linq;

public class SurviveModeExpManager
{
    int _killedEnemiesCount;
    int _killedCountForLvlUp;
    List<UpgradeCardData> _upgradeCards;
    List<CharacteristicsName> _characteristicsNames;

    public SurviveModeExpManager(List<UpgradeCardData> copyUpgradeCards, int enemyAmountForLvlUp)
    {
        _characteristicsNames = Enum.GetValues(typeof(CharacteristicsName)).Cast<CharacteristicsName>().ToList();
        _upgradeCards = copyUpgradeCards;
        _killedCountForLvlUp = enemyAmountForLvlUp;
    }

    public void OnStartMode()
    {
        _killedEnemiesCount = 0;     
    }

    public void OnEnemyKilled()
    {
        _killedEnemiesCount++;
        if (_killedEnemiesCount % _killedCountForLvlUp == 0)
        {
            OnPlayerLvlUp();
        }
    }

    void OnPlayerLvlUp()
    {
        List<UpgradeCardData> randomCards = new();
        List<UpgradeCardData> upgradeCardsCopy = _upgradeCards.FindAll(card => card.UpgradeItemType == UpgradeItemType.Weapon);

        while (randomCards.Count < 3)
        {
            int randomIndex = UnityEngine.Random.Range(0, upgradeCardsCopy.Count);
            randomCards.Add(upgradeCardsCopy[randomIndex]);
            upgradeCardsCopy.RemoveAt(randomIndex);
        }

        SurviveModeUpgradePanel.Instance.ConfigPanel(randomCards);
    }

    public SMWeaponData OnSelectWeaponUpgradeCard(UpgradeCardData upgradeCardData, SMWeaponData curentWD)
    {
        SMWeaponData newWeaponData = curentWD;

        switch (upgradeCardData.CharacteristicsName)
        {
            case (CharacteristicsName.WeaponKineticDmg):
                newWeaponData.kineticDamage += upgradeCardData.ChangeValue;
                break;
            //case (CharacteristicsName.WeaponEnergyDmg):

            //    break;
            case (CharacteristicsName.WeaponFireRate):
                newWeaponData.fireRate += upgradeCardData.ChangeValue;
                break;
            case (CharacteristicsName.WeaponReloadTime):
                newWeaponData.reloadTime += upgradeCardData.ChangeValue;
                if (newWeaponData.reloadTime <= 0.3f) newWeaponData.reloadTime = 0.3f;
                break;
            case (CharacteristicsName.WeaponMagCapacity):
                newWeaponData.magCapacity += (int)upgradeCardData.ChangeValue;
                break;
        }
        return newWeaponData;
    }

    //public void OnSelectVehicleUpgradeCard(UpgradeCardData upgradeCardData, SMWeaponData curentWD, out SMWeaponData newWD)
    //{
    //    SMWeaponData newWeaponData = curentWD;

    //    switch (upgradeCardData.CharacteristicsName)
    //    {
    //        case (CharacteristicsName.WeaponKineticDmg):
    //            newWeaponData.kineticDamage += upgradeCardData.ChangeValue;
    //            break;
    //        //case (CharacteristicsName.WeaponEnergyDmg):

    //        //    break;
    //        case (CharacteristicsName.WeaponFireRate):
    //            newWeaponData.fireRate += upgradeCardData.ChangeValue;
    //            break;
    //        case (CharacteristicsName.WeaponReloadTime):
    //            newWeaponData.reloadTime += upgradeCardData.ChangeValue;
    //            if (newWeaponData.reloadTime <= 0.3f) newWeaponData.reloadTime = 0.3f;
    //            break;
    //        case (CharacteristicsName.WeaponMagCapacity):
    //            newWeaponData.magCapacity += (int)upgradeCardData.ChangeValue;
    //            break;
    //            //case (CharacteristicsName.VehicleHullHP):

    //            //    break;
    //            //case (CharacteristicsName.VehicleShieldHP):

    //            //    break;
    //            //case (CharacteristicsName.VehicleShieldRegRate):

    //            //    break;

    //    }
    //    newWD = newWeaponData;
    //}
}
