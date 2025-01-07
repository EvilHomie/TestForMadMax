using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance;

    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _mouseSensitivity = 0.5f;
    [SerializeField] float _fingerSensitivity = 0.1f;
    [SerializeField] Transform[] _weaponPointsTransform;
    [SerializeField] GameObject _cameraCursor;
    //[SerializeField] bool test;
    Vector3 _currentCameraRotation;

    Dictionary<int, PlayerWeapon> _weaponsByIndex = new();

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
        //DetectFingerManager.Instance.Init();
        OnCloseInventory();
    }

    private void LateUpdate()
    {
        //YandexGame.EnvironmentData.isDesktop = test;
        if (_playerIsDead || !_playerOnRaid) return;
        if (GameFlowManager.GameFlowState == GameFlowState.PAUSE) return;
        RotateWeaponToCamera();
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
    }

    void CreateSpecificWeaponInstance(WeaponData weaponData)
    {
        PlayerWeapon weaponPF = GameAssets.Instance.GameItems.Weapons.Find(weapon => weapon.name == weaponData.DeffItemName);
        Transform matchPoint = _weaponPointsTransform[0];
        PlayerWeapon newWeaponInstance = Instantiate(weaponPF, matchPoint);
        newWeaponInstance.SetItemData(weaponData);
        newWeaponInstance.TargetMarker.SetActive(false);
        _weaponsByIndex[1] = newWeaponInstance;
    }

    public void OnPlayerStartRaid()
    {
        _playerOnRaid = true;
        _playerIsDead = false;
        _weaponsByIndex[_selectedWeaponIndex].TargetMarker.SetActive(true);
        _cameraCursor.SetActive(true);
    }


    public void OnPlayerEndRaid()
    {
        _playerOnRaid = false;
        _weaponsByIndex[_selectedWeaponIndex].StopShooting();
        _cameraCursor.SetActive(false);
    }

    public void OnStartSurviveMod(WeaponData startWeaponData)
    {
        ResetData();
        CreateSpecificWeaponInstance(startWeaponData);
        ResetCameraPos();
        OnPlayerStartRaid();
    }

    void ResetCameraPos()
    {
        Vector3 newPos = _weaponPointsTransform[0].position + _weaponsByIndex[1].ObserverPos;
        CameraManager.Instance.ChangeInitPos(newPos);
        Camera.main.transform.SetPositionAndRotation(newPos, Quaternion.Euler(Vector3.zero));
        _currentCameraRotation = Vector3.zero;
        _cameraCursor.SetActive(false);
    }

    public void ChangeWeapon(int selectedSlotIndex)
    {
        _weaponsByIndex[_selectedWeaponIndex].TargetMarker.SetActive(false);
        _weaponsByIndex[selectedSlotIndex].TargetMarker.SetActive(true);
        Vector3 newPos = _weaponPointsTransform[selectedSlotIndex - 1].position + _weaponsByIndex[selectedSlotIndex].ObserverPos;
        CameraManager.Instance.ChangeInitPos(newPos);
        _selectedWeaponIndex = selectedSlotIndex;
    }

    void ClampCameraRotation()
    {
        if (YandexGame.EnvironmentData.isDesktop)
        {
            _currentCameraRotation.y = Mathf.Clamp(_currentCameraRotation.y, -GameConfig.Instance.MaxYRotateAngle, GameConfig.Instance.MaxYRotateAngle);
            _currentCameraRotation.x = Mathf.Clamp(_currentCameraRotation.x, -GameConfig.Instance.MaxXRotateAngle, GameConfig.Instance.MaxXRotateAngle);
        }
        else
        {
            _currentCameraRotation.x = Mathf.Clamp(_currentCameraRotation.x, -GameConfig.Instance.MaxXRotateAngleAndroid, GameConfig.Instance.MaxXRotateAngleAndroid);
            _currentCameraRotation.y = Mathf.Clamp(_currentCameraRotation.y, -GameConfig.Instance.MaxYRotateAngleAndroid, GameConfig.Instance.MaxYRotateAngleAndroid);
        }
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

    public void RotateCameraByFinger(Vector3 fingerPosDifference)
    {
        if (_playerIsDead || !_playerOnRaid) return;
        _currentCameraRotation.x += fingerPosDifference.y * _fingerSensitivity;
        _currentCameraRotation.y -= fingerPosDifference.x * _fingerSensitivity;
        ClampCameraRotation();
        Camera.main.transform.rotation = Quaternion.Euler(_currentCameraRotation);
    }

    void MouseCursorFollowMousePos()
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f;
        _cameraCursor.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    void CheckMouseDeadZone(out Vector2 pointOutOfDeadZone)
    {
        float detectSquareSize = Screen.height / 8;
        Vector2 screenCenter = new(Screen.width / 2, Screen.height / 2);
        Vector2 mousePosOnScreen = Input.mousePosition;


        Vector2 mouseOffsetOfCenter = mousePosOnScreen - screenCenter;

        bool inLimiterAreaByX = Mathf.Abs(mouseOffsetOfCenter.x) < detectSquareSize;
        bool inLimiterAreaByY = Mathf.Abs(mouseOffsetOfCenter.y) < detectSquareSize;

        if (inLimiterAreaByX && inLimiterAreaByY)
        {
            pointOutOfDeadZone = Vector2.zero;
        }
        else
        {
            Vector2 zeroToOne = new(mousePosOnScreen.x / Screen.width, mousePosOnScreen.y / Screen.height);
            Vector2 minusOneToOne;

            minusOneToOne.x = ((1 - zeroToOne.x) - 0.5f) * -2f;
            minusOneToOne.y = ((1 - zeroToOne.y) - 0.5f) * -2f;
            pointOutOfDeadZone = minusOneToOne;
        }
    }

    void RotateCameraToMousePoint()
    {
        MouseCursorFollowMousePos();
        CheckMouseDeadZone(out Vector2 pointOutOfDeadZone);
        if (pointOutOfDeadZone != Vector2.zero)
        {
            _currentCameraRotation.y += pointOutOfDeadZone.x * _mouseSensitivity;
            _currentCameraRotation.x -= pointOutOfDeadZone.y * _mouseSensitivity;
            ClampCameraRotation();
            Camera.main.transform.rotation = Quaternion.Euler(_currentCameraRotation);
        }
    }




    void RotateWeaponToCamera()
    {
        Ray cameraRay;
        if (YandexGame.EnvironmentData.isDesktop)
        {
            RotateCameraToMousePoint();
            cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        else
        {
            //RotateCameraByFinger();
            cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }

        Vector3 _lastMouseHitPoint;
        if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, 1000000, _layerMask))
        {
            _lastMouseHitPoint = hitInfo.point;
        }
        else return;

        Vector3 weaponRayStartPos = Vector3.zero;
        Transform aimPoint = _weaponsByIndex[_selectedWeaponIndex].TargetMarker.transform.parent;

        if (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers.Length == 1)
        {
            weaponRayStartPos = _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers[0].transform.position;
        }
        else if (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers.Length > 1)
        {
            Vector3 positionsSumm = Vector3.zero;
            int count = 0;
            foreach (var firePoint in _weaponsByIndex[_selectedWeaponIndex].FirePointsManagers)
            {
                positionsSumm += firePoint.transform.position;
                count++;
            }

            weaponRayStartPos = positionsSumm / count;
        }


        Ray weaponRay = new(weaponRayStartPos, aimPoint.forward);

        if (Physics.Raycast(weaponRay, out RaycastHit firePointhitInfo, 1000000, _layerMask))
        {
            Vector3 markerDir = Vector3.Normalize(firePointhitInfo.point - Camera.main.transform.position);
            Vector3 pos = Camera.main.transform.position + markerDir * 200;
            _weaponsByIndex[_selectedWeaponIndex].TargetMarker.transform.position = pos;
        }

        float RotationRadiansSpeed = _weaponsByIndex[_selectedWeaponIndex].RotationSpeed * Mathf.Deg2Rad;

        Vector3 targetDirection = _lastMouseHitPoint - weaponRayStartPos;
        Vector3 newDirection = Vector3.RotateTowards(aimPoint.forward, targetDirection, RotationRadiansSpeed * Time.deltaTime, 0.0f);
        Vector3 lookRotation = Quaternion.LookRotation(newDirection).eulerAngles;

        RotateSelectedWeapon(lookRotation);

        if (_weaponsByIndex[_selectedWeaponIndex].FirePointsManagers.Length > 1)
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

    void RotateSelectedWeapon(Vector3 rotation)
    {
        _weaponsByIndex[_selectedWeaponIndex].BaseTransform.rotation = Quaternion.Euler(0, rotation.y, 0);
        _weaponsByIndex[_selectedWeaponIndex].TurretTransform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
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
        Vector3 startRotation = Camera.main.transform.rotation.eulerAngles;
        Vector3 endRotation = new(GameConfig.Instance.MaxXRotateAngle, startRotation.y, 0);

        while (t < 1)
        {
            t += Time.deltaTime / GameConfig.Instance.TimeForChangeSpeed;

            Quaternion rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, Quaternion.Euler(endRotation), 15 * Time.deltaTime);
            Camera.main.transform.rotation = rotation;
            RotateSelectedWeapon(rotation.eulerAngles);
            yield return null;
        }
    }
}
