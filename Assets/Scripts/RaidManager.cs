using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RaidManager : MonoBehaviour
{
    public static RaidManager Instance;

    [SerializeField] Image _speedSliderFillImage;
    [SerializeField] MeshRenderer _mainRoadRenderer;

    float _playerMoveSpeed = 5f;
    public float PlayerMoveSpeed => _playerMoveSpeed;

    Coroutine _UpdateSpeedCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    private void Start()
    {
        _playerMoveSpeed = GameLogicParameters.Instance.MinSpeed;        
    }

    void FixedUpdate()
    {
        MoveMainRoad();
    }

    void MoveMainRoad()
    {
        _mainRoadRenderer.material.mainTextureOffset += GameLogicParameters.Instance.MoveRoadMod * _playerMoveSpeed * Time.fixedDeltaTime * Vector2.left;
    }


    public void ChangeSpeed(float value)
    {
        float newSpeed = GameLogicParameters.Instance.MinSpeed + value * GameLogicParameters.Instance.MinSpeed;

        if (_UpdateSpeedCoroutine != null)
        {
            StopCoroutine(_UpdateSpeedCoroutine);
            _UpdateSpeedCoroutine = StartCoroutine(LerpSpeed(newSpeed, value));
        }
        else
        {
            _UpdateSpeedCoroutine = StartCoroutine(LerpSpeed(newSpeed, value));
        }
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
}
