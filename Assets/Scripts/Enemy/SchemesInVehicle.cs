using UnityEngine;

public class SchemesInVehicle : MonoBehaviour
{
    [SerializeField] WeaponSchemeData _scheme;

    public void DropScheme()
    {
        if (CheckExistingItem())
        {
            return;
        }
        UINewSchemeManager.Instance.OnAddNewScheme();
        PlayerData.Instance.PlayerItemsData.Add(_scheme);
    }

    bool CheckExistingItem()
    {
        IItemData itemData = PlayerData.Instance.GetItemDataByName(_scheme.DeffItemName);

        if (itemData != null) return true;
        else
        {
            itemData = PlayerData.Instance.GetItemDataByName(_scheme.weaponData.DeffItemName);
            if (itemData != null) return true;
            else return false;
        }
    }
}
