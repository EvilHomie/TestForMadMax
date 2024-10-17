using UnityEngine;

public class CollisionsLogic : MonoBehaviour
{
    [SerializeField] LayerMask _vehicleBodyLayer;
    [SerializeField] LayerMask _roadLayer;
    [SerializeField] LayerMask _wheelsLayer;
    EnemyVehicleManager _enemyVehicleManager;
    Rigidbody _RB;

    private void Awake()
    {
        _enemyVehicleManager = transform.root.GetComponent<EnemyVehicleManager>();
        _RB = _enemyVehicleManager.Rigidbody;
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool isBodyCollider = 1 << collision.GetContact(0).thisCollider.gameObject.layer == _vehicleBodyLayer.value;
        if (isBodyCollider && _enemyVehicleManager.IsDead == false)
        {           
            bool collideWithRoad = false;
            if (_roadLayer == (_roadLayer | (1 << collision.GetContact(0).otherCollider.gameObject.layer)))
            {
                collideWithRoad = true;
            }

            bool collideWithotherBody = 1 << collision.GetContact(0).otherCollider.gameObject.layer == _vehicleBodyLayer.value;

            if (collideWithRoad || collideWithotherBody)
            {
                _enemyVehicleManager.OnBodyCollision();
                _RB.AddForceAtPosition(GameConfig.Instance.TouchRoadImpulse * Vector3.up, collision.GetContact(0).point, ForceMode.VelocityChange);
            }
        }
        else if(1 << collision.GetContact(0).thisCollider.gameObject.layer != _wheelsLayer.value)
        {
            bool collideWithotherBody = 1 << collision.GetContact(0).otherCollider.gameObject.layer == _vehicleBodyLayer.value;

            bool collideWithRoad = false;
            if (_roadLayer == (_roadLayer | (1 << collision.GetContact(0).otherCollider.gameObject.layer)))
            {
                collideWithRoad = true;
            }

            if (collideWithRoad || collideWithotherBody)
            {
                collision.GetContact(0).thisCollider.gameObject.GetComponent<VehiclePartManager>().OnPartDestroyLogic();
            }
        }
        else if (_enemyVehicleManager.IsDead == true && 1 << collision.GetContact(0).thisCollider.gameObject.layer == _wheelsLayer.value)
        {
            bool collideWithotherBody = 1 << collision.GetContact(0).otherCollider.gameObject.layer == _vehicleBodyLayer.value;

            bool collideWithRoad = false;
            if (_roadLayer == (_roadLayer | (1 << collision.GetContact(0).otherCollider.gameObject.layer)))
            {
                collideWithRoad = true;
            }

            if (collideWithRoad || collideWithotherBody)
            {
                collision.GetContact(0).thisCollider.gameObject.GetComponent<VehiclePartManager>().OnPartDestroyLogic();
            }
        }


















        //if (_isCrashed) return;
        //bool isBodyCollider = 1 << collision.GetContact(0).thisCollider.gameObject.layer == _vehicleBodyLayer.value;
        //if (!isBodyCollider) return;

        //bool collideWithRoad = 1 << collision.GetContact(0).otherCollider.gameObject.layer == _roadLayer.value;
        //bool collideWithotherBody = 1 << collision.GetContact(0).otherCollider.gameObject.layer == _vehicleBodyLayer.value;

        //if (collideWithRoad || collideWithotherBody)
        //{
        //    _isCrashed = true;
        //    _enemyVehicleManager.OnBodyCollision();
        //    _RB.AddForceAtPosition(GameConfig.Instance.TouchRoadImpulse * Vector3.up, collision.GetContact(0).point, ForceMode.VelocityChange);
        //}
    }
}
