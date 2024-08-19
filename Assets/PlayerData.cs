using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    Dictionary<ResourcesType, int> _availableResources = new();
    List<IItemData> _playerItemsData = new();
    Dictionary<int, string> _equipedItems = new();

    public List<IItemData> PlayerItemsData => _playerItemsData;
    public Dictionary<ResourcesType, int> AvailableResources { get => _availableResources; set => _availableResources = value; }

    // ��� �������� 0 ��� ����������, ��������� = ����� ������
    public Dictionary<int, string> EquipedItems { get => _equipedItems; set => _equipedItems = value; }


    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public IItemData GetItemDataByName(string itemName)
    {
        return PlayerItemsData.Find(item => item.ItemName == itemName);
    }
}
