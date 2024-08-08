using UnityEngine;

public interface IDamageable
{
    public void OnHit(float hullDmgValue, float shieldDmgValue, AudioClip hitSound);
}
