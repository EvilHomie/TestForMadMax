using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [SerializeField] List<EnemyWeapon> _weapons;
    [SerializeField] float _accuracy;

    bool _allWeaponsDestroyed = false;

    Vector3 _targetPos = new(0, 100, 0);

    float _rotationSpeedMod = 2f; //необходим для корректировки скорости поворота, чтобы оружие успело повернутся до истечения времени на прицеливание   

    IEnumerator LockOnPlayer()
    {
        float t = 0;
        float lockDuration = GameConfig.Instance.LockOnPlayerDuration;
        float rotationSpeed = Time.deltaTime * _rotationSpeedMod / lockDuration;

        while (t <= 1)
        {
            t += Time.deltaTime / lockDuration;

            foreach (var weapon in _weapons)
            {
                if (weapon == null) continue; 
                Vector3 targetDirection = _targetPos - weapon.transform.position;
                Vector3 newDirection = Vector3.RotateTowards(weapon.transform.forward, targetDirection, rotationSpeed, 0.0f);
                weapon.transform.rotation = Quaternion.LookRotation(newDirection);
            }
            yield return null;
        }

        while (!_allWeaponsDestroyed)
        {
            foreach (var weapon in _weapons)
            {
                if (weapon == null) continue; 
                weapon.transform.LookAt(_targetPos);
            }
            yield return null;
        }
    }

    public void StartShootLogic()
    {
        _allWeaponsDestroyed = false;
        StartCoroutine(LockOnPlayer());
        StartCoroutine(StartShootWhithDelay());
    }

    IEnumerator StartShootWhithDelay()
    {
        yield return new WaitForSeconds(GameConfig.Instance.LockOnPlayerDuration);
        foreach (var weapon in _weapons)
        {
            if (weapon == null) continue;
            weapon.StartShooting(_accuracy);
        }
    }

    public void StopShooting()
    {
        StopAllCoroutines();
        foreach (var weapon in _weapons)
        {
            if (weapon == null) continue;
            weapon.StopShooting();
        }
    }

    public bool CheckAvailableWeapons()
    {
        Debug.LogWarning("CHECK W DESTROED");

        foreach (var weapon in _weapons)
        {
            Debug.Log(weapon.gameObject.name);
        }



        if (_weapons.TrueForAll(weapon => weapon.gameObject == null))
        {
            Debug.LogWarning("ALL W DESTROED");
            _allWeaponsDestroyed = true;
            StopShooting();
            return false;
        }
        else return true;
    }
}

