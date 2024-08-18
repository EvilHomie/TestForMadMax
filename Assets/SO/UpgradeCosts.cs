using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeCosts", menuName = "ScriptableObjects/UpgradeCosts")]
public class UpgradeCosts : ScriptableObject
{
    [SerializeField] List<UpgradeCost> _upgradeCosts;

    public List<UpgradeCost> Costs => _upgradeCosts;
}
