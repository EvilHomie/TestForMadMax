using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelWindow : MonoBehaviour
{
    public static SelectLevelWindow Instance;

    [SerializeField] Transform _levelsContainer;
    [SerializeField] TextMeshProUGUI _headText;
    [SerializeField] ScrollRect _levelsScrollRect;
    [SerializeField] Button _closeButton;

    List<UILevelInfo> _UILevelInfos = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }



    public void Init(LevelsData levelsData, UILevelInfo uILevelInfo)
    {
        _headText.text = TextConstants.LEVELS;
        foreach (Transform transform in _levelsContainer)
        {
            Destroy(transform.gameObject);
        }

        foreach (LevelParameters levelParameters in levelsData.Levels)
        {
            UILevelInfo info = Instantiate(uILevelInfo, _levelsContainer);
            LevelParameters levelParametersCopy = Instantiate(levelParameters);
            info.Init(_levelsScrollRect, levelParametersCopy);
            _UILevelInfos.Add(info);
        }
        _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public List<UILevelInfo> GetCreatedLevels()
    {
        return _UILevelInfos;
    }
}
