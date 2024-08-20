using UnityEngine;
using UnityEngine.UI;

public class UIHPBarsManager : MonoBehaviour
{
    public static UIHPBarsManager Instance;

    [SerializeField] Slider _HullHPSlider;
    [SerializeField] Slider _ShieldHPSlider;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void OnPlayerStartRaid()
    {

    }
}
