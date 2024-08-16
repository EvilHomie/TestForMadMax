using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RaidManager : MonoBehaviour
{
    public static RaidManager Instance;

    [SerializeField] Image _speedSliderFillImage;
    [SerializeField] MeshRenderer _mainRoadRenderer;

    float _playerMoveSpeed = 0f;
    public float PlayerMoveSpeed => _playerMoveSpeed;

    Coroutine _UpdateSpeedCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void Update()
    {
        MoveMainRoad();
    }

    void MoveMainRoad()
    {
        _mainRoadRenderer.material.mainTextureOffset += GameConfig.Instance.MoveRoadMod * _playerMoveSpeed * Time.deltaTime * Vector2.left;
    }


    public void ChangeSpeedWhileInRaid(float sliderValue)
    {
        float newSpeed = GameConfig.Instance.MinPlayerSpeed + sliderValue * GameConfig.Instance.MinPlayerSpeed;

        if (_UpdateSpeedCoroutine != null)
        {
            StopCoroutine(_UpdateSpeedCoroutine);
            _UpdateSpeedCoroutine = StartCoroutine(LerpSpeed(newSpeed, sliderValue));
        }
        else
        {
            _UpdateSpeedCoroutine = StartCoroutine(LerpSpeed(newSpeed, sliderValue));
        }
    }

    public void ChangeSpeedOnStartRaid(float startMoveDelay, float speed, float reachSpeedDuration)
    {
        StartCoroutine(StartLerpSpeed(startMoveDelay, speed, reachSpeedDuration));
    }
    public void OnPlayerEndRaid()
    {
        StopAllCoroutines();
        _playerMoveSpeed = 0;
        _UpdateSpeedCoroutine = null;
    }


    IEnumerator LerpSpeed(float newSpeed, float sliderValue)
    {
        float t = 0;
        float startFillValue = _speedSliderFillImage.fillAmount;
        float _lastSpeed = _playerMoveSpeed;
        while (t <= 1)
        {
            t += Time.deltaTime / GameConfig.Instance.TimeForChangeSpeed;
            _playerMoveSpeed = Mathf.Lerp(_lastSpeed, newSpeed, t);
            _speedSliderFillImage.fillAmount = Mathf.Lerp(startFillValue, sliderValue, t);
            yield return null;
        }
        _UpdateSpeedCoroutine = null;
    }

    IEnumerator StartLerpSpeed(float startMoveDelay, float speed, float reachSpeedDuration)
    {
        yield return new WaitForSeconds(startMoveDelay);
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / reachSpeedDuration;
            _playerMoveSpeed = Mathf.Lerp(0, speed, t);
            yield return null;
        }
        _UpdateSpeedCoroutine = null;
    }
}
