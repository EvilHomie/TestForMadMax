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
    [SerializeField] GameObject _targetMarkerForMobile;
    [SerializeField] GameObject _mouseCursor;
    Vector3 _currentCameraRotation;

    Dictionary<int, PlayerWeapon> _weaponsByIndex = new();
    Dictionary<int, Vector2> _lastCameraRotationDataByWeaponIndex = new();

    int _selectedWeaponIndex = 1;

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
        
        if (YandexGame.EnvironmentData.isDesktop)
        {
            _targetMarkerForMobile.SetActive(false);            
        }
        else
        {
            _targetMarkerForMobile.SetActive(true);
            _mouseCursor.transform.localPosition = new Vector3(0, 0, 7);
        }
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
        _currentCameraRotation.x = Mathf.Clamp(_currentCameraRotation.x, -GameConfig.Instance.MaxYRotateAngleAndroid, GameConfig.Instance.MaxYRotateAngleAndroid);
        _currentCameraRotation.y = Mathf.Clamp(_currentCameraRotation.y, -GameConfig.Instance.MaxXRotateAngleAndroid, GameConfig.Instance.MaxXRotateAngleAndroid);

        Camera.main.transform.rotation = Quaternion.Euler(_currentCameraRotation.y, _currentCameraRotation.x, 0);
        

        Ray cameraRay = new(Camera.main.transform.position, Camera.main.transform.forward);
        Vector3 _lastCameraHitPoint;
        if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, 1000000, _layerMask))
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

    void CheckMouseDeadZone(out Vector2 pointOutOfDeadZoneMod)
    {
        float detectSquareSize = Screen.height / 8;
        Vector2 screenCenter = new(Screen.width / 2, Screen.height / 2);
        Vector2 mousePosOnScreen = Input.mousePosition;

       
        Vector2 mouseOffsetOfCenter = mousePosOnScreen - screenCenter;

        bool inLimiterAreaByX = Mathf.Abs(mouseOffsetOfCenter.x) < detectSquareSize;
        bool inLimiterAreaByY = Mathf.Abs(mouseOffsetOfCenter.y) < detectSquareSize;

        if (inLimiterAreaByX && inLimiterAreaByY)
        {
            pointOutOfDeadZoneMod = Vector2.zero;
        }
        else
        {
            Vector2 zeroToOne = new(mousePosOnScreen.x / Screen.width, mousePosOnScreen.y / Screen.height);
            Vector2 minusOneToOne;

            minusOneToOne.x = ((1 - zeroToOne.x) - 0.5f) * -2f;
            minusOneToOne.y = ((1 - zeroToOne.y) - 0.5f) * -2f;
            pointOutOfDeadZoneMod = minusOneToOne;
        }
    }

    void RotateCameraToMousePoint(Vector2 pointOutOfLimitArea)
    {
        _currentCameraRotation.y += pointOutOfLimitArea.x * _mouseSensitivity;
        _currentCameraRotation.x -= pointOutOfLimitArea.y * _mouseSensitivity;
        _currentCameraRotation.y = Mathf.Clamp(_currentCameraRotation.y, -GameConfig.Instance.MaxYRotateAngle, GameConfig.Instance.MaxYRotateAngle);
        _currentCameraRotation.x = Mathf.Clamp(_currentCameraRotation.x, -GameConfig.Instance.MaxXRotateAngle, GameConfig.Instance.MaxXRotateAngle);
        
        Camera.main.transform.rotation = Quaternion.Euler(_currentCameraRotation);
    }
    void MouseCursorFollowMousePos()
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f;
        _mouseCursor.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }



    void RotateWeaponAndCameraByMouse()
    {
        MouseCursorFollowMousePos();
        CheckMouseDeadZone(out Vector2 pointOutOfLimitdistanceMod);

        if (pointOutOfLimitdistanceMod != Vector2.zero)
        {
            RotateCameraToMousePoint(pointOutOfLimitdistanceMod);
        }

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 _lastMouseHitPoint;
        if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, 1000000, _layerMask))
        {
            _lastMouseHitPoint = hitInfo.point;
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

        if (Physics.Raycast(weaponRay, out RaycastHit firePointhitInfo, 1000000, _layerMask))
        {
            Vector3 markerDir = Vector3.Normalize(firePointhitInfo.point - Camera.main.transform.position);
            Vector3 pos = Camera.main.transform.position + markerDir * 200;
            _weaponsByIndex[_selectedWeaponIndex].TargetMarker.transform.position = pos;
        }

        float RotationRadiansSpeed = _weaponsByIndex[_selectedWeaponIndex].RotationSpeed * Mathf.Deg2Rad;

        Vector3 targetDirection = _lastMouseHitPoint - weaponRayStartPos;
        Vector3 newDirection = Vector3.RotateTowards(aimPoint.forward, targetDirection, RotationRadiansSpeed * Time.deltaTime, 0.0f);
        Quaternion quaternion = Quaternion.LookRotation(newDirection);

        Vector3 eulerAngles = quaternion.eulerAngles;

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
