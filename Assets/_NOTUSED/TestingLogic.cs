using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class TestingLogic : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _fpsCounter;
    [SerializeField] Button _reset;

    int _fps = 0;
    string _strFPS;
    private void Awake()
    {
        _reset.onClick.AddListener(ResetProgress);
    }

    private void Start()
    {
        //Debug.LogWarning(FInishStatus.Done);
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
            //Debug.LogWarning("SAVE CLEAR");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UIResourcesManager.Instance.AddResources(1000, 1000, 1000);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            TrigerSend("B");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            TrigerSend("N");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            TrigerSend("M");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Send("ZZZ");
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Send("lvl1");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Send("lvl2");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Send("lvl3");
        }

    }
    private void ResetProgress()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
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


