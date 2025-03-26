using UnityEngine;

public class SurviveModeEnemyHPSpawner : MonoBehaviour
{
    public static SurviveModeEnemyHPSpawner Instance;

    [SerializeField] Vector3 _startPos;
    [SerializeField] Vector3 _endPos;
    [SerializeField] GameObject _enemyHPPF;

    [SerializeField] float _spawnDelay;
    [SerializeField] float _restoreHpPercent;
    public Vector3 EndPos => _endPos;
    public float RestoreHpPercent => _restoreHpPercent;

    GameObject _spawnedEnemy;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    public void OnStartSurviveMode()
    {
        InvokeRepeating(nameof(SpawnHPEnemy), _spawnDelay, _spawnDelay);
    }
    public void OnStopSurviveMode()
    {
        CancelInvoke();
        if (_spawnedEnemy != null)
        {
            Destroy(_spawnedEnemy);
        }
    }

    void SpawnHPEnemy()
    {
        if (_spawnedEnemy != null) return;

        _spawnedEnemy = Instantiate(_enemyHPPF, _startPos, _enemyHPPF.transform.rotation);
    }
}
