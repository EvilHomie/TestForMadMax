using UnityEngine;

public abstract class AbstarctFirePointEffects : MonoBehaviour
{    
    [SerializeField] protected ParticleSystem[] _shootParticles;

    protected AnimationCurve _animationCurve;
    protected AudioSource _audioSource;

    public abstract void PlayEffects();

    public abstract void StopEffects();
}
