using UnityEngine;

[CreateAssetMenu(fileName = "VehicleData", menuName = "ScriptableObjects/VehicleData")]
public class VehicleData : ScriptableObject, IItemData
{
    public string deffVehicleName;
    public string vehicleNameEN;
    public string vehicleNameRU;
    public Raritie vehicleRaritie;

    [Header("HULLHP")]
    public float hullHPByLvl;
    public int hullHPCurLvl;
    public int hullHPMaxLvl;

    [Header("SHIELDHP")]
    public float shieldHPByLvl;
    public int shieldHPCurLvl;
    public int shieldHPMaxLvl;

    [Header("SHIELREGENRATE")]
    public float shieldRegenRateByLvl;
    public int shieldRegenCurtLvl;
    public int shieldRegenMaxLvl;

    [Header("WEAPONSCOUNT")]
    public int curWeaponsCount;
    public int maxWeaponsCount;

    public string TranslatedItemName
    {
        get
        {
            if (TextConstants.Language == Language.ru) return vehicleNameRU;
            else return vehicleNameEN;
        }
    }

    public string DeffItemName => deffVehicleName;

    public void SetData(VehicleDataForSave vehicleData)
    {
        deffVehicleName = vehicleData.deffVehicleName;
        vehicleNameEN = vehicleData.vehicleNameEN;
        vehicleNameRU = vehicleData.vehicleNameRU;
        vehicleRaritie = vehicleData.vehicleRaritie;
        hullHPByLvl = vehicleData.hullHPByLvl;
        hullHPCurLvl = vehicleData.hullHPCurLvl;
        hullHPMaxLvl = vehicleData.hullHPMaxLvl;
        shieldHPByLvl = vehicleData.shieldHPByLvl;
        shieldHPCurLvl = vehicleData.shieldHPCurLvl;
        shieldHPMaxLvl = vehicleData.shieldHPMaxLvl;
        shieldRegenRateByLvl = vehicleData.shieldRegenRateByLvl;
        shieldRegenCurtLvl = vehicleData.shieldRegenCurtLvl;
        shieldRegenMaxLvl = vehicleData.shieldRegenMaxLvl;
        curWeaponsCount = vehicleData.curWeaponsCount;
        maxWeaponsCount = vehicleData.maxWeaponsCount;
    }
}
