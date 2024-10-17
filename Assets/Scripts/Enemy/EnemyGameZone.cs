using System.Collections.Generic;
using UnityEngine;

public class EnemyGameZone : MonoBehaviour
{
    public static EnemyGameZone Instance;

    [SerializeField] List<PosInEnemyGameZone> _posInEnemyGameZones;  

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public PosInEnemyGameZone GetPosInGameZone()
    {
        List<PosInEnemyGameZone> freePositions = _posInEnemyGameZones.FindAll(pos => pos.IsReserved == false);   

        if (freePositions != null)
        {
            int randomIndex = Random.Range(0, freePositions.Count);
            return freePositions[randomIndex];
        }
        else
        {
            int randomIndex = Random.Range(0, _posInEnemyGameZones.Count);
            return _posInEnemyGameZones[randomIndex];
        }
    }
}