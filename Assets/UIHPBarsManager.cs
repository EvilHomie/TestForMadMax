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

    public void UpdateHPBars(float HPValue, float shieldValue)
    {
        Debug.Log($"{HPValue}    {shieldValue}");
        _HullHPSlider.value = HPValue;
        _ShieldHPSlider.value = shieldValue;
    }
}
