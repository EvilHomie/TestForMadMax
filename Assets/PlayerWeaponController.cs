using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] Transform _weaponTransform;
    [SerializeField] float _rotationSpeed = 5.0f;    

    float targetPosY = 0;
    float targetPosX = 0;


    void FixedUpdate()
    {
        RotateCamera(TouchController.Instance.GetTouchPosition);
        RotateCameraByWASD();
    }

    void RotateCamera(Vector2 movementVector)
    {
        targetPosY += movementVector.x * Time.fixedDeltaTime * _rotationSpeed;
        targetPosX += movementVector.y * Time.fixedDeltaTime * _rotationSpeed;

        float targetPosYClamped = Mathf.Clamp(targetPosY, -GameLogicParameters.Instance.MaxYRotateAngle, GameLogicParameters.Instance.MaxYRotateAngle);
        float targetPosXClamped = Mathf.Clamp(targetPosX, -GameLogicParameters.Instance.MaxXRotateAngle, GameLogicParameters.Instance.MaxXRotateAngle);

        _weaponTransform.rotation = Quaternion.Euler(-targetPosXClamped, targetPosYClamped, 0);
        Camera.main.transform.rotation = _weaponTransform.rotation;
    }

    void RotateCameraByWASD()
    {
        targetPosY += Input.GetAxis("Horizontal") * Time.fixedDeltaTime * _rotationSpeed;
        targetPosX += Input.GetAxis("Vertical") * Time.fixedDeltaTime * _rotationSpeed;

        if (targetPosY < -GameLogicParameters.Instance.MaxYRotateAngle) targetPosY = -GameLogicParameters.Instance.MaxYRotateAngle;
        if (targetPosY > GameLogicParameters.Instance.MaxYRotateAngle) targetPosY = GameLogicParameters.Instance.MaxYRotateAngle;

        if (targetPosX < -GameLogicParameters.Instance.MaxXRotateAngle) targetPosX = -GameLogicParameters.Instance.MaxXRotateAngle;
        if (targetPosX > GameLogicParameters.Instance.MaxXRotateAngle) targetPosX = GameLogicParameters.Instance.MaxXRotateAngle;

        //float targetPosYClamped = Mathf.Clamp(targetPosY, -45, 45); // Clamp вызывает ступор при достижении краёв.
        //float targetPosXClamped = Mathf.Clamp(targetPosX, -30, 0); // Clamp вызывает ступор при достижении краёв.

        _weaponTransform.rotation = Quaternion.Euler(-targetPosX, targetPosY, 0);
        Camera.main.transform.rotation = _weaponTransform.rotation;
    }

}
