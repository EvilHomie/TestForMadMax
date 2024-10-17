using UnityEngine;

public class DetachLogic : MonoBehaviour
{
    [SerializeField] ReboundDirection _detachDirection = ReboundDirection.Y;
    [SerializeField] float _detachForceValue = 250;
    [SerializeField] float _detachTorqueValue = 5;
    [SerializeField] float _onAditionCollideForce = 50;
    Rigidbody rb;
    bool _isDetached = false;

    private void FixedUpdate()
    {
        if (_isDetached)
        {
            rb.AddForce(GameConfig.Instance.SpeedMod / 20 * Vector3.left, ForceMode.VelocityChange);
            if (transform.position.x < -GameConfig.Instance.XOffsetForDestroyObject)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Detach()
    {
        if (!_isDetached)
        {
            Destroy(gameObject, 10);
            _isDetached = true;
            gameObject.transform.parent = null;
            rb = gameObject.AddComponent<Rigidbody>();

            Vector3 dir = _detachDirection switch
            {
                ReboundDirection.X => transform.right,
                ReboundDirection.Y => transform.up,
                ReboundDirection.Z => transform.forward,
                ReboundDirection.XY => transform.right + Vector3.up,
                ReboundDirection.XZ => transform.right + Vector3.forward,
                ReboundDirection.YZ => transform.up + Vector3.forward,
                _ => Vector3.zero
            };

            Vector3 torqDir = Random.onUnitSphere;

            rb.AddForce(dir * _detachForceValue, ForceMode.VelocityChange);
            rb.AddTorque(torqDir * _detachTorqueValue, ForceMode.VelocityChange);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_isDetached)
        {
            rb.AddForce(Vector3.up * _onAditionCollideForce, ForceMode.VelocityChange);
            return;
        }
    }
}
