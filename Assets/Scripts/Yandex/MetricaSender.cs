using System.Collections.Generic;
using YG;

public class MetricaSender
{
    //static string _Done = "Done";
    //static string _Failed = "Failed";


    public static void SendLevelStatus(UILevelInfo levelInfo, LevelStatus status)
    {
        string goalKey = $"{levelInfo.LevelName}_{status}";
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

}

public enum LevelStatus
{
    Start,
    Done,
    Failed,
}