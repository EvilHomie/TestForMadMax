using System.Collections.Generic;
using UnityEngine;

public class LevelConfig : MonoBehaviour
{
    public static LevelConfig Instance;

    [SerializeField] List<EnemyVehicleManager> enemyList;


    public List<EnemyVehicleManager> EnemyList => enemyList;
    

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
}
