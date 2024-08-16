using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "GameItems", menuName = "ScriptableObjects/GameItems")]
public class GameItems : ScriptableObject
{
    [SerializeField] List<Weapon> _weapons;
    [SerializeField] SpriteAtlas _itemsSpritesAtlas;



    public List<Weapon> Weapons => _weapons;

    public SpriteAtlas ItemsSpritesAtlas => _itemsSpritesAtlas;
}
