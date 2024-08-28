using UnityEngine;

public static class SystemDebuger
{
    public static void Log(string log)
    {
        if (GameConfig.Instance.IsTesting)
        {
            Debug.Log(log);
        }
    }
}
