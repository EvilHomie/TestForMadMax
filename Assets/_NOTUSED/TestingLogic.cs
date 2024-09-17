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

    void Update()
    {
        if(_fpsCounter == null) return;
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
    }
    private void ResetProgress()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }
}


