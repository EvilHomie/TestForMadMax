using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyVehicleMovementController : MonoBehaviour
{
    List<Transform> _frontWheels;
    NavMeshObstacle _navMeshObstacle;

    EnemyVehicleManager _enemyVehicleManager;
    NavMeshAgent _navMeshAgent;
    PosInEnemyGameZone _reservedPosInEnemyGameZone;
    Rigidbody _rigidbody;


    bool _gameZoneIsReached = false;
    bool _isDead = false;

    float _currentOffset;

    float _wheelRotationSpeed = 120;
    float bodyRotationSpeed = 15;
    Vector2 _wheelsRotationMaxAngles = new(75, 105);

    float _lastBodyZPos = 0;
    float _rotationTreshhold = 0.5f;
    bool _isInit = false;
    float _runSpeedMod = 3;
    bool _tryRunAway = false;

    public void Init(EnemyVehicleManager enemyVehicleManager, Rigidbody rigidbody, List<Transform> frontWheels, NavMeshObstacle navMeshObstacle)
    {
        _navMeshObstacle = navMeshObstacle;
        _frontWheels = frontWheels;
        _rigidbody = rigidbody;
        _enemyVehicleManager = enemyVehicleManager;
        _reservedPosInEnemyGameZone = EnemyGameZone.Instance.GetPosInGameZone();
        _reservedPosInEnemyGameZone.IsReserved = true;

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshObstacle.enabled = false;
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.enabled = true;

        _navMeshAgent.SetDestination(_reservedPosInEnemyGameZone.transform.position);
        _isInit = true;
    }

    public void CustomUpdate()
    {
        if (!_isInit) return;
        if(_tryRunAway) return;
        if (!_isDead)
        {
            RotationLogic();
            _rigidbody.transform.position = new(transform.position.x, _rigidbody.transform.position.y, transform.position.z);
            _lastBodyZPos = transform.position.z;

            if (!_gameZoneIsReached && Vector3.Distance(transform.position, _reservedPosInEnemyGameZone.transform.position) <= 500)
            {
                _gameZoneIsReached = true;
                _navMeshAgent.speed = _enemyVehicleManager.EnemyCharacteristics.VehicleSlideSpeed;
                _enemyVehicleManager.OnReachGameZone();
                InvokeRepeating(nameof(ChangeDestination), 0, GameConfig.Instance.ChangeDestinationDelay);
            }
        }
    }
    private void FixedUpdate()
    {
        if (_isDead)
        {
            OnDieTranslation();
        }
    }

    void RotationLogic()
    {
        if (math.abs(transform.position.z - _lastBodyZPos) > _rotationTreshhold)
        {
            if (transform.position.z - _lastBodyZPos > 0)
            {
                foreach (var wheel in _frontWheels)
                {
                    if (wheel == null) continue;
                    RotateTransform(wheel, Direction.Left, _wheelRotationSpeed);
                }
                RotateTransform(_rigidbody.transform, Direction.Left, bodyRotationSpeed);
            }
            else
            {
                foreach (var wheel in _frontWheels)
                {
                    if (wheel == null) continue;
                    RotateTransform(wheel, Direction.Right, _wheelRotationSpeed);
                }
                RotateTransform(_rigidbody.transform, Direction.Right, bodyRotationSpeed);
            }
        }
        else
        {
            foreach (var wheel in _frontWheels)
            {
                if (wheel == null) continue;
                RotateTransform(wheel, Direction.Front, _wheelRotationSpeed);
            }
            RotateTransform(_rigidbody.transform, Direction.Front, bodyRotationSpeed);
        }
    }

    void RotateTransform(Transform transform, Direction direction, float rotationSpeed)
    {
        if (direction == Direction.Front)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z), rotationSpeed * Time.deltaTime);
        }
        else if (direction == Direction.Left)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, _wheelsRotationMaxAngles.x, transform.rotation.z), rotationSpeed * Time.deltaTime);
        }
        else if (direction == Direction.Right)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, _wheelsRotationMaxAngles.y, transform.rotation.z), rotationSpeed * Time.deltaTime);
        }
    }

    void ChangeDestination()
    {
        PosInEnemyGameZone newPos = EnemyGameZone.Instance.GetPosInGameZone();
        newPos.IsReserved = true;

        _reservedPosInEnemyGameZone.IsReserved = false;
        _reservedPosInEnemyGameZone = newPos;
        _navMeshAgent.SetDestination(_reservedPosInEnemyGameZone.transform.position);
    }

    public void OnDie()
    {
        CancelInvoke();
        StopAllCoroutines();
        _navMeshAgent.enabled = false;
        _navMeshObstacle.enabled = true;
        _reservedPosInEnemyGameZone.IsReserved = false;
        _isDead = true;
    }

    public void OnTryRunMovementLogic()
    {
        StartCoroutine(RunAwayTranslation(true));
    }

    void OnDieTranslation()
    {
        _rigidbody.maxLinearVelocity = GameConfig.Instance.SpeedMod * 4;
        _rigidbody.AddForce(GameConfig.Instance.SpeedMod * 2 * Vector3.left, ForceMode.Acceleration);

        if (_rigidbody.transform.position.x < -GameConfig.Instance.XOffsetForDestroyObject)
        {
            InRaidManager.Instance.OnEnemyObjectDestroyed(_enemyVehicleManager);
            _enemyVehicleManager.OnObjectDestroy();
            Destroy(gameObject);
        }
    }

    IEnumerator RunAwayTranslation(bool withDelay)
    {
        if (withDelay)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(GameConfig.Instance.MinDelayForRun, GameConfig.Instance.MaxDelayForRun));
        }
        _tryRunAway = true;
        CancelInvoke();
        _reservedPosInEnemyGameZone.IsReserved = false;
        Vector3 escapePos = EnemyEscapeZone.Instance.GetRandomEscapePos();
        _navMeshAgent.SetDestination(escapePos);

        //float runSpeedMod = escapePos.x < transform.position.x ? _runSpeedMod.x : _runSpeedMod.y;

        _navMeshAgent.speed = _enemyVehicleManager.EnemyCharacteristics.VehicleSlideSpeed * _runSpeedMod;
        yield return null;
        while (Vector3.Distance(transform.position, escapePos) >= 500)
        {
            _rigidbody.transform.position = new(transform.position.x, _rigidbody.transform.position.y, transform.position.z);
            yield return null;
        }
        InRaidManager.Instance.OnEnemyEscaped(_enemyVehicleManager);
        _enemyVehicleManager.OnObjectDestroy();
        Destroy(gameObject);
    }

    public void OnPlayerDie()
    {
        StartCoroutine(RunAwayTranslation(false));
    }
}
