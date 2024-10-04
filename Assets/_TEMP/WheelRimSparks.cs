using UnityEngine;

public class WheelRimSparks : MonoBehaviour
{
    [SerializeField] ParticleSystem _wheelRimPS;
    [SerializeField] ParticleSystem _sparksPS;
    public ParticleSystem WheelRimPS => _wheelRimPS;


    public void EnableSparks()
    {
        _sparksPS = _wheelRimPS.GetComponentInChildren<ParticleSystem>();
        _sparksPS.collision.AddPlane(RaidManager.Instance.MainRoadTransform);
        _wheelRimPS.Play();
    }
}
