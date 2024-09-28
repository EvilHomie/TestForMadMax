using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance;

    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _mouseSensitivity = 5f;
    [SerializeField] Transform[] _weaponPointsTransform;
    Vector3 _currentCameraRotation;


    //Dictionary<int, Transform> _weaponPoints = new();
    Dictionary<int, PlayerWeapon> _weaponsByIndex = new();
    Dictionary<int, Vector2> _weaponsRotationData = new();

    int _selectedWeaponIndex = 1;
    float curWeaponRotationY = 0;
    float curWeaponRotationX = 0;

    bool _playerIsDead = false;
    bool _playerOnRaid = false;
    bool _isFirstLevel = false;
    bool _firstTouchStatus = false;


    Vector3 _lastCameraHitPoint;

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
        //if (YandexGame.EnvironmentData.isDesktop)
        //{
        //    RotateWeaponAndCameraByMouse();
        //}
        //else
        //{
        //    RotateWeaponAndCameraByJoystick(UIJoystickTouchController.Instance.GetJoystickPosition);
        //}

        RotateWeaponAndCameraByMouse();
        //RotateWeaponAndCameraByJoystick(UIJoystickTouchController.Instance.GetJoystickPosition);


        //RotateWeaponAndCameraByWASD();

        foreach (var item in _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers)
        {
            Debug.DrawRay(item.transform.position, item.transform.forward * 100000, Color.green);
            if (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers.Length == 2)
            {
                Vector3 startPos = (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[0].transform.position + _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[1].transform.position) / 2;
                Debug.DrawRay(startPos, (_lastCameraHitPoint - startPos) * 100000, Color.black);
            }
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100000, Color.red);
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
        _weaponsRotationData.Clear();
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
        newWeaponInstance.TargetMarkerLine.gameObject.SetActive(false);
        _weaponsByIndex[weaponIndex] = newWeaponInstance;
        _weaponsRotationData[weaponIndex] = Vector2.zero;
        CustomLogDebuger.Log("WC");
    }

    public void OnPlayerStartRaid()
    {
        _playerOnRaid = true;
        _playerIsDead = false;
        _weaponsByIndex[_selectedWeaponIndex].TargetMarkerLine.gameObject.SetActive(true);
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
        Camera.main.transform.rotation = Quaternion.Euler(0,0,0);
    }

    public void ChangeWeapon(int selectedSlotIndex)
    {
        _weaponsByIndex[_selectedWeaponIndex].TargetMarkerLine.gameObject.SetActive(false);
        _weaponsRotationData[_selectedWeaponIndex] = new Vector2(curWeaponRotationX, curWeaponRotationY);

        _weaponsByIndex[selectedSlotIndex].TargetMarkerLine.gameObject.SetActive(true);
        curWeaponRotationX = _weaponsRotationData[selectedSlotIndex].x;
        curWeaponRotationY = _weaponsRotationData[selectedSlotIndex].y;

        CameraManager.Instance.ChangeInitPos(_weaponPointsTransform[selectedSlotIndex].position + _weaponsByIndex[selectedSlotIndex].ObserverPos);
        Camera.main.transform.rotation = Quaternion.Euler(-curWeaponRotationX, curWeaponRotationY, 0);

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

        curWeaponRotationX += movementVector.x * Time.deltaTime * _weaponsByIndex[_selectedWeaponIndex].RotationSpeed;
        curWeaponRotationY -= movementVector.y * Time.deltaTime * _weaponsByIndex[_selectedWeaponIndex].RotationSpeed;
        curWeaponRotationX = Mathf.Clamp(curWeaponRotationX, -GameConfig.Instance.MaxYRotateAngle, GameConfig.Instance.MaxYRotateAngle);
        curWeaponRotationY = Mathf.Clamp(curWeaponRotationY, -GameConfig.Instance.MaxXRotateAngle, GameConfig.Instance.MaxXRotateAngle);

        Camera.main.transform.rotation = Quaternion.Euler(curWeaponRotationY, curWeaponRotationX,  0);        

        Ray cameraRay = new(Camera.main.transform.position, Camera.main.transform.forward);

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
            startPos = (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[0].transform.position + _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[1].transform.position)/2;
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

        if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, 100000, _layerMask))
        {
            _lastCameraHitPoint = hitInfo.point;
        }
        else return;

        
        Vector3 weaponRayStartPos = Vector3.zero;
        Vector3 weaponRayDirection = Vector3.zero;
        if (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers.Length == 1)
        {
            weaponRayStartPos = _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[0].transform.parent.position;
            weaponRayDirection = _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[0].transform.parent.forward;
        }
        else if (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers.Length == 2)
        {
            weaponRayStartPos = (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[0].transform.position + _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[1].transform.position) / 2;
            weaponRayDirection = (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[0].transform.forward + _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[1].transform.forward) / 2;
        }


        Ray weaponRay = new(weaponRayStartPos, weaponRayDirection);

        if (Physics.Raycast(weaponRay, out RaycastHit firePointhitInfo, 100000, _layerMask))
        {
            Transform aimPoint = _weaponsByIndex[_selectedWeaponIndex].TargetMarkerLine.transform.parent;
            _weaponsByIndex[_selectedWeaponIndex].TargetMarkerLine.SetPosition(0, aimPoint.position);
            _weaponsByIndex[_selectedWeaponIndex].TargetMarkerLine.SetPosition(1, firePointhitInfo.point);
        }

        float RotationRadiansSpeed = _weaponsByIndex[_selectedWeaponIndex].RotationSpeed * Mathf.Deg2Rad;

        Vector3 targetDirection = _lastCameraHitPoint - weaponRayStartPos;
        Vector3 newDirection = Vector3.RotateTowards(_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[0].transform.forward, targetDirection, RotationRadiansSpeed * Time.deltaTime, 0.0f);
        Quaternion quaternion = Quaternion.LookRotation(newDirection);

        Vector3 eulerAngles = quaternion.eulerAngles;

        curWeaponRotationX = eulerAngles.x;
        curWeaponRotationY = eulerAngles.y;

        //if (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers.Length == 2)
        //{
        //    foreach (var weaponBarrel in _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers)
        //    {
        //        weaponBarrel.transform.parent.LookAt(_lastCameraHitPoint);
        //    }
        //}
        RotateSelectedWeapon(curWeaponRotationX, curWeaponRotationY);
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
*/
