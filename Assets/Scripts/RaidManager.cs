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
    //private void Start()
    //{
    //    _playerMoveSpeed = GameLogicParameters.Instance.MinSpeed;        
    //}

    void FixedUpdate()
    {
        MoveMainRoad();
    }

    void MoveMainRoad()
    {
        _mainRoadRenderer.material.mainTextureOffset += GameLogicParameters.Instance.MoveRoadMod * _playerMoveSpeed * Time.fixedDeltaTime * Vector2.left;
    }


    public void ChangeSpeed(float sliderValue)
    {
        float newSpeed = GameLogicParameters.Instance.MinSpeed + sliderValue * GameLogicParameters.Instance.MinSpeed;

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

    public void StartMove(float speed, float duration)
    {
        StartCoroutine(StartLerpSpeed(speed, duration));
    }


    IEnumerator LerpSpeed(float newSpeed, float sliderValue)
    {
        float t = 0;
        float startFillValue = _speedSliderFillImage.fillAmount;
        float _lastSpeed = _playerMoveSpeed;
        while (t <= 1)
        {
            t += Time.deltaTime / GameLogicParameters.Instance.TimeForChangeSpeed;
            _playerMoveSpeed = Mathf.Lerp(_lastSpeed, newSpeed, t);
            _speedSliderFillImage.fillAmount = Mathf.Lerp(startFillValue, sliderValue, t);
            yield return null;
        }
        _UpdateSpeedCoroutine = null;
    }

    IEnumerator StartLerpSpeed(float speed, float duration)
    {
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / duration;
            _playerMoveSpeed = Mathf.Lerp(0, speed, t);
            yield return null;
        }
        _UpdateSpeedCoroutine = null;
    }
}
