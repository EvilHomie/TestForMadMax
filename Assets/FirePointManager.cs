using UnityEngine;

public class FirePointManager : MonoBehaviour
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] ParticleSystem[] _particleSystems;
    public void OneShoot(AudioClip shootSound)
    {  
        foreach (var particleSystem in _particleSystems)
        {
            particleSystem.Emit(1);
        }

        //if (!soundSource.isPlaying)
            soundSource.PlayOneShot(shootSound);
    }
}
