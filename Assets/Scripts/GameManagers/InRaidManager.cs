using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InRaidManager : MonoBehaviour
{
    public static InRaidManager Instance;

    [SerializeField] MeshRenderer _mainRoadRenderer;
    [SerializeField] float _speedMod;
    [SerializeField] float _spawnNewEnemyDelay;
    [SerializeField] float _spawnNewEnemyRepitRate;
    [SerializeField] int _maxEnemyCountInRaid;

    [SerializeField] Image _totalBlackoutImage;
    [SerializeField] float _blackoutDelay;
    [SerializeField] float _blackoutPause;
    [SerializeField] float _blackoutDuration;


    UILevelInfo _selectedLevelInfo;
    bool _selectedLevelCompleteStatus = false;
    float _worldMoveSpeed = 0f;
    bool _onRaid = false;
    int _killedEnemiesCount = 0;
    int _escapedEnemiesCount = 0;
    int _enemyTotalCountOnLevel = 0;


    public float PlayerMoveSpeed => _worldMoveSpeed;
    public Transform MainRoadTransform => _mainRoadRenderer.transform;

    public UILevelInfo SelectedLevelInfo => _selectedLevelInfo;


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
        _totalBlackoutImage.gameObject.SetActive(false);

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
    }

    public void OnPlayerStartRaid()
    {
        ConfigureDataOnStartRaid();
        PlayerVehicleManager.Instance.GetVehicleMovingData(out float startMoveDelay, out float fullSpeed, out float reachFullSpeedDuration);

        PlayerVehicleManager.Instance.OnPlayerStartRaid();
        OldHangarManager.Instance.OnPlayerStartRaid(startMoveDelay);
        StartCoroutine(AccelerationMoveSpeed(startMoveDelay, fullSpeed, reachFullSpeedDuration));
        UIComboCounterManager.Instance.OnPlayerStartRaid();
        MetricaSender.SendLevelStatus(_selectedLevelInfo.LevelParameters.LevelName, LevelStatus.Start);

        StartCoroutine(StartRaidLogic());
        
    }

    IEnumerator StartRaidLogic()
    {
        AudioManager.Instance.StartEngineSoundLogic(_blackoutDelay, _blackoutDuration, _blackoutPause, _blackoutDuration);
        yield return new WaitForSeconds(_blackoutDelay);
        float t = 0;
        _totalBlackoutImage.gameObject.SetActive(true);
        while (t <= 1)
        {
            t += Time.deltaTime / _blackoutDuration;
            Color color = Color.Lerp(Color.clear, Color.black, t);
            _totalBlackoutImage.color = color;
            yield return null;
        }
        AudioManager.Instance.ToggleMusic(true);
        OldHangarManager.Instance.DisableGarage();
        yield return new WaitForSeconds(_blackoutPause);
        
        t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / _blackoutDuration;
            Color color = Color.Lerp(Color.black, Color.clear, t);
            _totalBlackoutImage.color = color;
            yield return null;
        }
        _totalBlackoutImage.gameObject.SetActive(false);
        EnemySpawner.Instance.OnPlayerStartRaid();
    }

    void ConfigureDataOnStartRaid()
    {
        Cursor.visible = false;

        _onRaid = true;
        _selectedLevelInfo = LevelManager.Instance.GetSelectedLevelinfo();
        _selectedLevelCompleteStatus = false;
        int bossCount = _selectedLevelInfo.LevelParameters.Boss == null ? 0 : 1;
        _enemyTotalCountOnLevel = _selectedLevelInfo.LevelParameters.GetTotalSimpleEnemyCount() + bossCount;
        _killedEnemiesCount = 0;
        _escapedEnemiesCount = 0;
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
        AudioManager.Instance.ToggleMusic(false);
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
        _escapedEnemiesCount++;
        if (_selectedLevelInfo.LevelParameters.LevelName == "1-1") MetricaSender.KillEnemyOnFirstLevel(_selectedLevelInfo.LevelParameters.LevelName, _escapedEnemiesCount, LevelEnemyStatus.Escaped);
        
        EnemySpawner.Instance.OnEnemyObjectDestroyed(enemy);
        CheckRaidCompleteStatus();
    }

    public void OnPlayerKillEnemy()
    {
        _killedEnemiesCount++;
        if (_selectedLevelInfo.LevelParameters.LevelName == "1-1") MetricaSender.KillEnemyOnFirstLevel(_selectedLevelInfo.LevelParameters.LevelName, _killedEnemiesCount, LevelEnemyStatus.Killed);
       
        UIComboCounterManager.Instance.OnEnemyKilled();
        CheckRaidCompleteStatus();
    }

    void CheckRaidCompleteStatus()
    {
        if (_killedEnemiesCount + _escapedEnemiesCount >= _enemyTotalCountOnLevel)
        {
            _onRaid = false;
            _selectedLevelCompleteStatus = true;
            MetricaSender.SendLevelStatus(_selectedLevelInfo.LevelParameters.LevelName, LevelStatus.Done);
            LevelManager.Instance.UnlockNextLevel();
            FinishLevelManager.Instance.OnFinishLevel(_selectedLevelCompleteStatus);
            SaveLoadManager.Instance.SaveData();
        }
        else
        {
            EnemySpawner.Instance.OnKilledOrEscapedEnemy();
        }
    }


    public void OnPLayerDie()
    {
        _selectedLevelCompleteStatus = false;
        MetricaSender.SendLevelStatus(_selectedLevelInfo.LevelParameters.LevelName, LevelStatus.Failed);
        StartCoroutine(ChangeSpeedOnDie());
        EnemySpawner.Instance.OnPLayerDie();
        FinishLevelManager.Instance.OnFinishLevel(_selectedLevelCompleteStatus);
        SaveLoadManager.Instance.SaveData();
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