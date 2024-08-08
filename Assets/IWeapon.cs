using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void StartShooting();
    public void StopShooting();

    public float RotationSpeed { get;}
}
