using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] Transform _levelsContainer;
    [SerializeField] GameObject _selectLevelsWindow;

    [SerializeField] List<EnemyVehicleManager> _enemyList;
    [SerializeField] int _enemyCount = 1;
    [SerializeField] float _enemySlideDistanceMod = 1;
    [SerializeField] float _enemyDmgMod = 1;
    [SerializeField] float _enemyHPMod = 1;

    List<UILevelInfo> UILevelInfos = new();

    UILevelInfo lastSelectedLevel;

    public List<EnemyVehicleManager> EnemyList => _enemyList;
    public int EnemyCount => _enemyCount;
    public float EnemySlideDistanceMod => _enemySlideDistanceMod;
    public float EnemyDmgMod => _enemyDmgMod;
    public float EnemyHPMod => _enemyHPMod;
    public GameObject SelectLevelsWindow => _selectLevelsWindow;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        foreach (Transform levelInfo in _levelsContainer)
        {
            UILevelInfo info = levelInfo.GetComponent<UILevelInfo>();
            UILevelInfos.Add(info);
            info.SelectBtn.onClick.AddListener(delegate { SetData(info); });
            info.Deselect();
        }

        lastSelectedLevel = UILevelInfos.Find(level => level.LevelName == PlayerData.Instance.LastSelectedLevelName);
        lastSelectedLevel.Select();
        SetData(lastSelectedLevel);
        _selectLevelsWindow.SetActive(false);
    }

    void SetData(UILevelInfo UILevelInfo)
    {
        _enemyList = UILevelInfo.EnemyList;
        _enemyCount = UILevelInfo.EnemyCount;
        _enemySlideDistanceMod = UILevelInfo.EnemySlideDistanceMod;
        _enemyDmgMod = UILevelInfo.EnemyDmgMod;
        _enemyHPMod = UILevelInfo.EnemyHPMod;

        lastSelectedLevel.Deselect();
        lastSelectedLevel = UILevelInfo;
        lastSelectedLevel.Select();
        PlayerData.Instance.LastSelectedLevelName = lastSelectedLevel.LevelName;
        _selectLevelsWindow.SetActive(false);
    }
}
