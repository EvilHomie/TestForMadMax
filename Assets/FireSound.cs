using UnityEngine;

public class FireSound : MonoBehaviour
{
    [SerializeField] AudioClip _shootSound;
    [SerializeField] AudioSource _soundSource;

    [SerializeField] float _fireRate;

    bool _isShooting;

    float _nextTimeTofire = 0;


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
    }

    public void StopShoot()
    {
        _isShooting = false;
    }

}
