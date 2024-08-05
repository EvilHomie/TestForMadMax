using UnityEngine;

public class WeaponParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem[] _particleSystems;
    public void Emit(int count)
    {
        foreach (var particleSystem in _particleSystems) 
        {
            particleSystem.Emit(count);
        }
    }
}
