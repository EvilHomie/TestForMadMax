using UnityEngine;

public class SchemesInVehicle : MonoBehaviour
{
    [SerializeField] SchemeData _scheme;

    public SchemeData scheme => _scheme;

    public void DropScheme()
    {
        if (CheckExistingItem())
        {            
            return;
        }
        UINewSchemeManager.Instance.OnAddNewScheme();
        PlayerData.Instance.PlayerItemsData.Add((IItemData)_scheme);
    }

    bool CheckExistingItem()
    {
        IItemData itemData = PlayerData.Instance.GetItemDataByName(_scheme.SchemeName);

        if (itemData != null)
        {
            //Debug.LogWarning("EXIST as Scheme");
            return true;
        }
        else
        {
            itemData = PlayerData.Instance.GetItemDataByName(_scheme.ItemNameInScheme);
            if (itemData != null)
            {
                //Debug.LogWarning("EXIST as Item");
                return true;
            }
            else return false;
        }
    }
}
