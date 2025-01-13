using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHpPanel : MonoBehaviour
{
    public static UIPlayerHpPanel Instance;

    [SerializeField] Slider _hullHPSlider;
    [SerializeField] Slider _shieldHPSlider;
    [SerializeField] HorizontalLayoutGroup _hullHPseparatorsContainer;
    [SerializeField] HorizontalLayoutGroup _shieldHPseparatorsContainer;
    [SerializeField] RectTransform _separatorPF;


    RectTransform _hullHPRT;
    RectTransform _shieldHPRT;


    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        _hullHPRT = _hullHPSlider.GetComponent<RectTransform>();
        _shieldHPRT = _shieldHPSlider.GetComponent<RectTransform>();
    }

    public void UpdateHPBars(float HPValue, float shieldValue)
    {
        _hullHPSlider.value = HPValue;
        _shieldHPSlider.value = shieldValue;
    }

    public void ConfigHPSeparators(float hullHPSepAmount, float shieldHPSepAmount)
    {
        Debug.LogWarning($"HUUL{hullHPSepAmount}   SHIELD{shieldHPSepAmount}");

        UpdateSeparators(hullHPSepAmount, _hullHPseparatorsContainer);
        UpdateSeparators(shieldHPSepAmount, _shieldHPseparatorsContainer);
    }



    void UpdateSeparators(float count, HorizontalLayoutGroup containerGroup)
    {
        foreach (Transform child in containerGroup.transform)
        {
            Destroy(child.gameObject);
        }

        float space = (_hullHPRT.rect.width - _separatorPF.rect.width * (count + 1)) / count;

        containerGroup.spacing = space /*+ _separatorPF.rect.width*/;

        for (int i = 0; i <= count; i++)
        {
            Instantiate(_separatorPF, containerGroup.transform);
        }
    }
}
