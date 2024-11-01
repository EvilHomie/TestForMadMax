using UnityEngine;

public class RenderQueue3001 : MonoBehaviour
{
    public void Init()
    {
        GetComponent<Renderer>().material.renderQueue = 3000;
    }

    private void OnEnable()
    {
        Init();
    }
}
