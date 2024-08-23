using System.Collections.Generic;
using UnityEngine;

public class LevelConfig : MonoBehaviour
{
    public static LevelConfig Instance;

    [SerializeField] List<EnemyVehicleManager> enemyList;
    [SerializeField] int _enemyCount = 1;


    [SerializeField] float _enemySlideDistanceMod = 1;
    [SerializeField] float _enemyFireRateMod = 1;
    [SerializeField] float _enemyDmgMod = 1;
    [SerializeField] float _enemyHPMod = 1;

    public List<EnemyVehicleManager> EnemyList => enemyList;
    public int EnemyCount => _enemyCount;

    public float EnemySlideDistanceMod => _enemySlideDistanceMod;
    public float EnemyFireRateMod => _enemyFireRateMod;
    public float EnemyDmgMod => _enemyDmgMod;
    public float EnemyHPMod => _enemyHPMod;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
}
