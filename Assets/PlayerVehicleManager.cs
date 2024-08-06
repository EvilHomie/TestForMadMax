using System.Collections;
using UnityEngine;

public class PlayerVehicleManager : MonoBehaviour
{
    public static PlayerVehicleManager Instance;
    VehicleVisualController _vehicleVisualController;
    AudioSource _audioSource;

    [SerializeField] GameObject _garage;
    [SerializeField] ParticleSystem[] _wheelsDustPS;

    float _lastMoveSpeed = 0;
    float _audioPitch = 0.9f;

    bool _isStarted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        _vehicleVisualController = GetComponent<VehicleVisualController>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!_isStarted) return;
        _vehicleVisualController.RotateWheels();
        if (_lastMoveSpeed != RaidManager.Instance.PlayerMoveSpeed)
        {
            _vehicleVisualController.UpdateVisualEffect();

            if (RaidManager.Instance.PlayerMoveSpeed <= 5f) return;
            _audioSource.pitch = _audioPitch + ((RaidManager.Instance.PlayerMoveSpeed - 5f) / 50f);
            _lastMoveSpeed = RaidManager.Instance.PlayerMoveSpeed;
        }
    }


    public void StartVehicle()
    {
        StartCoroutine(StartMovement());
        _isStarted = true;
    }


    IEnumerator StartMovement()
    {
        _audioSource.Play();
        ShakeCamera.Instance.Shake(0.8f, 0.5f);
        yield return new WaitForSeconds(2);
        StartCoroutine(MoveGarage());
        RaidManager.Instance.StartMove(5, 11);

        foreach (var ps in _wheelsDustPS)
        {
            ps.Play();
        }
    }

    IEnumerator MoveGarage()
    {
        while (_garage.transform.position.x > -10000)
        {
            _garage.transform.position += Vector3.left * RaidManager.Instance.PlayerMoveSpeed * 170 * Time.deltaTime;
            yield return null;
        }
        _garage.SetActive(false);
    }
}
