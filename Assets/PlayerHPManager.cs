using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
    public static PlayerHPManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void OnPlayerStartRaid()
    {
    }


    public void OnHit(float hullDmgValue, float shieldDmgValue, AudioClip hitSound)
    {

        //Debug.Log($"{hullDmgValue}    {shieldDmgValue}");







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
