using System.Collections;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public static ShakeCamera Instance;
    //[SerializeField] float _shakeDuration = 0.2f;
    

    Vector3 _initialPos;
    float _shakeIntensity = 0.1f;
    float _currentChakeDuration = 0.0f;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void Start()
    {
        _initialPos = Camera.main.transform.localPosition;
        //Camera.main.transform.localPosition = new Vector3(100,0,0);
    }
    void Update()
    {

        Vector3 randomOffset = Random.insideUnitSphere * _shakeIntensity;
        transform.position = _initialPos + randomOffset;
        _currentChakeDuration -= Time.deltaTime;
        //Debug.LogWarning(_initialPos + randomOffset);



        //if (_currentChakeDuration > 0)
        //{
        //    Vector3 randomOffset = Random.insideUnitSphere * _shakeIntensity;
        //    transform.position = _initialPos + randomOffset;
        //    _currentChakeDuration -= Time.deltaTime;
        //    Debug.LogWarning(_initialPos + randomOffset);
        //}
        //else
        //{
        //    Camera.main.transform.localPosition = _initialPos;
        //}
    }

    public void Shake(float duration, float intensity)
    {
        _shakeIntensity += intensity;
        _currentChakeDuration = duration;
        StartCoroutine(ReturnShakeIntensity(duration, intensity));
    }

    IEnumerator ReturnShakeIntensity(float duration, float intensity)
    {
        yield return new WaitForSeconds(duration);
        _shakeIntensity -= intensity;
    }
}
