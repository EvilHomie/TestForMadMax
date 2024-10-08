using UnityEngine;

public class CollisionWithRoadLogic : MonoBehaviour
{
    [SerializeField] LayerMask _vehicleBodyLayer;
    [SerializeField] LayerMask _roadLayer;
    EnemyVehicleManager _enemyVehicleManager;
    Rigidbody _RB;
    bool _isCrashed = false;

    private void Awake()
    {
        _RB = GetComponent<Rigidbody>();
        _enemyVehicleManager = GetComponent<EnemyVehicleManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCrashed) return;
        bool isBodyCollider = 1 << collision.GetContact(0).thisCollider.gameObject.layer == _vehicleBodyLayer.value;
        if (!isBodyCollider) return;

        bool collideWithRoad = 1 << collision.GetContact(0).otherCollider.gameObject.layer == _roadLayer.value;
        bool collideWithotherBody = 1 << collision.GetContact(0).otherCollider.gameObject.layer == _vehicleBodyLayer.value;

        if (collideWithRoad || collideWithotherBody)
        {
            _isCrashed = true;
            _enemyVehicleManager.OnBodyCollisionWithRoad();
            _RB.AddForceAtPosition(GameConfig.Instance.TouchRoadImpulse * RaidManager.Instance.PlayerMoveSpeed * Vector3.up, collision.GetContact(0).point, ForceMode.VelocityChange);
        }
    }
}
