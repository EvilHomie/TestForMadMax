using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicleManager : MonoBehaviour
{
    VehicleVisualController _vehicleVisualController;
    AudioSource _audioSource;

    [SerializeField] GameObject _garage;
    [SerializeField] ParticleSystem[] _wheelsDustPS;

    float _lastMoveSpeed = 0;

    float _audioPitch = 0.9f;

    private void Awake()
    {
        _vehicleVisualController = GetComponent<VehicleVisualController>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _vehicleVisualController.RotateWheels();
        if (_lastMoveSpeed != RaidManager.Instance.PlayerMoveSpeed)
        {
            _vehicleVisualController.UpdateVisualEffect();

            if (RaidManager.Instance.PlayerMoveSpeed <= 5f) return;
            _audioSource.pitch = _audioPitch + ((RaidManager.Instance.PlayerMoveSpeed - 5f) / 50f);
            _lastMoveSpeed = RaidManager.Instance.PlayerMoveSpeed;
        }
    }


    private void Start()
    {
        StartCoroutine(StartMovement());
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
