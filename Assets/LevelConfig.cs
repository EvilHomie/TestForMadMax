using System.Collections.Generic;
using UnityEngine;

public class LevelConfig : MonoBehaviour
{
    public static LevelConfig Instance;

    [SerializeField] List<EnemyVehicleManager> enemyList;
    [SerializeField] int _enemyCount;


    [SerializeField] float _enemySlideDistanceMod;
    [SerializeField] float _enemyFireRateMod;
    [SerializeField] float _enemyDmgMod;
    [SerializeField] float _enemyHPMod;

    public List<EnemyVehicleManager> EnemyList => enemyList;
    

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
}
