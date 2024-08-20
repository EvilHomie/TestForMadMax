using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPManager : PartHPManager
{
    public static PlayerHPManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void OnPlayerStartRaid()
    {
        _vehiclePart = EnumVehiclePart.Body;
    }


    public void OnHit(float hullDmgValue, float shieldDmgValue, AudioClip hitSound)
    {
        //_hullHP -= hullDmgValue;
        //_vehicleAudioSource.PlayOneShot(hitSound);
        //_hitVisualCoroutine ??= StartCoroutine(HitEffect());

        //if (_hullHP <= 0)
        //{
        //    if (_vehiclePart == EnumVehiclePart.Other)
        //    {
        //        Destroy(gameObject);
        //    }
        //    else if (_vehiclePart == EnumVehiclePart.Body && !_isDead)
        //    {
        //        _isDead = true;
        //        _enemyVehicleManager.OnBodyDestoyed();
        //    }
        //    else if (_vehiclePart == EnumVehiclePart.Weapon)
        //    {
        //        _enemyVehicleManager.OnWeaponDestroy();
        //        Destroy(gameObject);
        //    }
        //}
    }


}
