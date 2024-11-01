using System.Collections;
using TMPro;
using UnityEngine;

public class UIWaveIsApproachingPanel : MonoBehaviour
{
    public static UIWaveIsApproachingPanel Instance;
    [SerializeField] TextMeshProUGUI _waveIsApproachingText;
    //[SerializeField] float _showDelay;
    [SerializeField] float _showDuration;
    [SerializeField] float _flickeringSpeedMod;
    //[SerializeField] float _flickeringCount;
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
        StartCoroutine(ShowTextCoroutine());
    }

    public void ShowBossText()
    {
        _waveIsApproachingText.text = TextConstants.BOSSISAPPROACHING;
        StartCoroutine(ShowTextCoroutine());
    }

    public void HideText()
    {
        StopAllCoroutines();
        _canvasGroup.alpha = 0;
    }

    IEnumerator ShowTextCoroutine()
    {
        //yield return new WaitForSeconds(_showDelay);

        float currentShowDuration = 0;
        while (currentShowDuration < _showDuration)
        {
            currentShowDuration += Time.deltaTime;

            float t = (Mathf.Sin(Time.time * _flickeringSpeedMod) + 1) / 2;
            _canvasGroup.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
        HideText();
    }
}
