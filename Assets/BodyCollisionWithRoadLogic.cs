using UnityEngine;

public class BodyCollisionWithRoadLogic : MonoBehaviour
{
    [SerializeField] LayerMask _collisionLayer;

    VehicleMovementController _vehicleMovementController;
    HealthManager _HealthManager;
    Rigidbody _RB;

    bool _isDead = false;

    private void Awake()
    {
        _RB = GetComponent<Rigidbody>();
        _vehicleMovementController = GetComponent<VehicleMovementController>();
        _HealthManager = GetComponent<HealthManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isDead) return;
        if (1 << collision.GetContact(0).thisCollider.gameObject.layer != _collisionLayer.value) return;
        _isDead = true;
        _vehicleMovementController.OnDead();
        _HealthManager.OnBodyCollision();
        _RB.AddForceAtPosition(GameLogicParameters.Instance.TouchRoadImpulseMod * RaidManager.Instance.PlayerMoveSpeed * Vector3.up, collision.GetContact(0).point, ForceMode.Impulse);
    }
}
