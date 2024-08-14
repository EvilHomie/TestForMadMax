using UnityEngine;

public interface IItem 
{
    public object GetItemData();

    public Sprite GetItemSprite();

    public void SetItemCopyData(object obj);
}
