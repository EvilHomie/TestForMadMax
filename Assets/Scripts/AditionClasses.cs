using System;
using UnityEngine;

[Serializable]
public class Weapon
{
    public AudioClip shootSound;
    public AudioClip hitSound;
    public Transform transform;
    public float fireRate;
    public WeaponParticles weaponParticlesManager;
}

[Serializable]
public class DropedResource
{
    public ResourcesType type;
    public int dropChance;
    public int amount;
}
