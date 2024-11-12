using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelParameters", menuName = "ScriptableObjects/LevelParameters")]
public class LevelParameters : ScriptableObject
{
    [SerializeField] string _levelName;
    [SerializeField] Sprite _levelImage;
    [SerializeField] EnemyLevel _enemiesLevel;
    [SerializeField] List<Wave> _waves;
    [Header("OPTIONAL")]
    [SerializeField] List<int> _blockedEnemiesLinesInGameZone;
    [SerializeField] EnemyVehicleManager _boss;
    public string LevelName => _levelName;
    public Sprite LevelImage => _levelImage;

    public List<int> BlockedEnemiesLinesInGameZone => _blockedEnemiesLinesInGameZone;
    public EnemyVehicleManager Boss => _boss;
    public EnemyLevel EnemyLevel => _enemiesLevel;
    public int WavesCount => _waves.Count;

    public int GetWaveEnemyCount(int waveNumber)
    {
        List<WaveEnemie> waveEnemies = _waves[waveNumber - 1].waveEnemies;
        return waveEnemies.Sum(wave => wave.count);
    }

    public int GetTotalSimpleEnemyCount()
    {
        List<WaveEnemie> waveEnemies = _waves.SelectMany(wave => wave.waveEnemies).ToList();
        return waveEnemies.Sum(enemy => enemy.count);
    }

    public Wave GetWaveData(int waveNumber)
    {
        return _waves[waveNumber - 1];
    }
}
