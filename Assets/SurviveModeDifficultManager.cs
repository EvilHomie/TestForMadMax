using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SurviveModeDifficultManager
{
    Slider _difficultSlider;
    Transform _difficultsColorContainer;
    GameObject _scullPF;
    RectTransform _scullsContainer;
    ModeDifficult _difficultData;
    LevelParameters _SMLevelParameters;
    EnemyLevel _currentEnemyLevel;
    int _difficultsCount;
    float _enemyPowerLevel = 1;
    List<EnemyLevel> _enemyTirs;

    readonly float  _startEnemyDmgMod;
    readonly float _startEnemyHpMod;
    float _timerValue;

    public ModeDifficult ModeDifficult => _difficultData;
    public LevelParameters LevelParameters => _SMLevelParameters;


    public SurviveModeDifficultManager(Slider difficultSlider, Transform difficultsColorContainer, GameObject scullPF, RectTransform scullsContainer, ModeDifficult difficultData, LevelParameters sMLevelParameters)
    {
        _difficultSlider = difficultSlider;
        _difficultsColorContainer = difficultsColorContainer;
        _scullPF = scullPF;
        _scullsContainer = scullsContainer;
        _difficultsCount = _difficultsColorContainer.childCount;
        _enemyTirs = Enum.GetValues(typeof(EnemyLevel)).Cast<EnemyLevel>().ToList();

        _difficultData = difficultData;
        _SMLevelParameters = sMLevelParameters;
        _startEnemyDmgMod = difficultData.enemyDmgMod;
        _startEnemyHpMod = difficultData.enemyHpMod;
    }

    public void OnStartMode()
    {
        _difficultData.enemyHpMod = _startEnemyHpMod;
        _difficultData.enemyDmgMod = _startEnemyDmgMod;
        _currentEnemyLevel = EnemyLevel.SuperEasy;
        _SMLevelParameters.ChangeEnemiesLevel(_currentEnemyLevel);
        _enemyPowerLevel = 1;
        _difficultSlider.value = 0;
        _timerValue = 0;
        

        foreach (Transform c in _scullsContainer)
        {
            UnityEngine.Object.Destroy(c.gameObject);
        }
        _difficultSlider.transform.parent.gameObject.SetActive(true);
    }

    public void IncreaseEnemyPowerInTime()
    {
        _timerValue += Time.deltaTime;

        if (_timerValue < _difficultData.enemyPowerUpDelay)
        {
            _difficultSlider.value = Mathf.InverseLerp(1, 5, _enemyPowerLevel + _timerValue / _difficultData.enemyPowerUpDelay);
        }
        else
        {
            _timerValue = 0;
            IncreaseEnemyPower();
        }

        TESTSurviveModStatistics.Instance.UpdateEnemyData(_difficultData.enemyPowerUpDelay - _timerValue, _difficultData.enemyDmgMod, _currentEnemyLevel);


        //float startSliderValue = _difficultSlider.value;
        //float endSliderValue = startSliderValue + 1f / _difficultsCount;
        //float t = 0;
        //while (t < 1)
        //{
        //    t += Time.deltaTime / _difficultData.enemyPowerUpDelay;
        //    _difficultSlider.value = Mathf.Lerp(startSliderValue, endSliderValue, t);
        //    yield return null;
        //}
        //IncreaseEnemyPower();
    }

    void IncreaseEnemyPower()
    {
        _enemyPowerLevel++;
        if (_enemyPowerLevel > _difficultsCount && _currentEnemyLevel != _enemyTirs.Last())
        {
            _enemyPowerLevel = 1;
            _difficultSlider.value = 0;
            _difficultData.enemyDmgMod = _startEnemyDmgMod;
            _difficultData.enemyHpMod = _startEnemyHpMod;
            IncreaseEnemyTir();
            return;
        }
        else if (_enemyPowerLevel > _difficultsCount && _currentEnemyLevel == _enemyTirs.Last())
        {
            UnityEngine.Object.Instantiate(_scullPF, _scullsContainer);
        }

        _difficultData.enemyDmgMod += _startEnemyDmgMod * _difficultData.increaseEnemyPowerValue;
        _difficultData.enemyHpMod += _startEnemyHpMod * _difficultData.increaseEnemyPowerValue;

        //StartCoroutine(IncreaseEnemyPowerInTime());
    }

    void IncreaseEnemyTir()
    {
        if (_currentEnemyLevel != _enemyTirs.Last())
        {
            int index = _enemyTirs.FindIndex(element => element == _currentEnemyLevel);
            index++;
            _currentEnemyLevel = _enemyTirs[index];
            _SMLevelParameters.ChangeEnemiesLevel(_currentEnemyLevel);
        }
    }

    public void OnDisableMode()
    {

        _difficultSlider.transform.parent.gameObject.SetActive(false);
    }
}
