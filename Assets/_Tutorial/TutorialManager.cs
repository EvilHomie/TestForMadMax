using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField] float _whiteAreaTransformationDuration = 1f;
    [SerializeField] TutorialTextPanel _tutorialTextPanel;
    [SerializeField] RectTransform _tutorialArrow;
    [SerializeField] RectTransform _clickableAreaRT;
    [SerializeField] Image _clickableAreaBlockRaycastImage;
    [SerializeField] GameObject _tutorialInteractWindow;
    [SerializeField] Button _fullScreenConfirmButton;

    TutorialShowStageManager _tutorialShowStageManager;
    TutorialStageAditionActionManager _tutorialStageAditionActionManager;
    CheckTutorialStages _checkTutorialStages;

    int _upgradeRotationSpeedCounter = 0;
    int _upgradeFireRateCounter = 0;

    [SerializeField] List<TutorialStage> _tutorialStages;

    Dictionary<StageName, TutorialStage> _tutorialStageByName;

    StageName _lastStageName;
    public int UpgradeRotationSpeedCounter { get => _upgradeRotationSpeedCounter; set => _upgradeRotationSpeedCounter = value; }
    public int UpgradeFireRateCounter { get => _upgradeFireRateCounter; set => _upgradeFireRateCounter = value; }

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _tutorialInteractWindow.SetActive(false);
        if (YandexGame.savesData.tutorilaIsComplete)
        {
            EndTutorial();
            return;
        }
        _checkTutorialStages = GetComponent<CheckTutorialStages>();
        //_checkTutorialStages.Init();


        _tutorialShowStageManager = GetComponent<TutorialShowStageManager>();
        _tutorialStageAditionActionManager = GetComponent<TutorialStageAditionActionManager>();
        _tutorialShowStageManager.Init(_tutorialArrow, _tutorialTextPanel, _clickableAreaRT, _clickableAreaBlockRaycastImage, _whiteAreaTransformationDuration, _fullScreenConfirmButton);
        _tutorialStageAditionActionManager.Init(_clickableAreaRT);
        _tutorialTextPanel.ConfirmationButton.onClick.AddListener(delegate { TryConfirmStage(_lastStageName); });
        _fullScreenConfirmButton.onClick.AddListener(delegate { TryConfirmStage(_lastStageName); });

        _tutorialStageByName = new();
        foreach (var stage in _tutorialStages)
        {
            if (stage.stageName == StageName.None) continue;
            _tutorialStageByName.Add(stage.stageName, stage);
        }
        TryEnableStage(StageName.Greetings);
    }

    public void EnableStage(StageName stageName)
    {
        Cursor.visible = true;
        GameFlowManager.Instance.SetPause(this);
        _tutorialInteractWindow.SetActive(true);
        _lastStageName = stageName;
        TutorialStage tutorialStage = _tutorialStageByName[stageName];
        _tutorialStageAditionActionManager.AditionStageActionOnEnable(stageName);
        _tutorialShowStageManager.ConfigureStage(tutorialStage);
        MetricaSender.SendTutorialData($"{stageName} Enable");
    }

    public void ConfirmStage(StageName stageName)
    {
        Cursor.visible = false;
        GameFlowManager.Instance.Unpause(this);
        _tutorialShowStageManager.OnPassStage();
        _tutorialInteractWindow.SetActive(false);
        _tutorialStageAditionActionManager.AditionStageActionOnDisable(stageName);
        MetricaSender.SendTutorialData($"{stageName} Confirm");
        if (_tutorialStageByName.Last().Key == stageName)
        {
            EndTutorial();
            return;
        }

        StageName stageOnConfirm = _tutorialStageByName[stageName].stageOnConfirm;
        if (stageOnConfirm != StageName.None)
        {
            EnableStage(stageOnConfirm);
        }
    }

    void EndTutorial()
    {
        YandexGame.savesData.tutorilaIsComplete = true;
        SaveLoadManager.Instance.SaveData();
        _clickableAreaRT.gameObject.SetActive(false);
        _tutorialTextPanel.ConfirmationButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }

    public bool TryEnableStage(StageName stageName)
    {
        if (YandexGame.savesData.tutorilaIsComplete) return false;
        if (YandexGame.savesData.completedTutorialStages.Contains(stageName)) return false;
        if (_tutorialStageByName[stageName] == null) return false;


        EnableStage(stageName);
        return true;
    }
    public bool TryConfirmStage(StageName stageName)
    {
        if (YandexGame.savesData.tutorilaIsComplete) return false;
        if (YandexGame.savesData.completedTutorialStages.Contains(stageName)) return false;
        if (_tutorialStageByName[stageName] == null) return false;


        ConfirmStage(stageName);
        AddStageToCompletedTutorialStagesList(stageName);
        return true;
    }

    void AddStageToCompletedTutorialStagesList(StageName stageName)
    {
        //Debug.LogWarning(stageName.ToString());
        if (!YandexGame.savesData.completedTutorialStages.Contains(stageName))
        {
            YandexGame.savesData.completedTutorialStages.Add(stageName);
            SaveLoadManager.Instance.SaveData();
        }
    }

}

[Serializable]
public class TutorialStage
{
    public string redactorStageName;
    public StageName stageName;
    public string stageText;
    //public bool stageFinished;
    public bool stageConfirmViaScript;
    public RectTransform selectedUIElement;
    //public Vector2 areaAditionOffset;
    //public Vector2 areaAditionSize;
    public bool withArrow;
    //public bool objectIsRound;
    public bool blockRaycastInCenter;
    public SidePositions arrowSidePositions;
    public SidePositions textPivotPositions;
    public Vector2 textPanelAditionOffset;
    public StageName stageOnConfirm;
    //public List<GameObject> stageConfirmationObjects;
}

public enum SidePositions
{
    ScreenCenter,
    TopLeft,
    TopMidle,
    TopRight,
    MidleLeft,
    MidleRight,
    BottomLeft,
    BottomMidle,
    BottomRight
}

public enum StageName
{
    None,
    Greetings,
    FirstRaidLaunch,
    ShowWaveWarning,
    FirstLevelCompleted,
    FirstOpenInventory,
    ShowEquipedPanel,
    SelectFirstWeapon,
    ShowUpgradeDiscription,
    UpgradeRotateSpeed,
    CloseInventory,
    WishGoodluck,
    FirstLevelFailed,
    OnLevelFailed,
    ShowInventoryButton,
    ShowLevelStatisticPanel,
    UpgradeFireRate,
    SecondLevelCompleted
}