using System;
using UnityEngine;

[Serializable]
public class Weapon
{
    public AudioClip shootSound;
    public AudioClip hitSound;
    public Transform transform;
    public float fireRate;
    public FirePointManager weaponParticlesManager;
}

[Serializable]
public class DropedResource
{
    public ResourcesType type;
    public int dropChance;
    public int amount;
}

[Serializable]

public class FirePoint
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] ParticleSystem[] shootPS;
}