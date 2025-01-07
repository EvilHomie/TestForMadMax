using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeLevelButton : MonoBehaviour
{
    public static UIChangeLevelButton Instance;
    [SerializeField] TextMeshProUGUI _levelNumberText;
    [SerializeField] TextMeshProUGUI _levelFullNameText;
    [SerializeField] Image _levelImage;
    [SerializeField] LevelProgressPresentation _levelProgressPresentation;


    Dictionary<string, List<LevelParameters>> _allLevelsByChapter;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {

    }

    public void UpdateSelectedLevel(UILevelInfo uILevelInfo)
    {
        if (_allLevelsByChapter == null) FillAllLevelsData();

        _levelImage.sprite = uILevelInfo.LevelParameters.LevelImage;
        _levelNumberText.text = uILevelInfo.LevelParameters.LevelName;
        _levelFullNameText.text = TextConstants.LEVELSFULLNAMES[uILevelInfo.LevelParameters.LevelChapterName];

        Dictionary<string, List<string>> unlockedLevelsInChapter = new();

        foreach (var levelName in PlayerData.Instance.UnlockedLevelsNames)
        {
            string[] splitedName = levelName.Split('-');

            if (unlockedLevelsInChapter.ContainsKey(splitedName[0]) == false)
            {
                unlockedLevelsInChapter.Add(splitedName[0], new() { splitedName[1] });
            }
            else
            {
                unlockedLevelsInChapter[splitedName[0]].Add(splitedName[1]);
            }
        }

        string[] selectedLevelParts = uILevelInfo.LevelParameters.LevelName.Split('-');
        string chapterNumber = selectedLevelParts[0];
        int selectedLevelNumber = int.Parse(selectedLevelParts[1]);
        //int levelsAmountInChapter = _allLevelsByChapter[chapterNumber].Count;

        List<LevelParameters> chapterLevelsParameters = _allLevelsByChapter[chapterNumber];

        int unlockLevelsInChapterAmount = unlockedLevelsInChapter[chapterNumber].Count;

        _levelProgressPresentation.ConfigPanel(chapterLevelsParameters, unlockLevelsInChapterAmount, selectedLevelNumber);


        //foreach (var item in unlockedLevelsIncHapter)
        //{            
        //    foreach (var levelName in item.Value)
        //    {
        //        Debug.LogWarning($"{item.Key}   {levelName}");
        //    }
        //}
    }
    void FillAllLevelsData()
    {
        _allLevelsByChapter = new();

        foreach (var levelParameters in LevelManager.Instance.LevelsData.Levels)
        {
            string[] splitedName = levelParameters.LevelName.Split('-');

            if (_allLevelsByChapter.ContainsKey(splitedName[0]) == false)
            {
                _allLevelsByChapter.Add(splitedName[0], new() { levelParameters });
            }
            else
            {
                _allLevelsByChapter[splitedName[0]].Add(levelParameters);
            }
        }

        //foreach (var item in _allLevelsByChapter)
        //{
        //    foreach (var levelName in item.Value)
        //    {
        //        Debug.LogWarning($"{item.Key}   {levelName}");
        //    }
        //}
    }
}
