using UnityEngine;
using UnityEngine.UI;

public class SurviveModeDifficultProgress : MonoBehaviour
{
    public static SurviveModeDifficultProgress Instance;

    [SerializeField] Slider _difficultSlider;
    [SerializeField] Transform _difficultsColorContainer;
    [SerializeField] GameObject _scullPF;
    [SerializeField] RectTransform _scullsContainer;

    public int DifficultLevelsAmount => _difficultsColorContainer.childCount;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void OnStartMode()
    {
        _difficultSlider.value = 0;
        foreach (Transform c in _scullsContainer)
        {
            Destroy(c.gameObject);
        }
        gameObject.SetActive(true);
    }

    public void OnFinishMode()
    {
        gameObject.SetActive(false);
    }

    public void UpdateSliderValue(float value)
    {
        _difficultSlider.value = value;
    }

    public void AddDifficultScull()
    {
        Instantiate(_scullPF, _scullsContainer);
    }
}
