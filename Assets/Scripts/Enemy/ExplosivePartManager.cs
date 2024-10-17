using UnityEngine;
using UnityEngine.Animations;

public class ExplosivePartManager : VehiclePartManager
{
    [SerializeField] ParticleSystem _blowParticleSystem;
    [SerializeField] AudioSource _blowAudioSource;
    [SerializeField] AudioClip _blowAudioClip;
    [SerializeField] RotationConstraint _smokeRotationConstraint;
    ConstraintSource constraintSource;

    protected override void Start()
    {
        base.Start();
        constraintSource.sourceTransform = InRaidManager.Instance.transform;
        constraintSource.weight = 1.0f;
        _smokeRotationConstraint.AddSource(constraintSource);
    }

    public override void OnPartDestroyLogic()
    {
        _blowParticleSystem.Play();
        _blowAudioSource.PlayOneShot(_blowAudioClip);

        _enemyVehicleManager.OnExplosivePartLooseHP();
        base.OnPartDestroyLogic();
    }
}
