using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSchemeData", menuName = "ScriptableObjects/WeaponSchemeData")]
public class VehicleSchemeData : ScriptableObject, IItemData
{
    public VehicleData vehicleData;
    public int copperAmountForUnlock;
    public int wiresAmountForUnlock;
    public int scrapMetalAmountForUnlock;

    public int dropChanceIfDuplicate;
    public string TranslatedItemName
    {
        get
        {
            if (TextConstants.Language == Language.ru) return vehicleData.vehicleNameRU;
            else return vehicleData.deffVehicleName;
        }
    }

    public string DeffItemName => $"{vehicleData.deffVehicleName}_Scheme";
}
