using UnityEngine;

public abstract class AbstarctFirePoint : MonoBehaviour
{    
    [SerializeField] protected ParticleSystem[] _shootParticles;

    protected AnimationCurve _animationCurve;
    protected AudioSource _audioSource;
    protected AudioClip _shootSound;

    public void Init(AudioSource audioSource, AnimationCurve animationCurve, AudioClip shootSound)
    {
        _audioSource = audioSource;
        _animationCurve = animationCurve;
        //_audioSource.clip = shootSound;
        _shootSound = shootSound;
    }

    public abstract void PlayEffects(float fireRate = 0);

    public abstract void StopEffects();
}
