using UnityEngine;

public interface IDamageable
{
    public void OnHit(int hitValue, AudioClip hitSound);
}
