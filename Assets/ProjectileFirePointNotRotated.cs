using System.Collections;
using UnityEngine;

public class ProjectileFirePointNotRotated : AbstarctFirePoint
{
    Vector3 _barrelDefPos;

    private void Start()
    {
        _barrelDefPos = transform.parent.localPosition;
    }

    public override void PlayEffects(float fireRate)
    {
        foreach (var particleSystem in _shootParticles)
        {
            particleSystem.Emit(1);
        }

        _audioSource.PlayOneShot(_shootSound);

        //if(!_audioSource.isPlaying)
        //{
        //    _audioSource.Play();
        //}

        StartCoroutine(BarrelShootAnimation(fireRate));
    }

    public override void StopEffects()
    {
        throw new System.NotImplementedException();
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
