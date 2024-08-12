using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    Vector3 _initialPos;
    float _passiveOnRaidShakeIntensity = 0.1f;
    float _shakeIntensity = 0;

    bool _playerOnRaid = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    //void Start()
    //{
    //    _initialPos = Camera.main.transform.localPosition;
    //}
    void Update()
    {
        if(!_playerOnRaid) return;
        Vector3 randomOffset = Random.insideUnitSphere * _shakeIntensity;
        transform.position = _initialPos + randomOffset;
    }

    public void ChangeInitPos(Vector3 newPos)
    {
        _initialPos = newPos;
    }

    public void OnPlayerStartRaid()
    {
        //_initialPos = Camera.main.transform.localPosition;
        _playerOnRaid = true;
        _shakeIntensity = _passiveOnRaidShakeIntensity;
    }

    public void OnPlayerEndRaid()
    {
        _playerOnRaid = false;
        _shakeIntensity = 0;
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
