using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] LevelsData _levelsData;
    [SerializeField] UILevelInfo _UILevelInfoPF;


    [Header("If TrySelect LockedLevel")]
    [SerializeField] float _shakeDuration = 0.5f;
    [SerializeField] float _shakeIntensity = 5f;


    GameObject _selectLevelsWindow;
    UILevelInfo _lastSelectedLevel;
    List<UILevelInfo> _UILevelInfos = new();
    public GameObject SelectLevelsWindow => _selectLevelsWindow;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        //LevelsData levelsDataCopy = Instantiate(_levelsData);
        SelectLevelWindow.Instance.Init(_levelsData, _UILevelInfoPF);
        _selectLevelsWindow = SelectLevelWindow.Instance.transform.gameObject;

        _UILevelInfos = SelectLevelWindow.Instance.GetCreatedLevels();

        foreach (UILevelInfo levelInfo in _UILevelInfos)
        {
            levelInfo.SelectBtn.onClick.AddListener(delegate { SelectLevel(levelInfo); });           
        }

        _lastSelectedLevel = _UILevelInfos.Find(info => info.LevelParameters.LevelName == PlayerData.Instance.LastSelectedLevelName);
        _lastSelectedLevel.Select();
        
        SelectLevel(_lastSelectedLevel);
    }

    void SelectLevel(UILevelInfo UILevelInfo)
    {
        if (!PlayerData.Instance.UnlockedLevelsNames.Contains(UILevelInfo.LevelParameters.LevelName))
        {
            StartCoroutine(UILevelInfo.Shake(_shakeDuration, _shakeIntensity));
            return;
        }

        _lastSelectedLevel.Deselect();
        _lastSelectedLevel = UILevelInfo;
        _lastSelectedLevel.Select();
        PlayerData.Instance.LastSelectedLevelName = _lastSelectedLevel.LevelParameters.LevelName;
        _selectLevelsWindow.SetActive(false);
        UIChangeLevelButton.Instance.UpdateSelectedLevel(_lastSelectedLevel);
    }

    public UILevelInfo GetSelectedLevelinfo()
    {
        return _lastSelectedLevel;
    }

    public void UnlockNextLevel()
    {
        int nextLevelIndex = _UILevelInfos.IndexOf(_lastSelectedLevel) + 1;
        if(nextLevelIndex >= _UILevelInfos.Count) return;
        UILevelInfo nextLevelInfo = _UILevelInfos[nextLevelIndex];

        if (PlayerData.Instance.UnlockedLevelsNames.Contains(nextLevelInfo.LevelParameters.LevelName)) return;

        nextLevelInfo.UnlockLevel();
        PlayerData.Instance.UnlockedLevelsNames.Add(nextLevelInfo.LevelParameters.LevelName);
        PlayerData.Instance.LastSelectedLevelName = nextLevelInfo.LevelParameters.LevelName;
        SelectLevel(nextLevelInfo);
    }

    
}
