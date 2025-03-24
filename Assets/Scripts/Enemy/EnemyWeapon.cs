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

    [SerializeField]  float _dmgMod = 0;

    float _deffDmg = 0;

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
            _dmgMod = SurviveModeManager.Instance.EnemyDMGMod;
            _deffDmg = _hullDmg;
            _hullDmg = _deffDmg * _dmgMod;
            _shieldDmg = _deffDmg * _dmgMod;
            SurviveModeManager.Instance._onIncreaseEnemyPowerMod += OnIncreaseEnemyPowerMod;
        }
    }

    void OnIncreaseEnemyPowerMod(float dmgMod)
    {
        _dmgMod += dmgMod;
        _hullDmg = _deffDmg * _dmgMod;
        _shieldDmg = _deffDmg * _dmgMod;
    }

    private void OnDisable()
    {
        SurviveModeManager.Instance._onIncreaseEnemyPowerMod -= OnIncreaseEnemyPowerMod;
    }
}
