using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurviveModeDifficultManager
{
    ModeDifficult _difficultData;
    LevelParameters _SMLevelParameters;
    EnemyLevel _currentEnemyLevel;
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
        _timerValue = 0;
        SurviveModeDifficultProgress.Instance.AddDifficultScull();
    }

    public void IncreaseEnemyPowerInTime()
    {
        _timerValue += Time.deltaTime;

        _difficultData.enemyDmgMod +=_difficultData.increaseEnemyPowerMod * Time.deltaTime;
        _difficultData.enemyHpMod +=_difficultData.increaseEnemyPowerMod * Time.deltaTime;
        SurviveModeDifficultProgress.Instance.UpdateSliderValue(_timerValue / _difficultData.enemyIncreaseTirDelay);

        if (_timerValue >= _difficultData.enemyIncreaseTirDelay)
        {
            _timerValue = 0;
            SurviveModeDifficultProgress.Instance.AddDifficultScull();
            TryIncreaseEnemyTir();
        }

        TESTSurviveModStatistics.Instance.UpdateEnemyData(_difficultData.enemyIncreaseTirDelay - _timerValue, _difficultData.enemyDmgMod, _currentEnemyLevel);
    }

    void TryIncreaseEnemyTir()
    {
        if (_currentEnemyLevel != _enemyTirs.Last())
        {
            _difficultData.enemyDmgMod = _startEnemyDmgMod;
            _difficultData.enemyHpMod = _startEnemyHpMod;
            int index = _enemyTirs.FindIndex(element => element == _currentEnemyLevel);
            index++;
            _currentEnemyLevel = _enemyTirs[index];
            _SMLevelParameters.ChangeEnemiesLevel(_currentEnemyLevel);
        }
    }
}
