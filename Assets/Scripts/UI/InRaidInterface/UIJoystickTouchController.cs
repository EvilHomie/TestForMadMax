using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIJoystickTouchController : MonoBehaviour
{
    public static UIJoystickTouchController Instance;

    [SerializeField] RectTransform _joystickArea;
    [SerializeField] Slider _speedSlider;

    CanvasGroup _canvasGroup;
    bool _isRotating = false;
    Vector2 _joystickPosition;
    bool _controllerIsAcive = false;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        _canvasGroup = GetComponent<CanvasGroup>();
        _speedSlider.onValueChanged.AddListener(OnChangeSpeed);
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

    public void HideControllers()
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
