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

    float _maxHullHp;
    float _maxShieldHp;

    bool _onRaid = false;

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
        UIPlayerHpManager.Instance.UpdateHPBars(_playerHullHP / _maxHullHp, shieldHPValue);
    }


    public void OnHit(float hullDmgValue, float shieldDmgValue, AudioClip hitSound)
    {
        CameraManager.Instance.Shake(0.1f, _onHitShakeIntensity);
        _hitAudioSource.PlayOneShot(hitSound);
        if (_playerShieldHP > 0)
        {
            float difference = shieldDmgValue - _playerShieldHP;
            _playerShieldHP -= shieldDmgValue;

            if (difference > 0)
            {
                _playerShieldHP = 0;
                _playerHullHP -= hullDmgValue + difference;
                Debug.Log($"HULLD DMG  {hullDmgValue + difference}");
            }
        }
        else
        {
            _playerHullHP -= hullDmgValue;
            Debug.Log($"HULLD DMG  {hullDmgValue}");
        }



        if (_playerHullHP <= 0)
        {
            OnPlayerVehicleDestroyed();
        }
    }

    void OnPlayerVehicleDestroyed()
    {
        GameManager.Instance.OnPlayerVehicleDestroyed();
    }
}
