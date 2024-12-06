using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class FirePointManager : MonoBehaviour
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] ParticleSystem[] _particleSystems;
    [SerializeField] AnimationCurve _animationCurve;
    [SerializeField] Transform _animationT;

    Vector3 _barrelDefPos;
    Coroutine _rotateCoroutine;
    Coroutine _slowDownRotateCoroutine;
    float _rotationSpeed;
    float _rotationMod = 180;

    bool _isShooting;

    // 0.15 до 1    20   0,9  1,2   10  40

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
        _rotateCoroutine ??= StartCoroutine(MiniGunShootLogic(shootSound, fireRate));
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

    IEnumerator MiniGunShootLogic(AudioClip shootSound, float fireRate)
    {
        float t = 0;
        float lastShootTime = 0;
        bool warmup = false;


        float frMod = Mathf.InverseLerp(10, 50, fireRate);
        float pitchmod = Mathf.Lerp(0.9f, 1.2f, frMod);
        soundSource.pitch = pitchmod;

        while (true)
        {

            if (_isShooting)
            {
                soundSource.loop = true;
                if (!soundSource.isPlaying)
                {
                    soundSource.time = 0;
                    soundSource.Play();
                }
                else if (warmup == false)
                {
                    float time = shootSound.length - soundSource.time;
                    soundSource.time = time;
                }
                warmup = true;
                t += Time.deltaTime;
                if (t >= 0.7f) t = 0.7f;
            }
            else
            {
                soundSource.loop = false;
                if (warmup)
                {
                    float time = shootSound.length - 0.8f;
                    soundSource.Stop();
                    Debug.LogWarning(time);
                    soundSource.time = time;
                    soundSource.Play();
                }

                warmup = false;

                t -= Time.deltaTime;
                if (t <= 0) t = 0;
            }
            float tMod = Mathf.InverseLerp(0, 0.8f, t);
            _rotationSpeed = fireRate * _rotationMod * _animationCurve.Evaluate(tMod) * Time.deltaTime;
            _animationT.Rotate(Vector3.forward, -_rotationSpeed);

            if (t <= 0)
            {
                _rotateCoroutine = null;
                soundSource.Stop();
                yield break;
            }
            else if (t >= 0.7f)
            {
                if (Time.time > lastShootTime)
                {
                    ShootEffects(shootSound, false);
                    lastShootTime = Time.time + 1 / fireRate;
                }
            }

            yield return null;
        }
    }

    void ShootEffects(AudioClip shootSound, bool withSingleSound = true)
    {
        foreach (var particleSystem in _particleSystems)
        {
            particleSystem.Emit(1);
        }
        if (withSingleSound)
        {
            soundSource.PlayOneShot(shootSound);
        }

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
        //soundSource.Stop();
    }
}
