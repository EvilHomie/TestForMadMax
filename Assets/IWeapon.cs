using UnityEngine;

public interface IWeapon
{
    public float RotationSpeed { get;}
    public Vector3 ObserverPos { get; }
    public GameObject TargetMarker { get; }
    public void StartShooting();
    public void StopShooting();
}
