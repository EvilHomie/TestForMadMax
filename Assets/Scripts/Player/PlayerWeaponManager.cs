using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using YG;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance;

    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _mouseSensitivity = 5f;
    [SerializeField] Transform[] _weaponPointsTransform;
    [SerializeField] GameObject _targetMarkerForMobile;

    Vector3 _currentCameraRotation;


    //Dictionary<int, Transform> _weaponPoints = new();
    Dictionary<int, PlayerWeapon> _weaponsByIndex = new();
    Dictionary<int, Vector2> _lastCameraRotationDataByWeaponIndex = new();

    int _selectedWeaponIndex = 1;
    //float curWeaponRotationY = 0;
    //float curWeaponRotationX = 0;

    bool _playerIsDead = false;
    bool _playerOnRaid = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Init()
    {
        OnCloseInventory();
    }

    private void LateUpdate()
    {
        if (_playerIsDead || !_playerOnRaid) return;
        if (YandexGame.EnvironmentData.isDesktop)
        {
            _targetMarkerForMobile.SetActive(false);
            RotateWeaponAndCameraByMouse();
        }
        else
        {
            _targetMarkerForMobile.SetActive(true);
            RotateWeaponAndCameraByJoystick(UIJoystickTouchController.Instance.GetJoystickPosition);
        }
    }

    public void OnCloseInventory()
    {
        ResetData();
        CreateWeaponsOnPoints();
        ResetCameraPos();
    }

    void ResetData()
    {
        foreach (var point in _weaponPointsTransform)
        {
            if (point.childCount > 0)
            {
                foreach (Transform t in point) Destroy(t.gameObject);
            }
        }
        _weaponsByIndex.Clear();
        _lastCameraRotationDataByWeaponIndex.Clear();
        _selectedWeaponIndex = 1;
    }

    void CreateWeaponsOnPoints()
    {
        for (int weaponIndex = 1; weaponIndex < PlayerData.Instance.EquipedItems.Count; weaponIndex++)
        {
            CreateWeaponInstance(weaponIndex);
        }
    }


    void CreateWeaponInstance(int weaponIndex)
    {
        string weaponName = PlayerData.Instance.EquipedItems[weaponIndex];
        WeaponData wData = (WeaponData)PlayerData.Instance.GetItemDataByName(weaponName);
        PlayerWeapon weaponPF = GameAssets.Instance.GameItems.Weapons.Find(weapon => weapon.name == weaponName);

        Transform matchPoint = _weaponPointsTransform[weaponIndex - 1];

        PlayerWeapon newWeaponInstance = Instantiate(weaponPF, matchPoint);
        newWeaponInstance.SetItemData(wData);
        newWeaponInstance.TargetMarker.SetActive(false);
        _weaponsByIndex[weaponIndex] = newWeaponInstance;
        _lastCameraRotationDataByWeaponIndex[weaponIndex] = Vector2.zero;
        //CustomLogDebuger.Log("WC");
    }

    public void OnPlayerStartRaid()
    {
        _playerOnRaid = true;
        _playerIsDead = false;
        if (YandexGame.EnvironmentData.isDesktop)
            _weaponsByIndex[_selectedWeaponIndex].TargetMarker.SetActive(true);
    }


    public void OnPlayerEndRaid()
    {
        _playerOnRaid = false;
        _weaponsByIndex[_selectedWeaponIndex].StopShooting();
    }

    void ResetCameraPos()
    {
        Vector3 newPos = _weaponPointsTransform[0].position + _weaponsByIndex[1].ObserverPos;
        CameraManager.Instance.ChangeInitPos(newPos);
        Camera.main.transform.position = newPos;
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void ChangeWeapon(int selectedSlotIndex)
    {
        if (YandexGame.EnvironmentData.isDesktop)
        {
            _weaponsByIndex[_selectedWeaponIndex].TargetMarker.SetActive(false);
            _weaponsByIndex[selectedSlotIndex].TargetMarker.SetActive(true);
        }

        Vector3 newPos = _weaponPointsTransform[selectedSlotIndex - 1].position + _weaponsByIndex[selectedSlotIndex].ObserverPos;

        CameraManager.Instance.ChangeInitPos(newPos);
        Camera.main.transform.position = newPos;

        if (!YandexGame.EnvironmentData.isDesktop)
        {
            _lastCameraRotationDataByWeaponIndex[_selectedWeaponIndex] = new(_currentCameraRotation.y, _currentCameraRotation.x);
            Vector2 newRotation = _lastCameraRotationDataByWeaponIndex[selectedSlotIndex] == null ? Vector2.zero : _lastCameraRotationDataByWeaponIndex[selectedSlotIndex];
            _currentCameraRotation.x = newRotation.y;
            _currentCameraRotation.y = newRotation.x;
            Camera.main.transform.rotation = Quaternion.Euler(newRotation.x, newRotation.y, 0);
        }

        _selectedWeaponIndex = selectedSlotIndex;
    }

    public void StartShoot()
    {
        if (_playerIsDead) return;
        _weaponsByIndex[_selectedWeaponIndex].StartShooting();
    }

    public void StopShoot()
    {
        _weaponsByIndex[_selectedWeaponIndex].StopShooting();
    }

    void RotateWeaponAndCameraByJoystick(Vector2 movementVector)
    {
        if (movementVector == Vector2.zero) return;

        _currentCameraRotation.x += movementVector.x * Time.deltaTime * _weaponsByIndex[_selectedWeaponIndex].RotationSpeed;
        _currentCameraRotation.y -= movementVector.y * Time.deltaTime * _weaponsByIndex[_selectedWeaponIndex].RotationSpeed;
        _currentCameraRotation.x = Mathf.Clamp(_currentCameraRotation.x, -GameConfig.Instance.MaxYRotateAngle, GameConfig.Instance.MaxYRotateAngle);
        _currentCameraRotation.y = Mathf.Clamp(_currentCameraRotation.y, -GameConfig.Instance.MaxXRotateAngle, GameConfig.Instance.MaxXRotateAngle);

        Camera.main.transform.rotation = Quaternion.Euler(_currentCameraRotation.y, _currentCameraRotation.x, 0);
        

        Ray cameraRay = new(Camera.main.transform.position, Camera.main.transform.forward);
        Vector3 _lastCameraHitPoint;
        if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, 100000, _layerMask))
        {
            _lastCameraHitPoint = hitInfo.point;
        }
        else return;

        Vector3 startPos = Vector3.zero;

        if (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers.Length == 1)
        {
            startPos = _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[0].transform.position;
        }
        else if (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers.Length == 2)
        {
            startPos = (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[0].transform.position + _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[1].transform.position) / 2;
        }

        Vector3 targetDirection = _lastCameraHitPoint - startPos;
        Quaternion quaternion = Quaternion.LookRotation(targetDirection);

        Vector3 eulerAngles = quaternion.eulerAngles;

        if (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers.Length == 2)
        {
            foreach (var weaponBarrel in _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers)
            {
                weaponBarrel.transform.parent.LookAt(_lastCameraHitPoint);
            }
        }

        RotateSelectedWeapon(eulerAngles.x, eulerAngles.y);
    }



    void RotateWeaponAndCameraByMouse()
    {
        _currentCameraRotation.x += Input.GetAxis("Mouse X") * _mouseSensitivity;
        _currentCameraRotation.y -= Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _currentCameraRotation.y = Mathf.Clamp(_currentCameraRotation.y, -GameConfig.Instance.MaxXRotateAngle, GameConfig.Instance.MaxXRotateAngle);
        _currentCameraRotation.x = Mathf.Clamp(_currentCameraRotation.x, -GameConfig.Instance.MaxYRotateAngle, GameConfig.Instance.MaxYRotateAngle);

        Camera.main.transform.rotation = Quaternion.Euler(_currentCameraRotation.y, _currentCameraRotation.x, 0);
        Ray cameraRay = new(Camera.main.transform.position, Camera.main.transform.forward);
        Vector3 _lastCameraHitPoint;
        if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, 100000, _layerMask))
        {
            _lastCameraHitPoint = hitInfo.point;
        }
        else return;


        Vector3 weaponRayStartPos = Vector3.zero;
        Transform aimPoint = _weaponsByIndex[_selectedWeaponIndex].TargetMarkerLine.transform.parent;
        Vector3 weaponRayDirection = aimPoint.forward;
        if (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers.Length == 1)
        {
            weaponRayStartPos = _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[0].transform.parent.position;
        }
        else if (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers.Length == 2)
        {
            weaponRayStartPos = (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[0].transform.position + _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[1].transform.position) / 2;
        }


        Ray weaponRay = new(weaponRayStartPos, weaponRayDirection);

        if (Physics.Raycast(weaponRay, out RaycastHit firePointhitInfo, 100000, _layerMask))
        {
            Vector3 markerDir = Vector3.Normalize(firePointhitInfo.point - Camera.main.transform.position);
            Vector3 pos = Camera.main.transform.position + markerDir * 200;
            _weaponsByIndex[_selectedWeaponIndex].TargetMarker.transform.position = pos;
        }

        float RotationRadiansSpeed = _weaponsByIndex[_selectedWeaponIndex].RotationSpeed * Mathf.Deg2Rad;

        Vector3 targetDirection = _lastCameraHitPoint - weaponRayStartPos;
        Vector3 newDirection = Vector3.RotateTowards(aimPoint.forward, targetDirection, RotationRadiansSpeed * Time.deltaTime, 0.0f);
        Quaternion quaternion = Quaternion.LookRotation(newDirection);

        Vector3 eulerAngles = quaternion.eulerAngles;

        //curWeaponRotationX = eulerAngles.x;
        //curWeaponRotationY = eulerAngles.y;

        RotateSelectedWeapon(eulerAngles.x, eulerAngles.y);

        if (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers.Length == 2)
        {
            foreach (var weaponBarrel in _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers)
            {
                Vector3 dir = firePointhitInfo.point - weaponBarrel.transform.parent.position;
                Vector3 newDir = Vector3.RotateTowards(weaponBarrel.transform.parent.forward, dir, RotationRadiansSpeed * Time.deltaTime, 0.0f);
                Quaternion quaternion1 = Quaternion.LookRotation(newDir);

                Vector3 eulerAngles1 = quaternion1.eulerAngles;
                weaponBarrel.transform.parent.rotation = Quaternion.Euler(eulerAngles1.x, eulerAngles1.y, 0);
            }
        }
    }

    void RotateSelectedWeapon(float x, float y)
    {
        _weaponsByIndex[_selectedWeaponIndex].BaseTransform.rotation = Quaternion.Euler(0, y, 0);
        _weaponsByIndex[_selectedWeaponIndex].TurretTransform.localRotation = Quaternion.Euler(x, 0, 0);
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

/*
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

            //_weaponsByIndex[_selectedWeaponIndex].TargetMarkerLine.SetPosition(0, aimPoint.position);
            //_weaponsByIndex[_selectedWeaponIndex].TargetMarkerLine.SetPosition(1, firePointhitInfo.point);
            ////_weaponsByIndex[_selectedWeaponIndex].TargetMarkerTEST.transform.position = firePointhitInfo.point;


            //Vector3 directionToPoint = firePointhitInfo.point - Camera.main.transform.position;

            //Debug.DrawRay(Camera.main.transform.position, directionToPoint * 100000, Color.yellow);

            //Vector3 directionToPoint = firePointhitInfo.point - Camera.main.transform.position;

            //Debug.DrawRay(Camera.main.transform.position, directionToPoint * 100000, Color.yellow);

            //Vector3 dir = Vector3.Normalize(firePointhitInfo.point - Camera.main.transform.position);
            //Vector3 pos = Camera.main.transform.position + dir * 200;


//void ChangeWeaponOnPoint(int weaponIndex)
    //{
    //    Transform point = _weaponPoints[weaponIndex];

    //    if (point.childCount > 1)
    //    {
    //        foreach (Transform t in point) Destroy(t.gameObject);
    //        CreateWeaponInstance(weaponIndex, point);
    //    }
    //    else if (_weaponPoints[weaponIndex].childCount == 0)
    //    {
    //        CreateWeaponInstance(weaponIndex, point);
    //    }
    //    else if (point.childCount == 1)
    //    {
    //        WeaponData existWeaponData = (WeaponData)_weapons[weaponIndex].GetItemData();
    //        if (PlayerData.Instance.EquipedItems[weaponIndex].Equals(existWeaponData.deffWeaponName)) return;
    //        foreach (Transform t in point) Destroy(t.gameObject);
    //        CreateWeaponInstance(weaponIndex, point);
    //    }
    //}

    //void CreateWeaponInstance(int weaponIndex, Transform weaponPoint)
    //{
    //    string weaponName = PlayerData.Instance.EquipedItems[weaponIndex];
    //    WeaponData wData = (WeaponData)PlayerData.Instance.GetItemDataByName(weaponName);
    //    PlayerWeapon weaponPF = GameAssets.Instance.GameItems.Weapons.Find(weapon => weapon.name == weaponName);

    //    PlayerWeapon newWeaponInstance = Instantiate(weaponPF, weaponPoint);
    //    newWeaponInstance.SetItemData(wData);
    //    newWeaponInstance.TargetMarker.SetActive(false);
    //    _weapons[weaponIndex] = newWeaponInstance;
    //    _weaponsRotationData[weaponIndex] = Vector2.zero;
    //}

 public void OnPlayerStartRaid()
    {
        _playerOnRaid = true;
        _playerIsDead = false;
        _weaponsByIndex[_selectedWeaponIndex].TargetMarker.SetActive(true);



        //UpdateWeaponsData();
        //_selectedWeaponIndex = 1;
        //foreach (var weapon in _weaponsByIndex)
        //{
        //    //if (weapon.Value == null)
        //    //{
        //    //    _weapons.Remove(weapon.Key);
        //    //    continue;
        //    //}


        //    //Debug.LogWarning(weapon.Key);
        //    //Debug.LogWarning(weapon.Value);

        //    if (weapon.Key != _selectedWeaponIndex)
        //    {
        //        weapon.Value.TargetMarker.SetActive(false);
        //    }
        //    else
        //        weapon.Value.TargetMarker.SetActive(true);
        //}

        //foreach (var point in _weaponPoints)
        //{
        //    point.Value.rotation = Quaternion.Euler(0, 0, 0);
        //}

        //for (int i = 1; i <= _weaponsRotationData.Count; i++)
        //{
        //    _weaponsRotationData[i] = Vector2.zero;
        //}

        //CameraManager.Instance.ChangeInitPos(_weaponPoints[1].position + _weaponsByIndex[1].ObserverPos);
        //Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
        //curWeaponRotationX = 0;
        //curWeaponRotationY = 0;

        //if (LevelManager.Instance.GetSelectedLevelinfo().LevelName == "1-1")
        //{
        //    _isFirstLevel = true;
        //}
        //else
        //{
        //    _isFirstLevel = false;
        //}
    }

 ////_weaponPoints = PlayerVehicleManager.Instance.PlayerVehicle.WeaponPoints.ToDictionary(slot => slot.Index, slot => slot.Transform);

        //for (int weaponIndex = 1; weaponIndex <= _weapons.Count; weaponIndex++)
        //{
        //    if (!PlayerData.Instance.EquipedItems.ContainsKey(weaponIndex))
        //    {
        //        _weapons.Remove(weaponIndex);
        //    }
        //    //Debug.LogWarning(weaponIndex);
        //    //Debug.LogWarning(_weapons[weaponIndex]);
        //    //Debug.LogWarning(PlayerData.Instance.EquipedItems[weaponIndex]);
        //}


        //for (int weaponIndex = 1; weaponIndex < PlayerData.Instance.EquipedItems.Count; weaponIndex++)
        //{
        //    //Debug.LogWarning(weaponIndex);
        //    ChangeWeaponOnPoint(weaponIndex);
        //}


//RotateWeaponAndCameraByMouse();
        //RotateWeaponAndCameraByJoystick(UIJoystickTouchController.Instance.GetJoystickPosition);


        //RotateWeaponAndCameraByWASD();

        //foreach (var item in _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers)
        //{
        //    Debug.DrawRay(item.transform.position, item.transform.forward * 100000, Color.green);
        //    if (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers.Length == 2)
        //    {

        //        Vector3 startPos = (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[0].transform.position + _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[1].transform.position) / 2;
        //        Debug.DrawRay(startPos, (_lastCameraHitPoint - startPos) * 100000, Color.black);
        //    }
        //}
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100000, Color.red);
*/
