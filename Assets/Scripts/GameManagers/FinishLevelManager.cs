using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinishLevelManager : MonoBehaviour
{
    public static FinishLevelManager Instance;

    [SerializeField] Image _blackoutImage;
    [SerializeField] float _blackoutDuration;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    private void Start()
    {
        _blackoutImage.gameObject.SetActive(false);
    }

    public void OnFinishLevel()
    {
        StartCoroutine(OnLevelClearLogic());        
        
    }

    IEnumerator OnLevelClearLogic()
    {
        float t = 0;
        _blackoutImage.gameObject.SetActive(true);
        while (t <= 1)
        {
            t += Time.deltaTime / _blackoutDuration;
            Color color = Color.Lerp(Color.clear, Color.black, t);

            _blackoutImage.color = color;
            yield return null;
        }
        UILevelStatistic.Instance.ShowStatistic();
        GameManager.Instance.OnReturnToGarage();
    }

    public void HideBlackOutImage()
    {
        _blackoutImage.gameObject.SetActive(false);
    }
    
}
