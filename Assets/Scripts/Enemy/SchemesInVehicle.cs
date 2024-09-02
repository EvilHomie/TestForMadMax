using UnityEngine;

public class SchemesInVehicle : MonoBehaviour
{
    [SerializeField] WeaponSchemeData _scheme;

    public void DropScheme()
    {
        if (PlayerData.Instance.PlayerItemsData.Exists(scheme => scheme.DeffItemName == _scheme.DeffItemName))
        {
            return;
        }
        else
        {
            Debug.LogWarning($"{_scheme.DeffItemName} ADDDDEEEEDDD");
            UINewSchemeManager.Instance.OnAddNewScheme();
            PlayerData.Instance.PlayerItemsData.Add(_scheme);
        }
    }

    //bool CheckAlreadyGeted()
    //{
    //    IItemData itemData = PlayerData.Instance.GetItemDataByName(_scheme.DeffItemName);
    //    if (itemData != null)
    //    {

    //        return true;
    //    }
    //    else
    //    {

    //    }
    //}
}
