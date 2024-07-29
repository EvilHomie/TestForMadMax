using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] float _globalMoveSpeed;

    [SerializeField] float _newSpeed = 5f;
    [SerializeField] float _timeForReachNewSpeed;
    [SerializeField] float _t = 0;

    public static float GlobalMoveSpeed;

    bool _speedUpdated;

    float _lastSpeed = 5f;

    Coroutine _UpdateSpeedCoroutine;

    [SerializeField] Image _sliderFillImage;

    private void Update()
    {
        GlobalMoveSpeed = _globalMoveSpeed;

        //if (!_speedUpdated)
        //{
            

        //    _globalMoveSpeed = Mathf.Lerp(_globalMoveSpeed, _newSpeed, _t);
        //}

        //_t += Time.deltaTime / _timeForReachNewSpeed;
        //if (_t > 1.0f)
        //{
        //    _t = 0.0f;
        //}

    }

    public void ChangeSpeed(float value)
    {
        _newSpeed = 5f + value * 5f;

        if (_UpdateSpeedCoroutine != null)
        {            
            StopCoroutine(_UpdateSpeedCoroutine);
            _UpdateSpeedCoroutine = StartCoroutine(LerpSpeed(_newSpeed, value));
        }
        else
        {
            _UpdateSpeedCoroutine = StartCoroutine(LerpSpeed(_newSpeed, value));
        }


        //_globalMoveSpeed = 5f + value * 5f;
    }


    IEnumerator LerpSpeed(float newSpeed, float sliderValue)
    {
        _t = 0;
        float startFillValue = _sliderFillImage.fillAmount;
        _lastSpeed = _globalMoveSpeed;
        while (_t <= 1)
        {
            Debug.LogWarning("NewSPEED");
            _t += Time.deltaTime / _timeForReachNewSpeed;
            _globalMoveSpeed = Mathf.Lerp(_lastSpeed, newSpeed, _t);
            _sliderFillImage.fillAmount = Mathf.Lerp(startFillValue, sliderValue, _t);
            yield return null;
        }
        _UpdateSpeedCoroutine = null;
    }
    
}
