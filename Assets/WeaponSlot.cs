using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] InventoryItem _inventoryItem;
    [SerializeField] Button _selectBtn;
    [SerializeField] int _slotIndex;

    public Button SelectBtn => _selectBtn;
    public InventoryItem InventoryItem => _inventoryItem;   
    public int SlotIndex => _slotIndex;
}
