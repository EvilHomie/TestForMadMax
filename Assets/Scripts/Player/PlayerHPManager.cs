using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
    public static PlayerHPManager Instance;

    [SerializeField] float _playerHullHP;
    [SerializeField] float _playerShieldHP;
    [SerializeField] float _playerShieldRegRate;
    

    [SerializeField] float _onHitShakeIntensity;
    [SerializeField] AudioSource _musicAudioSource;
    [SerializeField] AudioSource _hitAudioSource;
    [SerializeField] ParticleSystem _explosionPS;
    [SerializeField] AudioClip _onDieExplosionSound;

    float _maxHullHp;
    float _maxShieldHp;

    bool _onRaid = false;
    bool _isDead = false;

    public bool IsDead => _isDead;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
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
        _musicAudioSource.Play();
    }

    public void OnPlayerEndRaid()
    { 
        _onRaid = false;
        _musicAudioSource.Stop();
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
        _hitAudioSource.PlayOneShot(hitSound);
        if (_playerShieldHP > 0)
        {
            _playerShieldHP -= shieldDmgValue;

            if (_playerShieldHP < 0)
            {
                _playerHullHP -= hullDmgValue + _playerShieldHP;
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

        if (_playerHullHP <= 0)
        {
            _isDead = true;
            OnPlayerVehicleDestroyed();
        }
    }

    void OnPlayerVehicleDestroyed()
    {
        _explosionPS.Play();
        _hitAudioSource.PlayOneShot(_onDieExplosionSound);
        FinishLevelManager.Instance.OnFinishLevel(isSuccessfully: false);
        RaidManager.Instance.OnPLayerDie();
        PlayerWeaponManager.Instance.OnPlayerDie();
    }
}
