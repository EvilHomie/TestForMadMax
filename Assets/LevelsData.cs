using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "ScriptableObjects/LevelsData")]
public class LevelsData : ScriptableObject
{
    [SerializeField] List<LevelParameters> _levels;

    public List<LevelParameters> Levels => _levels;
}
