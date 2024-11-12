using System.Collections;
using UnityEngine;

public class InRaidManager : MonoBehaviour
{
    public static InRaidManager Instance;

    [SerializeField] MeshRenderer _mainRoadRenderer;
    [SerializeField] float _speedMod;
    //[SerializeField] MeshRenderer _BG;
    [SerializeField] float _spawnNewEnemyDelay;
    [SerializeField] float _spawnNewEnemyRepitRate;
    [SerializeField] int _maxEnemyCountInRaid;
    //[SerializeField] float _mod;

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
        UIComboCounterManager.Instance.Init();
        FinishLevelManager.Instance.Init();
    }


    void Update()
    {
        MoveMainRoad();
    }

    void MoveMainRoad()
    {
        _mainRoadRenderer.material.mainTextureOffset += _speedMod * _worldMoveSpeed * Time.deltaTime * Vector2.left;
        //_BG.material.mainTextureOffset -= GameConfig.Instance.MoveRoadMod * _worldMoveSpeed * Time.deltaTime * Vector2.left / _mod;
    }

    public void OnPlayerStartRaid()
    {
        ConfigureDataOnStartRaid();
        PlayerVehicleManager.Instance.GetVehicleMovingData(out float startMoveDelay, out float fullSpeed, out float reachFullSpeedDuration);

        PlayerVehicleManager.Instance.OnPlayerStartRaid();
        OldHangarManager.Instance.OnPlayerStartRaid(startMoveDelay);
        StartCoroutine(AccelerationMoveSpeed(startMoveDelay, fullSpeed, reachFullSpeedDuration));
        EnemySpawner.Instance.OnPlayerStartRaid();
        UIComboCounterManager.Instance.OnPlayerStartRaid();
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
        UIComboCounterManager.Instance.OnPlayerEndRaid();
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
    public void OnEnemyEscaped(EnemyVehicleManager enemy)
    {
        if (!_onRaid) return;
        EnemySpawner.Instance.OnEnemyObjectDestroyed(enemy);
        CheckRaidCompleteStatus();
    }

    public void OnPlayerKillEnemy()
    {
        UIComboCounterManager.Instance.OnEnemyKilled();
        CheckRaidCompleteStatus();
    }

    void CheckRaidCompleteStatus()
    {
        _killedEnemiesCount++;

        if (_killedEnemiesCount >= _enemyTotalCountOnLevel)
        {
            _onRaid = false;
            LevelManager.Instance.UnlockNextLevel();
            FinishLevelManager.Instance.OnFinishLevel(isSuccessfully: true);
            SaveLoadManager.Instance.SaveData();
        }
        else
        {
            EnemySpawner.Instance.OnPlayerKillEnemy();
        }
    }
     

    public void OnPLayerDie()
    {
        StartCoroutine(ChangeSpeedOnDie());
        EnemySpawner.Instance.OnPLayerDie();
        FinishLevelManager.Instance.OnFinishLevel(isSuccessfully: false);
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