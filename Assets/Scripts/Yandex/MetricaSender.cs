using System.Collections.Generic;
using YG;

public class MetricaSender
{    
    public static void SendLevelStatus(string levelName, LevelStatus status)
    {
        string goalKey = $"{levelName}_{status}";
        var eventParams = new Dictionary<string, string>
        {
             { "LevelsPassing", goalKey }
        };
        YandexMetrica.Send("LevelsPassing", eventParams);
    }

    public static void SendInventoryData(string eventName)
    {
        var eventParams = new Dictionary<string, string>
        {
             { "InventoryData", eventName }
        };
        YandexMetrica.Send("InventoryData", eventParams);
    }

    public static void SendFirstControllerTouch()
    {
        var eventParams = new Dictionary<string, string>
        {
             { "LevelsPassing", "FirstControllerTouch" }
        };
        YandexMetrica.Send("LevelsPassing", eventParams);
    }

    public static void SendTutorialData(string eventName)
    {
        var eventParams = new Dictionary<string, string>
        {
             { "TutorialPassing", eventName }
        };
        YandexMetrica.Send("TutorialPassing", eventParams);
    }

    public static void KillEnemyOnFirstLevel(string levelName, int enemyNumber, LevelEnemyStatus levelEnemyStatus)
    {
        string goalKey = $"{levelName}_Enemy_{enemyNumber}_{levelEnemyStatus}";
        var eventParams = new Dictionary<string, string>
        {
             { "LevelEnemyStatistic", goalKey }
        };
        YandexMetrica.Send("LevelEnemyStatistic", eventParams);
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