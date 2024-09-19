using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class UIJoystickTouchController : MonoBehaviour
{
    public static UIJoystickTouchController Instance;

    [SerializeField] RectTransform _joystickArea;
    [SerializeField] Slider _speedSlider;
    [SerializeField] GameObject _joystick;
    [SerializeField] GameObject _shootBtn;

    CanvasGroup _canvasGroup;
    bool _isRotating = false;
    Vector2 _joystickPosition;
    bool _controllerIsAcive = false;
    bool _PCVersion = false;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;        
    }

    public void Init()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _speedSlider.onValueChanged.AddListener(OnChangeSpeed);
        HideInRaidInterface();

        if (YandexGame.EnvironmentData.isDesktop)
        {
            _PCVersion = true;
            _joystick.SetActive(false);
            _shootBtn.SetActive(false);
        }
    }
    private void Update()
    {
        if(!_PCVersion) return;
        if (!_controllerIsAcive) return;

        if (Input.GetMouseButtonDown(0))
        {
            PlayerWeaponManager.Instance.StartShoot();
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            PlayerWeaponManager.Instance.StopShoot();
        }

    }

    public Vector2 GetJoystickPosition
    {
        get { return _joystickPosition; }
    }

    public void OnStartRotation()
    {
        _isRotating = true;
    }

    public void OnStopRotation()
    {
        _isRotating = false;
        _joystickPosition = _joystickArea.anchoredPosition = Vector2.zero;
    }

    public void OnPressShootBtn()
    {
        PlayerWeaponManager.Instance.StartShoot();
    }

    public void OnReleasedShootBtn()
    {
        PlayerWeaponManager.Instance.StopShoot();
    }

    public void OnRotation(Vector2 value)
    {
        if (_isRotating)
        {
            // convert the value between 1 0 to -1 +1
            _joystickPosition.x = ((1 - value.x) - 0.5f) * 2f;
            _joystickPosition.y = ((1 - value.y) - 0.5f) * 2f;
        }
    }

    void OnChangeSpeed(float sliderValue)
    {
        if(!_controllerIsAcive) return;
        RaidManager.Instance.ChangeSpeedWhileInRaid(sliderValue);
    }

    public void HideInRaidInterface()
    {
        StopAllCoroutines();
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _controllerIsAcive = false;
    }

    public void ShowControllers(float delay)
    {
        _speedSlider.value = 0;
        StartCoroutine(FlickeringAppearance(delay));
    }

    IEnumerator FlickeringAppearance(float delay)
    {
        float flickDuration = 0.4f;
        float flickCount = 3;
        yield return new WaitForSeconds(delay);


        for (int i = 1; i <= flickCount; i++) 
        {
            _canvasGroup.alpha = 0;
            yield return new WaitForSeconds(flickDuration);
            _canvasGroup.alpha = 1;
            yield return new WaitForSeconds(flickDuration);
        }
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _controllerIsAcive = true;
    }
}
