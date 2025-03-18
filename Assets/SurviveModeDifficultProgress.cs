using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SurviveModeDifficultProgress : MonoBehaviour
{
    public static SurviveModeDifficultProgress Instance;
    [SerializeField] GameObject _scullPF;
    [SerializeField] RectTransform _scullsContainer;
    [SerializeField] Transform _difficultArrow;
    [SerializeField] Vector3 _minEulerAngles;
    [SerializeField] Vector3 _maxEulerAngles;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        _difficultArrow.rotation = Quaternion.Euler(_minEulerAngles);
        gameObject.SetActive(false);
    }

    public void OnStartMode()
    {
        _difficultArrow.rotation = Quaternion.Euler(_minEulerAngles);
        foreach (Transform c in _scullsContainer)
        {
            Destroy(c.gameObject);
        }
        gameObject.SetActive(true);
    }

    public void OnFinishMode()
    {
        _difficultArrow.rotation = Quaternion.Euler(_minEulerAngles);
        gameObject.SetActive(false);
    }

    public void UpdateSliderValue(float value)
    {
        Vector3 targetEulers = Vector3.Lerp(_minEulerAngles, _maxEulerAngles, value);

        _difficultArrow.rotation = Quaternion.Euler(targetEulers);
    }

    public void AddDifficultScull()
    {
        Instantiate(_scullPF, _scullsContainer);
    }
}
