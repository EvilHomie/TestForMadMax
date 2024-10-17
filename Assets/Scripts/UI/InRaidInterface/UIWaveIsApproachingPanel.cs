using UnityEngine;

public class UIWaveIsApproachingPanel : MonoBehaviour
{
    public static UIWaveIsApproachingPanel Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {

    }
}
