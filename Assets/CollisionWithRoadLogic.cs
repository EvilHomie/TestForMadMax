using UnityEngine;

public class CollisionWithRoadLogic : MonoBehaviour
{
    [SerializeField] LayerMask _vehicleBodyLayer;
    EnemyVehicleManager _enemyVehicleManager;
    Rigidbody _RB;
    bool _isDead = false;

    private void Awake()
    {
        _RB = GetComponent<Rigidbody>();
        _enemyVehicleManager = GetComponent<EnemyVehicleManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isDead) return;
        if (1 << collision.GetContact(0).thisCollider.gameObject.layer != _vehicleBodyLayer.value) return;
        _isDead = true;
        _enemyVehicleManager.OnBodyCollisionWithRoad();
        _RB.AddForceAtPosition(GameLogicParameters.Instance.TouchRoadImpulseMod * RaidObjectsManager.Instance.PlayerMoveSpeed * Vector3.up, collision.GetContact(0).point, ForceMode.Impulse);
    }
}
