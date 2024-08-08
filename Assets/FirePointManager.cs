using UnityEngine;

public class FirePointManager : MonoBehaviour
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] ParticleSystem[] _particleSystems;
    public void OneShoot(AudioClip shootSound)
    {
        soundSource.PlayOneShot(shootSound);
        foreach (var particleSystem in _particleSystems) 
        {
            particleSystem.Emit(1);
        }
    }
}
