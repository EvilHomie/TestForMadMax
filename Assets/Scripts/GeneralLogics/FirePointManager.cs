using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
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

    bool _isShooting;
    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _barrelDefPos = transform.parent.localPosition;
    }
    public void SimpleShoot(AudioClip shootSound, float fireRate, bool withAnimation = false)
    {
        ShootEffects(shootSound);
        if (withAnimation)
        {
            StartCoroutine(BarrelShootAnimation(fireRate));
        }
    }

    public void RotateShoot(AudioClip shootSound, float fireRate, bool withAnimation = false)
    {
        _isShooting = true;
        _rotateCoroutine ??= StartCoroutine(ChangeRotationSpeed(shootSound, fireRate));
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

    IEnumerator ChangeRotationSpeed(AudioClip shootSound, float fireRate)
    {
        float t = 0;

        while (true)
        {
            if (_isShooting)
            {
                t += Time.deltaTime;
                if (t >= 1) t = 1;
            }
            else
            {
                t -= Time.deltaTime;
                if (t <= 0) t = 0;
            }
            _rotationSpeed = fireRate * _rotationMod * _animationCurve.Evaluate(t) * Time.deltaTime;
            _animationT.Rotate(Vector3.forward, -_rotationSpeed);
            if (t <= 0)
            {
                _rotateCoroutine = null;
                yield break;
            }
            else if (t >= 1)
            {
                ShootEffects(shootSound);
                yield return new WaitForSeconds(1 / fireRate);
            }

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
        //if (Physics.Raycast(_firePointManagers[0].transform.position, _firePointManagers[0].transform.forward, out RaycastHit hitInfo))
        //{
        //    hitInfo.collider.GetComponent<IDamageable>()?.OnHit(CurHullDmg, CurShieldDmg, _hitSound);
        //    Instantiate(hitFXEffect, hitInfo.point, hitFXEffect.transform.rotation);
        //    hitInfo.collider.GetComponent<IHitable>()?.OnHit(hitInfo.point, _hitSound);
        //}
    }

    public void StopShooting()
    {
        _isShooting = false;
    }
}
