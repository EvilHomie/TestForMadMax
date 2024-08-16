using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyWeapon
{
    public AudioClip shootSound;
    public AudioClip hitSound;
    public Transform transform;
    public float fireRate;
    public FirePointManager weaponParticlesManager;
}

[Serializable]
public struct DropedResource
{
    [SerializeField] ResourcesType _type;
    [SerializeField] int _dropChance;
    [SerializeField] int _amount;

    public ResourcesType ResourcesType => _type;
    public int DropChance => _dropChance;
    public int Amount => _amount;
}

[Serializable]
public struct UpgradeCost
{
    [SerializeField] Raritie _raritieType;
    [SerializeField] List<LvlCost> _lvlCosts;

    public Raritie RaritieType => _raritieType;
    public List<LvlCost> LvlCosts => _lvlCosts;
}


[Serializable]
public struct LvlCost
{
    [SerializeField] int _lvlNumber;
    [SerializeField] List<ResCost> _resCosts;

    public int lvlNumber => _lvlNumber;
    public List<ResCost> ResCost => _resCosts;
}

[Serializable]
public struct ResCost
{
    [SerializeField] ResourcesType _resourcesType;
    [SerializeField] int _amount;

    public ResourcesType ResourcesType => _resourcesType;
    public int Amount => _amount;
}

[Serializable]
public struct ResSprite
{
    [SerializeField] ResourcesType _resourcesType;
    [SerializeField] Sprite _sprite;
    [SerializeField] Color _textColor;

    public ResourcesType ResourcesType => _resourcesType;
    public Sprite Sprite => _sprite;
    public Color TextColor => _textColor;

}