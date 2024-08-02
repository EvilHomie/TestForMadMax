using UnityEngine;

public class GameLogicParameters : MonoBehaviour
{
    public static GameLogicParameters Instance;

    [SerializeField] bool _isTesting = false;

    [Header("RAID")]
    [SerializeField] float _minSpeed = 5f;
    [SerializeField] float _maxSpeed = 10f;
    [SerializeField] float _timeForChangeSpeed = 2f;
    [SerializeField] float _moveRoadMod = 0.1f;
    [SerializeField] float _speedMod = 185f;
    [SerializeField] float _xOffsetForDestroyObject = 10000f;

    [Header("VISUAL")]
    [SerializeField] float _wheelsRotateSpeedMod = 3;    
    [SerializeField] float _dustPSEmmisionRateMod = 5f;

    [Header("ENEMY TRANSLATE TO PLAYER")]
    [SerializeField] float _gameZoneXSize = 500f;
    [SerializeField] float _minTranslateDuration = 7;
    [SerializeField] float _maxTranslateDuration = 10;
    [SerializeField] float _valueToStartSlowTranslate = 0.8f;
    [SerializeField] float _slowTranslateValue = 5f;

    [Header("ENEMY SLIDE")]
    [SerializeField] float _changeSlideOffsetDelay = 5;
    [SerializeField] float _slideOffsetXValue = 500f;

    [Header("PlayerWeapon")]
    [SerializeField] float _maxYRotateAngle;
    [SerializeField] float _maxXRotateAngle;

    [Header("PhysicData")]
    [SerializeField] float _touchRoadImpulseMod = 25;


    public float MinSpeed => _minSpeed;
    public float MaxSpeed => _maxSpeed;
    public float TimeForChangeSpeed => _timeForChangeSpeed;
    public float MoveRoadMod => _moveRoadMod;
    public float SpeedMod => _speedMod;
    public float XOffsetForDestroyObject => _xOffsetForDestroyObject;


    public float WheelsRotateSpeedMod => _wheelsRotateSpeedMod;
    public float DustPSEmmisionRateMod => _dustPSEmmisionRateMod;


    public float GameZoneXSize => _gameZoneXSize;
    public float MinTranslateDuration => _minTranslateDuration;
    public float MaxTranslateDuration => _maxTranslateDuration;
    public float ValueToStartSlowTranslate => _valueToStartSlowTranslate;
    public float SlowTranslateValue => _slowTranslateValue;


    public float ChangeSlideOffsetDelay => _changeSlideOffsetDelay;
    public float SlideOffsetXValue => _slideOffsetXValue;


    public float MaxYRotateAngle => _maxYRotateAngle;
    public float MaxXRotateAngle => _maxXRotateAngle;

    public float TouchRoadImpulseMod => _touchRoadImpulseMod;


    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        Application.targetFrameRate = 1000;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!_isTesting) return;

        if (Input.GetKeyDown(KeyCode.T)) Time.timeScale += 1;
        if (Input.GetKeyDown(KeyCode.Y)) Time.timeScale -= 1;

    }
}
