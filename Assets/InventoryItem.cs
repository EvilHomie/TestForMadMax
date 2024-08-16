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
        _itemImage.sprite = GameAssets.Instance.GameItems.ItemsSpritesAtlas.GetSprite(_item.ItemName);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        InventoryManager.Instance.OnSelectInventoryItem(_item);
    }
}
