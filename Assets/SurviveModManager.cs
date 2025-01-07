using UnityEngine;
using YG;

public class SurviveModManager : MonoBehaviour
{
    public static SurviveModManager Instance;

    [SerializeField] LevelParameters _deffLevelParameters;
    [SerializeField] VehicleData[] _vehicleDatas;
    [SerializeField] WeaponData[] _weaponDatas;

    [SerializeField] int _maxEnemiesCount;
    [SerializeField] float _startEnemyDmgMod;
    [SerializeField] float _startEnemyHpMod;
    [SerializeField] float _startEnemyFRMod;

    [SerializeField] float _increaseEnemyPowerDelay;
    [SerializeField] float _increaseEnemyPowerValue;
    [SerializeField] float _increaseEnemyLevelDelay;

    public LevelParameters CopyLevelParameters => _copyLevelParameters;
    public int MaxEnemiesCount => _maxEnemiesCount;


    float _enemyDmgMod;
    float _enemyHpMod;
    float _enemyFRMod;

    public float EnemyDmgMod => _enemyDmgMod;
    public float EnemyHpMod => _enemyHpMod;
    public float EnemyFRMod => _enemyFRMod;


    LevelParameters _copyLevelParameters;
    float _showControllerDelay = 3;
    EnemyLevel _currentEnemyLevel;
    int _enemyTotalCountOnLevel = 0;
    int _killedEnemiesCount = 0;
    int _escapedEnemiesCount = 0;
    VehicleData _currentVehicleData;
    WeaponData _currentWeaponData;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void OnStartMod()
    {
        SaveLoadManager.Instance.SaveData();
        ResetData();

        PlayerWeaponManager.Instance.OnStartSurviveMod(_currentWeaponData);
        PlayerVehicleManager.Instance.CreateSpecificVehicleInstance(_currentVehicleData);
        UIJoystickTouchController.Instance.OnStartRaid(_showControllerDelay);
        UIWeaponsSwitcher.Instance.OnPlayerStartRaid();
        CameraManager.Instance.OnPlayerStartRaid();
        InRaidManager.Instance.OnStartSurviveMod();
        PlayerHPManager.Instance.OnPlayerStartRaid();
        UIEnemyHpPanel.Instance.OnPlayerStartRaid();
        UILevelStatistic.Instance.OnPlayerStartRaid();
        YandexGame.GameplayStart();

        InvokeRepeating(nameof(IncreaseEnemyLevel), _increaseEnemyLevelDelay, _increaseEnemyLevelDelay);
        InvokeRepeating(nameof(IncreaseEnemyPower), _increaseEnemyPowerDelay, _increaseEnemyPowerDelay);
    }

    void ResetData()
    {
        StopAllCoroutines();
        CancelInvoke();
        _enemyDmgMod = _startEnemyDmgMod;
        _enemyHpMod = _startEnemyHpMod;
        _enemyFRMod = _startEnemyFRMod;

        _copyLevelParameters = Instantiate(_deffLevelParameters);
        _currentEnemyLevel = EnemyLevel.SuperEasy;
        _killedEnemiesCount = 0;
        _escapedEnemiesCount = 0;
        _enemyTotalCountOnLevel = _copyLevelParameters.GetTotalSimpleEnemyCount();
        _copyLevelParameters.ChangeEnemiesLevel(_currentEnemyLevel);
        _currentVehicleData = Instantiate(_vehicleDatas[0]);
        _currentWeaponData = Instantiate(_weaponDatas[0]);
    }

    void IncreaseEnemyLevel()
    {
        if (_currentEnemyLevel == EnemyLevel.SuperEasy) _currentEnemyLevel = EnemyLevel.VeryEasy;
        else if (_currentEnemyLevel == EnemyLevel.VeryEasy) _currentEnemyLevel = EnemyLevel.Easy;
        else return;

        _copyLevelParameters.ChangeEnemiesLevel(_currentEnemyLevel);
    }

    void IncreaseEnemyPower()
    {
        _enemyDmgMod += _startEnemyDmgMod * _increaseEnemyPowerValue;
        _enemyHpMod += _startEnemyHpMod * _increaseEnemyPowerValue;
        _enemyFRMod += _startEnemyFRMod * _increaseEnemyPowerValue;
    }

    public void OnEnemyObjectDestroyed()
    {

    }

    public void OnEnemyKilled()
    {
        _killedEnemiesCount++;
    }
    public void OnEnemyEscaped()
    {
        _escapedEnemiesCount++;
    }

    public void OnCompleteSurviveMod()
    {

    }

    public void OnPlayerDie()
    {

    }

}
