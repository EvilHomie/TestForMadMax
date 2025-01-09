using UnityEngine;

public class ProjectileFirePointEffects : AbstarctFirePointEffects
{
    AudioClip _shootSound;


    protected void Init(AudioSource audioSource, AnimationCurve animationCurve, AudioClip shootSound)
    {
        _audioSource = audioSource;
        _animationCurve = animationCurve;
        _shootSound = shootSound;

        _audioSource.clip = _shootSound;
    }


    public override void PlayEffects()
    {
        foreach (var particleSystem in _shootParticles)
        {
            particleSystem.Emit(1);
        }
        if(!_audioSource.isPlaying)
        {

        }

        //soundSource.PlayOneShot(shootSound);
    }

    public override void StopEffects()
    {
        throw new System.NotImplementedException();
    }
}
