using UnityEngine;

public class PlayerWeaponPointManager : MonoBehaviour
{
    public static PlayerWeaponPointManager Instance;

    [SerializeField] Transform[] _weaponPoints;

    IWeapon[] _weapons;

    [SerializeField] int _selectedWeaponIndex = 0;


    //[SerializeField] AudioClip _shootSound;
    //[SerializeField] AudioClip _hitSound;
    //[SerializeField] AudioSource _soundSource;
    ////[SerializeField] AudioSource _soundSource2;
    //[SerializeField] ParticleSystem _bulletsPS;
    //[SerializeField] ParticleSystem[] _shootsPS;
    //[SerializeField] float _fireRate;

    //[SerializeField] Transform _weaponTransform;
    //[SerializeField] float _rotationSpeed = 5.0f;

    float targetPosY = 0;
    float targetPosX = 0;

    bool _isShooting;

    float _nextTimeTofire = 0;

    ParticleSystem.EmissionModule _emissionModule;

    //[SerializeField] float _shakeOnShootDuration = 0.2f;
    //[SerializeField] float _shakeOnShootIntensity = 0.1f;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        _weapons = new IWeapon[_weaponPoints.Length];

        for (int i = 0; i < _weaponPoints.Length; i++)
        {
            if (_weaponPoints[i].childCount != 0)
            {
                _weapons[i] = _weaponPoints[i].GetComponentInChildren<IWeapon>();
            }
        }

        //_emissionModule = _bulletsPS.emission;
        //StopShoot();
    }

    private void LateUpdate()
    {
        //_emissionModule.rateOverTime = _fireRate;

        //if (_isShooting && Time.time >= _nextTimeTofire)
        //{
        //    _nextTimeTofire = Time.time + 1f / _fireRate;
        //    _bulletsPS.Emit(1);
        //    foreach (var ps in _shootsPS) ps.Emit(1);
        //    _soundSource.PlayOneShot(_shootSound);
        //    //_soundSource2.PlayOneShot(_shootSound);

        //    ShakeCamera.Instance.Shake(1f / _fireRate, 0.2f);

        //    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo))
        //    {
        //        hitInfo.collider.GetComponent<IDamageable>()?.OnHit(10, _hitSound);
        //    }
        //}

        RotateWeaponAndCamera(TouchController.Instance.GetJoystickPosition);
        RotateCameraByWASD();
    }

    public void StartShoot()
    {
        _weapons[_selectedWeaponIndex].StartShooting();
    }

    public void StopShoot()
    {
        _weapons[_selectedWeaponIndex].StopShooting();
    }

    void RotateWeaponAndCamera(Vector2 movementVector)
    {
        targetPosY += movementVector.x * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;
        targetPosX += movementVector.y * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;

        float targetPosYClamped = Mathf.Clamp(targetPosY, -GameLogicParameters.Instance.MaxYRotateAngle, GameLogicParameters.Instance.MaxYRotateAngle);
        float targetPosXClamped = Mathf.Clamp(targetPosX, -GameLogicParameters.Instance.MaxXRotateAngle, GameLogicParameters.Instance.MaxXRotateAngle);

        _weaponPoints[_selectedWeaponIndex].rotation = Quaternion.Euler(-targetPosXClamped, targetPosYClamped, 0);
        Camera.main.transform.rotation = _weaponPoints[_selectedWeaponIndex].rotation;
    }

    void RotateCameraByWASD()
    {
        targetPosY += Input.GetAxis("Horizontal") * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;
        targetPosX += Input.GetAxis("Vertical") * Time.deltaTime * _weapons[_selectedWeaponIndex].RotationSpeed;

        if (targetPosY < -GameLogicParameters.Instance.MaxYRotateAngle) targetPosY = -GameLogicParameters.Instance.MaxYRotateAngle;
        if (targetPosY > GameLogicParameters.Instance.MaxYRotateAngle) targetPosY = GameLogicParameters.Instance.MaxYRotateAngle;

        if (targetPosX < -GameLogicParameters.Instance.MaxXRotateAngle) targetPosX = -GameLogicParameters.Instance.MaxXRotateAngle;
        if (targetPosX > GameLogicParameters.Instance.MaxXRotateAngle) targetPosX = GameLogicParameters.Instance.MaxXRotateAngle;

        //float targetPosYClamped = Mathf.Clamp(targetPosY, -45, 45); // Clamp вызывает ступор при достижении краёв.
        //float targetPosXClamped = Mathf.Clamp(targetPosX, -30, 0); // Clamp вызывает ступор при достижении краёв.

        _weaponPoints[_selectedWeaponIndex].rotation = Quaternion.Euler(-targetPosX, targetPosY, 0);
        Camera.main.transform.rotation = _weaponPoints[_selectedWeaponIndex].rotation;
    }
}
