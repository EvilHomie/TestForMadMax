using System.Collections;
using UnityEngine;

public class VehicleMovementController : MonoBehaviour
{
    [SerializeField] Transform[] _wheels;
    [SerializeField] ParticleSystem[] _wheelsDustPS;
    [SerializeField] bool _isPlayerVehicle = false;

    Vector3 _startPos;
    float _currentOffset;
    float _translateDuration;
    float _lastMoveSpeed;
    bool _reachGameZone = false;

    private void Start()
    {
        _startPos = transform.position;
        _translateDuration = Random.Range(GameLogicParameters.Instance.MinTranslateDuration, GameLogicParameters.Instance.MaxTranslateDuration);
        UpdateDustPS();
        if (_isPlayerVehicle) return;
        StartCoroutine(TranslateToPlayer());
    }

    void FixedUpdate()
    {
        RotateWheels();

        if (!_isPlayerVehicle && _reachGameZone)
        {
            SlideVehicle();
        }

        if (_lastMoveSpeed != RaidManager.Instance.PlayerMoveSpeed)
        {
            UpdateDustPS();
            _lastMoveSpeed = RaidManager.Instance.PlayerMoveSpeed;
        }
    }
    void RotateWheels()
    {
        foreach (var wheel in _wheels) wheel.Rotate(GameLogicParameters.Instance.WheelsRotateSpeedMod * RaidManager.Instance.PlayerMoveSpeed, 0, 0, Space.Self);
    }

    void UpdateDustPS()
    {
        if (_wheelsDustPS == null) return;

        foreach (var dust in _wheelsDustPS)
        {
            var main = dust.main;
            main.startSpeed = GameLogicParameters.Instance.DustPSSpeedMod * RaidManager.Instance.PlayerMoveSpeed;

            var emmision = dust.emission;
            emmision.rateOverTime = GameLogicParameters.Instance.DustPSEmmisionRateMod * RaidManager.Instance.PlayerMoveSpeed;
        }
    }

    void SlideVehicle()
    {
        float offsetX = _currentOffset * Mathf.Cos(Time.time);

        float xPos = _startPos.x + offsetX;

        transform.position = new Vector3(xPos, transform.position.y, _startPos.z);
    }

    IEnumerator TranslateToPlayer()
    {
        yield return null;
        float randomXOffset = Random.Range(-GameLogicParameters.Instance.GameZoneXSize, GameLogicParameters.Instance.GameZoneXSize);
        Vector3 targetPos = new(randomXOffset, _startPos.y, _startPos.z);

        float t = 0;
        while (t <= 1)
        {

            if (t >= GameLogicParameters.Instance.ValueToStartSlowTranslate)
            {
                t += Time.deltaTime / _translateDuration / GameLogicParameters.Instance.SlowTranslateValue;
            }
            else
            {
                t += Time.deltaTime / _translateDuration;
            }
            transform.position = Vector3.Lerp(_startPos, targetPos, t);
            yield return null;
        }
        _startPos = targetPos;

        InvokeRepeating(nameof(ChangeOffset), 0, GameLogicParameters.Instance.ChangeSlideOffsetDelay);
        _reachGameZone = true;
    }

    void ChangeOffset()
    {
        StartCoroutine(LerpXOffset());
    }

    IEnumerator LerpXOffset()
    {
        float _targetOffset = Random.Range(-GameLogicParameters.Instance.SlideXValue, GameLogicParameters.Instance.SlideXValue);
        float startOffset = _currentOffset;

        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / 5;
            _currentOffset = Mathf.Lerp(startOffset, _targetOffset, t);
            yield return null;
        }
    }

}
