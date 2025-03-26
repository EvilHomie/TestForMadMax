using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HPEnemyMovementController : EnemyVehicleMovementController
{
    public override void Init(EnemyVehicleManager enemyVehicleManager, Rigidbody rigidbody, List<Transform> frontWheels, NavMeshObstacle navMeshObstacle)
    {
        _navMeshObstacle = navMeshObstacle;
        _frontWheels = frontWheels;
        _rigidbody = rigidbody;
        _enemyVehicleManager = enemyVehicleManager;

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshObstacle.enabled = false;
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.enabled = true;

        _navMeshAgent.SetDestination(SurviveModeEnemyHPSpawner.Instance.EndPos);
        _isInit = true;
    }

    public override void CustomUpdate()
    {
        if(_isDead) return;
        _rigidbody.transform.position = new(transform.position.x, _rigidbody.transform.position.y, transform.position.z);

        if (_navMeshAgent.remainingDistance < 50)
            Destroy(transform.gameObject);
    }

    public override void OnDie()
    {
        CancelInvoke();
        StopAllCoroutines();
        _navMeshAgent.enabled = false;
        _navMeshObstacle.enabled = true;
        _isDead = true;
        PlayerHPManager.Instance.OnHPEnemyDestroyed();
    }


}
