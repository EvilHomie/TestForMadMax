using UnityEngine;

public class EnemyCharacteristics : MonoBehaviour
{
    [SerializeField] float _bodyHullHP;
    [SerializeField] bool _haveShields;
    [SerializeField] float _weaponDMGMod;
    [SerializeField] float _weaponFRMod;

    readonly float _shieldHPMod = 0.5f;
    readonly float _whellHPMod = 0.25f;
    readonly float _explosivePartHPMod = 0.1f;
    readonly float _weaponHPMod = 0.5f;
    readonly float _armoredPartsHPMod = 0.5f;
    readonly float _otherPartsHPMod = 0.1f;
    public float BodyHullHP => _bodyHullHP;
    public float BodyHullShieldHP => _haveShields ? _bodyHullHP * _shieldHPMod : 0;

    public float WheelHP => _bodyHullHP * _whellHPMod;
    public float WheelShieldHP => _haveShields ? WheelHP * _shieldHPMod : 0;

    public float ExplosivePartHP => _bodyHullHP * _explosivePartHPMod;
    public float ExplosivePartShieldHP => _haveShields ? ExplosivePartHP * _shieldHPMod : 0;

    public float WeaponHP => _bodyHullHP * _weaponHPMod;
    public float WeaponShieldHP => _haveShields ? WeaponHP * _shieldHPMod : 0;

    public float ArmoredPartHP => _bodyHullHP * _armoredPartsHPMod;
    public float ArmoredPartShieldHP => _haveShields ? ArmoredPartHP * _shieldHPMod : 0;

    public float OtherPartHP => _bodyHullHP * _otherPartsHPMod;
    public float OtherPartShieldHP => _haveShields ? OtherPartHP * _shieldHPMod : 0;

    public float WeaponDMGMod => _weaponDMGMod;
    public float WeaponFRMod => _weaponFRMod;
}
