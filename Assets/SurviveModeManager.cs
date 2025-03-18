using System;
using System.Collections;
using UnityEngine;
using YG;

public class SurviveModeManager : MonoBehaviour
{
    public static SurviveModeManager Instance;

    [SerializeField] AbstractAmmunitionBelt _abstractAmmunitionBelt;

    [Header("Difficult Data")]
    [SerializeField] ModeDifficult _deffDifficultData;
    [SerializeField] LevelParameters _deffLevelParameters;
    [SerializeField] float _startDifficultLogicDelay;

    [Header("Mode Items")]
    [SerializeField] VehicleData[] _vehicleDatas;
    [SerializeField] AbstractPlayerWeapon[] _originalWeapons;
    [SerializeField] NewWeaponData[] _weaponsDeffData;
    [SerializeField] SMVehicleData[] _vehicleDeffData;

    [Header("Player LvlUp Data")]
    [SerializeField] UpgradeCardData[] _upgradeCardsDeffData;
    [SerializeField] bool _showUpgradeCardsAutomatic;
    [SerializeField] int _maxCardsCount;

    SurviveModeDifficultManager surviveModeDifficultManager;
    SurviveModeExpManager surviveModeExpManager;


    public LevelParameters SMLevelParameters => surviveModeDifficultManager.LevelParameters;
    public int MaxEnemiesCount => surviveModeDifficultManager.ModeDifficult.maxEnemiesCount;
    public float EnemyDmgMod => surviveModeDifficultManager.ModeDifficult.enemyDmgMod;
    public float EnemyHpMod => surviveModeDifficultManager.ModeDifficult.enemyHpMod;


    AbstractPlayerWeapon _currentPlayerWeapon;
    NewWeaponData _currentWeaponData;
    SMVehicleData _currentVehicleData;

    int _curWeaponIndex;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {        
        gameObject.SetActive(false);
        TESTSurviveModStatistics.Instance.Init();

        surviveModeDifficultManager = new(_deffDifficultData, Instantiate(_deffLevelParameters));
        surviveModeExpManager = new(_deffDifficultData);
        SurviveModeDifficultProgress.Instance.Init();
        SurviveModeUpgradePanel.Instance.Init(_showUpgradeCardsAutomatic);
        SurviveModeUpgradeService.Instance.Init(new(_upgradeCardsDeffData), _maxCardsCount);

        _abstractAmmunitionBelt.Init();
    }

    public void ChangeDifficultValues(float PUDelay, float PUValue, int KillAmount)
    {
        _deffDifficultData.enemyPowerUpDelay = PUDelay == 0 ? _deffDifficultData.enemyPowerUpDelay : PUDelay;
        _deffDifficultData.increaseEnemyPowerValue = PUValue == 0 ? _deffDifficultData.increaseEnemyPowerValue : PUValue;
        _deffDifficultData.killAmountForLvlUp = KillAmount == 0 ? _deffDifficultData.killAmountForLvlUp : KillAmount;
    }

    public void OnStartMode()
    {
        gameObject.SetActive(true);
        _curWeaponIndex = 0;
        _currentWeaponData = new(_weaponsDeffData[_curWeaponIndex]);
        CreateWeapon(_curWeaponIndex);
        CreateVehicle(0);
        ConfigManagersOnStartMode();

        StartCoroutine(ModeTimersCoroutine(_startDifficultLogicDelay));

        TESTSurviveModStatistics.Instance.UpdatePlayerVehicleData(_currentVehicleData.hullHP, _currentVehicleData.shieldHP, _currentVehicleData.shieldRegRate);
        TESTSurviveModStatistics.Instance.UpdatePlayerWeaponData(_currentWeaponData.kineticDamage, _currentWeaponData.fireRate, _currentWeaponData.reloadTime, _currentWeaponData.magCapacity);
    }

    void CreateWeapon(int weaponIndex)
    {        
        PlayerWeaponManager.Instance.OnSurviveModChangeWeapon(_originalWeapons[weaponIndex].gameObject, out AbstractPlayerWeapon createdWeapon);
        _currentPlayerWeapon = createdWeapon;
        _currentPlayerWeapon.Init(_currentWeaponData, _abstractAmmunitionBelt);
        _abstractAmmunitionBelt.OnChangeWeapon(_currentWeaponData);
    }

    void CreateVehicle(int vehicleIndex)
    {
        PlayerVehicleManager.Instance.CreateSpecificVehicleInstance(_vehicleDatas[vehicleIndex]);
        PlayerHPManager.Instance.OnStartSurviveMode(_vehicleDeffData[vehicleIndex]);
        _currentVehicleData = _vehicleDeffData[vehicleIndex];
    }

    void ConfigManagersOnStartMode()
    {
        surviveModeExpManager.OnStartSurviveMode();
        surviveModeDifficultManager.OnStartSurviveMode();
        UIJoystickTouchController.Instance.OnStartSurviveMode();
        SurviveModeDifficultProgress.Instance.OnStartMode();
        UIResourcesManager.Instance.DisablePanel();
        UIWeaponsSwitcher.Instance.OnStartSurviveMode();
        CameraManager.Instance.OnPlayerStartRaid();
        InRaidManager.Instance.OnStartSurviveMode();
        UIEnemyHpPanel.Instance.OnPlayerStartRaid();
        UILevelStatistic.Instance.OnPlayerStartRaid();
        SurviveModeUpgradePanel.Instance.OnStartMode();
        PlayerWeaponManager.Instance.OnStartSurviveMod();
        _abstractAmmunitionBelt.OnStartSurviveMode(_currentWeaponData);
        YandexGame.GameplayStart();
    }

    IEnumerator ModeTimersCoroutine(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            surviveModeDifficultManager.IncreaseEnemyPowerInTime();
            yield return null;
        }
    }

    public NewWeaponData GetCurrentWeaponData()
    {
        return _currentWeaponData;
    }

    public SMVehicleData GetCurrentVehicleData()
    {
        return _currentVehicleData;
    }

    public void OnWeaponUpgrade(NewWeaponData newWeaponData, CharacteristicsName characteristicsName)
    {
        _currentWeaponData = newWeaponData;
        if (characteristicsName == CharacteristicsName.WeaponMagCapacity)
        {
            _abstractAmmunitionBelt.OnChangeMagCapacity();
        }
        TESTSurviveModStatistics.Instance.UpdatePlayerWeaponData(_currentWeaponData.kineticDamage, _currentWeaponData.fireRate, _currentWeaponData.reloadTime, _currentWeaponData.magCapacity);
    }

    public void OnVehicleUpgrade(SMVehicleData newSMVehicleData)
    {
        _currentVehicleData = newSMVehicleData;
        PlayerHPManager.Instance.OnChangeValuesInSurviveMode(_currentVehicleData);
        TESTSurviveModStatistics.Instance.UpdatePlayerVehicleData(_currentVehicleData.hullHP, _currentVehicleData.shieldHP, _currentVehicleData.shieldRegRate);
    }

    public bool OnChangeWeapon()
    {
        _curWeaponIndex++;
        if(_curWeaponIndex >= _weaponsDeffData.Length) return false;
        SurviveModeUpgradePanel.Instance.OnGiveNewWeapon(_weaponsDeffData[_curWeaponIndex].weaponName);

        _currentWeaponData.maxFireRate = _weaponsDeffData[_curWeaponIndex].maxFireRate;
        _currentWeaponData.minReloadTime = _weaponsDeffData[_curWeaponIndex].minReloadTime;
        CreateWeapon(_curWeaponIndex);       
        return true;
    }






    public void OnEnemyObjectDestroyed()
    {

    }

    public void OnEnemyKilled()
    {
        surviveModeExpManager.OnEnemyKilled();
    }
    public void OnEnemyEscaped()
    {

    }

    public void OnCompleteSurviveMod()
    {

    }

    public void OnPlayerDie()
    {
        Disable();
    }

    public void OnDisableMode()
    {
        Disable();
    }

    void Disable()
    {
        StopAllCoroutines();
        CancelInvoke();
        FinishLevelManager.Instance.OnFinishLevel(false);
        _abstractAmmunitionBelt.DisablePanel();
        SurviveModeDifficultProgress.Instance.OnFinishMode();
        UIExpPresentationManager.Instance.OnStopSurviveMode();
        UIJoystickTouchController.Instance.OnStopSurviveMode();
    }


}
[Serializable]
public class NewWeaponData
{
    public string weaponName;
    public float kineticDamage;
    public float energyDamage;
    public float fireRate;
    public int magCapacity;
    public float reloadTime;

    [Header("Logic Data")]
    public float rotationSpeed;
    public float maxFireRate;
    public float minReloadTime;
    public float shakeOnShootIntensity;
    public float shakeOnShootDuration;

    public bool isShooting;
    public bool isReloading;
    public float nextTimeTofire;
    public int bulletInMagLeft;

    public NewWeaponData(NewWeaponData newWeaponData)
    {
        weaponName = newWeaponData.weaponName;
        kineticDamage = newWeaponData.kineticDamage;
        energyDamage = newWeaponData.energyDamage;
        fireRate = newWeaponData.fireRate;
        magCapacity = newWeaponData.magCapacity;
        reloadTime = newWeaponData.reloadTime;
        rotationSpeed = newWeaponData.rotationSpeed;
        maxFireRate = newWeaponData.maxFireRate;
        minReloadTime = newWeaponData.minReloadTime;
        shakeOnShootIntensity = newWeaponData.shakeOnShootIntensity;
        shakeOnShootDuration = newWeaponData.shakeOnShootDuration;
        isShooting = newWeaponData.isShooting;
        isReloading = newWeaponData.isReloading;
        nextTimeTofire = newWeaponData.nextTimeTofire;
        bulletInMagLeft = newWeaponData.bulletInMagLeft;
    }
}

[Serializable]
public struct SMVehicleData
{
    public string vehicleName;
    public float hullHP;
    public float shieldHP;
    public float shieldRegRate;
}

[Serializable]
public struct UpgradeCardData
{
    [SerializeField] string _upgradeText;
    [SerializeField] UpgradeItemType _upgradeItemType;
    [SerializeField] CharacteristicsName _characteristicsName;
    [SerializeField] float _changeValue;
    [SerializeField] Sprite _icon;

    public readonly string UpgradeText => _upgradeText;
    public readonly UpgradeItemType UpgradeItemType => _upgradeItemType;
    public readonly CharacteristicsName CharacteristicsName => _characteristicsName;
    public readonly float ChangeValue => _changeValue;

    public readonly Sprite Icon => _icon;
}

[Serializable]
public struct ModeDifficult
{
    public int maxEnemiesCount;
    public float enemyDmgMod;
    public float enemyHpMod;
    public float enemyPowerUpDelay;
    public float increaseEnemyPowerValue;
    public int killAmountForLvlUp;
    public int killAmountForNewWeapon;
    //public float enemyTirUpDelay;
}