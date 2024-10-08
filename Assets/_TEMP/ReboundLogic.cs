using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReboundLogic : MonoBehaviour
{
    [SerializeField] bool testRebound;
    [SerializeField] ReboundDirection reboundDirection;
    [SerializeField] float reboundForceValue;
    [SerializeField] float reboundTorqueValue;
    [SerializeField] float translateSpeed;

    Rigidbody rb;
    bool braker = false;

    private void Update()
    {
        


        if (testRebound && !braker) 
        {
            braker = true;
            gameObject.transform.parent = transform.root;
            rb = gameObject.AddComponent<Rigidbody>();
            //rb.mass = 1f;

            Vector3 dir = reboundDirection switch
            {
                ReboundDirection.X => transform.right,
                ReboundDirection.Y => transform.up,
                ReboundDirection.Z => transform.forward,
                ReboundDirection.XY => transform.right + Vector3.up,
                ReboundDirection.XZ => transform.right + Vector3.forward,
                ReboundDirection.YZ => transform.up + Vector3.forward,
                _ => Vector3.zero
            };

            Debug.Log(dir);
            Debug.Log(Random.onUnitSphere);

            rb.AddForce(dir * reboundForceValue, ForceMode.VelocityChange);
            rb.AddTorque(Random.onUnitSphere * reboundTorqueValue, ForceMode.VelocityChange);
        }
        if (braker)
        {
            //transform.Translate(translateSpeed * Time.deltaTime * Vector3.back, Space.World);
            rb.AddForce(Vector3.back * 100, ForceMode.Acceleration);
        }
    }
    

    public void Detach()
    {
        if (!braker)
        {
            braker = true;
            gameObject.transform.parent = transform.root;
            rb = gameObject.AddComponent<Rigidbody>();
            //rb.mass = 1f;

            Vector3 dir = reboundDirection switch
            {
                ReboundDirection.X => transform.right,
                ReboundDirection.Y => transform.up,
                ReboundDirection.Z => transform.forward,
                ReboundDirection.XY => transform.right + Vector3.up,
                ReboundDirection.XZ => transform.right + Vector3.forward,
                ReboundDirection.YZ => transform.up + Vector3.forward,
                _ => Vector3.zero
            };

            //Debug.Log(dir);
            Vector3 torqDir = Random.onUnitSphere;
            Debug.Log(Random.onUnitSphere);

            rb.AddForce(dir * reboundForceValue, ForceMode.VelocityChange);
            rb.AddTorque(torqDir * reboundTorqueValue, ForceMode.VelocityChange);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        rb.AddForce(Vector3.up * 50, ForceMode.VelocityChange);
    }

    enum ReboundDirection
    {
        X,
        Y,
        Z,
        XY,
        XZ,
        YZ

    }
   
}
