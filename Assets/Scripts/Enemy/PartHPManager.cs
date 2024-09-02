using System.Collections;
using UnityEngine;

public class PartHPManager : MonoBehaviour, IDamageable
{
    [SerializeField] EnumVehiclePart _vehiclePart;
    [SerializeField] float _hullHP = 100;
    [SerializeField] float _shieldHP = 100;
    [SerializeField] Renderer _partRenderer;

    protected EnemyVehicleManager _enemyVehicleManager;
    Coroutine _hitVisualCoroutine;
    bool _partIsDestroyed = false;

    protected float _maxHullHp;
    protected float _maxShieldHp;

    private void Awake()
    {
        _enemyVehicleManager = transform.root.GetComponent<EnemyVehicleManager>();
    }

    protected virtual void Start()
    {
        _partRenderer.material.DisableKeyword("_EMISSION");
        _hullHP *= LevelManager.Instance.GetSelectedLevelinfo().EnemyHPMod;
        _shieldHP *= LevelManager.Instance.GetSelectedLevelinfo().EnemyHPMod;

        _maxHullHp = _hullHP;
        _maxShieldHp = _shieldHP;
    }

    public void OnHit(float hullDmgValue, float shieldDmgValue, AudioClip hitSound)
    {
        if(hitSound != null) _enemyVehicleManager.VehicleAudioSource.PlayOneShot(hitSound);
        if (_partIsDestroyed) return;

        if (_shieldHP > 0)
        {
            _shieldHP -= shieldDmgValue;
            _hitVisualCoroutine ??= StartCoroutine(HitEffect(Color.blue));
            _enemyVehicleManager.OnHitPart();
            return;
        }

        _hullHP -= hullDmgValue;
        _hitVisualCoroutine ??= StartCoroutine(HitEffect(Color.red));

        _enemyVehicleManager.OnHitPart();

        if (_hullHP <= 0)
        {
            _partIsDestroyed = true;
            OnPartDestroyLogic();
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

    protected virtual void OnPartDestroyLogic()
    {
        if (_vehiclePart == EnumVehiclePart.Other)
        {
            Destroy(gameObject);
        }
        else if (_vehiclePart == EnumVehiclePart.Wheel)
        {
            Destroy(gameObject);
        }
        else if (_vehiclePart == EnumVehiclePart.Body)
        {
            _enemyVehicleManager.OnBodyDestoyed();
        }
        else if (_vehiclePart == EnumVehiclePart.Weapon)
        {
            _enemyVehicleManager.OnWeaponLossHP(gameObject);
        }
    }

    public void ExplosionDamage()
    {
        OnHit(_maxHullHp/5, _maxShieldHp/5, null);
    }

    public void GetBodyHPRelativeValues(out float hullHPRelativeValue,out float shieldHPRelativeValue)
    {
        hullHPRelativeValue = _hullHP / _maxHullHp;
        shieldHPRelativeValue = _maxShieldHp == 0 ? 0 : _shieldHP / _maxShieldHp;
    }
}
