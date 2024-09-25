using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllWithMouse : MonoBehaviour
{
    public float sensitivity = 10f;
    public float maxYAngle = 80f;
    public float maxXAngle = 30f;
    public Vector2 currentRotation;

    public GameObject cannon;

    private void Update()
    {
        //Debug.LogWarning(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
        currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        //currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
        currentRotation.y = Mathf.Clamp(currentRotation.y, -maxXAngle, maxXAngle);
        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxYAngle, maxYAngle);
        Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);

        cannon.transform.rotation = Camera.main.transform.rotation;

    }
}
