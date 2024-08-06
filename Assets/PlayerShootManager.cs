using UnityEngine;

public class PlayerShootManager : MonoBehaviour
{
    [SerializeField] AudioClip _shootSound;
    [SerializeField] AudioClip _hitSound;
    [SerializeField] AudioSource _soundSource;
    [SerializeField] ParticleSystem _bulletsPS;
    [SerializeField] ParticleSystem[] _shootsPS;

    [SerializeField] float _fireRate;

    bool _isShooting;

    float _nextTimeTofire = 0;

    ParticleSystem.EmissionModule _emissionModule;



    [SerializeField] float _shakeDuration = 0.2f;
    [SerializeField] float _shakeIntensity = 0.1f;

    //Vector3 _initialPos;
    //float _currentChakeDuration = 0.0f;


    private void Awake()
    {
        _emissionModule = _bulletsPS.emission;
        StopShoot();
    }

    //private void Start()
    //{
    //    _initialPos = Camera.main.transform.localPosition;
    //}

    private void Update()
    {
        _emissionModule.rateOverTime = _fireRate;


        if (_isShooting && Time.time >= _nextTimeTofire)
        {
            _nextTimeTofire = Time.time + 1f / _fireRate;
            _bulletsPS.Emit(1);
            foreach (var ps in _shootsPS) ps.Emit(1);
            _soundSource.PlayOneShot(_shootSound);

            ShakeCamera.Instance.Shake(1f / _fireRate, 0.2f);

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo))
            {
                //Debug.Log(hitInfo.collider.name);
                hitInfo.collider.GetComponent<IDamageable>()?.OnHit(10, _hitSound);
            }
        }

        //if (_currentChakeDuration > 0)
        //{
        //    Vector3 randomOffset = Random.insideUnitSphere * _shakeIntensity;
        //    Camera.main.transform.localPosition = _initialPos + randomOffset;
        //    _currentChakeDuration -= Time.deltaTime;
        //}
        //else
        //{
        //    Camera.main.transform.localPosition = _initialPos;
        //}


        //Debug.DrawRay(transform.position, transform.forward * 1000, Color.green) ;


        
    }

    public void StartShoot()
    {
        _isShooting = true;
        //_shootsPS.Play();

        //_particleSystem.Emit(1);
        //_soundSource.PlayOneShot(_shootSound);
        //_particleSystem.Play();
        //_emissionModule.enabled = true;
    }

    public void StopShoot()
    {
        _isShooting = false;
        //_bulletsPS.Stop();
        //_shootsPS.Stop();
    }

    //void ShakeWeapon()
    //{
    //    _currentChakeDuration = _shakeDuration;
    //}

}
