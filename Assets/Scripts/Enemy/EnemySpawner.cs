using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField] List<EnemySpawnPos> _enemySpawnPositions;

    int _lastSpawnPosIndex = 0;

    List<EnemyVehicleManager> _enemiesInRaidList = new();

    float _spawnNewEnemyDelay;
    float _spawnNewEnemyRepitRate;
    int _maxEnemyCountInRaidInTime = 0;


    bool _bossIsSpawned = false;
    int _totalEnemiesCount;
    int _totalDestroyedEnemyCount = 0;
    int _waveEnemiesCount;
    int _waveSpawnedEnemyCount = 0;
    int _waveDestroyedEnemyCount = 0;

    int _currentWaveNumber = 0;
    int _totalWaveCount;
    //int _spawnedWavesCount = 0;
    LevelParameters _levelParametersCopy;
    List<WaveEnemie> _selectedWaveSimpleEnemies;



    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init(int maxEnemyCountInRaid, float spawnNewEnemyDelay, float spawnNewEnemyRepitRate)
    {
        UIWaveIsApproachingPanel.Instance.Init();
        CancelInvoke();
        _maxEnemyCountInRaidInTime = maxEnemyCountInRaid;
        _spawnNewEnemyDelay = spawnNewEnemyDelay;
        _spawnNewEnemyRepitRate = spawnNewEnemyRepitRate;

        DestroySpawnedEnemies();
    }

    public void OnPlayerStartRaid()
    {

        StopAllCoroutines();
        CancelInvoke();
        ConfigureDataOnStartRaid();

        StartCoroutine(SpawnFirstWaveWithDelay(_spawnNewEnemyDelay));
    }
    public void OnPlayerEndRaid()
    {
        UIWaveIsApproachingPanel.Instance.HideText();
        StopAllCoroutines();
        CancelInvoke();
        DestroySpawnedEnemies();
        _enemySpawnPositions.ForEach(position => { position.ResetStatusImmediately(); });
    }

    public void SpawnNewWave()
    {
        UIWaveIsApproachingPanel.Instance.ShowWaveText();
        CancelInvoke();
        _waveSpawnedEnemyCount = 0;
        _waveDestroyedEnemyCount = 0;

        if (_totalWaveCount != 0)
        {
            _selectedWaveSimpleEnemies = _levelParametersCopy.GetWaveData(_currentWaveNumber).waveEnemies;
            _waveEnemiesCount = _levelParametersCopy.GetWaveEnemyCount(_currentWaveNumber);
            InvokeRepeating(nameof(TrySpawnSimpleEnemy), 0, _spawnNewEnemyRepitRate);

        }
        else
        {
            if (_levelParametersCopy.Boss != null)
            {
                SpawnBoss();
            }
        }
    }

    IEnumerator SpawnFirstWaveWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnNewWave();
    }


    void ConfigureDataOnStartRaid()
    {
        _currentWaveNumber = 1;
        //_spawnedWavesCount = 0;
        _waveSpawnedEnemyCount = 0;
        _waveDestroyedEnemyCount = 0;
        _totalDestroyedEnemyCount = 0;
        _levelParametersCopy = Instantiate(LevelManager.Instance.GetSelectedLevelinfo().LevelParameters);
        _bossIsSpawned = false;
        _totalWaveCount = _levelParametersCopy.WavesCount;
        _totalEnemiesCount = _levelParametersCopy.GetTotalSimpleEnemyCount();
    }

    void DestroySpawnedEnemies()
    {
        foreach (var enemy in _enemiesInRaidList)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
        _enemiesInRaidList.Clear();
    }


    void TrySpawnSimpleEnemy()
    {
        if (_enemiesInRaidList.Count >= _maxEnemyCountInRaidInTime) return;
        if (_waveSpawnedEnemyCount < _waveEnemiesCount)
        {
            SpawnSimpleEnemy();
        }
    }

    public void OnPlayerKillEnemy()
    {
        _waveDestroyedEnemyCount++;
        _totalDestroyedEnemyCount++;
        if (_waveDestroyedEnemyCount >= _waveEnemiesCount)
        {
            _currentWaveNumber++;
            if (_currentWaveNumber <= _totalWaveCount)
            {
                SpawnNewWave();
            }
            else
            {
                //Debug.LogWarning("")
                CancelInvoke();
            }
        }
    }

    public void OnEnemyObjectDestroyed(EnemyVehicleManager enemy)
    {
        _enemiesInRaidList.Remove(enemy);
        //_waveDestroyedEnemyCount++;
        //_totalDestroyedEnemyCount++;
        //if (_waveDestroyedEnemyCount >= _waveEnemiesCount)
        //{
        //    _currentWaveNumber++;
        //    if (_currentWaveNumber <= _totalWaveCount)
        //    {
        //        SpawnNewWave();
        //    }
        //    else
        //    {
        //        //Debug.LogWarning("")
        //        CancelInvoke();
        //    }
        //}

        if (_totalDestroyedEnemyCount >= _totalEnemiesCount && _enemiesInRaidList.Count == 0)
        {
            if (_bossIsSpawned)
            {
                return;
            }
            else
            {
                if (_levelParametersCopy.Boss != null)
                {
                    SpawnBoss();
                }
            }
        }
    }

    Vector3 GetSpawnPos()
    {
        List<EnemySpawnPos> accessibleEnemySpawnPositions = _enemySpawnPositions.FindAll(pos => !pos.ReservedStatus);
        if (OldHangarManager.Instance.gameObject.activeSelf)
        {
            accessibleEnemySpawnPositions = accessibleEnemySpawnPositions.FindAll(pos => pos.transform.position.x > 0);
        }
        int randomPosIndex = Random.Range(0, accessibleEnemySpawnPositions.Count);
        EnemySpawnPos spwanPos = accessibleEnemySpawnPositions[randomPosIndex];

        spwanPos.ChangeStatus();
        return spwanPos.transform.position;
    }

    void SpawnSimpleEnemy()
    {
        //List<EnemySpawnPos> accessibleEnemySpawnPositions = null;
        //if (OldHangarManager.Instance.gameObject.activeSelf)
        //{
        //    accessibleEnemySpawnPositions = _enemySpawnPositions.FindAll(pos => pos.transform.position.x > 0);
        //}
        //else
        //{
        //    accessibleEnemySpawnPositions = _enemySpawnPositions;
        //}

        //int randomPosIndex = Random.Range(0, accessibleEnemySpawnPositions.Count);

        //if (randomPosIndex == _lastSpawnPosIndex)
        //{
        //    if (randomPosIndex == accessibleEnemySpawnPositions.Count - 1)
        //    {
        //        randomPosIndex--;
        //    }
        //    if (randomPosIndex == 0)
        //    {
        //        randomPosIndex++;
        //    }
        //}
        //_lastSpawnPosIndex = randomPosIndex;
        //Vector3 spwanPos = accessibleEnemySpawnPositions[randomPosIndex].transform.position;

        Vector3 spwanPos = GetSpawnPos();
        int randomIndex = Random.Range(0, _selectedWaveSimpleEnemies.Count);
        WaveEnemie selectedWave = _selectedWaveSimpleEnemies[randomIndex];
        EnemyType enemyType = selectedWave.enemyType;

        selectedWave.count--;
        if (selectedWave.count == 0)
        {
            _selectedWaveSimpleEnemies.Remove(selectedWave);
        }

        EnemyVehicleManager enemyPF = EnemiesCollection.Instance.GetEnemyPF(enemyType, _levelParametersCopy.EnemyLevel);

        EnemyVehicleManager enemy = Instantiate(enemyPF, spwanPos, enemyPF.transform.rotation);
        _enemiesInRaidList.Add(enemy);
        _waveSpawnedEnemyCount++;
    }

    void SpawnBoss()
    {
        UIWaveIsApproachingPanel.Instance.ShowBossText();
        int randomPosIndex = Random.Range(0, _enemySpawnPositions.Count);
        Vector3 spwanPos = _enemySpawnPositions[randomPosIndex].transform.position;

        EnemyVehicleManager bossEnemy = Instantiate(_levelParametersCopy.Boss, spwanPos, _levelParametersCopy.Boss.transform.rotation);

        _enemiesInRaidList.Add(bossEnemy);

        _bossIsSpawned = true;
    }

    public void OnPLayerDie()
    {
        foreach (var enemy in _enemiesInRaidList)
        {
            enemy.OnPlayerDie();
        }
    }
}

