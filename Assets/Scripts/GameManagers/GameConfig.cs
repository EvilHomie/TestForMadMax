using UnityEngine;

public class GameConfig : MonoBehaviour
{
    public static GameConfig Instance;

    [SerializeField] bool _isTesting = true;
    [SerializeField] bool _enableDebug = true;
    [SerializeField] Language _language;

    [Header("RAID")]
    [SerializeField] float _minPlayerSpeed = 5f;
    [SerializeField] float _maxPlayerSpeed = 10f;
    [SerializeField] float _timeForChangeSpeed = 2f;
    [SerializeField] float _moveRoadMod = 0.1f;
    [SerializeField] float _speedMod = 185f;
    [SerializeField] float _xOffsetForDestroyObject = 10000f;

    [Header("VISUAL")]
    [SerializeField] float _wheelsRotateSpeedMod = 3;
    [SerializeField] float _dustPSEmmisionRateMod = 5f;
    [SerializeField] float _hitVisualDuration = 0.1f;

    [Header("ENEMY TRANSLATE TO PLAYER")]
    [SerializeField] float _gameZoneXSize = 500f;
    [SerializeField] float _minTranslateDuration = 7;
    [SerializeField] float _maxTranslateDuration = 10;
    [SerializeField] float _valueToStartSlowTranslate = 0.8f;
    [SerializeField] float _slowTranslateValue = 5f;

    [Header("ENEMY BRAINS")]
    [SerializeField] float _changeDestinationDelay = 5;
    [SerializeField] float _slideOffsetXValue = 500f;
    [SerializeField] float _minDelayForRun = 2f;
    [SerializeField] float _maxDelayForRun = 4f;
    [SerializeField] float _minRunSpeed = 0.3f;
    [SerializeField] float _maxRunSpeed = 0.5f;
    [SerializeField] float _lockOnPlayerDuration = 2f;
    [SerializeField] float _onPlayerDieSpeedMod = 2f;

    [Header("PlayerWeapon")]
    [SerializeField] float _maxYRotateAngle;
    [SerializeField] float _maxXRotateAngle;

    [SerializeField] float _maxYRotateAngleAndroid;
    [SerializeField] float _maxXRotateAngleAndroid;

    [Header("PhysicData")]
    [SerializeField] float _touchRoadImpulse = 25;

    public bool IsTesting => _isTesting;
    public bool EnableDebug => _enableDebug;
    public Language Language => _language;

    public float MinPlayerSpeed => _minPlayerSpeed;
    public float MaxPlayerSpeed => _maxPlayerSpeed;
    public float TimeForChangeSpeed => _timeForChangeSpeed;
    public float MoveRoadMod => _moveRoadMod;
    public float SpeedMod => _speedMod;
    public float XOffsetForDestroyObject => _xOffsetForDestroyObject;


    public float WheelsRotateSpeedMod => _wheelsRotateSpeedMod;
    public float DustPSEmmisionRateMod => _dustPSEmmisionRateMod;
    public float HitVisualDuration => _hitVisualDuration;


    public float GameZoneXSize => _gameZoneXSize;
    public float MinTranslateDuration => _minTranslateDuration;
    public float MaxTranslateDuration => _maxTranslateDuration;
    public float ValueToStartSlowTranslate => _valueToStartSlowTranslate;
    public float SlowTranslateValue => _slowTranslateValue;


    public float ChangeDestinationDelay => _changeDestinationDelay;
    public float SlideOffsetXValue => _slideOffsetXValue;
    public float MinDelayForRun => _minDelayForRun;
    public float MaxDelayForRun => _maxDelayForRun;
    public float MinRunSpeed => _minRunSpeed;
    public float MaxRunSpeed => _maxRunSpeed;

    public float LockOnPlayerDuration => _lockOnPlayerDuration;
    public float OnPlayerDieSpeedMod => _onPlayerDieSpeedMod;
    public float MaxYRotateAngle => _maxYRotateAngle;
    public float MaxXRotateAngle => _maxXRotateAngle;

    public float MaxYRotateAngleAndroid => _maxYRotateAngleAndroid;
    public float MaxXRotateAngleAndroid => _maxXRotateAngleAndroid;

    public float TouchRoadImpulse => _touchRoadImpulse;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
}
