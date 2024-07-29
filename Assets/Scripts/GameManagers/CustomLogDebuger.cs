using UnityEngine;

public static class CustomLogDebuger
{
    public static void Log(string log)
    {
        if (GameConfig.Instance.IsTesting)
        {
            Debug.Log(log);
        }
    }
}
