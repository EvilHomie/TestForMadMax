using System;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;
    public static Action<bool> GameFlowChangeStateOnPause;

    public static GameFlowState GameFlowState;

    List<object> _holdPauseobjects = new();

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    //private void Update()
    //{
    //    Debug.Log(GameFlowState);
    //}


    public void SetPause(object obj)
    {
        _holdPauseobjects.Add(obj);
        Time.timeScale = 0f;
        GameFlowChangeStateOnPause?.Invoke(true);
        GameFlowState = GameFlowState.PAUSE;
        //Debug.LogWarning("PAUSE");

       //var test = obj as MonoBehaviour;
       // Debug.LogWarning($"PAUSE     {test.gameObject.name}");
    }

    public void Unpause(object obj)
    {
        _holdPauseobjects.Remove(obj);
        if (_holdPauseobjects.Count == 0)
        {
            Time.timeScale = 1f;
            GameFlowChangeStateOnPause?.Invoke(false);
            GameFlowState = GameFlowState.UNPAUSE;
            //Debug.LogWarning("UNPAUSE");
        }

        //var test = obj as MonoBehaviour;
        //Debug.LogWarning($"UNPAUSE     {test.gameObject.name}");
    }
}

public enum GameFlowState
{
    NONE,
    PAUSE,
    UNPAUSE
}