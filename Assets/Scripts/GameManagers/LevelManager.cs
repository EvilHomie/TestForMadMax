using System.Collections;
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

    [Header("If TrySelect LockedLevel")]
    [SerializeField] float _shakeDuration = 0.5f;
    [SerializeField] float _shakeIntensity = 5f;

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
            if (PlayerData.Instance.UnlockedLevelsNames.Contains(info.LevelName))
            {
                info.UnlockLevel();
            }
            else info.LockLevel();
        }

        lastSelectedLevel = UILevelInfos.Find(level => level.LevelName == PlayerData.Instance.LastSelectedLevelName);
        lastSelectedLevel.Select();
        SetData(lastSelectedLevel);
        _selectLevelsWindow.SetActive(false);
        UISelectedLevelPresentation.Instance.SetNewLevelPresentation(lastSelectedLevel);
    }

    void SetData(UILevelInfo UILevelInfo)
    {
        if (!PlayerData.Instance.UnlockedLevelsNames.Contains(UILevelInfo.LevelName))
        {
            StartCoroutine(UILevelInfo.Shake(_shakeDuration, _shakeIntensity));
            return;
        }

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
        UISelectedLevelPresentation.Instance.SetNewLevelPresentation(lastSelectedLevel);
    }

    public UILevelInfo GetSelectedLevelinfo()
    {
        return lastSelectedLevel;
    }

    public void UnlockNextLevel()
    {
        int nextLevelIndex = UILevelInfos.IndexOf(lastSelectedLevel) + 1;
        UILevelInfo nextLevelInfo = UILevelInfos[nextLevelIndex];

        if (PlayerData.Instance.UnlockedLevelsNames.Contains(nextLevelInfo.LevelName)) return;

        nextLevelInfo.UnlockLevel();
        PlayerData.Instance.UnlockedLevelsNames.Add(nextLevelInfo.LevelName);
        PlayerData.Instance.LastSelectedLevelName = nextLevelInfo.LevelName;
        SetData(lastSelectedLevel);
    }

    
}
