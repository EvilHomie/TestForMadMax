using UnityEngine;

public class PlayerShootManager : MonoBehaviour
{
    [SerializeField] AudioClip _shootSound;
    [SerializeField] AudioClip _hitSound;
    [SerializeField] AudioSource _soundSource;
    [SerializeField] ParticleSystem _particleSystem;

    [SerializeField] float _fireRate;

    bool _isShooting;

    float _nextTimeTofire = 0;

    ParticleSystem.EmissionModule _emissionModule;


    private void Awake()
    {
        _emissionModule = _particleSystem.emission;
        StopShoot();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        _emissionModule.rateOverTime = _fireRate;


        if (_isShooting && Time.time >= _nextTimeTofire)
        {
            _nextTimeTofire = Time.time + 1f / _fireRate;
            _particleSystem.Emit(1);
            _soundSource.PlayOneShot(_shootSound);


            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo))
            {
                //Debug.Log(hitInfo.collider.name);
                hitInfo.collider.GetComponent<IDamageable>()?.OnHit(10, _hitSound);
            }
        }

            

        Debug.DrawRay(transform.position, transform.forward * 1000, Color.green) ;


        
    }

    public void StartShoot()
    {
        _isShooting = true;

        //_particleSystem.Emit(1);
        //_soundSource.PlayOneShot(_shootSound);
        //_particleSystem.Play();
        //_emissionModule.enabled = true;
    }

    public void StopShoot()
    {
        _isShooting = false;
        _particleSystem.Stop();
    }

    

}
