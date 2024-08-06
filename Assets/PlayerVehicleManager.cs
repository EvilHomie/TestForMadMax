using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicleManager : MonoBehaviour
{
    VehicleVisualController _vehicleVisualController;
    RaidManager _raidManager;
    AudioSource _audioSource;

    [SerializeField] GameObject _garage;
    [SerializeField] ParticleSystem[] _wheelsDustPS;

    [SerializeField] float firstdelay;
    [SerializeField] float seconddelay;

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
        //_vehicleVisualController.StopVisualEffects();
        _audioSource.Play();
        ShakeCamera.Instance.Shake(0.8f, 0.5f);
        yield return new WaitForSeconds(2);
        StartCoroutine(MoveGarage());
        RaidManager.Instance.StartMove(5, 11);

        foreach (var ps in _wheelsDustPS)
        {
            ps.Play();
        }

        //yield return new WaitForSeconds(firstdelay);
        //Debug.LogWarning("1");
        //_wheelsDustPS[0].Play();
        //yield return new WaitForSeconds(seconddelay);
        //Debug.LogWarning("2");
        //_wheelsDustPS[1].Play();
    }

    IEnumerator MoveGarage()
    {
        while (_garage.transform.position.x > -10000)
        {
            _garage.transform.Translate(RaidManager.Instance.PlayerMoveSpeed * Vector3.left / 1.5f);
            yield return null;
        }
        _garage.SetActive(false);
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    _wheelsDustPS[0].Play();
    //}   
    //private void OnTriggerExit(Collider other)
    //{
    //    _wheelsDustPS[1].Play();
    //}


}
