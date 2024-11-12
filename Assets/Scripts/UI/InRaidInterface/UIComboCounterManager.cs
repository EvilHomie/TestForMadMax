using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIComboCounterManager : MonoBehaviour
{
    public static UIComboCounterManager Instance;

    [SerializeField] TextMeshProUGUI _comboText;
    [SerializeField] TextMeshProUGUI _comboCounterText;
    [SerializeField] Slider _durationSlider;
    [SerializeField] float _duration;
    [SerializeField] AnimationCurve _textAnimationCurve;

    CanvasGroup _canvasGroup;
    string _valueText;
    Coroutine _resetComboCoroutine;
    Coroutine _animationsCoroutine;
    float _lastEnemyDieTime;
    float _timeForBreakCombo;
    int _comboInRowCount;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

    }
    public void Init()
    {
        _comboText.text = TextConstants.COMBO;
        _canvasGroup = _comboCounterText.transform.parent.GetComponent<CanvasGroup>();
        ResetData();
    }
    void ResetData()
    {
        _canvasGroup.alpha = 0.0f;
        _resetComboCoroutine = null;
        _comboInRowCount = 0;
    }

    public void OnPlayerStartRaid()
    {
        ResetData();
    }

    public void OnPlayerEndRaid()
    {
        StopAllCoroutines();
        ResetData();
    }

    public void OnEnemyKilled()
    {
        _comboInRowCount++;
        _timeForBreakCombo = Time.time + _duration;
        _resetComboCoroutine ??= StartCoroutine(ResetCombo());
        if (_comboInRowCount > 1)
        {
            _animationsCoroutine ??= StartCoroutine(Animations());
            _valueText = $"X{_comboInRowCount}";
            _canvasGroup.alpha = 1.0f;
            _comboCounterText.text = _valueText;
        }
    }

    IEnumerator ResetCombo()
    {
        float remainingTimeOneToZero;
        while (Time.time < _timeForBreakCombo)
        {
            remainingTimeOneToZero = (_timeForBreakCombo - Time.time) / _duration;
            _durationSlider.value = remainingTimeOneToZero;
            yield return null;
        }
        ResetData();
    }

    IEnumerator Animations()
    {
        float t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime;
            if (t >= 1) t = 1;

            _comboText.transform.localScale = Vector3.one * _textAnimationCurve.Evaluate(t);
            yield return null;
        }
        _animationsCoroutine = null;
    }
}
