using UnityEngine;
using UnityEngine.Animations;

public class EnemyDieExplosion : MonoBehaviour
{
    ParticleSystem _blowParticleSystem;
    AudioSource _blowAudioSource;
    [SerializeField] AudioClip _blowAudioClip;
    [SerializeField] RotationConstraint _smokeRotationConstraint;

    ConstraintSource constraintSource;

    private void Awake()
    {
        EnemyVehicleManager enemyVehicleManager = transform.root.GetComponent<EnemyVehicleManager>();
        enemyVehicleManager.EnemyDieExplosion = this;
    }

    public void Init()
    {
        _blowParticleSystem = GetComponent<ParticleSystem>();
        _blowAudioSource = GetComponent<AudioSource>();
        constraintSource.sourceTransform = InRaidManager.Instance.transform;
        constraintSource.weight = 1.0f;
        _smokeRotationConstraint.AddSource(constraintSource);
    }

    public void OnDie()
    {
        _blowParticleSystem.Play();
        _blowAudioSource.PlayOneShot(_blowAudioClip);
    }
}
