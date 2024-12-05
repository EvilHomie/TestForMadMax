using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class FirePointManager : MonoBehaviour
{
    AudioSource soundSource;
    [SerializeField] ParticleSystem[] _particleSystems;
    [SerializeField] AnimationCurve _animationCurve;
    [SerializeField] Transform _animationT;

    Vector3 _barrelDefPos;
    Coroutine _rotateCoroutine;
    Coroutine _slowDownRotateCoroutine;
    float _rotationSpeed;
    float _rotationMod = 90;
    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _barrelDefPos = transform.parent.localPosition;
    }
    public void SimpleShootEffect(AudioClip shootSound, float fireRate, bool withAnimation = false)
    {
        ShootEffects(shootSound);
        if (withAnimation)
        {
            StartCoroutine(BarrelShootAnimation(fireRate));
        }
    }

    public void RotateShootEffect(AudioClip shootSound, float fireRate, bool withAnimation = false)
    {

        //foreach (var particleSystem in _particleSystems)
        //{
        //    particleSystem.Emit(1);
        //}
        //soundSource.PlayOneShot(shootSound);
        if (_slowDownRotateCoroutine != null)
        {
            StopCoroutine(_slowDownRotateCoroutine);
        }


        if (withAnimation && _rotateCoroutine == null)
        {
            _rotateCoroutine = StartCoroutine(BarrelRotateAnimation(shootSound, fireRate));
        }
    }

    IEnumerator BarrelShootAnimation(float fireRate)
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * fireRate;

            float zOffset = _animationCurve.Evaluate(t);
            Vector3 pos = transform.parent.localPosition;
            pos.z = _barrelDefPos.z + zOffset;
            transform.parent.localPosition = pos;
            yield return null;
        }
    }

    IEnumerator BarrelRotateAnimation(AudioClip shootSound, float fireRate)
    {
        float rotateSpeedMod = 0;

        while (rotateSpeedMod < 1)
        {
            rotateSpeedMod += Time.deltaTime;
            _rotationSpeed = fireRate * _rotationMod * _animationCurve.Evaluate(rotateSpeedMod) * Time.deltaTime;            
            _animationT.Rotate(Vector3.forward, -_rotationSpeed);
            yield return null;
        }
        ShootEffects(shootSound);
        float nextTimeTofire = Time.time + 1f / fireRate;

        while (true)
        {
            _animationT.Rotate(Vector3.forward, -_animationCurve.Evaluate(rotateSpeedMod) * _rotationMod * fireRate * Time.deltaTime);
            if (Time.time > nextTimeTofire)
            {
                ShootEffects(shootSound);
                nextTimeTofire = Time.time + 1f / fireRate;
            }
            yield return null;
        }
    }

    //IEnumerator SpeedUpRotation()
    //{

    //}

    IEnumerator SlowDownRotation()
    {
        float t = 0;
        float incomeRotateSpeed = _rotationSpeed;

        while (t < 1)
        {
            t += Time.deltaTime;

            _animationT.Rotate(Vector3.forward, -Mathf.Lerp(incomeRotateSpeed, 0 , t));

            //_rotationSpeed *= _animationCurve.Evaluate(1 - t) * Time.deltaTime;

            //Debug.LogWarning(_rotationSpeed);
            //_animationT.Rotate(Vector3.forward, -_rotationSpeed);
            yield return null;
        }
    }


    void ShootEffects(AudioClip shootSound)
    {
        foreach (var particleSystem in _particleSystems)
        {
            particleSystem.Emit(1);
        }
        soundSource.PlayOneShot(shootSound);
    }

    public void StopShooting()
    {
        if (_rotateCoroutine != null)
        {
            StopCoroutine(_rotateCoroutine);
            _rotateCoroutine = null;
        }

        _slowDownRotateCoroutine = StartCoroutine(SlowDownRotation());
    }
}
