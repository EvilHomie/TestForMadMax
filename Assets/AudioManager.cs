using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;


    [SerializeField] AudioSource _inventoryAS;
    [SerializeField] AudioClip _upgradeSound;
    [SerializeField] AudioClip _equipSound;

    [SerializeField] AudioSource _musicAS;
    [SerializeField] AudioSource _hitToPlayerAS;

    [SerializeField] AudioSource _playerEngineAS;
    [SerializeField] AudioClip _startEnginePart;
    [SerializeField] AudioClip _engineLoopPart;


    public AudioSource PlayerEngineAS => _playerEngineAS;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void PlayInventorySound(UISound sound)
    {
        if (sound == UISound.UpgradeSound) _inventoryAS.PlayOneShot(_upgradeSound);
        else if (sound == UISound.EquipSound) _inventoryAS.PlayOneShot(_equipSound);
    }

    public void PlayHitToPLayerSound(AudioClip hitsound)
    {
        _hitToPlayerAS.PlayOneShot(hitsound);
    }

    public void ToggleMusic(bool activeStatus)
    {
        if (activeStatus) _musicAS.Play();
        else _musicAS.Stop();
    }

    public void StartEngineSoundLogic(float fadeOutDelay, float fadeOutDuration, float blackoutPause, float fadeInDuration)
    {
        StartCoroutine(PlayerEngineSoundLogic(fadeOutDelay, fadeOutDuration, blackoutPause, fadeInDuration));
    }


    IEnumerator PlayerEngineSoundLogic(float fadeOutDelay, float fadeOutDuration, float blackoutPause, float fadeInDuration)
    {
        _playerEngineAS.clip = _startEnginePart;
        _playerEngineAS.Play();
        yield return new WaitForSeconds(fadeOutDelay);
        MainAudioManager.Instance.FadeOutSFX(fadeOutDuration);
        yield return new WaitForSeconds(fadeOutDuration);
        _playerEngineAS.Stop();
        _playerEngineAS.clip = _engineLoopPart;
        _playerEngineAS.Play();
        yield return new WaitForSeconds(blackoutPause);
        MainAudioManager.Instance.FadeInSFX(fadeInDuration);
        
    }

}

public enum UISound
{
    UpgradeSound,
    EquipSound
}