using System.Collections;
using UnityEngine;

public class PartHPManager : MonoBehaviour, IDamageable
{
    [SerializeField] EnumVehiclePart _vehiclePart;
    [SerializeField] float _HP = 100;

    AudioSource _vehicleAudioSource;
    Renderer _renderer;
    EnemyVehicleManager _enemyVehicleManager;
    Coroutine _hitVisualCoroutine;
    bool _isDead = false;

    private void Awake()
    {
        _vehicleAudioSource = transform.root.GetComponent<AudioSource>();
        _enemyVehicleManager = transform.root.GetComponent<EnemyVehicleManager>();
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        _renderer.material.DisableKeyword("_EMISSION");
    }   

    public void OnHit(float hullDmgValue, float shieldDmgValue, AudioClip hitSound)
    {
        _HP -= hullDmgValue;
        _vehicleAudioSource.PlayOneShot(hitSound);
        _hitVisualCoroutine ??= StartCoroutine(HitEffect());

        if (_HP <= 0)
        {
            if (_vehiclePart == EnumVehiclePart.Other)
            {
                Destroy(gameObject);
            }
            else if (_vehiclePart == EnumVehiclePart.Body && !_isDead)
            {
                _isDead = true;
                _enemyVehicleManager.OnBodyDestoyed();                
            }
            else if (_vehiclePart == EnumVehiclePart.Weapon)
            {
                _enemyVehicleManager.OnWeaponDestroy();
                Destroy(gameObject);
            }
        }
    }

    IEnumerator HitEffect()
    {
        _renderer.material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(GameConfig.Instance.HitVisualDuration);
        _renderer.material.DisableKeyword("_EMISSION");
        _hitVisualCoroutine = null;
    }
}
