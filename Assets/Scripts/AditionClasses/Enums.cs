using UnityEngine;

public enum EnumVehiclePart
{
    OtherPart,
    Weapon,
    Body,
    Wheel,
    ExplosivePart,
    HeadLight,
    ArmoredPart,
    BigWheel
}

public enum ResourcesType
{
    Ñopper,
    Wires,
    ScrapMetal
}
public enum WeaponType
{
    SingleBarreled,
    MultyBarreled,
    Beam
}

public enum Raritie
{
    Common,
    Uncommon,
    Rare,
    Epic
}

public enum MenuNavigation
{
    InRaid,
    InGarage,
    EnterUpgrades,
    ExitUpgrades
}

public enum SchemeType
{
    Weapon,
    Vehicle
}

public enum Direction
{
    Front,
    Left,
    Right    
}

public enum CharacteristicsName
{
   //WeaponEnergyDmg,
   WeaponKineticDmg,
   WeaponFireRate,
   WeaponReloadTime,
   WeaponMagCapacity,

    VehicleHullHP,
    VehicleShieldHP,
    VehicleShieldRegRate
}
public enum UpgradeItemType
{
    Weapon,
    Vehicle
}