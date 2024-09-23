using UnityEngine;
using YG;

public class MetricaSender
{
    //static string _Done = "Done";
    //static string _Failed = "Failed";


    public static void SendLevelStatus(UILevelInfo levelInfo, LevelStatus status)
    {
        string goalKey = $"{levelInfo.LevelName}{status}";
        YandexMetrica.Send(goalKey);
    }

}

public enum LevelStatus
{
    Start,
    Done,
    Failed
}