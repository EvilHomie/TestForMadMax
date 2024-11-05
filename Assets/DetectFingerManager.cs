using UnityEngine;
using UnityEngine.EventSystems;
using YG;

public class DetectFingerManager : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public static DetectFingerManager Instance;
    Vector3 _lastFingerPos;
    Vector3 _fingerPosDifference;

    int _firstFingerId;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void OnPlayerStartRaid()
    {
        if (!YandexGame.EnvironmentData.isDesktop)
        {
            gameObject.SetActive(true);
        }
    }
    public void OnPlayerEndRaid()
    {
        gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.pointerId != 0) return;

        Vector3 currentPos = Camera.main.ScreenToViewportPoint(eventData.position);
        _fingerPosDifference = _lastFingerPos - currentPos;
        _lastFingerPos = currentPos;
        //PlayerWeaponManager.Instance.RotateCameraByFinger(_fingerPosDifference);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerId != 0) return;

        //_firstFingerId = eventData.pointerId;
        _fingerPosDifference = Vector3.zero;
        _lastFingerPos = Camera.main.ScreenToViewportPoint(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId != 0) return;

        _fingerPosDifference = Vector3.zero;
        _lastFingerPos = Camera.main.ScreenToViewportPoint(eventData.position);
    }
}
