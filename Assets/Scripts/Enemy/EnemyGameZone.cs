using System.Collections.Generic;
using UnityEngine;

public class EnemyGameZone : MonoBehaviour
{
    public static EnemyGameZone Instance;

    [SerializeField] List<PosInEnemyGameZone> _allPosInEnemyGameZones;


    [SerializeField] List<PosInEnemyGameZone> _availablePosInEnemyGameZones = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void OnPlayerStartRaid(LevelParameters levelParametersCopy)
    {
        _availablePosInEnemyGameZones.Clear();

        if (levelParametersCopy.BlockedEnemiesLinesInGameZone.Count != 0)
        {
            foreach (var pos in _allPosInEnemyGameZones)
            {
                if (levelParametersCopy.BlockedEnemiesLinesInGameZone.Contains(pos.LineIndex))
                {
                    continue;
                }
                else
                {
                    _availablePosInEnemyGameZones.Add(pos);
                }
            }
        }
        else
        {
            _availablePosInEnemyGameZones = new(_allPosInEnemyGameZones);
        }

        
    }
    

    public PosInEnemyGameZone GetPosInGameZone()
    {
        List<PosInEnemyGameZone> freePositions = _availablePosInEnemyGameZones.FindAll(pos => pos.IsReserved == false);   

        if (freePositions.Count != 0)
        {
            int randomIndex = Random.Range(0, freePositions.Count);
            return freePositions[randomIndex];
        }
        else
        {
            int randomIndex = Random.Range(0, _availablePosInEnemyGameZones.Count);
            return _availablePosInEnemyGameZones[randomIndex];
        }
    }
}