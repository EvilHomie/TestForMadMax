using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class ControllWithMouse : MonoBehaviour
{
    public float sensitivity = 10f;
    public float maxYAngle = 80f;
    public float maxXAngle = 30f;
    public Vector2 currentRotation;

    public GameObject cannonBase;
    public GameObject cannonTurret;
    public GameObject FirePoint;

    public LayerMask layerMask;
    public bool rotateCamera;
    public GameObject cameraMarker;
    public GameObject weaponMarker;

    public float markerPosMod;

    public float rotationAngleSpeed = 10f;
    float RotationRadiansSpeed => rotationAngleSpeed * Mathf.Deg2Rad;

    private void Start()
    {

    }
    private void LateUpdate()
    {
        //FirstVersion();

        SecondVersion();

    }


    void FirstVersion()
    {
        Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward * 100);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layerMask))
        {
            Debug.LogWarning(hitInfo.point);
            FirePoint.transform.LookAt(hitInfo.point);
        }
        //Debug.LogWarning(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
        currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        //currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
        currentRotation.y = Mathf.Clamp(currentRotation.y, -maxXAngle, maxXAngle);
        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxYAngle, maxYAngle);
        if (rotateCamera)
        {
            Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        }

        cannonBase.transform.rotation = Quaternion.Euler(cannonBase.transform.rotation.y, currentRotation.x, 0);
        cannonTurret.transform.localRotation = Quaternion.Euler(currentRotation.y, 0, 0);

        Debug.DrawRay(FirePoint.transform.position, FirePoint.transform.forward * 100, Color.green);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100, Color.red);
    }

    void SecondVersion()
    {
        Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);

        Vector3 point = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 10000, layerMask))
        {
            point = hitInfo.point;
            //cameraMarker.transform.localPosition = new Vector3(0,0, point.z);
        }

        Ray firePointRay = new(FirePoint.transform.position, FirePoint.transform.forward);

        Vector3 firePointRayPoint = Vector3.zero;

        if (Physics.Raycast(firePointRay, out RaycastHit firePointhitInfo, 10000, layerMask))
        {
            firePointRayPoint = firePointhitInfo.point;
            //weaponMarker.transform.position = firePointRayPoint;

            Vector3 directionToPoint = firePointRayPoint - Camera.main.transform.position;

            Debug.DrawRay(Camera.main.transform.position, directionToPoint * 1000, Color.blue);

            //Vector3 pos = directionToPoint.normalized * 35;

            //weaponMarker.transform.position = pos;



            Vector3 dir = Vector3.Normalize(firePointhitInfo.point - Camera.main.transform.position);
            Vector3 pos = Camera.main.transform.position + dir * 35;
            weaponMarker.transform.position = pos;

        }


        //Debug.LogWarning(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
        currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        //currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
        currentRotation.y = Mathf.Clamp(currentRotation.y, -maxXAngle, maxXAngle);
        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxYAngle, maxYAngle);
        if (rotateCamera)
        {
            Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        }

        Vector3 targetDirectionBase = point - FirePoint.transform.position;
        Vector3 newDirectionForBase = Vector3.RotateTowards(FirePoint.transform.forward, targetDirectionBase, RotationRadiansSpeed * Time.deltaTime, 0.0f);
        Quaternion quaternion = Quaternion.LookRotation(newDirectionForBase);

        Vector3 eulerAngles = quaternion.eulerAngles;


        cannonBase.transform.rotation = Quaternion.Euler(0, eulerAngles.y, 0);
        cannonTurret.transform.localRotation = Quaternion.Euler(eulerAngles.x, 0, 0);
        //FirePoint.transform.rotation = quaternion;

        Debug.DrawRay(FirePoint.transform.position, FirePoint.transform.forward * 1000, Color.green);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1000, Color.red);




    }

    public Vector3 LerpByDistance(Vector3 A, Vector3 B, float x)
    {
        Vector3 P = x * Vector3.Normalize(B - A) + A;
        return P;
    }
}
