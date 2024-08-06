using System.Collections;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public static ShakeCamera Instance;    

    Vector3 _initialPos;
    float _shakeIntensity = 0.1f;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void Start()
    {
        _initialPos = Camera.main.transform.localPosition;
    }
    void Update()
    {

        Vector3 randomOffset = Random.insideUnitSphere * _shakeIntensity;
        transform.position = _initialPos + randomOffset;
    }

    public void Shake(float duration, float intensity)
    {
        _shakeIntensity += intensity;
        StartCoroutine(ReturnShakeIntensity(duration, intensity));
    }

    IEnumerator ReturnShakeIntensity(float duration, float intensity)
    {
        yield return new WaitForSeconds(duration);
        _shakeIntensity -= intensity;
    }
}
