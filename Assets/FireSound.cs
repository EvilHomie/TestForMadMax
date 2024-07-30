using UnityEngine;

public class FireSound : MonoBehaviour
{
    [SerializeField] AudioClip _shootSound;
    [SerializeField] AudioSource _soundSource;
    [SerializeField] ParticleSystem _particleSystem;

    [SerializeField] float _fireRate;

    bool _isShooting;

    float _nextTimeTofire = 0;


    private void Awake()
    {
        StopShoot();
    }
    private void Update()
    {
        if (_isShooting && Time.time >= _nextTimeTofire)
        {
            _nextTimeTofire = Time.time + 1f / _fireRate;
            _soundSource.PlayOneShot(_shootSound);
        }
    }

    public void StartShoot()
    {
        _isShooting = true;

        var emission = _particleSystem.emission;
        emission.enabled = true;
    }

    public void StopShoot()
    {
        _isShooting = false;

        var emission = _particleSystem.emission;
        emission.enabled = false;
    }

}
