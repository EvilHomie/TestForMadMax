using UnityEngine;

public class EnemyWeapon : WeaponLogic
{
    [SerializeField] WeaponType weaponType;
    [SerializeField] float _hullDmg;
    [SerializeField] float _shieldDmg;
    [SerializeField] float _fireRate;
    protected override float CurHullDmg => _hullDmg;
    protected override float CurShieldDmg => _shieldDmg;
    protected override float CurFireRate => _fireRate;


    float _baseHullDmg;
    float _baseShieldDmg;
    float _survModeDmgMod;
    float _onchangeEnemyTirBonus;

    public void StartShooting()
    {
        ShootAsBot(weaponType);
    }

    public void StopShooting()
    {
        OnStopShooting();
    }

    public void SetCharacteristics(float dmgMod, float FRMod)
    {
        _hullDmg *= dmgMod;
        _shieldDmg *= dmgMod;
        _fireRate *= FRMod;

        if (InRaidManager.Instance.InSurviveMod)
        {
            _baseHullDmg = _hullDmg;
            _baseShieldDmg = _shieldDmg;
            _survModeDmgMod = 0;
            _onchangeEnemyTirBonus = 0;
            SurviveModeManager.Instance._onIncreaseEnemyPowerMod += OnIncreaseEnemyPowerMod;
        }
    }

    void OnIncreaseEnemyPowerMod(float dmgMod)
    {
        if (_survModeDmgMod - _onchangeEnemyTirBonus < dmgMod)
        {
            _survModeDmgMod = dmgMod + _onchangeEnemyTirBonus;
        }
        else
        {
            _onchangeEnemyTirBonus = _survModeDmgMod - 1;
        }

        _hullDmg = _baseHullDmg * _survModeDmgMod;
        _shieldDmg = _baseShieldDmg * _survModeDmgMod;
    }

    private void OnDisable()
    {
        SurviveModeManager.Instance._onIncreaseEnemyPowerMod -= OnIncreaseEnemyPowerMod;
    }
}
