using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHangar : MonoBehaviour, IHitable
{
    [SerializeField] AudioSource _audioSource;

    public void OnHit(Vector3 hitPos, AudioClip hitSound)
    {
        _audioSource.transform.position = hitPos;
        if (hitSound != null) _audioSource.PlayOneShot(hitSound);
    }
}
