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
    [SerializeField] bool _showUpgradeCardsAutomatic;
    [SerializeField] int _maxCardsCount;

    SurviveModeDifficultManager surviveModeDifficultManager;
    SurviveModeExpManager surviveModeExpManager;


    public LevelParameters SMLevelParameters => surviveModeDifficultManager.LevelParameters;
    public int MaxEnemiesCount => surviveModeDifficultManager.ModeDifficult.maxEnemiesCount;
    public float EnemyDmgMod => surviveModeDifficultManager.ModeDifficult.enemyDmgMod;
    public float EnemyHpMod => surviveModeDifficultManager.ModeDifficult.enemyHpMod;


    AbstractWeapon _currentWeapon;
    SMWeaponData _currentWeaponData;
    SMVehicleData _currentVehicleData;

    int _curWeaponIndex;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _difficultSlider.transform.parent.gameObject.SetActive(false);
        surviveModeDifficultManager = new(_difficultSlider, _difficultsColorContainer, _scullPF, _scullsContainer, _deffDifficultData, Instantiate(_deffLevelParameters));
        surviveModeExpManager = new(_deffDifficultData);
        SurviveModeUpgradePanel.Instance.Init(_showUpgradeCardsAutomatic);
        SurviveModeUpgradeService.Instance.Init(new(_upgradeCardsDeffData), _maxCardsCount);
    }

    public void ChangeDifficultValues(float PUDelay, float PUValue, int KillAmount)
    {
        _deffDifficultData.enemyPowerUpDelay = PUDelay == 0 ? _deffDifficultData.enemyPowerUpDelay : PUDelay;
        _deffDifficultData.increaseEnemyPowerValue = PUValue == 0 ? _deffDifficultData.increaseEnemyPowerValue : PUValue;
        _deffDifficultData.killAmountForLvlUp = KillAmount == 0 ? _deffDifficultData.killAmountForLvlUp : KillAmount;
    }

    public void OnStartMode()
    {
        _curWeaponIndex = 0;
        _currentWeaponData = _weaponsDeffData[_curWeaponIndex];
        CreateWeapon(_curWeaponIndex);
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
            weapon.TEMPSetValues(_currentWeaponData);
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
        surviveModeExpManager.OnStartMode();
        surviveModeDifficultManager.OnStartMode();
        UIResourcesManager.Instance.DisablePanel();
        UIJoystickTouchController.Instance.OnStartRaid();
        UIWeaponsSwitcher.Instance.OnPlayerStartRaid();
        CameraManager.Instance.OnPlayerStartRaid();
        InRaidManager.Instance.OnStartSurviveMod();
        UIEnemyHpPanel.Instance.OnPlayerStartRaid();
        UILevelStatistic.Instance.OnPlayerStartRaid();
        SurviveModeUpgradePanel.Instance.OnStartMode();
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

    public SMWeaponData GetCurrentWeaponData()
    {
        return _currentWeaponData;
    }

    public SMVehicleData GetCurrentVehicleData()
    {
        return _currentVehicleData;
    }

    public void OnWeaponUpgrade(SMWeaponData newSMWeaponData)
    {
        _currentWeaponData = newSMWeaponData;
        if (_currentWeapon is ProjectileWeaponNotMiniGun weapon)
        {
            weapon.TEMPSetValues(_currentWeaponData);
        }
        TESTSurviveModStatistics.Instance.UpdatePlayerWeaponData(_currentWeaponData.kineticDamage, _currentWeaponData.fireRate, _currentWeaponData.reloadTime, _currentWeaponData.magCapacity);
    }

    public void OnVehicleUpgrade(SMVehicleData newSMVehicleData)
    {
        _currentVehicleData = newSMVehicleData;
        PlayerHPManager.Instance.OnChangeValuesInSurviveMode(_currentVehicleData);
        TESTSurviveModStatistics.Instance.UpdatePlayerVehicleData(_currentVehicleData.hullHP, _currentVehicleData.shieldHP, _currentVehicleData.shieldRegRate);
    }

    public void OnChangeWeapon()
    {
        _curWeaponIndex++;
        if(_curWeaponIndex >= _weaponsDeffData.Length) return;
        SMWeaponData lastWD = _currentWeaponData;
        SMWeaponData newWD = _weaponsDeffData[_curWeaponIndex];
        _currentWeaponData = lastWD;
        _currentWeaponData.maxFireRate = newWD.maxFireRate;
        _currentWeaponData.minReloadTime = newWD.minReloadTime;
        CreateWeapon(_curWeaponIndex);
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

    public float maxFireRate;
    public float minReloadTime;
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
    public int killAmountForNewWeapon;
    //public float enemyTirUpDelay;
}