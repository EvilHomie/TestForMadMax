using System.Collections.Generic;
using YG;

public class MetricaSender
{
    static void SendGoal(string goalName, string goalKey)
    {
        var eventParams = new Dictionary<string, string>
        {
             { goalName, goalKey }
        };
        YandexMetrica.Send(goalName, eventParams);
    }

    public static void SendLevelStatus(string levelName, LevelStatus status)
    {
        string goalKey = $"{levelName}_{status}";
        SendGoal("LevelsPassing", goalKey);
    }

    public static void SendInventoryData(string goalKey)
    {
        SendGoal("InventoryData", goalKey);
    }

    public static void SendFirstControllerTouch()
    {
        SendGoal("LevelsPassing", "FirstControllerTouch");
    }

    public static void SendTutorialData(string goalKey)
    {
        SendGoal("TutorialPassing", goalKey);
    }

    public static void KillEnemyOnFirstLevel(string levelName, int enemyNumber, LevelEnemyStatus levelEnemyStatus)
    {
        string goalKey = $"{levelName}_Enemy_{enemyNumber}_{levelEnemyStatus}";
        SendGoal("LevelEnemyStatistic", goalKey);
    }

    public static void QuickImprovementAfterLevel(string levelName, RewardStatus rewardStatus)
    {
        string goalKey = $"{levelName}_{rewardStatus}";
        SendGoal("QuickImprovement", goalKey);
    }

    public static void QuickImprovement(string levelName)
    {
        string goalKey = $"{levelName}_JustUpgrade";
        SendGoal("QuickImprovement", goalKey);
    }

    public static void RestoreHPByADD(string levelName, RewardStatus rewardStatus)
    {
        string goalKey = $"{levelName}_{rewardStatus}";
        SendGoal("RestoreHP", goalKey);
    }

    public static void SendSurviveModeGoal(SurviveModeGoal goalName, string parameter = null)
    {
        string goalKey = parameter == null ? $"{goalName}" : $"{goalName}_{parameter}";
        SendGoal("SurviveMode", goalKey);
    }
}

public enum LevelStatus
{
    Start,
    Done,
    Failed,
}
public enum LevelEnemyStatus
{
    Killed,
    Escaped
}

public enum QuickImprovement
{
    Error,
    AcceptOffer,
    CancelOffer,
    JustUpgrade
}

public enum SurviveModeGoal
{
    StartMode,
    OpenUpgrades,
    GetBonusCard,
    StopMode
}