using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance;

    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _mouseSensitivity = 5f;
    Vector3 _currentRotation;


    Dictionary<int, Transform> _weaponPoints = new();
    Dictionary<int, PlayerWeapon> _weapons = new();
    Dictionary<int, Vector2> _weaponsRotationData = new();

    int _selectedWeaponIndex = 1;
    float curWeaponRotationY = 0;
    float curWeaponRotationX = 0;

    bool _playerIsDead = false;
    bool _playerOnRaid = false;
    bool _isFirstLevel = false;
    bool _firstTouchStatus = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    private void LateUpdate()
    {
        if (_playerIsDead || !_playerOnRaid) return;
        if (YandexGame.EnvironmentData.isDesktop)
        {
            RotateWeaponAndCameraByMouse();
        }
        else
        {
            RotateWeaponAndCameraByJoystick(UIJoystickTouchController.Instance.GetJoystickPosition);
        }
        //RotateWeaponAndCameraByWASD();
    }

    public void UpdateWeaponsData()
    {
        _weaponPoints = PlayerVehicleManager.Instance.PlayerVehicle.WeaponPoints.ToDictionary(slot => slot.Index, slot => slot.Transform);

        for (int weaponIndex = 1; weaponIndex <= _weapons.Count; weaponIndex++)
        {
            if (!PlayerData.Instance.EquipedItems.ContainsKey(weaponIndex))
            {
                _weapons.Remove(weaponIndex);
            }
            //Debug.LogWarning(weaponIndex);
            //Debug.LogWarning(_weapons[weaponIndex]);
            //Debug.LogWarning(PlayerData.Instance.EquipedItems[weaponIndex]);
        }


        for (int weaponIndex = 1; weaponIndex < PlayerData.Instance.EquipedItems.Count; weaponIndex++)
        {
            //Debug.LogWarning(weaponIndex);
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
            if (PlayerData.Instance.EquipedItems[weaponIndex].Equals(existWeaponData.deffWeaponName)) return;
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
        _playerOnRaid = true;
        _playerIsDead = false;
        UpdateWeaponsData();
        _selectedWeaponIndex = 1;
        foreach (var weapon in _weapons)
        {
            //if (weapon.Value == null)
            //{
            //    _weapons.Remove(weapon.Key);
            //    continue;
            //}


            //Debug.LogWarning(weapon.Key);
            //Debug.LogWarning(weapon.Value);

            if (weapon.Key != _selectedWeaponIndex)
            {
                weapon.Value.TargetMarker.SetActive(false);
            }
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

        if (LevelManager.Instance.GetSelectedLevelinfo().LevelName == "1-1")
        {
            _isFirstLevel = true;
        }
        else
        {
            _isFirstLevel = false;
        }
    }


    public void OnPlayerEndRaid()
    {
        _playerOnRaid = false;
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
        if (_playerIsDead) return;
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
        if (!_playerOnRaid) return;
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) return;

        if (_isFirstLevel && !_firstTouchStatus)
        {
            _firstTouchStatus = true;
            MetricaSender.SendFirstControllerTouch();
        }

        curWeaponRotationY += Input.GetAxis("Horizontal") * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;
        curWeaponRotationX += Input.GetAxis("Vertical") * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;

        curWeaponRotationY = Mathf.Clamp(curWeaponRotationY, -GameConfig.Instance.MaxYRotateAngle, GameConfig.Instance.MaxYRotateAngle);
        curWeaponRotationX = Mathf.Clamp(curWeaponRotationX, -GameConfig.Instance.MaxXRotateAngle, GameConfig.Instance.MaxXRotateAngle);

        Quaternion newRotation = Quaternion.Euler(-curWeaponRotationX, curWeaponRotationY, 0);

        _weaponPoints[_selectedWeaponIndex].rotation = newRotation;
        Camera.main.transform.rotation = newRotation;
    }

    void RotateWeaponAndCameraByMouse()
    {
        Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);
        Vector3 point = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 10000, _layerMask))
        {
            point = hitInfo.point;
            //cameraMarker.transform.localPosition = new Vector3(0,0, point.z);
        }

        Ray firePointRay = new(_weapons[_selectedWeaponIndex].transform.position, _weapons[_selectedWeaponIndex].transform.forward);

        //Vector3 firePointRayPoint = Vector3.zero;

        if (Physics.Raycast(firePointRay, out RaycastHit firePointhitInfo, 10000, _layerMask))
        {
            //firePointRayPoint = firePointhitInfo.point;
            //weaponMarker.transform.position = firePointRayPoint;

            Vector3 directionToPoint = firePointhitInfo.point - Camera.main.transform.position;

            Debug.DrawRay(Camera.main.transform.position, directionToPoint * 1000, Color.blue);

            //Vector3 pos = directionToPoint.normalized * 35;

            //weaponMarker.transform.position = pos;



            Vector3 dir = Vector3.Normalize(firePointhitInfo.point - Camera.main.transform.position);
            Vector3 pos = Camera.main.transform.position + dir * 35;
            _weapons[_selectedWeaponIndex].TargetMarker.transform.position = pos;

        }


        //Debug.LogWarning(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        _currentRotation.x += Input.GetAxis("Mouse X") * _mouseSensitivity;
        _currentRotation.y -= Input.GetAxis("Mouse Y") * _mouseSensitivity;
        //currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
        _currentRotation.y = Mathf.Clamp(_currentRotation.y, -GameConfig.Instance.MaxXRotateAngle, GameConfig.Instance.MaxXRotateAngle);
        _currentRotation.x = Mathf.Clamp(_currentRotation.x, -GameConfig.Instance.MaxYRotateAngle, GameConfig.Instance.MaxYRotateAngle);
        Camera.main.transform.rotation = Quaternion.Euler(_currentRotation.y, _currentRotation.x, 0);

        float RotationRadiansSpeed = _weapons[_selectedWeaponIndex].RotationSpeed * Mathf.Deg2Rad;

        Vector3 targetDirectionBase = point - _weapons[_selectedWeaponIndex].transform.position;
        Vector3 newDirectionForBase = Vector3.RotateTowards(_weapons[_selectedWeaponIndex].transform.forward, targetDirectionBase, RotationRadiansSpeed * Time.deltaTime, 0.0f);
        Quaternion quaternion = Quaternion.LookRotation(newDirectionForBase);

        Vector3 eulerAngles = quaternion.eulerAngles;

        _weaponPoints[_selectedWeaponIndex].rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 0); ;
        //cannonBase.transform.rotation = Quaternion.Euler(0, eulerAngles.y, 0);
        //cannonTurret.transform.localRotation = Quaternion.Euler(eulerAngles.x, 0, 0);
        //FirePoint.transform.rotation = quaternion;

        Debug.DrawRay(_weaponPoints[_selectedWeaponIndex].transform.position, _weaponPoints[_selectedWeaponIndex].transform.forward * 1000, Color.green);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1000, Color.red);
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
