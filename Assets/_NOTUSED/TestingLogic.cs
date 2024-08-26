using TMPro;
using UnityEngine;

public class TestingLogic : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _fpsCounter;

    int _fps = 0;
    string _strFPS;

    void Update()
    {
        _fps = (int)(1 / Time.deltaTime);
        _fpsCounter.text = _fps.ToString();

        if (Input.GetKeyDown(KeyCode.T))
            Time.timeScale += 1;

        if (Input.GetKeyDown(KeyCode.Y))
            Time.timeScale -= 1;
    }
}


