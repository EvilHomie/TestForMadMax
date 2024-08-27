
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RaidManager : MonoBehaviour
{
    public static RaidManager Instance;

    [SerializeField] Image _speedSliderFillImage;
    [SerializeField] MeshRenderer _mainRoadRenderer;
    [SerializeField] List<EnemySpawnLines> enemySpawnLines;
    [SerializeField] List<int> _reservedSpawnLinesNumbers;
    [SerializeField] List<int> _freeSpawnLinesNumbers;


    List<EnemyVehicleManager> _enemiesList = new();
    UILevelInfo _selectedLevelInfo;

    bool allLinesReserved = false;
    float _playerMoveSpeed = 0f;
    Coroutine _UpdateSpeedCoroutine;
    bool _onRaid = false;
    int _spawnedEnemyCount = 0;
    int _killedEnemyCount = 0;
    public float PlayerMoveSpeed => _playerMoveSpeed;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void Update()
    {
        MoveMainRoad();
    }

    void MoveMainRoad()
    {
        _mainRoadRenderer.material.mainTextureOffset += GameConfig.Instance.MoveRoadMod * _playerMoveSpeed * Time.deltaTime * Vector2.left;
    }


    public void ChangeSpeedWhileInRaid(float sliderValue)
    {
        float newSpeed = GameConfig.Instance.MinPlayerSpeed + sliderValue * GameConfig.Instance.MinPlayerSpeed;

        if (_UpdateSpeedCoroutine != null)
        {
            StopCoroutine(_UpdateSpeedCoroutine);
            _UpdateSpeedCoroutine = StartCoroutine(LerpSpeed(newSpeed, sliderValue));
        }
        else
        {
            _UpdateSpeedCoroutine = StartCoroutine(LerpSpeed(newSpeed, sliderValue));
        }
    }

    public void OnPlayerStartRaid(float startMoveDelay, float speed, float reachSpeedDuration)
    {
        _onRaid = true;
        _selectedLevelInfo = LevelManager.Instance.GetSelectedLevelinfo();
        _spawnedEnemyCount = 0;
        _killedEnemyCount = 0;
        _speedSliderFillImage.fillAmount = 0;
        _UpdateSpeedCoroutine = StartCoroutine(StartLerpSpeed(startMoveDelay, speed, reachSpeedDuration));
        InvokeRepeating(nameof(SpawnEnemy), 5, 5);
    }
    public void OnPlayerEndRaid()
    {
        _onRaid = false;
        StopAllCoroutines();
        CancelInvoke();
        _playerMoveSpeed = 0;
        _UpdateSpeedCoroutine = null;


        _reservedSpawnLinesNumbers.Clear();
        _freeSpawnLinesNumbers.Clear();

        foreach (var line in enemySpawnLines)
        {
            _freeSpawnLinesNumbers.Add(line.lineIndex);
        }

        foreach (var enemy in _enemiesList)
        {
            Destroy(enemy.gameObject);
        }
        _enemiesList.Clear();
    }


    IEnumerator LerpSpeed(float newSpeed, float sliderValue)
    {
        float t = 0;
        float startFillValue = _speedSliderFillImage.fillAmount;
        float _lastSpeed = _playerMoveSpeed;
        while (t <= 1)
        {
            t += Time.deltaTime / GameConfig.Instance.TimeForChangeSpeed;
            _playerMoveSpeed = Mathf.Lerp(_lastSpeed, newSpeed, t);
            _speedSliderFillImage.fillAmount = Mathf.Lerp(startFillValue, sliderValue, t);
            yield return null;
        }
        _UpdateSpeedCoroutine = null;
    }

    IEnumerator StartLerpSpeed(float startMoveDelay, float speed, float reachSpeedDuration)
    {
        yield return new WaitForSeconds(startMoveDelay);
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / reachSpeedDuration;
            _playerMoveSpeed = Mathf.Lerp(0, speed, t);
            yield return null;
        }
        _UpdateSpeedCoroutine = null;
    }

    void SpawnEnemy()
    {
        if (_freeSpawnLinesNumbers.Count == 0) return;
        if(_spawnedEnemyCount >= _selectedLevelInfo.EnemyCount) return;

        int randomLineIndex = UnityEngine.Random.Range(0, _freeSpawnLinesNumbers.Count);
        int freeLineNumber = _freeSpawnLinesNumbers[randomLineIndex];

        int randomEnemyIndex = UnityEngine.Random.Range(0, LevelManager.Instance.EnemyList.Count);
        EnemyVehicleManager enemyPF = LevelManager.Instance.EnemyList[randomEnemyIndex];

        int randomPosIndex = UnityEngine.Random.Range(0, enemySpawnLines[freeLineNumber].spawnPositions.Count);
        Vector3 spwanPos = enemySpawnLines[freeLineNumber].spawnPositions[randomPosIndex];

        EnemyVehicleManager enemy = Instantiate(enemyPF, spwanPos, enemyPF.transform.rotation);
        _enemiesList.Add(enemy);
        _spawnedEnemyCount ++;
        enemy.ReservedLineNumber = freeLineNumber;

        _freeSpawnLinesNumbers.Remove(freeLineNumber);
        _reservedSpawnLinesNumbers.Add(freeLineNumber);

        //if (freeSpawnLinesNumbers.Count == 0)
        //    allLinesReserved = true;

        Debug.LogWarning($"ENEMY IS SPAWNED AT LINE {freeLineNumber}");
    }

    public void OnEnemyDestroyed(EnemyVehicleManager enemy, int reservedLineNumber)
    {
        if(!_onRaid) return;
        _enemiesList.Remove(enemy);
        _freeSpawnLinesNumbers.Add(reservedLineNumber);
        _reservedSpawnLinesNumbers.Remove(reservedLineNumber);

        _killedEnemyCount++;
        if (_killedEnemyCount >= _selectedLevelInfo.EnemyCount)
        {
            LevelManager.Instance.UnlockNextLevel();
            GameManager.Instance.OnPlayerKillAllEnemy();
        }
    }
}

[Serializable]
public struct EnemySpawnLines
{
    public int lineIndex;
    public List<Vector3> spawnPositions;
}