using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameItems", menuName = "ScriptableObjects/GameItems")]
public class GameItems : ScriptableObject
{
    public int test = 0;
    public List<PlayerWeapon> playerWeapons;
}
