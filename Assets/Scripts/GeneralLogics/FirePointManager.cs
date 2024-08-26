using UnityEngine;

public class FirePointManager : MonoBehaviour
{
    AudioSource soundSource;
    [SerializeField] ParticleSystem[] _particleSystems;

    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
    }
    public void OneShoot(AudioClip shootSound)
    {
        foreach (var particleSystem in _particleSystems)
        {
            particleSystem.Emit(1);
        }
        soundSource.PlayOneShot(shootSound);
    }
}
