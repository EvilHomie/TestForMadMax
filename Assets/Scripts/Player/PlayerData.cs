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
    string _lastSelectedLevelName;
    List<string> _unlockedLevelNames = new();

    public List<IItemData> PlayerItemsData => _playerItemsData;
    public Dictionary<ResourcesType, int> AvailableResources { get => _availableResources; set => _availableResources = value; }

    // под индексом 0 имя транспорта, остальное = имена оружий
    public Dictionary<int, string> EquipedItems { get => _equipedItems; set => _equipedItems = value; }

    public string LastSelectedLevelName { get => _lastSelectedLevelName; set => _lastSelectedLevelName = value; }

    public List<string> UnlockedLevelsNames { get => _unlockedLevelNames; set => _unlockedLevelNames = value; }

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public IItemData GetItemDataByName(string itemName)
    {
        return PlayerItemsData.Find(item => item.ItemName == itemName);
    }

    public void OnPlayerStartRaid()
    {

    }
}
