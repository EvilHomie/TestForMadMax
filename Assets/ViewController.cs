using UnityEngine;

public class ViewController : MonoBehaviour
{    
    [SerializeField] float _rotationSpeed = 5.0f;
    [SerializeField] float _minMaxYPos;
    [SerializeField] float _minMaxXPos;
    [SerializeField] TouchController touchController;

    float targetPosY;
    float targetPosX;


    void FixedUpdate()
    {

        RotateCamera(touchController.GetTouchPosition);



        RotateCameraByWASD();



    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Time.timeScale += 1;

        if (Input.GetKeyDown(KeyCode.Y))
            Time.timeScale -= 1;
    }


    void RotateCamera(Vector2 movementVector)
    {
        targetPosY += movementVector.x * Time.fixedDeltaTime * _rotationSpeed;
        targetPosX += movementVector.y * Time.fixedDeltaTime * _rotationSpeed;

        if (targetPosY < -_minMaxYPos) targetPosY = -_minMaxYPos;
        if (targetPosY > _minMaxYPos) targetPosY = _minMaxYPos;

        if (targetPosX < -_minMaxXPos) targetPosX = -_minMaxXPos;
        if (targetPosX > _minMaxXPos) targetPosX = _minMaxXPos;

        //float targetPosYClamped = Mathf.Clamp(targetPosY, -45, 45); // Clamp вызывает ступор при достижении краёв.
        //float targetPosXClamped = Mathf.Clamp(targetPosX, -30, 0); // Clamp вызывает ступор при достижении краёв.

        transform.rotation = Quaternion.Euler(-targetPosX, targetPosY, 0);
        Camera.main.transform.rotation = transform.rotation;
    }

    void RotateCameraByWASD()
    {
        targetPosY += Input.GetAxis("Horizontal") * Time.fixedDeltaTime * _rotationSpeed;
        targetPosX += Input.GetAxis("Vertical") * Time.fixedDeltaTime * _rotationSpeed;

        if (targetPosY < -_minMaxYPos) targetPosY = -_minMaxYPos;
        if (targetPosY > _minMaxYPos) targetPosY = _minMaxYPos;

        if (targetPosX < -_minMaxXPos) targetPosX = -_minMaxXPos;
        if (targetPosX > _minMaxXPos) targetPosX = _minMaxXPos;

        //float targetPosYClamped = Mathf.Clamp(targetPosY, -45, 45); // Clamp вызывает ступор при достижении краёв.
        //float targetPosXClamped = Mathf.Clamp(targetPosX, -30, 0); // Clamp вызывает ступор при достижении краёв.

        transform.rotation = Quaternion.Euler(-targetPosX, targetPosY, 0);
        Camera.main.transform.rotation = transform.rotation;
    }

}
