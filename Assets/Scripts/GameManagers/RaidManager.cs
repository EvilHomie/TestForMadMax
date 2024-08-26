
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
    [SerializeField] List<int> reservedSpawnLinesNumbers;
    [SerializeField] List<int> freeSpawnLinesNumbers;


    List<EnemyVehicleManager> enemiesList = new();

    bool allLinesReserved = false;

    float _playerMoveSpeed = 0f;
    public float PlayerMoveSpeed => _playerMoveSpeed;

    Coroutine _UpdateSpeedCoroutine;

    bool _onRaid = false;

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


        reservedSpawnLinesNumbers.Clear();
        freeSpawnLinesNumbers.Clear();

        foreach (var line in enemySpawnLines)
        {
            freeSpawnLinesNumbers.Add(line.lineIndex);
        }

        foreach (var enemy in enemiesList)
        {
            Destroy(enemy.gameObject);
        }
        enemiesList.Clear();
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
        if (freeSpawnLinesNumbers.Count == 0) return;
        int randomLineIndex = UnityEngine.Random.Range(0, freeSpawnLinesNumbers.Count);
        int freeLineNumber = freeSpawnLinesNumbers[randomLineIndex];

        int randomEnemyIndex = UnityEngine.Random.Range(0, LevelManager.Instance.EnemyList.Count);
        EnemyVehicleManager enemyPF = LevelManager.Instance.EnemyList[randomEnemyIndex];

        int randomPosIndex = UnityEngine.Random.Range(0, enemySpawnLines[freeLineNumber].spawnPositions.Count);
        Vector3 spwanPos = enemySpawnLines[freeLineNumber].spawnPositions[randomPosIndex];

        EnemyVehicleManager enemy = Instantiate(enemyPF, spwanPos, enemyPF.transform.rotation);
        enemiesList.Add(enemy);
        enemy.ReservedLineNumber = freeLineNumber;

        freeSpawnLinesNumbers.Remove(freeLineNumber);
        reservedSpawnLinesNumbers.Add(freeLineNumber);

        if (freeSpawnLinesNumbers.Count == 0)
            allLinesReserved = true;

        Debug.LogWarning($"ENEMY IS SPAWNED AT LINE {freeLineNumber}");
    }

    public void OnEnemyDestroyed(EnemyVehicleManager enemy, int reservedLineNumber)
    {
        if(!_onRaid) return;
        enemiesList.Remove(enemy);
        freeSpawnLinesNumbers.Add(reservedLineNumber);
        reservedSpawnLinesNumbers.Remove(reservedLineNumber);
    }
}

[Serializable]
public struct EnemySpawnLines
{
    public int lineIndex;
    public List<Vector3> spawnPositions;
}