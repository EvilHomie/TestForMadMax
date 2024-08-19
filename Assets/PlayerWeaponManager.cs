using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance;

    List<Transform> _weaponPoints = new();
    Dictionary<int, PlayerWeapon> _weapons = new();

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

    //public void OnChangeVehicle()
    //{
    //    PlayerVehicle newVehicle = PlayerVehicleManager.Instance.PlayerVehicle;
    //    _weapons.Clear();
    //    _weaponPoints = newVehicle.WeaponPoints;
    //    UpdateWeapons();
    //}

    public void UpdateWeaponsData()
    {
        _weaponPoints = PlayerVehicleManager.Instance.PlayerVehicle.WeaponPoints;
        for (int weaponIndex = 1; weaponIndex < PlayerData.Instance.EquipedItems.Count; weaponIndex++)
        {
            Debug.LogWarning(weaponIndex);
            ChangeWeaponOnPoint(_weaponPoints[weaponIndex - 1], weaponIndex);
        }










        //for (int i = 0; i < PlayerData.Instance.SelectedWeapons.Count; i++)
        //{
        //    string weaponName = PlayerData.Instance.SelectedWeapons[i];
        //    PlayerWeapon originalWeapon = GameAssets.Instance.GameItems.Weapons.Find(weapon => weapon.name == weaponName);
        //    PlayerWeapon newWeapon = Instantiate(originalWeapon, _weaponPoints[i]);
        //    newWeapon.SetItemData(PlayerData.Instance.GetItemData(weaponName));
        //    _weapons.Add(newWeapon);
        //}
    }

    void ChangeWeaponOnPoint(Transform weaponPoint, int weaponIndex)
    {
        if (weaponPoint.childCount > 1)
        {
            foreach (Transform t in weaponPoint) Destroy(t.gameObject);
            CreateWeaponInstance(weaponPoint, weaponIndex);
        }
        else if (weaponPoint.childCount == 0)
        {
            CreateWeaponInstance(weaponPoint, weaponIndex);
        }
        else if (weaponPoint.childCount == 1)
        {
            WeaponData existWeaponData = (WeaponData)_weapons[weaponIndex].GetItemData();
            if (PlayerData.Instance.EquipedItems[weaponIndex] == existWeaponData.weaponName) return;
            foreach (Transform t in weaponPoint) Destroy(t.gameObject);
            CreateWeaponInstance(weaponPoint, weaponIndex);
        }
    }

    void CreateWeaponInstance(Transform weaponPoint, int weaponIndex)
    {
        string weaponName = PlayerData.Instance.EquipedItems[weaponIndex];
        WeaponData wData = (WeaponData)PlayerData.Instance.GetItemDataByName(weaponName);
        PlayerWeapon weaponPF = GameAssets.Instance.GameItems.Weapons.Find(weapon => weapon.name == weaponName);

        PlayerWeapon newWeaponInstance = Instantiate(weaponPF, weaponPoint);
        newWeaponInstance.SetItemData(wData);
        _weapons[weaponIndex] = newWeaponInstance;
    }


    public void OnPlayerStartRaid()
    {

    }


    public void OnPlayerEndRaid()
    {
        UpdateWeaponsData();
        _selectedWeaponIndex = 1;
        CameraManager.Instance.ChangeInitPos(_weaponPoints[_selectedWeaponIndex - 1].position + _weapons[_selectedWeaponIndex].ObserverPos);








        //_selectedWeaponIndex = 0;
        //CameraManager.Instance.ChangeInitPos(_weaponPoints[0].position + _weapons[0].ObserverPos);
        //targetRotationY = 0;
        //targetRotationX = 0;
        //Camera.main.transform.position = _weaponPoints[0].position + _weapons[0].ObserverPos;
        //Array.Clear(_lastWeaponsRotations, 0, _lastWeaponsRotations.Length);

        //foreach (PlayerWeapon weapon in _weapons)
        //{
        //    if (weapon == _weapons[0])
        //    {
        //        weapon.TargetMarker.SetActive(true);
        //        continue;
        //    }
        //    else
        //    {
        //        weapon.TargetMarker.SetActive(false);
        //    }
        //}
    }

    public void ChangeWeapon(int index)
    {
        //if (_selectedWeaponIndex == index) return;

        //_selectedWeaponIndex = index;

        //foreach (PlayerWeapon weapon in _weapons)
        //{
        //    if (weapon == _weapons[index])
        //    {
        //        weapon.TargetMarker.SetActive(true);
        //        targetRotationX = _lastWeaponsRotations[index].x;
        //        targetRotationY = _lastWeaponsRotations[index].y;
        //        continue;
        //    }

        //    if (weapon != _weapons[index])
        //    {
        //        weapon.TargetMarker.SetActive(false);
        //    }
        //}
        //CameraManager.Instance.ChangeInitPos(_weaponPoints[index].position + _weapons[index].ObserverPos);
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
        if (movementVector == Vector2.zero) return;

        targetRotationY += movementVector.x * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;
        targetRotationX += movementVector.y * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;

        float targetPosYClamped = Mathf.Clamp(targetRotationY, -GameConfig.Instance.MaxYRotateAngle, GameConfig.Instance.MaxYRotateAngle);
        float targetPosXClamped = Mathf.Clamp(targetRotationX, -GameConfig.Instance.MaxXRotateAngle, GameConfig.Instance.MaxXRotateAngle);

        _weaponPoints[_selectedWeaponIndex - 1].rotation = Quaternion.Euler(-targetPosXClamped, targetPosYClamped, 0);
        Camera.main.transform.rotation = _weaponPoints[_selectedWeaponIndex - 1].rotation;
        _lastWeaponsRotations[_selectedWeaponIndex - 1] = new Vector2(targetRotationX, targetRotationY);
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

        //float targetPosYClamped = Mathf.Clamp(targetPosY, -45, 45); // Clamp �������� ������ ��� ���������� ����.
        //float targetPosXClamped = Mathf.Clamp(targetPosX, -30, 0); // Clamp �������� ������ ��� ���������� ����.

        _weaponPoints[_selectedWeaponIndex - 1].rotation = Quaternion.Euler(-targetRotationX, targetRotationY, 0);
        Camera.main.transform.rotation = _weaponPoints[_selectedWeaponIndex - 1].rotation;
        _lastWeaponsRotations[_selectedWeaponIndex - 1] = new Vector2(targetRotationX, targetRotationY);
    }
}
