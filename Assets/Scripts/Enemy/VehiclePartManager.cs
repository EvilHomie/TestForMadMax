using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class VehiclePartManager : MonoBehaviour, IDamageable
{
    [SerializeField] EnumVehiclePart _vehiclePart;
    [SerializeField] bool _isFrontWheel;

    float _hullHP = 100;
    float _shieldHP = 100;
    Renderer _partRenderer;

    protected EnemyVehicleManager _enemyVehicleManager;
    EnemyCharacteristics _enemyCharacteristics;
    Coroutine _hitVisualCoroutine;
    bool _partIsDestroyed = false;

    protected float _maxHullHp;
    protected float _maxShieldHp;


    private void Awake()
    {        
        _enemyVehicleManager = transform.root.GetComponent<EnemyVehicleManager>();
        _enemyCharacteristics = transform.root.GetComponent<EnemyCharacteristics>();
        _partRenderer = GetComponent<Renderer>();
        FillManagers();
    }

    protected virtual void Start()
    {
        _partRenderer.material.DisableKeyword("_EMISSION");
        SetHPValues();
    }

    void SetHPValues()
    {
        switch (_vehiclePart)
        {
            case EnumVehiclePart.Body:
                _hullHP = _enemyCharacteristics.BodyHullHP;
                _shieldHP = _enemyCharacteristics.BodyHullShieldHP;
                break;
            case EnumVehiclePart.Wheel:
                _hullHP = _enemyCharacteristics.WheelHP;
                _shieldHP = _enemyCharacteristics.WheelShieldHP;
                break;
            case EnumVehiclePart.BigWheel:
                _hullHP = _enemyCharacteristics.ArmoredPartHP;
                _shieldHP = _enemyCharacteristics.ArmoredPartShieldHP;
                break;
            case EnumVehiclePart.ExplosivePart:
                _hullHP = _enemyCharacteristics.ExplosivePartHP;
                _shieldHP = _enemyCharacteristics.ExplosivePartShieldHP;
                break;
            case EnumVehiclePart.Weapon:
                _hullHP = _enemyCharacteristics.WeaponHP;
                _shieldHP = _enemyCharacteristics.WeaponShieldHP;
                break;
            case EnumVehiclePart.ArmoredPart:
                _hullHP = _enemyCharacteristics.ArmoredPartHP;
                _shieldHP = _enemyCharacteristics.ArmoredPartShieldHP;
                break;
            case EnumVehiclePart.OtherPart:
                _hullHP = _enemyCharacteristics.OtherPartHP;
                _shieldHP = _enemyCharacteristics.OtherPartShieldHP;
                break;
            default:
                break;
        }

        if (InRaidManager.Instance.InSurviveMod)
        {
            _hullHP *= SurviveModeManager.Instance.EnemyHpMod;
            _shieldHP *= SurviveModeManager.Instance.EnemyHpMod;
        }

        _maxHullHp = _hullHP;
        _maxShieldHp = _shieldHP;
    }

    //void OnIncreaseEnemyPowerMod(float dmgMod, float hpMod)
    //{
    //    _maxHullHp = _baseHullHP * dmgMod;
    //    _maxShieldHp = _baseShieldHP * dmgMod;

    //    _hullHP = _baseHullHP * dmgMod;
    //    _shieldHP = _baseShieldHP * dmgMod;
    //}

    //private void OnDisable()
    //{
    //    SurviveModeManager.Instance._onIncreaseEnemyPowerMod -= OnIncreaseEnemyPowerMod;
    //}

    void FillManagers()
    {
        switch (_vehiclePart)
        {
            case EnumVehiclePart.Body:
                _enemyVehicleManager.Rigidbody = GetComponent<Rigidbody>();
                _enemyVehicleManager.BodyPartHPManager = this;
                _enemyVehicleManager.NavMeshObstacle = GetComponent<NavMeshObstacle>();
                _enemyVehicleManager.VehicleAudioSource = GetComponent<AudioSource>();
                break;
            case EnumVehiclePart.Wheel:
                _enemyVehicleManager.Wheels.Add(transform);
                if (_isFrontWheel) _enemyVehicleManager.FrontWheels.Add(transform);
                break;
            case EnumVehiclePart.BigWheel:
                _enemyVehicleManager.Wheels.Add(transform);
                if (_isFrontWheel) _enemyVehicleManager.FrontWheels.Add(transform);
                break;
            case EnumVehiclePart.Weapon:
                _enemyVehicleManager.Weapons.Add(GetComponent<EnemyWeapon>());
                break;
            default:
                break;
        }
    }


    public void OnHit(float hullDmgValue, float shieldDmgValue, AudioClip hitSound)
    {
        if (hitSound != null) _enemyVehicleManager.VehicleAudioSource.PlayOneShot(hitSound);
        if (_partIsDestroyed) return;


        if (_shieldHP > 0)
        {
            _shieldHP -= shieldDmgValue;
            UILevelStatistic.Instance.OnDamageDone(0, shieldDmgValue);
            _hitVisualCoroutine ??= StartCoroutine(HitEffect(Color.blue));
            ShowPartHP();
            if (_shieldHP > 0)
            {
                //_enemyVehicleManager.OnHitPart();
                return;
            }
        }

        _hullHP -= hullDmgValue;
        UILevelStatistic.Instance.OnDamageDone(hullDmgValue, 0);
        _hitVisualCoroutine ??= StartCoroutine(HitEffect(Color.red));


        if (_hullHP <= 0)
        {
            OnPartDestroyLogic();
        }
        else
        {
            //_enemyVehicleManager.OnHitPart();
        }
        ShowPartHP();
    }

    IEnumerator HitEffect(Color color)
    {
        _partRenderer.material.SetColor("_EmissionColor", color);
        _partRenderer.material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(GameConfig.Instance.HitVisualDuration);
        _partRenderer.material.DisableKeyword("_EMISSION");
        _hitVisualCoroutine = null;
    }

    public virtual void OnPartDestroyLogic()
    {
        if (_partIsDestroyed) return;
        _partIsDestroyed = true;
        UILevelStatistic.Instance.OnPartDestroyed(_vehiclePart);
        if (_vehiclePart != EnumVehiclePart.Body)
        {
            if (TryGetComponent<DetachLogic>(out var detachLogic)) detachLogic.Detach();
        }
        if (_vehiclePart == EnumVehiclePart.Body)
        {
            _enemyVehicleManager.OnBodyDestoyed();
        }
        else if (_vehiclePart == EnumVehiclePart.Weapon)
        {
            gameObject.GetComponent<EnemyWeapon>().StopShooting();
            _enemyVehicleManager.OnWeaponLossHP(gameObject);
        }
        else if (_vehiclePart == EnumVehiclePart.Wheel)
        {
            _enemyVehicleManager.OnLooseWheel(_isFrontWheel);
        }
        else if (_vehiclePart == EnumVehiclePart.BigWheel)
        {
            _enemyVehicleManager.OnLooseWheel(_isFrontWheel);
        }
    }

    public void ExplosionDamage()
    {
        OnHit(_maxHullHp / 5, _maxShieldHp / 5, null);
    }

    public void GetBodyHPRelativeValues(out float hullHPRelativeValue, out float shieldHPRelativeValue)
    {
        hullHPRelativeValue = _hullHP / _maxHullHp;
        shieldHPRelativeValue = _maxShieldHp == 0 ? 0 : _shieldHP / _maxShieldHp;
    }

    void ShowPartHP()
    {
        if(_enemyVehicleManager.IsDead) return;

        float hullHPRelativeValue = _hullHP / _maxHullHp;
        float shieldHPRelativeValue = _maxShieldHp == 0 ? 0 : _shieldHP / _maxShieldHp;

        UIEnemyHpPanel.Instance.UpdateHPBars(hullHPRelativeValue, shieldHPRelativeValue, _enemyVehicleManager);
    }
}
