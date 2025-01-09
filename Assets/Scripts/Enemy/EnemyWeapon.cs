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
            _hullDmg *= SurviveModeManager.Instance.EnemyDmgMod;
            _shieldDmg *= SurviveModeManager.Instance.EnemyDmgMod;
            //_fireRate *= SurviveModManager.Instance.EnemyFRMod;
            //if(_fireRate > 10) _fireRate = 10;
        }
    }
}
