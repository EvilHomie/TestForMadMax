using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class TestingLogic : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _fpsCounter;
    [SerializeField] TextMeshProUGUI _log;
    [SerializeField] Button _reset;
    [SerializeField] Button _ADD;
    [SerializeField] Button _unlock;
    int _fps = 0;
    string _strFPS;
    private void Awake()
    {
        _reset.onClick.AddListener(ResetProgress);
        _ADD.onClick.AddListener(UnlockAllData);
        //_unlock.onClick.AddListener(UnlockAllData);
    }

    private void Start()
    {
        Application.logMessageReceived += ShowMessageOnDisplay;
    }

    private void ShowMessageOnDisplay(string logString, string stackTrace, LogType type)
    {
        _log.text = logString +"     " + stackTrace;
    }
    void UnlockAllData()
    {
        //ResetProgress(_TESTdeffaultItemsNames);
        UIResourcesManager.Instance.AddResources(3000, 3000, 3000);
        PlayerData.Instance.UnlockedLevelsNames = new() { "1-1", "1-2", "1-3", "1-4", "1-5", "1-6", "1-7", "1-8", "1-9", "1-10", "2-1", "2-2", "2-3", "2-4", "2-5", "2-6", "2-7", "2-8", "2-9", "2-10", "3-1" };
    }


    void Update()
    {
        if (_fpsCounter == null) return;
        _fps = (int)(1 / Time.deltaTime);
        _fpsCounter.text = _fps.ToString();

        //if (Input.GetKeyDown(KeyCode.T))
        //    Time.timeScale += 1;

        //if (Input.GetKeyDown(KeyCode.Y))
        //    Time.timeScale -= 1;

        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    SaveLoadManager.Instance.SaveData();

        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    SaveLoadManager.Instance.LoadSaveData();
        //}
        if (Input.GetKeyDown(KeyCode.C))
        {
            YandexGame.ResetSaveProgress();
            YandexGame.SaveProgress();

            //PlayerPrefs.DeleteAll();
            Debug.LogWarning("SAVE CLEAR");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UIResourcesManager.Instance.AddResources(1000, 1000, 1000);
        }

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    TrigerSend("B");
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    TrigerSend("N");
        //}
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    TrigerSend("M");
        //}
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    Send("ZZZ");
        //}

        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    Send("lvl1");
        //}
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    Send("lvl2");
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Send("lvl3");
        //}

    }
    private void ResetProgress()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }
    void ADD()
    {
        UIResourcesManager.Instance.AddResources(1000, 1000, 1000);
    }



    public void Send(string id)
    {
        YandexMetrica.Send(id);
    }
    public void TrigerSend(string name)
    {
        var eventParams = new Dictionary<string, string>
        {
             { "triggers", name }
        };
        YandexMetrica.Send("triggers", eventParams);
    }
}


