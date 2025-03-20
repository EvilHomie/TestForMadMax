using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    Dictionary<ResourcesType, int> _availableResources;
    List<IItemData> _playerItemsData;
    Dictionary<int, string> _equipedItems;
    string _lastSelectedLevelName;
    List<string> _unlockedLevelNames;

    int _surviveRecordTime = 0;

    public List<IItemData> PlayerItemsData { get => _playerItemsData; set => _playerItemsData = value; }
    public Dictionary<ResourcesType, int> AvailableResources { get => _availableResources; set => _availableResources = value; }

    // под индексом 0 имя транспорта, остальное = имена оружий
    public Dictionary<int, string> EquipedItems { get => _equipedItems; set => _equipedItems = value; }

    public string LastSelectedLevelName { get => _lastSelectedLevelName; set => _lastSelectedLevelName = value; }

    public List<string> UnlockedLevelsNames { get => _unlockedLevelNames; set => _unlockedLevelNames = value; }

    public int SurviveRecordTime { get => _surviveRecordTime; set => _surviveRecordTime = value; }

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public IItemData GetItemDataByName(string itemName)
    {
        return PlayerItemsData.Find(item => item.DeffItemName == itemName);
    }

    public void OnPlayerStartRaid()
    {

    }
}
