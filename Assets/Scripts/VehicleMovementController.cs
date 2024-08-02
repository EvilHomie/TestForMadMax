using System.Collections;
using UnityEngine;

public class VehicleMovementController : MonoBehaviour
{
    [SerializeField] Transform[] _wheels;
    [SerializeField] ParticleSystem[] _wheelsDustPS;
    [SerializeField] bool _isPlayerVehicle = false;

    float _inGameZoneXPos;

    float _startZPos;
    float _startXPos;

    float _currentOffset;
    float _translateDuration;
    float _lastMoveSpeed;
    bool _reachGameZone = false;

    bool _isDead = false;

    private void Start()
    {
        _startZPos = transform.position.z;
        _startXPos = transform.position.x;
        //_inGameZonePos = transform.position;
        _translateDuration = Random.Range(GameLogicParameters.Instance.MinTranslateDuration, GameLogicParameters.Instance.MaxTranslateDuration);
        UpdateDustPS();
        if (_isPlayerVehicle) return;
        StartCoroutine(TranslateToPlayer());
    }

    void FixedUpdate()
    {
        RotateWheels();

        if (_isDead) return;

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
        foreach (var wheel in _wheels)
        {
            if (wheel == null) continue;
            wheel.Rotate(GameLogicParameters.Instance.WheelsRotateSpeedMod * RaidManager.Instance.PlayerMoveSpeed, 0, 0, Space.Self);
        }
    }

    void UpdateDustPS()
    {
        foreach (var dust in _wheelsDustPS)
        {
            if (dust == null) continue;
            var main = dust.main;
            main.startSpeed = GameLogicParameters.Instance.SpeedMod * RaidManager.Instance.PlayerMoveSpeed;

            var emmision = dust.emission;
            emmision.rateOverTime = GameLogicParameters.Instance.DustPSEmmisionRateMod * RaidManager.Instance.PlayerMoveSpeed;
        }
    }

    void SlideVehicle()
    {
        float offsetX = _currentOffset * Mathf.Cos(Time.time);

        float xPos = _inGameZoneXPos + offsetX;

        transform.position = new Vector3(xPos, transform.position.y, _startZPos);
    }

    IEnumerator TranslateToPlayer()
    {
        yield return null;
        float randomXOffset = Random.Range(-GameLogicParameters.Instance.GameZoneXSize, GameLogicParameters.Instance.GameZoneXSize);
        float targetXPos = randomXOffset;

        float t = 0;
        while (t <= 1)
        {
            if (_isDead) yield break;
            if (t >= GameLogicParameters.Instance.ValueToStartSlowTranslate)
            {
                t += Time.deltaTime / _translateDuration / GameLogicParameters.Instance.SlowTranslateValue;
            }
            else
            {
                t += Time.deltaTime / _translateDuration;
            }
            transform.position = Vector3.Lerp(new Vector3(_startXPos, transform.position.y, _startZPos), new Vector3(targetXPos, transform.position.y, _startZPos), t);
            yield return null;
        }
        _inGameZoneXPos = targetXPos;

        InvokeRepeating(nameof(ChangeOffset), 0, GameLogicParameters.Instance.ChangeSlideOffsetDelay);
        _reachGameZone = true;
    }

    void ChangeOffset()
    {
        StartCoroutine(LerpXOffset());
    }

    IEnumerator LerpXOffset()
    {
        float _targetOffset = Random.Range(-GameLogicParameters.Instance.SlideOffsetXValue, GameLogicParameters.Instance.SlideOffsetXValue);
        float startOffset = _currentOffset;

        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / 3;
            _currentOffset = Mathf.Lerp(startOffset, _targetOffset, t);
            yield return null;
        }
    }

    public void OnDead()
    {
        _isDead = true;
        foreach (var dust in _wheelsDustPS)
        {
            if (dust == null) continue;
            var emmision = dust.emission;
            emmision.enabled = false;
        }
        StartCoroutine(MoveIfDead());
    }

    IEnumerator MoveIfDead()
    {
        while (transform.position.x > -GameLogicParameters.Instance.XOffsetForDestroyObject)
        {
            transform.Translate(GameLogicParameters.Instance.SpeedMod * RaidManager.Instance.PlayerMoveSpeed * Time.deltaTime * -Vector3.right, Space.World);
            yield return null;
        }
        Destroy(gameObject);
    }

}
