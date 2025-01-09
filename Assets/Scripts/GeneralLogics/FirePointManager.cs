using System.Collections;
using UnityEngine;

public class FirePointManager : MonoBehaviour
{
    AudioSource soundSource;
    [SerializeField] ParticleSystem[] _particleSystems;
    [SerializeField] AnimationCurve _animationCurve;

    Vector3 _barrelDefPos;

    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _barrelDefPos = transform.parent.localPosition;
    }
    public void OneShoot(AudioClip shootSound, float fireRate, bool withAnimation = false)
    {
        foreach (var particleSystem in _particleSystems)
        {
            particleSystem.Emit(1);
        }
        soundSource.PlayOneShot(shootSound);
        if (withAnimation)
        {
            StartCoroutine(BarrelShootAnimation(fireRate));
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
}
