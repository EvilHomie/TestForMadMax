using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "GameItems", menuName = "ScriptableObjects/GameItems")]
public class GameItems : ScriptableObject
{
    [SerializeField] List<PlayerWeapon> _weapons;
    [SerializeField] SpriteAtlas _itemsSpritesAtlas;

    [SerializeField] List<PlayerVehicle> _playerVehicles;
    [SerializeField] List<WeaponSchemeData> _weaponSchemeData;

    public List<PlayerWeapon> Weapons => _weapons;
    public List<PlayerVehicle> PlayerVehicles => _playerVehicles;
    public List<WeaponSchemeData> WeaponSchemeData => _weaponSchemeData;

    public SpriteAtlas ItemsSpritesAtlas => _itemsSpritesAtlas;
}
