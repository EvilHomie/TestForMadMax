using UnityEngine;

public static class CustomLogDebuger
{
    public static void Log(object log)
    {
        if (GameConfig.Instance.EnableDebug)
        {
            Debug.Log(log);
        }
    }
}
