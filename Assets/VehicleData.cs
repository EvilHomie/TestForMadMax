using System;
using UnityEngine;

[CreateAssetMenu(fileName = "VehicleData", menuName = "ScriptableObjects/VehicleData")]
public class VehicleData : ScriptableObject, IItemData
{
    [SerializeField] string _vehicleName;
    [SerializeField] Raritie _vehicleRaritie;

    [Header(Constants.HULLHP)]
    public float hullHPByLvl;
    public int hullHPCurLvl;
    [SerializeField] int _hullHPMaxLvl;

    [Header(Constants.SHIELDHP)]
    public float shieldHPByLvl;
    public int shieldHPCurLvl;
    [SerializeField] int _shieldHPMaxLvl;

    [Header(Constants.SHIELREGENRATE)]
    public float shieldRegenRateByLvl;
    public int shieldRegenCurtLvl;
    [SerializeField] int _shieldRegenMaxLvl;

    [Header(Constants.WEAPONSCOUNT)]
    public int curWeaponsCount;
    [SerializeField] int maxWeaponsCount;

    public string ItemName => _vehicleName;

    public Raritie Raritie => _vehicleRaritie;


    public int HullHPMaxLvl => _hullHPMaxLvl;
    public int ShieldHPMaxLvl => _shieldHPMaxLvl;
    public int ShieldRegenMaxLvl => _shieldRegenMaxLvl;
    public int MaxWeaponsCount => maxWeaponsCount;
}
