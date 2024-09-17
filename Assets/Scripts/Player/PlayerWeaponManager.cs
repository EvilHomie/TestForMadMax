using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance;

    Dictionary<int, Transform> _weaponPoints = new();
    Dictionary<int, PlayerWeapon> _weapons = new();
    Dictionary<int, Vector2> _weaponsRotationData = new();

    int _selectedWeaponIndex = 1;
    float curWeaponRotationY = 0;
    float curWeaponRotationX = 0;

    bool _playerIsDead = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    private void LateUpdate()
    {
        if (_playerIsDead) return;
        RotateWeaponAndCameraByJoystick(UIJoystickTouchController.Instance.GetJoystickPosition);
        RotateWeaponAndCameraByWASD();
    }

    public void UpdateWeaponsData()
    {
        _weaponPoints = PlayerVehicleManager.Instance.PlayerVehicle.WeaponPoints.ToDictionary(slot => slot.Index, slot => slot.Transform);

        for (int weaponIndex = 1; weaponIndex < PlayerData.Instance.EquipedItems.Count; weaponIndex++)
        {
            ChangeWeaponOnPoint(weaponIndex);
        }
    }

    void ChangeWeaponOnPoint(int weaponIndex)
    {
        Transform point = _weaponPoints[weaponIndex];

        if (point.childCount > 1)
        {
            foreach (Transform t in point) Destroy(t.gameObject);
            CreateWeaponInstance(weaponIndex, point);
        }
        else if (_weaponPoints[weaponIndex].childCount == 0)
        {
            CreateWeaponInstance(weaponIndex, point);
        }
        else if (point.childCount == 1)
        {
            WeaponData existWeaponData = (WeaponData)_weapons[weaponIndex].GetItemData();
            if (PlayerData.Instance.EquipedItems[weaponIndex] == existWeaponData.deffWeaponName) return;
            foreach (Transform t in point) Destroy(t.gameObject);
            CreateWeaponInstance(weaponIndex, point);
        }
    }

    void CreateWeaponInstance(int weaponIndex, Transform weaponPoint)
    {
        string weaponName = PlayerData.Instance.EquipedItems[weaponIndex];
        WeaponData wData = (WeaponData)PlayerData.Instance.GetItemDataByName(weaponName);
        PlayerWeapon weaponPF = GameAssets.Instance.GameItems.Weapons.Find(weapon => weapon.name == weaponName);

        PlayerWeapon newWeaponInstance = Instantiate(weaponPF, weaponPoint);
        newWeaponInstance.SetItemData(wData);
        newWeaponInstance.TargetMarker.SetActive(false);
        _weapons[weaponIndex] = newWeaponInstance;
        _weaponsRotationData[weaponIndex] = Vector2.zero;
    }


    public void OnPlayerStartRaid()
    {
        _playerIsDead = false;
        UpdateWeaponsData();
        _selectedWeaponIndex = 1;
        foreach (var weapon in _weapons)
        {
            if (weapon.Key != _selectedWeaponIndex)
                weapon.Value.TargetMarker.SetActive(false);
            else
                weapon.Value.TargetMarker.SetActive(true);
        }

        foreach (var point in _weaponPoints)
        {
            point.Value.rotation = Quaternion.Euler(0, 0, 0);
        }

        for (int i = 1; i <= _weaponsRotationData.Count; i++)
        {
            _weaponsRotationData[i] = Vector2.zero;
        }

        CameraManager.Instance.ChangeInitPos(_weaponPoints[1].position + _weapons[1].ObserverPos);
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
        curWeaponRotationX = 0;
        curWeaponRotationY = 0;
    }


    public void OnPlayerEndRaid()
    {
        _weapons[_selectedWeaponIndex].StopShooting();
    }

    public void ChangeWeapon(int selectedSlotIndex)
    {
        _weapons[_selectedWeaponIndex].TargetMarker.SetActive(false);
        _weaponsRotationData[_selectedWeaponIndex] = new Vector2(curWeaponRotationX, curWeaponRotationY);

        _weapons[selectedSlotIndex].TargetMarker.SetActive(true);
        curWeaponRotationX = _weaponsRotationData[selectedSlotIndex].x;
        curWeaponRotationY = _weaponsRotationData[selectedSlotIndex].y;

        CameraManager.Instance.ChangeInitPos(_weaponPoints[selectedSlotIndex].position + _weapons[selectedSlotIndex].ObserverPos);
        Camera.main.transform.rotation = Quaternion.Euler(-curWeaponRotationX, curWeaponRotationY, 0);

        _selectedWeaponIndex = selectedSlotIndex;
    }

    public void StartShoot()
    {
        if(_playerIsDead) return;
        _weapons[_selectedWeaponIndex].StartShooting();
    }

    public void StopShoot()
    {
        _weapons[_selectedWeaponIndex].StopShooting();
    }

    void RotateWeaponAndCameraByJoystick(Vector2 movementVector)
    {
        if (movementVector == Vector2.zero) return;

        curWeaponRotationY += movementVector.x * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;
        curWeaponRotationX += movementVector.y * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;

        curWeaponRotationY = Mathf.Clamp(curWeaponRotationY, -GameConfig.Instance.MaxYRotateAngle, GameConfig.Instance.MaxYRotateAngle);
        curWeaponRotationX = Mathf.Clamp(curWeaponRotationX, -GameConfig.Instance.MaxXRotateAngle, GameConfig.Instance.MaxXRotateAngle);

        Quaternion newRotation = Quaternion.Euler(-curWeaponRotationX, curWeaponRotationY, 0);

        _weaponPoints[_selectedWeaponIndex].rotation = newRotation;
        Camera.main.transform.rotation = newRotation;
    }

    void RotateWeaponAndCameraByWASD()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) return;

        curWeaponRotationY += Input.GetAxis("Horizontal") * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;
        curWeaponRotationX += Input.GetAxis("Vertical") * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;

        curWeaponRotationY = Mathf.Clamp(curWeaponRotationY, -GameConfig.Instance.MaxYRotateAngle, GameConfig.Instance.MaxYRotateAngle);
        curWeaponRotationX = Mathf.Clamp(curWeaponRotationX, -GameConfig.Instance.MaxXRotateAngle, GameConfig.Instance.MaxXRotateAngle);

        Quaternion newRotation = Quaternion.Euler(-curWeaponRotationX, curWeaponRotationY, 0);

        _weaponPoints[_selectedWeaponIndex].rotation = newRotation;
        Camera.main.transform.rotation = newRotation;
    }

    public void OnPlayerDie()
    {
        _playerIsDead = true;
        StopShoot();
        StartCoroutine(RotateWeaponOnDie());
    }

    IEnumerator RotateWeaponOnDie()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / GameConfig.Instance.TimeForChangeSpeed;
            RotateWeaponAndCameraByJoystick(Vector2.down);
            yield return null;
        }
    }
}
