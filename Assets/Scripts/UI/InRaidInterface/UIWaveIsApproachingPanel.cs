using System.Collections;
using TMPro;
using UnityEngine;

public class UIWaveIsApproachingPanel : MonoBehaviour
{
    public static UIWaveIsApproachingPanel Instance;
    [SerializeField] TextMeshProUGUI _waveIsApproachingText;
    [SerializeField] float _flickeringCount;
    [SerializeField] float _flickeringDelay;
    [SerializeField] float _flickeringOneTimeDuration;
    CanvasGroup _canvasGroup;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        
        _canvasGroup = GetComponent<CanvasGroup>();
        HideText();
    }

    public void ShowWaveText()
    {
        _waveIsApproachingText.text = TextConstants.WAVEISAPPROACHING;
        StartCoroutine(FlickeringAppearance());
    }

    public void ShowBossText()
    {
        _waveIsApproachingText.text = TextConstants.BOSSISAPPROACHING;
        StartCoroutine(FlickeringAppearance());
    }

    public void HideText()
    {
        StopAllCoroutines();
        _canvasGroup.alpha = 0;
    }   

    IEnumerator FlickeringAppearance()
    {
        for (int i = 1; i <= _flickeringCount; i++)
        {
            float t = 0;

            while (t <= 1)
            {
                t += Time.deltaTime / _flickeringOneTimeDuration;
                _canvasGroup.alpha = t;
                yield return null;
            }

            if (i == 2)
            {
                TutorialManager.Instance.TryEnableStage(StageName.ShowWaveWarning);
            }

            while (t >= 0)
            {
                t -= Time.deltaTime / _flickeringOneTimeDuration;
                _canvasGroup.alpha = t;
                yield return null;
            }

            yield return new WaitForSeconds(_flickeringDelay);
        }
        HideText();
    }
}
