//using System.Collections;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class Manager : MonoBehaviour
//{
//    //[SerializeField] float _globalMoveSpeed;

//    //[SerializeField] float _newSpeed = 5f;
//    //[SerializeField] float _timeForReachNewSpeed;
//    //[SerializeField] float _t = 0;
//    //[SerializeField] TextMeshProUGUI _fpsCounter;

//    //public static float GlobalMoveSpeed;

//    //bool _speedUpdated;

//    //float _lastSpeed = 5f;

//    //Coroutine _UpdateSpeedCoroutine;

//    //[SerializeField] Image _sliderFillImage;

//    //private void Start()
//    //{
//    //    //Application.targetFrameRate = 240;
//    //}

//    //private void Update()
//    //{
//    //    GlobalMoveSpeed = _globalMoveSpeed;
//    //    _fpsCounter.text = string.Format("{0:f0}", 1 / Time.deltaTime);
//    //    //_fpsCounter.text = (1/Time.deltaTime).ToString();

//    //    //if (!_speedUpdated)
//    //    //{


//    //    //    _globalMoveSpeed = Mathf.Lerp(_globalMoveSpeed, _newSpeed, _t);
//    //    //}

//    //    //_t += Time.deltaTime / _timeForReachNewSpeed;
//    //    //if (_t > 1.0f)
//    //    //{
//    //    //    _t = 0.0f;
//    //    //}

//    //}

//    //public void ChangeSpeed(float value)
//    //{
//    //    _newSpeed = 5f + value * 5f;

//    //    if (_UpdateSpeedCoroutine != null)
//    //    {            
//    //        StopCoroutine(_UpdateSpeedCoroutine);
//    //        _UpdateSpeedCoroutine = StartCoroutine(LerpSpeed(_newSpeed, value));
//    //    }
//    //    else
//    //    {
//    //        _UpdateSpeedCoroutine = StartCoroutine(LerpSpeed(_newSpeed, value));
//    //    }


//    //    //_globalMoveSpeed = 5f + value * 5f;
//    //}


//    //IEnumerator LerpSpeed(float newSpeed, float sliderValue)
//    //{
//    //    _t = 0;
//    //    float startFillValue = _sliderFillImage.fillAmount;
//    //    _lastSpeed = _globalMoveSpeed;
//    //    while (_t <= 1)
//    //    {
//    //        _t += Time.deltaTime / _timeForReachNewSpeed;
//    //        _globalMoveSpeed = Mathf.Lerp(_lastSpeed, newSpeed, _t);
//    //        _sliderFillImage.fillAmount = Mathf.Lerp(startFillValue, sliderValue, _t);
//    //        yield return null;
//    //    }
//    //    _UpdateSpeedCoroutine = null;
//    //}
    
//}
