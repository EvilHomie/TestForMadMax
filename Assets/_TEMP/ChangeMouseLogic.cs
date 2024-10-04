
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ChangeMouseLogic : MonoBehaviour
{
    [SerializeField] GameObject _mouseCursor;
    [SerializeField] Transform _testObjectForRay;
    [SerializeField] Camera _Testcamera;
    [SerializeField] LayerMask _testLayerMask;
    [SerializeField] Vector3 _mouseDetectLimiter;


    Plane plane = new(Vector3.forward, Vector3.forward * 5);

    Vector3 _targetPos;

    Vector3 _targetPosPrev;

    Ray _cameraToMousePointRay;

    Vector3 _cameraRoation;

    private void Update()
    {

        RotateCameraWithLimiter2();

        //RotateCameraWithLimiter();
        //CheckLimiterArea();

        //PlaneRay();

        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));


        //_targetPosPrev = _targetPos;

        //if (Input.GetMouseButton(1))
        //    GetTargetPos();



        //RotateCamera();
        //Debug.Log(Input.mousePosition);

        //Debug.Log(Input.GetAxis("Mouse X"));
        //Debug.Log(Input.GetAxis("Mouse Y"));
    }

    void RotateCameraWithLimiter2()
    {
        CheckLimiterArea2(out Vector2 pointOutOfLimitdistanceMod);

        Debug.Log(pointOutOfLimitdistanceMod);

        if (pointOutOfLimitdistanceMod != Vector2.zero)
        {
            Debug.Log("ROTATE");
            RotateCameraToMousePoint(pointOutOfLimitdistanceMod);
        }
        else Debug.Log("STOP ROTATE");
    }


    void CheckLimiterArea2(out Vector2 pointOutOfLimitdistanceMod)
    {
        float detectSquareSize = Screen.height / 8;
        Vector2 screenCenter = new(Screen.width / 2, Screen.height / 2);
        Vector2 mousePosOnScreen = Input.mousePosition;
        Vector2 mouseOffsetOfCenter = mousePosOnScreen - screenCenter;

        bool inLimiterAreaByX = Mathf.Abs(mouseOffsetOfCenter.x) < detectSquareSize;
        bool inLimiterAreaByY = Mathf.Abs(mouseOffsetOfCenter.y) < detectSquareSize;

        if (inLimiterAreaByX && inLimiterAreaByY)
        {
            Cursor.visible = false;
            pointOutOfLimitdistanceMod = Vector2.zero;
        }
        else
        {
            Cursor.visible = true;
            Vector2 zeroToOne = new(mousePosOnScreen.x / Screen.width, mousePosOnScreen.y / Screen.height);
            Vector2 minusOneToOne;

            minusOneToOne.x = ((1 - zeroToOne.x) - 0.5f) * -2f;
            minusOneToOne.y = ((1 - zeroToOne.y) - 0.5f) * -2f;
            pointOutOfLimitdistanceMod = minusOneToOne;
        }


    }





    void RotateCameraWithLimiter1()
    {
        CheckLimiterArea1(out Vector3 pointOutOfLimitArea);
        if (pointOutOfLimitArea != Vector3.zero)
        {
            Debug.Log("ROTATE");
            RotateCameraToMousePoint(pointOutOfLimitArea);
        }
        else Debug.Log("STOP ROTATE");
    }


    void CheckLimiterArea1(out Vector3 pointOutOfLimitArea)
    {
        _cameraToMousePointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(_cameraToMousePointRay, out RaycastHit raycastHit, 50, _testLayerMask);

        //Debug.Log(raycastHit.distance);
        //raycastHit.


        bool inLimiterAreaByX = Mathf.Abs(raycastHit.point.x) < _mouseDetectLimiter.x;
        bool inLimiterAreaByY = Mathf.Abs(raycastHit.point.y) < _mouseDetectLimiter.y;

        if (inLimiterAreaByX && inLimiterAreaByY) pointOutOfLimitArea = Vector3.zero;
        else pointOutOfLimitArea = raycastHit.point;
    }

    void RotateCameraToMousePoint(Vector2 pointOutOfLimitArea)
    {
        _cameraRoation.y += pointOutOfLimitArea.x * 0.05f;
        _cameraRoation.x -= pointOutOfLimitArea.y * 0.05f;
        _cameraRoation.y = Mathf.Clamp(_cameraRoation.y, -30, 30);
        _cameraRoation.x = Mathf.Clamp(_cameraRoation.x, -60, 60);

        Camera.main.transform.rotation = Quaternion.Euler(_cameraRoation);
    }




    void PlaneRay()
    {
        Vector3 worldPosition = Vector3.zero;
        Vector3 screenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (plane.Raycast(ray, out float distance))
        {
            worldPosition = ray.GetPoint(distance);
        }
        _mouseCursor.transform.position = worldPosition;
        //worldPosition.z = 0;
        Debug.Log(worldPosition);
        //Debug.Log(distance);
    }

    private void RotateCamera()
    {
        //Vector3 temp = _targetPos;
        float RotationRadiansSpeed = 45f * Mathf.Deg2Rad;

        Vector3 targetDirection = _targetPos - Camera.main.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(Camera.main.transform.forward, targetDirection, RotationRadiansSpeed * Time.deltaTime, 0f);
        Quaternion quaternion = Quaternion.LookRotation(newDirection);

        Vector3 eulerAngles = quaternion.eulerAngles;

        Camera.main.transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 0);
        //_targetPos = _targetPosPrev;
    }

    private void GetTargetPos()
    {
        Ray ray = _Testcamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            _targetPos = raycastHit.point;
            _mouseCursor.transform.position = raycastHit.point;
        }
    }

    //private void LateUpdate()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    if (Physics.Raycast(ray, out RaycastHit raycastHit))
    //    {
    //        _mouseCursor.transform.position = raycastHit.point;


    //        //Camera.main.transform.LookAt(raycastHit.point);




    //        //if (Physics.Raycast(weaponRay, out RaycastHit firePointhitInfo, 100000, _layerMask))
    //        //{
    //        //    Vector3 markerDir = Vector3.Normalize(firePointhitInfo.point - Camera.main.transform.position);
    //        //    Vector3 pos = Camera.main.transform.position + markerDir * 200;
    //        //    _weaponsByIndex[_selectedWeaponIndex].TargetMarker.transform.position = pos;
    //        //}

    //        float RotationRadiansSpeed = 45f * Mathf.Deg2Rad;

    //        Vector3 targetDirection = raycastHit.point - Camera.main.transform.position;
    //        Vector3 newDirection = Vector3.RotateTowards(Camera.main.transform.forward, targetDirection, RotationRadiansSpeed * Time.deltaTime, 0.0f);
    //        Quaternion quaternion = Quaternion.LookRotation(newDirection);

    //        Vector3 eulerAngles = quaternion.eulerAngles;

    //        Camera.main.transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 0);

    //    }
    //}
}
