using TMPro;
using UnityEngine;

public class TestingLogic : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _fpsCounter;

    void Update()
    {
        _fpsCounter.text = string.Format("{0:f0}", 1 / Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.T))
            Time.timeScale += 1;

        if (Input.GetKeyDown(KeyCode.Y))
            Time.timeScale -= 1;
    }
}

