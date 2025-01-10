using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SurviveModeManager : MonoBehaviour
{
    public static SurviveModeManager Instance;
    [Header("Difficult Data")]
    [SerializeField] Slider _difficultSlider;
    [SerializeField] Transform _difficultsColorContainer;
    [SerializeField] GameObject _scullPF;
    [SerializeField] RectTransform _scullsContainer;
    [SerializeField] ModeDifficult _deffDifficultData;
    [SerializeField] LevelParameters _deffLevelParameters;
    [SerializeField] float _startDifficultLogicDelay;

    [Header("Mode Items")]
    [SerializeField] VehicleData[] _vehicleDatas;
    [SerializeField] AbstractWeapon[] _originalWeapons;
    [SerializeField] SMWeaponData[] _weaponsDeffData;
    [SerializeField] SMVehicleData[] _vehicleDeffData;

    [Header("Player LvlUp Data")]
    [SerializeField] UpgradeCardData[] _upgradeCardsDeffData;
    //[SerializeField] int _killedCountForLvlUp;
    [SerializeField] bool _showUpgradeCardsAutomatic;

    SurviveModeDifficultManager surviveModeDifficultManager;
    SurviveModeExpManager surviveModeExpManager;


    public LevelParameters SMLevelParameters => surviveModeDifficultManager.LevelParameters;
    public int MaxEnemiesCount => surviveModeDifficultManager.ModeDifficult.maxEnemiesCount;
    public float EnemyDmgMod => surviveModeDifficultManager.ModeDifficult.enemyDmgMod;
    public float EnemyHpMod => surviveModeDifficultManager.ModeDifficult.enemyHpMod; 
    
    
    AbstractWeapon _currentWeapon;
    SMWeaponData _currentWeaponData;
    SMVehicleData _currentVehicleData;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _difficultSlider.transform.parent.gameObject.SetActive(false);
        surviveModeDifficultManager = new(_difficultSlider, _difficultsColorContainer, _scullPF, _scullsContainer);
        surviveModeExpManager = new(new(_upgradeCardsDeffData), _deffDifficultData.killAmountForLvlUp, _showUpgradeCardsAutomatic);
    }

    public void ChangeDifficultValues(float PUDelay, float PUValue, int KillAmount)
    {
        Debug.Log($"{PUDelay} {PUValue} {KillAmount}");
        _deffDifficultData.enemyPowerUpDelay = PUDelay == 0 ? _deffDifficultData.enemyPowerUpDelay: PUDelay;
        _deffDifficultData.increaseEnemyPowerValue = PUValue == 0 ? _deffDifficultData.increaseEnemyPowerValue : PUValue;
        _deffDifficultData.killAmountForLvlUp = KillAmount == 0 ? _deffDifficultData.killAmountForLvlUp : KillAmount;
    }

    public void OnStartMode()
    {
        surviveModeExpManager.OnStartMode();
        surviveModeDifficultManager.OnStartMode(_deffDifficultData, Instantiate(_deffLevelParameters));
        CreateWeapon(0);
        CreateVehicle(0);
        Configmanagers();

        StartCoroutine(ModeTimersCoroutine(_startDifficultLogicDelay));

        TESTSurviveModStatistics.Instance.UpdatePlayerVehicleData(_currentVehicleData.hullHP, _currentVehicleData.shieldHP, _currentVehicleData.shieldRegRate);
        TESTSurviveModStatistics.Instance.UpdatePlayerWeaponData(_currentWeaponData.kineticDamage, _currentWeaponData.fireRate, _currentWeaponData.reloadTime, _currentWeaponData.magCapacity);
    }

    void CreateWeapon(int weaponIndex)
    {
        PlayerWeaponManager.Instance.OnStartSurviveMod(_originalWeapons[weaponIndex].gameObject, out AbstractWeapon createdWeapon);

        _currentWeapon = createdWeapon;
        if (_currentWeapon is ProjectileWeaponNotMiniGun weapon)
        {
            _currentWeaponData = _weaponsDeffData[weaponIndex];
            weapon.TEMPSetValues(_weaponsDeffData[weaponIndex]);
            weapon.Init();
        }
    }

    void CreateVehicle(int vehicleIndex)
    {
        PlayerVehicleManager.Instance.CreateSpecificVehicleInstance(_vehicleDatas[vehicleIndex]);
        PlayerHPManager.Instance.OnStartSurviveMode(_vehicleDeffData[vehicleIndex]);
        _currentVehicleData = _vehicleDeffData[vehicleIndex];
    }

    void Configmanagers()
    {
        UIResourcesManager.Instance.DisablePanel();        
        UIJoystickTouchController.Instance.OnStartRaid();
        UIWeaponsSwitcher.Instance.OnPlayerStartRaid();
        CameraManager.Instance.OnPlayerStartRaid();
        InRaidManager.Instance.OnStartSurviveMod();        
        UIEnemyHpPanel.Instance.OnPlayerStartRaid();
        UILevelStatistic.Instance.OnPlayerStartRaid();
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

    public void OnSelectWeaponUpgradeCard(UpgradeCardData upgradeCardData)
    {        
        _currentWeaponData = surviveModeExpManager.GetNewWeaponData(upgradeCardData, _currentWeaponData);
        if (_currentWeapon is ProjectileWeaponNotMiniGun weapon)
        {
            weapon.TEMPSetValues(_currentWeaponData);
        }
        TESTSurviveModStatistics.Instance.UpdatePlayerWeaponData(_currentWeaponData.kineticDamage, _currentWeaponData.fireRate, _currentWeaponData.reloadTime, _currentWeaponData.magCapacity);
    }

    public void OnSelectVehicleUpgradeCard(UpgradeCardData upgradeCardData)
    {
        _currentVehicleData = surviveModeExpManager.GetNewVehicleData(upgradeCardData, _currentVehicleData);
        PlayerHPManager.Instance.OnChangeValuesInSurviveMode(_currentVehicleData);
        TESTSurviveModStatistics.Instance.UpdatePlayerVehicleData(_currentVehicleData.hullHP, _currentVehicleData.shieldHP, _currentVehicleData.shieldRegRate);
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
        surviveModeDifficultManager.OnDisableMode();
        WeaponMagazinePresentation.Instance.DisablePanel();
    }

   
}
[Serializable]
public struct SMWeaponData
{
    public string weaponName;
    public float kineticDamage;
    public float energyDamage;
    public float fireRate;
    public int magCapacity;
    public float reloadTime;
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

    public readonly string UpgradeText => _upgradeText;
    public readonly UpgradeItemType UpgradeItemType => _upgradeItemType;
    public readonly CharacteristicsName CharacteristicsName => _characteristicsName;
    public readonly float ChangeValue => _changeValue;
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
    //public float enemyTirUpDelay;
}   