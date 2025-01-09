using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesCollection : MonoBehaviour
{
    public static EnemiesCollection Instance;

    Dictionary<EnemyType, EnemyVehicleManager> _enemyVehiclesSuperEasy;
    Dictionary<EnemyType, EnemyVehicleManager> _enemyVehiclesVeryEasy;
    Dictionary<EnemyType, EnemyVehicleManager> _enemyVehiclesEasy;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        EnemyVehicleManager[] superEasyEnemies = Resources.LoadAll<EnemyVehicleManager>("Enemy/Vehicles/SuperEasy");
        _enemyVehiclesSuperEasy = superEasyEnemies.ToDictionary(vehicle => vehicle.EnemyType, vehicle => vehicle);        

        EnemyVehicleManager[] veryEasyEnemies = Resources.LoadAll<EnemyVehicleManager>("Enemy/Vehicles/VeryEasy");
        _enemyVehiclesVeryEasy = veryEasyEnemies.ToDictionary(vehicle => vehicle.EnemyType, vehicle => vehicle);

        EnemyVehicleManager[] easyEnemies = Resources.LoadAll<EnemyVehicleManager>("Enemy/Vehicles/Easy");
        _enemyVehiclesEasy = easyEnemies.ToDictionary(vehicle => vehicle.EnemyType, vehicle => vehicle);
    }

    public EnemyVehicleManager GetEnemyPF(EnemyType enemyType, EnemyLevel enemyLevel)
    {
        return enemyLevel switch
        {
            EnemyLevel.SuperEasy => _enemyVehiclesSuperEasy[enemyType],
            EnemyLevel.VeryEasy => _enemyVehiclesVeryEasy[enemyType],
            EnemyLevel.Easy => _enemyVehiclesEasy[enemyType],
            _ => null
        };
    }

}




[Serializable]
public struct Wave
{
    public List<WaveEnemie> waveEnemies;
}

[Serializable]
public class WaveEnemie
{
    public EnemyType enemyType;
    public int count;
}

public enum EnemyType
{
    HP_lDMG_h,
    HP_mDMG_m,
    HP_hDMG_m,
    HP_hDMG_l,
    Boss
}
public enum EnemyLevel
{
    SuperEasy,
    VeryEasy,
    Easy
}