using System.Collections;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [SerializeField] Weapon[] _weapons;

    AudioSource _audioSource;

    bool _weaponsDestroyed = false;

    private void Awake()
    {
        _audioSource = transform.root.GetComponent<AudioSource>();
    }

    public void RotateToPlayer()
    {
        if (_weaponsDestroyed) return;

        foreach (var weapon in _weapons)
        {
            weapon.transform.LookAt(Camera.main.transform.position);
        }
    }

    public void StartShooting()
    {
        _weaponsDestroyed = false;
        foreach (var weapon in _weapons)
        {
            StartCoroutine(PlayShootEffects(weapon));
        }
    }

    IEnumerator PlayShootEffects(Weapon weapon)
    {
        while (!_weaponsDestroyed) 
        {
            //weapon.weaponParticlesManager.Emit(1);
            _audioSource.PlayOneShot(weapon.shootSound);
            yield return new WaitForSeconds(1 / weapon.fireRate);
        }
    }

    public void StopShooting()
    {
        _weaponsDestroyed = true;
    }
}

