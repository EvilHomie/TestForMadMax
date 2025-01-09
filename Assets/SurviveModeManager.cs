using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SurviveModeManager : MonoBehaviour
{
    public static SurviveModeManager Instance;

    [SerializeField] Slider _difficultSlider;
    [SerializeField] Transform _difficultsColorContainer;
    [SerializeField] GameObject _scullPF;
    [SerializeField] RectTransform _scullsContainer;
    [SerializeField] LevelParameters _deffLevelParameters;
    [SerializeField] VehicleData[] _vehicleDatas;
    [SerializeField] WeaponData[] _weaponDatas;

    [SerializeField] int _maxEnemiesCount;
    [SerializeField] float _startEnemyDmgMod;
    [SerializeField] float _startEnemyHpMod;
    //[SerializeField] float _startEnemyFRMod;

    [SerializeField] float _increaseEnemyPowerDelay;
    [SerializeField] float _increaseEnemyPowerValue;
    [SerializeField] float _increaseEnemyLevelDelay;

    public LevelParameters CopyLevelParameters => _copyLevelParameters;
    public int MaxEnemiesCount => _maxEnemiesCount;


    float _enemyDmgMod;
    float _enemyHpMod;
    //float _enemyFRMod;

    public float EnemyDmgMod => _enemyDmgMod;
    public float EnemyHpMod => _enemyHpMod;
    //public float EnemyFRMod => _enemyFRMod;

    int _difficultCount;
    LevelParameters _copyLevelParameters;
    float _showControllerDelay = 3;
    EnemyLevel _currentEnemyLevel;
    int _enemyTotalCountOnLevel = 0;
    int _killedEnemiesCount = 0;
    int _escapedEnemiesCount = 0;
    VehicleData _currentVehicleData;
    WeaponData _currentWeaponData;

    float enemyPowerLevel = 1;

    List<EnemyLevel> enemyLevels;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        enemyLevels = Enum.GetValues(typeof(EnemyLevel)).Cast<EnemyLevel>().ToList();
        _difficultCount = _difficultsColorContainer.childCount;
        _difficultSlider.transform.parent.gameObject.SetActive(false);
    }

    public void OnStartMode()
    {
        SaveLoadManager.Instance.SaveData();
        ResetData();
        _difficultSlider.transform.parent.gameObject.SetActive(true);

        UIResourcesManager.Instance.DisablePanel();
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

        StartCoroutine(IncreaseEnemyPowerInTime(10));
        //InvokeRepeating(nameof(IncreaseEnemyLevel), _increaseEnemyLevelDelay, _increaseEnemyLevelDelay);
        //InvokeRepeating(nameof(IncreaseEnemyPower), _increaseEnemyPowerDelay, _increaseEnemyPowerDelay);
    }

    void ResetData()
    {
        foreach (Transform c in _scullsContainer)
        {
            Destroy(c.gameObject);
        }

        StopAllCoroutines();
        CancelInvoke();
        _enemyDmgMod = _startEnemyDmgMod;
        _enemyHpMod = _startEnemyHpMod;
        //_enemyFRMod = _startEnemyFRMod;

        _copyLevelParameters = Instantiate(_deffLevelParameters);
        _currentEnemyLevel = EnemyLevel.SuperEasy;
        _killedEnemiesCount = 0;
        _escapedEnemiesCount = 0;
        enemyPowerLevel = 1;
        _difficultSlider.value = 0;
        _enemyTotalCountOnLevel = _copyLevelParameters.GetTotalSimpleEnemyCount();
        _copyLevelParameters.ChangeEnemiesLevel(_currentEnemyLevel);
        _currentVehicleData = Instantiate(_vehicleDatas[0]);
        _currentWeaponData = Instantiate(_weaponDatas[0]);

    }

    void IncreaseEnemyLevel()
    {
        //if (_currentEnemyLevel == EnemyLevel.SuperEasy) _currentEnemyLevel = EnemyLevel.VeryEasy;
        //else if (_currentEnemyLevel == EnemyLevel.VeryEasy) _currentEnemyLevel = EnemyLevel.Easy;
        //else return;

        if (_currentEnemyLevel != enemyLevels.Last())
        {
            int index = enemyLevels.FindIndex(element => element == _currentEnemyLevel);
            index++;
            _currentEnemyLevel = enemyLevels[index];
            _copyLevelParameters.ChangeEnemiesLevel(_currentEnemyLevel);
        }
    }

    IEnumerator IncreaseEnemyPowerInTime(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        float startSliderValue = _difficultSlider.value;
        float endSliderValue = startSliderValue + 1f / _difficultCount;
        float t = 0;
        while (t < 1)
        {
            Debug.Log(t);
            t += Time.deltaTime / _increaseEnemyPowerDelay;
            _difficultSlider.value = Mathf.Lerp(startSliderValue, endSliderValue, t);
            Debug.Log(endSliderValue);
            yield return null;
        }
        IncreaseEnemyPower();
    }

    void IncreaseEnemyPower()
    {
        enemyPowerLevel++;
        if (enemyPowerLevel > _difficultCount && _currentEnemyLevel != enemyLevels.Last())
        {
            enemyPowerLevel = 1;
            _difficultSlider.value = 0;
            _enemyDmgMod = _startEnemyDmgMod;
            _enemyHpMod = _startEnemyHpMod;
            IncreaseEnemyLevel();
        }
        else if(enemyPowerLevel > _difficultCount && _currentEnemyLevel == enemyLevels.Last())
        {
            Instantiate(_scullPF, _scullsContainer);
        }

        _enemyDmgMod += _startEnemyDmgMod * _increaseEnemyPowerValue;
        _enemyHpMod += _startEnemyHpMod * _increaseEnemyPowerValue;

        StartCoroutine(IncreaseEnemyPowerInTime());

        //_enemyFRMod += _startEnemyFRMod * _increaseEnemyPowerValue;
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
        FinishLevelManager.Instance.OnFinishLevel(false);
        StopAllCoroutines();
        CancelInvoke();
    }

    public void Disable()
    {
        _difficultSlider.transform.parent.gameObject.SetActive(false);
    }

}
