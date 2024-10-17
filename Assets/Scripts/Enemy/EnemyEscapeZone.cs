using System.Collections.Generic;
using UnityEngine;

public class EnemyEscapeZone : MonoBehaviour
{
    public static EnemyEscapeZone Instance;

    [SerializeField] List<Transform> _runAwayPositions;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public Vector3 GetRandomEscapePos()
    {
        int randomIndex = Random.Range(0, _runAwayPositions.Count);
        return _runAwayPositions[randomIndex].position;
    }
}
