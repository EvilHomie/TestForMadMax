using UnityEngine;

public class TouchController : MonoBehaviour
{
    public static TouchController Instance;

    [SerializeField] RectTransform joystickArea;
    private bool touchPresent = false;
    private Vector2 movementVector;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public Vector2 GetTouchPosition
    {
        get { return movementVector; }
    }

    public void BeginDrag()
    {
        touchPresent = true;
    }

    public void EndDrag()
    {
        touchPresent = false;
        movementVector = joystickArea.anchoredPosition = Vector2.zero;
    }

    public void OnValueChanged(Vector2 value)
    {
        if (touchPresent)
        {
            // convert the value between 1 0 to -1 +1
            movementVector.x = ((1 - value.x) - 0.5f) * 2f;
            movementVector.y = ((1 - value.y) - 0.5f) * 2f;
        }
    }
}
