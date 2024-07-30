using System;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [SerializeField] Weapon[] weapons;
    [SerializeField] bool _isShooting;

    private void FixedUpdate()
    {
        foreach (var weapon in weapons)
        {
            weapon.transform.LookAt(Camera.main.transform.position);
            Shoot(weapon);
        }
    }

    void Shoot(Weapon weapon)
    {
        if (_isShooting && Time.time >= weapon.nextTimeTofire)
        {
            weapon.nextTimeTofire = Time.time + 1f / weapon.fireRate;
            weapon.soundSource.PlayOneShot(weapon.shootSound);
        }
    }
}

[Serializable]
class Weapon
{
    public AudioClip shootSound;
    public AudioSource soundSource;
    public Transform transform;
    public float fireRate;
    public float nextTimeTofire;
}