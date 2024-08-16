using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance;

    Transform[] _weaponPoints;
    List<PlayerWeapon> _weapons = new();

    int _selectedWeaponIndex = 0;
    Vector2[] _lastWeaponsRotations = new Vector2[2];

    float targetRotationY = 0;
    float targetRotationX = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

       
    }
    private void LateUpdate()
    {
        RotateWeaponAndCameraByJoystick(TouchController.Instance.GetJoystickPosition);
        RotateWeaponAndCameraByWASD();
    }

    //private void Start()
    //{
    //    OnChangeVehicle(PlayerVehicleManager.Instance.PlayerVehicle);
    //}


    void OnChangeVehicle(PlayerVehicle newVehicle)
    {
        _weapons.Clear();
        _weaponPoints = newVehicle.WeaponPoints;
        foreach (Transform weapon in _weaponPoints)
        {
            _weapons.Add(weapon.GetComponentInChildren<PlayerWeapon>());
        }
    }


    public void OnPlayerEndRaid()
    {
        _selectedWeaponIndex = 0;
        CameraManager.Instance.ChangeInitPos(_weaponPoints[0].position + _weapons[0].ObserverPos);
        targetRotationY = 0;
        targetRotationX = 0;
        Camera.main.transform.position = _weaponPoints[0].position + _weapons[0].ObserverPos;
        Array.Clear(_lastWeaponsRotations, 0, _lastWeaponsRotations.Length);

        foreach (PlayerWeapon weapon in _weapons)
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

        foreach (PlayerWeapon weapon in _weapons)
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
        CameraManager.Instance.ChangeInitPos(_weaponPoints[index].position + _weapons[index].ObserverPos);
    }

    public void StartShoot()
    {
        _weapons[_selectedWeaponIndex].StartShooting();
    }

    public void StopShoot()
    {
        _weapons[_selectedWeaponIndex].StopShooting();
    }

    void RotateWeaponAndCameraByJoystick(Vector2 movementVector)
    {
        if(movementVector == Vector2.zero) return;

        targetRotationY += movementVector.x * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;
        targetRotationX += movementVector.y * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;

        float targetPosYClamped = Mathf.Clamp(targetRotationY, -GameConfig.Instance.MaxYRotateAngle, GameConfig.Instance.MaxYRotateAngle);
        float targetPosXClamped = Mathf.Clamp(targetRotationX, -GameConfig.Instance.MaxXRotateAngle, GameConfig.Instance.MaxXRotateAngle);

        _weaponPoints[_selectedWeaponIndex].rotation = Quaternion.Euler(-targetPosXClamped, targetPosYClamped, 0);
        Camera.main.transform.rotation = _weaponPoints[_selectedWeaponIndex].rotation;
        _lastWeaponsRotations[_selectedWeaponIndex] = new Vector2(targetRotationX, targetRotationY);
    }

    void RotateWeaponAndCameraByWASD()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) return;

        targetRotationY += Input.GetAxis("Horizontal") * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;
        targetRotationX += Input.GetAxis("Vertical") * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;

        if (targetRotationY < -GameConfig.Instance.MaxYRotateAngle) targetRotationY = -GameConfig.Instance.MaxYRotateAngle;
        if (targetRotationY > GameConfig.Instance.MaxYRotateAngle) targetRotationY = GameConfig.Instance.MaxYRotateAngle;

        if (targetRotationX < -GameConfig.Instance.MaxXRotateAngle) targetRotationX = -GameConfig.Instance.MaxXRotateAngle;
        if (targetRotationX > GameConfig.Instance.MaxXRotateAngle) targetRotationX = GameConfig.Instance.MaxXRotateAngle;

        //float targetPosYClamped = Mathf.Clamp(targetPosY, -45, 45); // Clamp вызывает ступор при достижении краёв.
        //float targetPosXClamped = Mathf.Clamp(targetPosX, -30, 0); // Clamp вызывает ступор при достижении краёв.

        _weaponPoints[_selectedWeaponIndex].rotation = Quaternion.Euler(-targetRotationX, targetRotationY, 0);
        Camera.main.transform.rotation = _weaponPoints[_selectedWeaponIndex].rotation;
        _lastWeaponsRotations[_selectedWeaponIndex] = new Vector2(targetRotationX, targetRotationY);
    }
}
