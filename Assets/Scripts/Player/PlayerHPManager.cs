using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
    public static PlayerHPManager Instance;

    [SerializeField] float _playerHullHP;
    [SerializeField] float _playerShieldHP;
    [SerializeField] float _playerShieldRegRate;


    [SerializeField] float _onHitShakeIntensity;

    [SerializeField] ParticleSystem _explosionPS;
    [SerializeField] AudioClip _onDieExplosionSound;



    float _maxHullHp;
    float _maxShieldHp;

    bool _onRaid = false;
    bool _isDead = false;
    bool _restoreOfferWasProposed = false;

    public bool IsDead => _isDead;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        OnPlayerEndRaid();
    }

    public void OnPlayerStartRaid()
    {
        VehicleData vehicleData = (VehicleData)PlayerVehicleManager.Instance.PlayerVehicle.GetItemData();
        _playerHullHP = vehicleData.hullHPByLvl * vehicleData.hullHPCurLvl;
        _playerShieldHP = vehicleData.shieldHPByLvl * vehicleData.shieldHPCurLvl;
        _playerShieldRegRate = vehicleData.shieldRegenRateByLvl * vehicleData.shieldRegenCurtLvl;
        _maxShieldHp = _playerShieldHP;
        _maxHullHp = _playerHullHP;
        _onRaid = true;
        _isDead = false;
        _restoreOfferWasProposed = false;
        UIPlayerHpPanel.Instance.ConfigHPSeparators(_maxHullHp, _maxShieldHp);
    }

    public void OnStartSurviveMode(SMVehicleData vehicleData)
    {
        _playerHullHP = vehicleData.hullHP;
        _playerShieldHP = vehicleData.shieldHP;
        _maxHullHp = _playerHullHP;
        _maxShieldHp = _playerShieldHP;
        _playerShieldRegRate = vehicleData.shieldRegRate;
        _onRaid = true;
        _isDead = false;
        UIPlayerHpPanel.Instance.ConfigHPSeparators(_maxHullHp, _maxShieldHp);
    }

    public void OnChangeValuesInSurviveMode(SMVehicleData vehicleData)
    {
        float maxHullHPDifference = vehicleData.hullHP - _maxHullHp;
        _maxHullHp = vehicleData.hullHP;
        _playerHullHP += maxHullHPDifference;

        float maxShieldHPDifference = vehicleData.shieldHP - _maxShieldHp;
        _maxShieldHp = vehicleData.shieldHP;
        _playerShieldHP += maxShieldHPDifference;

        _playerShieldRegRate = vehicleData.shieldRegRate;
        UIPlayerHpPanel.Instance.ConfigHPSeparators(_maxHullHp, _maxShieldHp);
    }

    public void OnPlayerEndRaid()
    {
        _onRaid = false;
    }

    private void Update()
    {
        if (!_onRaid) return;
        if (_playerShieldRegRate > 0 && _playerShieldHP < _maxShieldHp)
        {
            _playerShieldHP += _playerShieldRegRate * Time.deltaTime;
        }

        float shieldHPValue = _maxShieldHp == 0 ? 0 : _playerShieldHP / _maxShieldHp;
        UIPlayerHpPanel.Instance.UpdateHPBars(_playerHullHP / _maxHullHp, shieldHPValue);
    }


    public void OnHit(float hullDmgValue, float shieldDmgValue, AudioClip hitSound)
    {
        if (_isDead) return;
        CameraManager.Instance.Shake(0.1f, _onHitShakeIntensity);
        AudioManager.Instance.PlayHitToPLayerSound(hitSound);
        if (_playerShieldHP > 0)
        {
            _playerShieldHP -= shieldDmgValue;

            if (_playerShieldHP < 0)
            {
                _playerHullHP -= hullDmgValue - (hullDmgValue + _playerShieldHP);
                UILevelStatistic.Instance.OnDamageRecieved(hullDmgValue + _playerShieldHP, -_playerShieldHP);
                _playerShieldHP = 0;
            }
            else
            {
                UILevelStatistic.Instance.OnDamageRecieved(0, shieldDmgValue);
            }
        }
        else
        {
            _playerHullHP -= hullDmgValue;
            UILevelStatistic.Instance.OnDamageRecieved(hullDmgValue, 0);
        }

        if (_playerHullHP <= _maxHullHp * 0.1f && !_restoreOfferWasProposed && !InRaidManager.Instance.InSurviveMod)
        {
            _restoreOfferWasProposed = true;
            LowHpRewardOffer.Instance.ShowRewardOffer(OnSelectRewardOption, RewardName.RestoreHP, _playerHullHP / _maxHullHp);
            return;
        }

        if (_playerHullHP <= 0)
        {
            _isDead = true;
            OnPlayerVehicleDestroyed();
        }
    }

    void OnSelectRewardOption(bool GetRewardStatus)
    {
        if (GetRewardStatus)
        {
            RestoreHP();
        }
        else
        {
            if (_playerHullHP <= 0)
            {
                _isDead = true;
                OnPlayerVehicleDestroyed();
            }
        }
    }



    void OnPlayerVehicleDestroyed()
    {
        _explosionPS.Play();
        AudioManager.Instance.PlayHitToPLayerSound(_onDieExplosionSound);
        InRaidManager.Instance.OnPLayerDie();
        PlayerWeaponManager.Instance.OnPlayerDie();
    }

    void RestoreHP()
    {
        _playerHullHP = _maxHullHp / 2;
        Cursor.visible = false;
    }

    public void OnLeaveSurviveMode()
    {
        OnPlayerVehicleDestroyed();
    }

    public void OnHPEnemyDestroyed()
    {
        if (_playerHullHP == _maxHullHp)
        {
            EventTextPanel.Instance.ShowEventText(0, $"{TextConstants.FULLHP}");
            return;
        }
        float restorValue = _maxHullHp * SurviveModeEnemyHPSpawner.Instance.RestoreHpPercent / 100;
        if (_playerHullHP + restorValue > _maxHullHp)
        {
            restorValue = _maxHullHp - _playerHullHP;
            _playerHullHP = _maxHullHp;
        }
        else
        {
            _playerHullHP += restorValue;
        }


        EventTextPanel.Instance.ShowEventText((int)restorValue, $"{TextConstants.HULLHP} +");
    }
}
