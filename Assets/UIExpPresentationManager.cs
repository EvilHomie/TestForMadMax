using UnityEngine;
using UnityEngine.UI;
using YG;

public class UIExpPresentationManager : MonoBehaviour
{
    public static UIExpPresentationManager Instance;

    [SerializeField] Image _fillingImage;
    [SerializeField] GameObject _tipPCGO;

    int _forLevelUpAmount;

    int _totalKilledCount;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init(int forLevelUpAmount)
    {
        _forLevelUpAmount = forLevelUpAmount;
        gameObject.SetActive(false);
    }

    public void OnStartSurviveMode()
    {
        _fillingImage.fillAmount = 0;
        _tipPCGO.SetActive(false);
        gameObject.SetActive(true);
    }

    public void OnStopSurviveMode() 
    {
        gameObject.SetActive(false);
    }

    public void OnKillEnemy(int killedCount)
    {
        _totalKilledCount = killedCount;
        if (_fillingImage.fillAmount >= 1) return;
        _fillingImage.fillAmount += 1f / _forLevelUpAmount;
        if (_fillingImage.fillAmount >= 1)
        {
            if (YandexGame.EnvironmentData.isDesktop)
            {
                _tipPCGO.SetActive(true);
            }
        }
    }

    public void OnOpenUpgrades()
    {
        int remainder = _totalKilledCount % _forLevelUpAmount;

        _fillingImage.fillAmount = (float)remainder / _forLevelUpAmount;
        _tipPCGO.SetActive(false);
    }
}
