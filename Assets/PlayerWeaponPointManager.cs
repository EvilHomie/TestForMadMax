using System;
using UnityEngine;

public class PlayerWeaponPointManager : MonoBehaviour
{
    public static PlayerWeaponPointManager Instance;

    [SerializeField] Transform[] _weaponPoints;

    IWeapon[] _weapons;

    int _selectedWeaponIndex = 0;
    Vector2[] _lastWeaponsRotations = new Vector2[2];

    float targetRotationY = 0;
    float targetRotationX = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        _weapons = new IWeapon[_weaponPoints.Length];

        for (int i = 0; i < _weaponPoints.Length; i++)
        {
            if (_weaponPoints[i].childCount != 0)
            {
                _weapons[i] = _weaponPoints[i].GetComponentInChildren<IWeapon>();
            }
        }
    }

    private void LateUpdate()
    {
        RotateWeaponAndCamera(TouchController.Instance.GetJoystickPosition);
        RotateCameraByWASD();
    }

    public void OnPlayerEndRaid()
    {
        _selectedWeaponIndex = 0;
        CameraManager.Instance.ChangeInitPos(_weapons[0].ObserverPos);
        targetRotationY = 0;
        targetRotationX = 0;
        Camera.main.transform.position = _weapons[0].ObserverPos;
        for (int i = 0; i < _lastWeaponsRotations.Length; i++)
        {
            _lastWeaponsRotations[i] = Vector2.zero;
        }

        foreach (IWeapon weapon in _weapons)
        {
            if (weapon == _weapons[0])
            {
                weapon.TargetMarker.SetActive(true);
                continue;
            }
            else 
            {
                weapon.TargetMarker.SetActive(false);
            }
        }
    }

    public void ChangeWeapon(int index)
    {        
        if (_selectedWeaponIndex == index) return;

        _selectedWeaponIndex = index;

        foreach (IWeapon weapon in _weapons)
        {
            if (weapon == _weapons[index])
            {
                weapon.TargetMarker.SetActive(true);
                targetRotationX = _lastWeaponsRotations[index].x;
                targetRotationY = _lastWeaponsRotations[index].y;
                continue;
            }

            if (weapon != _weapons[index])
            {
                weapon.TargetMarker.SetActive(false);
            }
        }
        CameraManager.Instance.ChangeInitPos(_weapons[index].ObserverPos);
    }

    public void StartShoot()
    {
        _weapons[_selectedWeaponIndex].StartShooting();
    }

    public void StopShoot()
    {
        _weapons[_selectedWeaponIndex].StopShooting();
    }

    void RotateWeaponAndCamera(Vector2 movementVector)
    {
        if(movementVector == Vector2.zero) return;

        targetRotationY += movementVector.x * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;
        targetRotationX += movementVector.y * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;

        float targetPosYClamped = Mathf.Clamp(targetRotationY, -GameLogicParameters.Instance.MaxYRotateAngle, GameLogicParameters.Instance.MaxYRotateAngle);
        float targetPosXClamped = Mathf.Clamp(targetRotationX, -GameLogicParameters.Instance.MaxXRotateAngle, GameLogicParameters.Instance.MaxXRotateAngle);

        _weaponPoints[_selectedWeaponIndex].rotation = Quaternion.Euler(-targetPosXClamped, targetPosYClamped, 0);
        Camera.main.transform.rotation = _weaponPoints[_selectedWeaponIndex].rotation;
        _lastWeaponsRotations[_selectedWeaponIndex] = new Vector2(targetRotationX, targetRotationY);
    }

    void RotateCameraByWASD()
    {
        targetRotationY += Input.GetAxis("Horizontal") * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;
        targetRotationX += Input.GetAxis("Vertical") * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;

        if (targetRotationY < -GameLogicParameters.Instance.MaxYRotateAngle) targetRotationY = -GameLogicParameters.Instance.MaxYRotateAngle;
        if (targetRotationY > GameLogicParameters.Instance.MaxYRotateAngle) targetRotationY = GameLogicParameters.Instance.MaxYRotateAngle;

        if (targetRotationX < -GameLogicParameters.Instance.MaxXRotateAngle) targetRotationX = -GameLogicParameters.Instance.MaxXRotateAngle;
        if (targetRotationX > GameLogicParameters.Instance.MaxXRotateAngle) targetRotationX = GameLogicParameters.Instance.MaxXRotateAngle;

        //float targetPosYClamped = Mathf.Clamp(targetPosY, -45, 45); // Clamp вызывает ступор при достижении краёв.
        //float targetPosXClamped = Mathf.Clamp(targetPosX, -30, 0); // Clamp вызывает ступор при достижении краёв.

        _weaponPoints[_selectedWeaponIndex].rotation = Quaternion.Euler(-targetRotationX, targetRotationY, 0);
        Camera.main.transform.rotation = _weaponPoints[_selectedWeaponIndex].rotation;
        _lastWeaponsRotations[_selectedWeaponIndex] = new Vector2(targetRotationX, targetRotationY);
    }
}
