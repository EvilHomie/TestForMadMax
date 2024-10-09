using System;
using System.Collections.Generic;
using UnityEngine;

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
public struct UnlockCost
{
    [SerializeField] ResourcesType _type;
    [SerializeField] int _amount;
    public ResourcesType ResourcesType => _type;
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

public interface IItemData
{
    public string DeffItemName { get; }
    public string TranslatedItemName { get; }
}

public interface IItem
{
    public object GetItemData();

    public void SetItemData(object obj);
}

public interface IDamageable
{
    public void OnHit(float hullDmgValue, float shieldDmgValue, AudioClip hitSound);
}

public interface IHitable
{
    public void OnHit(Vector3 hitPos, AudioClip hitSound);
}

public enum ReboundDirection
{
    X,
    Y,
    Z,
    XY,
    XZ,
    YZ
}