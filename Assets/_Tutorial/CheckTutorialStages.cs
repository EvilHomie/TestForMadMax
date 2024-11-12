using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class CheckTutorialStages : MonoBehaviour
{

    public void Init()
    {
        CorrectingStagesForOldPlayers();
    }
    void CorrectingStagesForOldPlayers()
    {
        //CheckGreetingsStatus();
        //CheckFirstLevelComleteStatus();
        CheckWeaponsHaveImprovedFiveTimes();
        CheckVehiclesHaveImprovedFiveTimes();


        SaveLoadManager.Instance.SaveData();
    }
    void AddStageToCompletedTutorialStagesList(StageName stageName)
    {
        if (!YandexGame.savesData.completedTutorialStages.Contains(stageName))
        {
            YandexGame.savesData.completedTutorialStages.Add(stageName);
        }
    }

    void CheckGreetingsStatus()
    {
        //// Проверяем был ли пройден первый уровень
        //if (!YandexGame.savesData.completedTutorialStages.Contains(StageName.Greetings))
        //{
        //    if (PlayerData.Instance.UnlockedLevelsNames.Count > 1) AddStageToCompletedTutorialStagesList(StageName.Greetings);
        //}
    }

    void CheckFirstLevelComleteStatus()
    {
        // Проверяем был ли пройден первый уровень
        if (!YandexGame.savesData.completedTutorialStages.Contains(StageName.FirstRaidLaunch))
        {
            if (PlayerData.Instance.UnlockedLevelsNames.Count > 1) AddStageToCompletedTutorialStagesList(StageName.FirstRaidLaunch);
        }
    }

    void CheckWeaponsHaveImprovedFiveTimes()
    {
        // Проверяем была ли использована прокачка предметов 5 раз
        if (!YandexGame.savesData.completedTutorialStages.Contains(StageName.FirstOpenInventory))
        {
            int upgradeWeaponsCounter = 0;
            List<WeaponData> weaponsData = new();
            List<string> weaponsNames = new();

            foreach (var item in PlayerData.Instance.PlayerItemsData)
            {
                if (item is WeaponData WData)
                {
                    weaponsData.Add(WData);
                    weaponsNames.Add(item.DeffItemName);
                }
            }

            //Debug.LogWarning(weaponsData.Count);
            //foreach (var name in weaponsNames) Debug.LogWarning(name);

            List<WeaponData> deffWeaponsData = new();

            foreach (var weaponData in weaponsData)
            {
                PlayerWeapon weapon = GameAssets.Instance.GameItems.Weapons.Find(weapon => weapon.name == weaponData.deffWeaponName);
                if (weapon == null) continue;
                deffWeaponsData.Add(Instantiate((WeaponData)weapon.GetItemData()));
            }

            //Debug.LogWarning(deffWeaponsData.Count);

            foreach (var weaponName in weaponsNames)
            {
                upgradeWeaponsCounter += weaponsData.Find(data => data.DeffItemName == weaponName).hullDmgCurLvl - deffWeaponsData.Find(data => data.DeffItemName == weaponName).hullDmgCurLvl;
                upgradeWeaponsCounter += weaponsData.Find(data => data.DeffItemName == weaponName).shieldDmgCurLvl - deffWeaponsData.Find(data => data.DeffItemName == weaponName).shieldDmgCurLvl;
                upgradeWeaponsCounter += weaponsData.Find(data => data.DeffItemName == weaponName).fireRateCurtLvl - deffWeaponsData.Find(data => data.DeffItemName == weaponName).fireRateCurtLvl;
                upgradeWeaponsCounter += weaponsData.Find(data => data.DeffItemName == weaponName).rotationSpeedCurLvl - deffWeaponsData.Find(data => data.DeffItemName == weaponName).rotationSpeedCurLvl;
            }

            Debug.LogWarning("Detected Weapons Upgrades Count = " + upgradeWeaponsCounter);
            if (upgradeWeaponsCounter >= 1)
            {
                AddStageToCompletedTutorialStagesList(StageName.FirstOpenInventory);
                AddStageToCompletedTutorialStagesList(StageName.ShowEquipedPanel);
                AddStageToCompletedTutorialStagesList(StageName.SelectFirstWeapon);
                AddStageToCompletedTutorialStagesList(StageName.ShowUpgradeDiscription);
                AddStageToCompletedTutorialStagesList(StageName.UpgradeRotateSpeed);
                AddStageToCompletedTutorialStagesList(StageName.CloseInventory);
                AddStageToCompletedTutorialStagesList(StageName.WishGoodluck);
                //Debug.LogWarning(StageName.FirstOpenInventory.ToString() + "ALREADY DONE");
            }
        }
    }

    void CheckVehiclesHaveImprovedFiveTimes()
    {
        // Проверяем была ли использована прокачка предметов 5 раз
        if (!YandexGame.savesData.completedTutorialStages.Contains(StageName.FirstOpenInventory))
        {
            int upgradeVehiclesCounter = 0;
            List<VehicleData> vehiclesData = new();
            List<string> vehiclesNames = new();

            foreach (var item in PlayerData.Instance.PlayerItemsData)
            {

                if (item is VehicleData VData)
                {
                    vehiclesData.Add(VData);
                    vehiclesNames.Add(VData.DeffItemName);
                }
            }

            //Debug.LogWarning(vehiclesData.Count);
            //foreach (var name in vehiclesNames) Debug.LogWarning(name);

            List<VehicleData> deffVehiclesData = new();

            foreach (var vehicleData in vehiclesData)
            {
                PlayerVehicle playerVehicle = GameAssets.Instance.GameItems.PlayerVehicles.Find(vehicle => vehicle.name == vehicleData.deffVehicleName);
                if (playerVehicle == null) continue;
                deffVehiclesData.Add(Instantiate((VehicleData)playerVehicle.GetItemData()));
            }
;
            //Debug.LogWarning(deffVehiclesData.Count);

            foreach (var vehicleName in vehiclesNames)
            {
                upgradeVehiclesCounter += vehiclesData.Find(data => data.DeffItemName == vehicleName).hullHPCurLvl - deffVehiclesData.Find(data => data.DeffItemName == vehicleName).hullHPCurLvl;
                upgradeVehiclesCounter += vehiclesData.Find(data => data.DeffItemName == vehicleName).shieldHPCurLvl - deffVehiclesData.Find(data => data.DeffItemName == vehicleName).shieldHPCurLvl;
                upgradeVehiclesCounter += vehiclesData.Find(data => data.DeffItemName == vehicleName).shieldRegenCurtLvl - deffVehiclesData.Find(data => data.DeffItemName == vehicleName).shieldRegenCurtLvl;
                upgradeVehiclesCounter += vehiclesData.Find(data => data.DeffItemName == vehicleName).curWeaponsCount - deffVehiclesData.Find(data => data.DeffItemName == vehicleName).curWeaponsCount;
            }
            Debug.LogWarning("Detected Vehicle Upgrades Count = " + upgradeVehiclesCounter);
            if (upgradeVehiclesCounter >= 1)
            {
                AddStageToCompletedTutorialStagesList(StageName.FirstOpenInventory);
                AddStageToCompletedTutorialStagesList(StageName.ShowEquipedPanel);
                AddStageToCompletedTutorialStagesList(StageName.SelectFirstWeapon);
                AddStageToCompletedTutorialStagesList(StageName.ShowUpgradeDiscription);
                AddStageToCompletedTutorialStagesList(StageName.UpgradeRotateSpeed);
                AddStageToCompletedTutorialStagesList(StageName.CloseInventory);
                AddStageToCompletedTutorialStagesList(StageName.WishGoodluck);
            }
        }
    }
}
