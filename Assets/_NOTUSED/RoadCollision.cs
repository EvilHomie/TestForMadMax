using UnityEngine;

//public class RoadCollision : MonoBehaviour
//{
//    [SerializeField] LayerMask _collisionAffectLayer;

//    private void OnCollisionEnter(Collision collision)
//    {
//        //Debug.LogWarning(collision.GetContact(0).otherCollider.gameObject.name);
//        //Debug.LogWarning(collision.GetContact(0).thisCollider.gameObject.name);
//        //collision.GetContact(0).thisCollider



//        //if (1 << collision.GetContact(0).otherCollider.gameObject.layer == _layerMask.value)
//        //{
//        //    Debug.DrawRay(collision.GetContact(0).point, new Vector3(-1, 0, 0) * 1000f, Color.magenta, 1);
//        //    Debug.DrawRay(collision.GetContact(0).point, new Vector3(0, 1, 0) * 1000f, Color.magenta, 1);
//        //    Debug.LogWarning("AWDAWDWA");
//        //    Rigidbody rb = collision.GetContact(0).otherCollider.GetComponent<Rigidbody>();
//        //    rb.AddForceAtPosition(new Vector3(0,1,0) * RaidManager.Instance.PlayerMoveSpeed * 100f, collision.GetContact(0).point, ForceMode.VelocityChange);
//        //    Debug.LogWarning(RaidManager.Instance.PlayerMoveSpeed);
//        //    //Debug.DrawRay(collision.GetContact(0).point, Vector3.up * 1000f, Color.magenta, 1);
//        //}



//        //if (_vehiclePart == VehiclePart.Wheel)
//        //{
//        //    Debug.LogWarning("SSSSSSSSSSSSS");
//        //    return;
//        //}

//        //Debug.LogWarning("AWDAWDAW");


//        //if (_isBody && collision.gameObject.name == "MainRoad")
//        //{
//        //    _bodyRB.AddForce(Vector3.up * 1000f, ForceMode.VelocityChange);
//        //}
//    }
//}
