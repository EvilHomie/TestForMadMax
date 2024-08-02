using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour, IDamageable
{
    [SerializeField] float _HP = 100;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] Renderer _renderer;
    [SerializeField] float _hitVisualDuration = 0.1f;
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] VehiclePart _vehiclePart;
    [SerializeField] AudioClip _blowAudioClip;



    Coroutine _coroutine;

    VehicleMovementController _vehicleMovementController;






    private void Start()
    {
        _vehicleMovementController = GetComponent<VehicleMovementController>();
        _renderer.material.DisableKeyword("_EMISSION");
    }

    public void OnHit(int hitValue, AudioClip hitSound)
    {
        _HP -= hitValue;
        _audioSource.PlayOneShot(hitSound);
        _coroutine ??= StartCoroutine(OnHitVisualEffect());

        if (_HP <= 0)
        {
            if (_vehiclePart != VehiclePart.Body)
            {
                Destroy(gameObject);
            }
            else
            {
                _vehicleMovementController.OnDead();
                _particleSystem.Play();
                _audioSource.PlayOneShot(_blowAudioClip);
            }                      
        }
    }


    public void OnBodyCollision()
    {
        _vehicleMovementController.OnDead();
        _particleSystem.Play();
        _audioSource.PlayOneShot(_blowAudioClip);
    }

    IEnumerator OnHitVisualEffect()
    {
        _renderer.material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(_hitVisualDuration);
        _renderer.material.DisableKeyword("_EMISSION");
        _coroutine = null;
    }

   
}

enum VehiclePart
{
    Other,
    Body,
    Wheel

}