using UnityEngine;

[CreateAssetMenu(fileName = "VehicleSchemeData", menuName = "ScriptableObjects/VehicleSchemeData")]
public class VehicleSchemeData : SchemeData, IItemData
{
    public VehicleData vehicleData;

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

    public override string SchemeName => DeffItemName;
    public override string ItemNameInScheme => vehicleData.deffVehicleName;
}
