using System.Collections.Generic;
using UnityEngine;

public class SurviveModeUpgradeService : MonoBehaviour
{
    public static SurviveModeUpgradeService Instance;

    List<UpgradeCardData> _originalUpgradeCards;
    int _maxCardsCount;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init(List<UpgradeCardData> upgradeCardDatas, int maxCardsCount)
    {
        _originalUpgradeCards = upgradeCardDatas;
        _maxCardsCount = maxCardsCount;
    }


    public void OnWeaponUpgrade(UpgradeCardData upgradeCardData)
    {
        SMWeaponData newData = SurviveModeManager.Instance.GetCurrentWeaponData();

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
        SurviveModeManager.Instance.OnWeaponUpgrade(newData);
    }

    public void OnVehicleUpgrade(UpgradeCardData upgradeCardData)
    {
        SMVehicleData newData = SurviveModeManager.Instance.GetCurrentVehicleData();

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
        SurviveModeManager.Instance.OnVehicleUpgrade(newData);
    }

    public List<UpgradeCardData> GetCardsCollection()
    {
        List<UpgradeCardData> randomCardsPack = new();
        List<UpgradeCardData> upgradeCardsCopy = new(_originalUpgradeCards);

        FilterCards(upgradeCardsCopy);

        int targetCardsCount = upgradeCardsCopy.Count >= _maxCardsCount ? _maxCardsCount : upgradeCardsCopy.Count;

        while (randomCardsPack.Count < targetCardsCount)
        {
            int randomIndex = Random.Range(0, upgradeCardsCopy.Count);
            randomCardsPack.Add(upgradeCardsCopy[randomIndex]);
            upgradeCardsCopy.RemoveAt(randomIndex);
        }
        return randomCardsPack;
    }

    void FilterCards(List<UpgradeCardData> upgradeCards)
    {
        SMWeaponData currentWeaponData = SurviveModeManager.Instance.GetCurrentWeaponData();

        if (currentWeaponData.fireRate >= currentWeaponData.maxFireRate)
        {
            UpgradeCardData upgradeCard = upgradeCards.Find(card => card.CharacteristicsName == CharacteristicsName.WeaponFireRate);
            upgradeCards.Remove(upgradeCard);
        }

        if (currentWeaponData.reloadTime <= currentWeaponData.minReloadTime)
        {
            UpgradeCardData upgradeCard = upgradeCards.Find(card => card.CharacteristicsName == CharacteristicsName.WeaponReloadTime);
            upgradeCards.Remove(upgradeCard);
        }
    }
}
