using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public static ShakeCamera Instance;
    [SerializeField] float _shakeDuration = 0.2f;
    [SerializeField] float _shakeIntensity = 0.1f;

    Vector3 _initialPos;
    float _currentChakeDuration = 0.0f;

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
        if (_currentChakeDuration > 0)
        {
            Vector3 randomOffset = Random.insideUnitSphere * _shakeIntensity;
            Camera.main.transform.localPosition = _initialPos + randomOffset;
            _currentChakeDuration -= Time.deltaTime;
        }
        else
        {
            Camera.main.transform.localPosition = _initialPos;
        }
    }

    public void Shake()
    {
        _currentChakeDuration = _shakeDuration;
    }
}
