using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] InventoryItem _inventoryItem;
    [SerializeField] Button _selectBtn;
    [SerializeField] int _slotIndex;
    [SerializeField] CanvasGroup _canvasGroup;


    bool _activeStatus;

    public Button SelectBtn => _selectBtn;
    public InventoryItem InventoryItem => _inventoryItem;
    public int SlotIndex => _slotIndex;
    public bool ActiveStatus
    {
        get => _activeStatus;
        set
        {
            _activeStatus = value;

            _canvasGroup.alpha = value ? 1 : 0;
            _canvasGroup.blocksRaycasts = value;
        }
    }
}
