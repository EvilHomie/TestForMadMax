using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTCOLLISION : MonoBehaviour
{
    [SerializeField] LayerMask _vehicleBodyLayer;
    [SerializeField] LayerMask _A;
    [SerializeField] LayerMask _B;

    private void OnCollisionEnter(Collision collision)
    {
        Physics.IgnoreLayerCollision(_A.value, _B.value, true);


        if( 1 << collision.GetContact(0).thisCollider.gameObject.layer != _vehicleBodyLayer.value) return;


        Debug.Log(collision.GetContact(0).thisCollider.gameObject.name);
    }
}
