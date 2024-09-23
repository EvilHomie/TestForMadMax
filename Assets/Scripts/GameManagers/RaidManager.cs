using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] int _spawnNewEnemyDelay;
    [SerializeField] int _spawnNewEnemyRepitRate;

    List<EnemyVehicleManager> _enemiesList = new();
    UILevelInfo _selectedLevelInfo;

    float _playerMoveSpeed = 0f;
    Coroutine _UpdateSpeedCoroutine;
    bool _onRaid = false;
    int _spawnedSimpleEnemyCount = 0;
    int _killedSimpleEnemyCount = 0;
    bool _bossIsSpawned = false;


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
        CancelInvoke();
        _onRaid = true;
        _bossIsSpawned = false;
        _selectedLevelInfo = LevelManager.Instance.GetSelectedLevelinfo();
        _spawnedSimpleEnemyCount = 0;
        _killedSimpleEnemyCount = 0;
        _speedSliderFillImage.fillAmount = 0;
        _UpdateSpeedCoroutine = StartCoroutine(StartLerpSpeed(startMoveDelay, 10, reachSpeedDuration));    //отключил логику управления скорости хардкод в цифре 10
        InvokeRepeating(nameof(SpawnEnemy), _spawnNewEnemyDelay, _spawnNewEnemyRepitRate);
        MetricaSender.SendLevelStatus(_selectedLevelInfo, LevelStatus.Start);
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
        if (_spawnedSimpleEnemyCount < _selectedLevelInfo.EnemyCount)
        {
            SpawnSimpleEnemy();
        }
    }

    public void OnEnemyObjectDestroyed(EnemyVehicleManager enemy, int reservedLineNumber)
    {
        if (!Application.isPlaying) return;
        if (!_onRaid) return;
        _enemiesList.Remove(enemy);
        _freeSpawnLinesNumbers.Add(reservedLineNumber);
        _reservedSpawnLinesNumbers.Remove(reservedLineNumber);

        //_killedSimpleEnemyCount++;

        if (_bossIsSpawned || _reservedSpawnLinesNumbers.Count > 0)
        {
            return;
        }

        if (_killedSimpleEnemyCount >= _selectedLevelInfo.EnemyCount)
        {
            if (_selectedLevelInfo.BossEnemyVehicle != null)
            {
                SpawnBoss();
            }
        }
    }
    public void OnPlayerKillEnemy()
    {
        _killedSimpleEnemyCount++;

        int bossCount = _selectedLevelInfo.BossEnemyVehicle == null ? 0 : 1;
        if (_killedSimpleEnemyCount >= _selectedLevelInfo.EnemyCount + bossCount)
        {
            MetricaSender.SendLevelStatus(_selectedLevelInfo, LevelStatus.Done);
            LevelManager.Instance.UnlockNextLevel();
            FinishLevelManager.Instance.OnFinishLevel(isSuccessfully: true);
        }
    }

    void SpawnSimpleEnemy()
    {
        //UnityEngine.Random.InitState();
        int randomLineIndex = UnityEngine.Random.Range(0, _freeSpawnLinesNumbers.Count);
        int freeLineNumber = _freeSpawnLinesNumbers[randomLineIndex];

        int randomPosIndex = UnityEngine.Random.Range(0, enemySpawnLines[freeLineNumber].spawnPositions.Count);
        Vector3 spwanPos = enemySpawnLines[freeLineNumber].spawnPositions[randomPosIndex];

        int randomEnemyIndex = UnityEngine.Random.Range(0, _selectedLevelInfo.EnemyList.Count);
        EnemyVehicleManager enemyPF = _selectedLevelInfo.EnemyList[randomEnemyIndex];

        EnemyVehicleManager enemy = Instantiate(enemyPF, spwanPos, enemyPF.transform.rotation);
        _enemiesList.Add(enemy);
        _spawnedSimpleEnemyCount++;
        enemy.ReservedLineNumber = freeLineNumber;

        _freeSpawnLinesNumbers.Remove(freeLineNumber);
        _reservedSpawnLinesNumbers.Add(freeLineNumber);
        //Debug.LogWarning($"SIMPLE ENEMY IS SPAWNED AT LINE {freeLineNumber}");
    }

    void SpawnBoss()
    {
        int randomLineIndex = UnityEngine.Random.Range(0, _freeSpawnLinesNumbers.Count);
        int freeLineNumber = _freeSpawnLinesNumbers[randomLineIndex];

        int randomPosIndex = UnityEngine.Random.Range(0, enemySpawnLines[freeLineNumber].spawnPositions.Count);
        Vector3 spwanPos = enemySpawnLines[freeLineNumber].spawnPositions[randomPosIndex];

        EnemyVehicleManager enemy = Instantiate(_selectedLevelInfo.BossEnemyVehicle, spwanPos, _selectedLevelInfo.BossEnemyVehicle.transform.rotation);

        _enemiesList.Add(enemy);
        enemy.ReservedLineNumber = freeLineNumber;

        _freeSpawnLinesNumbers.Remove(freeLineNumber);
        _reservedSpawnLinesNumbers.Add(freeLineNumber);

        _bossIsSpawned = true;
        //Debug.LogWarning($"BOSS IS SPAWNED AT LINE {freeLineNumber}");
    }

    public void OnPLayerDie()
    {
        StartCoroutine(ChangeSpeedOnDie());
        foreach (var enemy in _enemiesList)
        {
            enemy.OnPlayerDie();
        }
        MetricaSender.SendLevelStatus(_selectedLevelInfo, LevelStatus.Failed);
    }

    IEnumerator ChangeSpeedOnDie()
    {
        float t = 0;
        float mod;
        float lastSpeed = _playerMoveSpeed;

        while (_playerMoveSpeed > 0)
        {
            t += Time.deltaTime / GameConfig.Instance.TimeForChangeSpeed;
            mod = Mathf.Lerp(1, 0, t);
            _playerMoveSpeed = lastSpeed * mod;
            yield return null;
        }
    }
}

[Serializable]
public struct EnemySpawnLines
{
    public int lineIndex;
    public List<Vector3> spawnPositions;
}