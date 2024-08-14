using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] Image _itemImage;

    IItem _item;

   
    public void SetImage(IItem item)
    {
        _item = item;
        _itemImage.sprite = _item.GetItemSprite();
    }
}
