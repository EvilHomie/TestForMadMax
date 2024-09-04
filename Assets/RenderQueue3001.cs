using UnityEngine;

public class RenderQueue3001 : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Renderer>().material.renderQueue = 3000;
    }
}
