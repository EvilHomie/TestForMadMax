using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Image _itemImage;
    IItemData _item;

    public void SetitemData(IItemData item)
    {
        _item = item;
        if (item == null)
        {
            _itemImage.gameObject.SetActive(false);
            return;
        }
        _itemImage.gameObject.SetActive(true);
        if (item is WeaponSchemeData)
        {
            string weaponName = item.DeffItemName.Replace("_Scheme","");
            _itemImage.sprite = GameAssets.Instance.GameItems.ItemsSpritesAtlas.GetSprite(weaponName);
        }
        else
        {
            _itemImage.sprite = GameAssets.Instance.GameItems.ItemsSpritesAtlas.GetSprite(_item.DeffItemName);
        }
    }

    public IItemData GetitemData()
    {
        return _item;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_item == null) return;
        InventoryManager.Instance.OnSelectInventoryItem(_item);
    }
}
