using UnityEngine;

public interface IItem 
{
    public object GetItemData();

    public Sprite GetItemSprite();

    public void SetItemData(object obj);
}
