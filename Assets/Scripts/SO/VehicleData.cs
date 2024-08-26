using UnityEngine;

[CreateAssetMenu(fileName = "VehicleData", menuName = "ScriptableObjects/VehicleData")]
public class VehicleData : ScriptableObject, IItemData
{
    public string vehicleName;
    public Raritie vehicleRaritie;

    [Header(Constants.HULLHP)]
    public float hullHPByLvl;
    public int hullHPCurLvl;
    public int hullHPMaxLvl;

    [Header(Constants.SHIELDHP)]
    public float shieldHPByLvl;
    public int shieldHPCurLvl;
    public int shieldHPMaxLvl;

    [Header(Constants.SHIELREGENRATE)]
    public float shieldRegenRateByLvl;
    public int shieldRegenCurtLvl;
    public int shieldRegenMaxLvl;

    [Header(Constants.WEAPONSCOUNT)]
    public int curWeaponsCount;
    public int maxWeaponsCount;

    public string ItemName => vehicleName;
}
