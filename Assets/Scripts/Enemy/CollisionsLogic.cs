using UnityEngine;

public class CollisionsLogic : MonoBehaviour
{
    [SerializeField] LayerMask _vehicleBodyLayer;
    [SerializeField] LayerMask _roadLayer;
    [SerializeField] LayerMask _wheelsLayer;
    [SerializeField] LayerMask _deffaultLayer;

    [SerializeField] LayerMask _layersForPartDestroy;
    EnemyVehicleManager _enemyVehicleManager;
    Rigidbody _RB;

    private void Awake()
    {
        _enemyVehicleManager = transform.root.GetComponent<EnemyVehicleManager>();
        _RB = _enemyVehicleManager.Rigidbody;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (1 << collision.GetContact(0).thisCollider.gameObject.layer == _vehicleBodyLayer.value)
        {
            OnBodyCollision(collision);
        }
        if (1 << collision.GetContact(0).thisCollider.gameObject.layer == _wheelsLayer.value)
        {
            OnWheelCollision(collision);
        }
        if (1 << collision.GetContact(0).thisCollider.gameObject.layer == _deffaultLayer.value)
        {
            OnOtherPartCollision(collision);
        }
    }


    void OnBodyCollision(Collision collision)
    {
        if (_enemyVehicleManager.IsDead == false)
        {
            bool collideForDestroy = (_layersForPartDestroy & (1 << collision.GetContact(0).otherCollider.gameObject.layer)) != 0;
            if (collideForDestroy)
            {
                _enemyVehicleManager.OnBodyCollision();
                _RB.AddForceAtPosition(GameConfig.Instance.TouchRoadImpulse * Vector3.up, collision.GetContact(0).point, ForceMode.VelocityChange);
            }
        }
    }

    void OnWheelCollision(Collision collision)
    {
        if (_enemyVehicleManager.IsDead == false)
        {
            bool collideWithBody = 1 << collision.GetContact(0).otherCollider.gameObject.layer == _vehicleBodyLayer.value;
            if (collideWithBody)
            {
                collision.GetContact(0).thisCollider.gameObject.GetComponent<VehiclePartManager>().OnPartDestroyLogic();
            }
        }
        else
        {
            collision.GetContact(0).thisCollider.gameObject.GetComponent<VehiclePartManager>().OnPartDestroyLogic();
        }
    }

    void OnOtherPartCollision(Collision collision)
    {
        if (_enemyVehicleManager.IsDead == false)
        {
            bool collideForDestroy = (_layersForPartDestroy & (1 << collision.GetContact(0).otherCollider.gameObject.layer)) != 0;
            if (collideForDestroy)
            {
                collision.GetContact(0).thisCollider.gameObject.GetComponent<VehiclePartManager>().OnPartDestroyLogic();
            }
        }
        else
        {
            collision.GetContact(0).thisCollider.gameObject.GetComponent<VehiclePartManager>().OnPartDestroyLogic();
        }
    }    
}
