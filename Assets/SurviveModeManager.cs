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
    [SerializeField] AbstractWeapon[] _originalWeapons;
    [SerializeField] SMWeaponData[] _weaponsStartData;
    [SerializeField] UpgradeCardData[] _upgradeCardsData;

    [SerializeField] int _killedCountForLvlUp;

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
    AbstractWeapon _currentWeapon;
    SMWeaponData _currentWeaponData;

    float _enemyPowerLevel = 1;

    List<EnemyLevel> _enemyTirs;

    List<CharacteristicsName> _characteristicsNames;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        _enemyTirs = Enum.GetValues(typeof(EnemyLevel)).Cast<EnemyLevel>().ToList();
        _characteristicsNames = Enum.GetValues(typeof(CharacteristicsName)).Cast<CharacteristicsName>().ToList();
        _difficultCount = _difficultsColorContainer.childCount;
        _difficultSlider.transform.parent.gameObject.SetActive(false);
    }

    public void OnStartMode()
    {
        SaveLoadManager.Instance.SaveData();
        ResetData();
        _difficultSlider.transform.parent.gameObject.SetActive(true);

        UIResourcesManager.Instance.DisablePanel();
        PlayerWeaponManager.Instance.OnStartSurviveMod(_currentWeapon.gameObject, out AbstractWeapon createdWeapon);
        PlayerVehicleManager.Instance.CreateSpecificVehicleInstance(_currentVehicleData);
        UIJoystickTouchController.Instance.OnStartRaid(_showControllerDelay);
        UIWeaponsSwitcher.Instance.OnPlayerStartRaid();
        CameraManager.Instance.OnPlayerStartRaid();
        InRaidManager.Instance.OnStartSurviveMod();
        PlayerHPManager.Instance.OnPlayerStartRaid();
        UIEnemyHpPanel.Instance.OnPlayerStartRaid();
        UILevelStatistic.Instance.OnPlayerStartRaid();
        YandexGame.GameplayStart();

        _currentWeapon = createdWeapon;
        if (_currentWeapon is ProjectileWeaponNotMiniGun weapon)
        {
            _currentWeaponData = _weaponsStartData[0];
            weapon.TEMPSetValues(_currentWeaponData);
            weapon.Init();
        }




        StartCoroutine(IncreaseEnemyPowerInTime(10));
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
        _enemyPowerLevel = 1;
        _difficultSlider.value = 0;
        _enemyTotalCountOnLevel = _copyLevelParameters.GetTotalSimpleEnemyCount();
        _copyLevelParameters.ChangeEnemiesLevel(_currentEnemyLevel);
        _currentVehicleData = Instantiate(_vehicleDatas[0]);
        //_currentWeaponData = Instantiate(_weaponDatas[0]);

        _currentWeapon = _originalWeapons[0];

    }

    void IncreaseEnemyLevel()
    {
        if (_currentEnemyLevel != _enemyTirs.Last())
        {
            int index = _enemyTirs.FindIndex(element => element == _currentEnemyLevel);
            index++;
            _currentEnemyLevel = _enemyTirs[index];
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
            t += Time.deltaTime / _increaseEnemyPowerDelay;
            _difficultSlider.value = Mathf.Lerp(startSliderValue, endSliderValue, t);
            yield return null;
        }
        IncreaseEnemyPower();
    }

    void IncreaseEnemyPower()
    {
        _enemyPowerLevel++;
        if (_enemyPowerLevel > _difficultCount && _currentEnemyLevel != _enemyTirs.Last())
        {
            _enemyPowerLevel = 1;
            _difficultSlider.value = 0;
            _enemyDmgMod = _startEnemyDmgMod;
            _enemyHpMod = _startEnemyHpMod;
            IncreaseEnemyLevel();
        }
        else if (_enemyPowerLevel > _difficultCount && _currentEnemyLevel == _enemyTirs.Last())
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
        if (_killedEnemiesCount % _killedCountForLvlUp == 0)
        {
            OnPlayerLvlUp();
        }
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

    void OnPlayerLvlUp()
    {
        List<UpgradeCardData> randomCards = new();
        List<UpgradeCardData> upgradeCardsCopy = new(_upgradeCardsData);

        while (randomCards.Count < 3)
        {
            int randomIndex = UnityEngine.Random.Range(0, upgradeCardsCopy.Count);
            randomCards.Add(upgradeCardsCopy[randomIndex]);
            upgradeCardsCopy.RemoveAt(randomIndex);
        }

        SurviveModeUpgradePanel.Instance.ConfigPanel(randomCards);
    }

    public void OnSelectUpgradeCard(UpgradeCardData upgradeCardData)
    {
        SMWeaponData newWeaponData = _currentWeaponData;




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
                //case (CharacteristicsName.VehicleHullHP):

                //    break;
                //case (CharacteristicsName.VehicleShieldHP):

                //    break;
                //case (CharacteristicsName.VehicleShieldRegRate):

                //    break;

        }
        _currentWeaponData = newWeaponData;

        if (_currentWeapon is ProjectileWeaponNotMiniGun weapon)
        {
            weapon.TEMPSetValues(_currentWeaponData);
        }
    }

}
[Serializable]
public struct SMWeaponData
{
    public float kineticDamage;
    public float energyDamage;
    public float fireRate;
    public int magCapacity;
    public float reloadTime;
}

[Serializable]
public struct UpgradeCardData
{
    [SerializeField] string _upgradeText;
    [SerializeField] CharacteristicsName _characteristicsName;
    [SerializeField] float _changeValue;

    public readonly string UpgradeText => _upgradeText;
    public readonly CharacteristicsName CharacteristicsName => _characteristicsName;
    public readonly float ChangeValue => _changeValue;
}