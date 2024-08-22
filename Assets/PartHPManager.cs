using System.Collections;
using UnityEngine;

public class PartHPManager : MonoBehaviour, IDamageable
{
    [SerializeField] protected EnumVehiclePart _vehiclePart;
    [SerializeField] float _hullHP = 100;
    [SerializeField] float _shieldHP = 100;
    [SerializeField] Renderer _partRenderer;
    EnemyVehicleManager _enemyVehicleManager;
    Coroutine _hitVisualCoroutine;
    bool _isDead = false;

    private void Awake()
    {
        _enemyVehicleManager = transform.root.GetComponent<EnemyVehicleManager>();
    }

    private void Start()
    {
        _partRenderer.material.DisableKeyword("_EMISSION");
    }   

    public void OnHit(float hullDmgValue, float shieldDmgValue, AudioClip hitSound)
    {
        _enemyVehicleManager.VehicleAudioSource.PlayOneShot(hitSound);

        if (_shieldHP > 0)
        {
            _shieldHP -= shieldDmgValue;
            _hitVisualCoroutine ??= StartCoroutine(HitEffect(Color.blue));
            return;
        }

        _hullHP -= hullDmgValue;
        _hitVisualCoroutine ??= StartCoroutine(HitEffect(Color.red));

        if (_hullHP <= 0)
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
                _enemyVehicleManager.OnWeaponLossHP(gameObject);                
            }
        }
    }

    IEnumerator HitEffect(Color color)
    {
        _partRenderer.material.SetColor("_EmissionColor", color);
        _partRenderer.material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(GameConfig.Instance.HitVisualDuration);
        _partRenderer.material.DisableKeyword("_EMISSION");
        _hitVisualCoroutine = null;
    }
}
