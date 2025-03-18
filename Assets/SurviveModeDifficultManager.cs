using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurviveModeDifficultManager
{
    ModeDifficult _difficultData;
    LevelParameters _SMLevelParameters;
    EnemyLevel _currentEnemyLevel;
    int _difficultsCount = 5;
    float _enemyPowerLevel = 1;
    List<EnemyLevel> _enemyTirs;

    readonly float  _startEnemyDmgMod;
    readonly float _startEnemyHpMod;
    float _timerValue;

    public ModeDifficult ModeDifficult => _difficultData;
    public LevelParameters LevelParameters => _SMLevelParameters;


    public SurviveModeDifficultManager(ModeDifficult difficultData, LevelParameters sMLevelParameters)
    {
        _enemyTirs = Enum.GetValues(typeof(EnemyLevel)).Cast<EnemyLevel>().ToList();

        _difficultData = difficultData;
        _SMLevelParameters = sMLevelParameters;
        _startEnemyDmgMod = difficultData.enemyDmgMod;
        _startEnemyHpMod = difficultData.enemyHpMod;
    }

    public void OnStartSurviveMode()
    {
        //_difficultsCount = SurviveModeDifficultProgress.Instance.DifficultLevelsAmount;
        _difficultData.enemyHpMod = _startEnemyHpMod;
        _difficultData.enemyDmgMod = _startEnemyDmgMod;
        _currentEnemyLevel = EnemyLevel.SuperEasy;
        _SMLevelParameters.ChangeEnemiesLevel(_currentEnemyLevel);
        _enemyPowerLevel = 1;        
        _timerValue = 0;
    }

    public void IncreaseEnemyPowerInTime()
    {
        _timerValue += Time.deltaTime;

        if (_timerValue < _difficultData.enemyPowerUpDelay)
        {
            SurviveModeDifficultProgress.Instance.UpdateSliderValue(Mathf.InverseLerp(1, 6, _enemyPowerLevel + _timerValue / _difficultData.enemyPowerUpDelay));
            
        }
        else
        {
            _timerValue = 0;
            IncreaseEnemyPower();
        }

        TESTSurviveModStatistics.Instance.UpdateEnemyData(_difficultData.enemyPowerUpDelay - _timerValue, _difficultData.enemyDmgMod, _currentEnemyLevel);
    }

    void IncreaseEnemyPower()
    {
        _enemyPowerLevel++;
        if (_enemyPowerLevel > _difficultsCount && _currentEnemyLevel != _enemyTirs.Last())
        {
            _enemyPowerLevel = 1;
            SurviveModeDifficultProgress.Instance.UpdateSliderValue(0);
            _difficultData.enemyDmgMod = _startEnemyDmgMod;
            _difficultData.enemyHpMod = _startEnemyHpMod;
            IncreaseEnemyTir();
            return;
        }
        else if (_enemyPowerLevel > _difficultsCount && _currentEnemyLevel == _enemyTirs.Last())
        {
            SurviveModeDifficultProgress.Instance.AddDifficultScull();            
        }

        _difficultData.enemyDmgMod += _startEnemyDmgMod * _difficultData.increaseEnemyPowerValue;
        _difficultData.enemyHpMod += _startEnemyHpMod * _difficultData.increaseEnemyPowerValue;
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
}
