using System.Collections.Generic;
using YG;

public class MetricaSender
{    
    public static void SendLevelStatus(LevelStatus status)
    {
        UILevelInfo selectedLevelInfo = LevelManager.Instance.GetSelectedLevelinfo();
        string goalKey = $"{selectedLevelInfo.LevelParameters.LevelName}_{status}";
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

}

public enum LevelStatus
{
    Start,
    Done,
    Failed,
}