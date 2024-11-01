using System.Collections;
using UnityEngine;

public class InRaidManager : MonoBehaviour
{
    public static InRaidManager Instance;

    [SerializeField] MeshRenderer _mainRoadRenderer;
    [SerializeField] float _spawnNewEnemyDelay;
    [SerializeField] float _spawnNewEnemyRepitRate;
    [SerializeField] int _maxEnemyCountInRaid;

    float _worldMoveSpeed = 0f;
    bool _onRaid = false;
    int _killedEnemiesCount = 0;
    int _enemyTotalCountOnLevel = 0;


    public float PlayerMoveSpeed => _worldMoveSpeed;
    public Transform MainRoadTransform => _mainRoadRenderer.transform;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _onRaid = false;
        StopAllCoroutines();
        _worldMoveSpeed = 0;

        OldHangarManager.Instance.OnPlayerEndRaid();
        UILevelStatistic.Instance.Init();
        EnemySpawner.Instance.Init(_maxEnemyCountInRaid, _spawnNewEnemyDelay, _spawnNewEnemyRepitRate);
    }


    void Update()
    {
        MoveMainRoad();
    }

    void MoveMainRoad()
    {
        _mainRoadRenderer.material.mainTextureOffset += GameConfig.Instance.MoveRoadMod * _worldMoveSpeed * Time.deltaTime * Vector2.left;
    }

    public void OnPlayerStartRaid()
    {
        ConfigureDataOnStartRaid();
        PlayerVehicleManager.Instance.GetVehicleMovingData(out float startMoveDelay, out float fullSpeed, out float reachFullSpeedDuration);

        PlayerVehicleManager.Instance.OnPlayerStartRaid();
        OldHangarManager.Instance.OnPlayerStartRaid(startMoveDelay);
        StartCoroutine(AccelerationMoveSpeed(startMoveDelay, fullSpeed, reachFullSpeedDuration));
        EnemySpawner.Instance.OnPlayerStartRaid();
        MetricaSender.SendLevelStatus(LevelStatus.Start);
    }

    void ConfigureDataOnStartRaid()
    {
        Cursor.visible = false;

        _onRaid = true;
        UILevelInfo selectedLevelInfo = LevelManager.Instance.GetSelectedLevelinfo();
        int bossCount = selectedLevelInfo.LevelParameters.Boss == null ? 0 : 1;
        _enemyTotalCountOnLevel = selectedLevelInfo.LevelParameters.GetTotalSimpleEnemyCount() + bossCount;
        _killedEnemiesCount = 0;
    }
    public void OnPlayerEndRaid()
    {
        Cursor.visible = true;
        _onRaid = false;
        StopAllCoroutines();
        _worldMoveSpeed = 0;
        OldHangarManager.Instance.OnPlayerEndRaid();
        EnemySpawner.Instance.OnPlayerEndRaid();
    }

    IEnumerator AccelerationMoveSpeed(float startMoveDelay, float speed, float reachSpeedDuration)
    {
        yield return new WaitForSeconds(startMoveDelay);
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / reachSpeedDuration;
            _worldMoveSpeed = Mathf.Lerp(0, speed, t);
            yield return null;
        }
    }

    public void OnEnemyObjectDestroyed(EnemyVehicleManager enemy)
    {
        if (!_onRaid) return;
        EnemySpawner.Instance.OnEnemyObjectDestroyed(enemy);
    }

    public void OnPlayerKillEnemy()
    {
        _killedEnemiesCount++;

        if (_killedEnemiesCount >= _enemyTotalCountOnLevel)
        {
            _onRaid = false;
            MetricaSender.SendLevelStatus(LevelStatus.Done);
            LevelManager.Instance.UnlockNextLevel();
            FinishLevelManager.Instance.OnFinishLevel(isSuccessfully: true);
        }
        else
        {
            EnemySpawner.Instance.OnPlayerKillEnemy();
        }
    }
    public void OnEnemyEscaped(EnemyVehicleManager enemy)
    {
        if (!_onRaid) return;
        EnemySpawner.Instance.OnEnemyObjectDestroyed(enemy);
        OnPlayerKillEnemy();
    }

    public void OnPLayerDie()
    {
        StartCoroutine(ChangeSpeedOnDie());
        EnemySpawner.Instance.OnPLayerDie();
        MetricaSender.SendLevelStatus(LevelStatus.Failed);
    }

    IEnumerator ChangeSpeedOnDie()
    {
        float t = 0;
        float mod;
        float lastSpeed = _worldMoveSpeed;

        while (_worldMoveSpeed > 0)
        {
            t += Time.deltaTime / GameConfig.Instance.TimeForChangeSpeed;
            mod = Mathf.Lerp(1, 0, t);
            _worldMoveSpeed = lastSpeed * mod;
            yield return null;
        }
    }
}