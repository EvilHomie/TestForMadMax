using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HitManager : MonoBehaviour, IDamageable
{
    [SerializeField] float _HP = 100;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] Renderer _renderer;
    [SerializeField] float _hitVisualDuration = 0.1f;
    //[SerializeField] AudioClip _audioClip;
    [SerializeField] VehiclePart _vehiclePart;
    Coroutine _coroutine;
    Rigidbody _bodyRB;

    Collider Collider;
    [SerializeField] LayerMask _layerMask;

    VehicleMovementController vehicleMovementController;
    private void Start()
    {
        vehicleMovementController = GetComponent<VehicleMovementController>();
        _renderer.material.DisableKeyword("_EMISSION");
        if (_vehiclePart == VehiclePart.Body) _bodyRB = GetComponent<Rigidbody>();
    }

    public void OnHit(int hitValue, AudioClip hitSound)
    {
        _HP -= hitValue;
        _audioSource.PlayOneShot(hitSound);
        _coroutine ??= StartCoroutine(HitVisualEffect());

        if (_HP <= 0)
        {
            Destroy(gameObject);
        }


    }



    IEnumerator HitVisualEffect()
    {
        _renderer.material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(_hitVisualDuration);
        _renderer.material.DisableKeyword("_EMISSION");
        _coroutine = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (_vehiclePart == VehiclePart.Wheel) return;
        //Debug.LogWarning("WADWA");

        if (1 << collision.GetContact(0).thisCollider.gameObject.layer == _layerMask.value)
        {
            vehicleMovementController.IsDead();
            _bodyRB.AddForceAtPosition(50 * RaidManager.Instance.PlayerMoveSpeed * Vector3.up, collision.GetContact(0).point, ForceMode.VelocityChange);
            StartCoroutine(MoveIfDead());
        }
    }

    IEnumerator MoveIfDead()
    {
        while (transform.position.x > -10000f)
        {
            transform.Translate(185 * RaidManager.Instance.PlayerMoveSpeed * Time.deltaTime * -Vector3.right, Space.World);
            yield return null;
        }
        Destroy(gameObject);
    }
}

enum VehiclePart
{
    Other,
    Body,
    Wheel

}