using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainAudioManager : MonoBehaviour
{
    public static MainAudioManager Instance;
    [SerializeField] AudioMixer _audioMixer;

    //[SerializeField] Button _toggleSFXButton;
    //[SerializeField] Button _toggleMusicButton;

    //[SerializeField] Image _toggleSFXImage;
    //[SerializeField] Image _toggleMusicImage;

    [SerializeField] Sprite _SFXEnabledIcon;
    [SerializeField] Sprite _SFXDisabledIcon;

    [SerializeField] Sprite _MusicEnabledIcon;
    [SerializeField] Sprite _MusicDisabledIcon;

    bool _SFXIsOn;
    bool _MusicIsOn;


    float _sfxMax = -1f;
    float _sfxMin = -80f;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    public void Init()
    {
        _SFXIsOn = true;
        _MusicIsOn = true;
        //_toggleSFXImage.sprite = _SFXEnabledIcon;
        //_toggleMusicImage.sprite = _MusicEnabledIcon;
        //_toggleSFXButton.onClick.AddListener(SwitchSFXVolume);
        //_toggleMusicButton.onClick.AddListener(SwitchMusicVolume);
        //GameFlowManager.GameFlowChangeStateOnPause += ConfigAudioOnChangeFlowState;
    }

    //void ConfigAudioOnChangeFlowState(bool onPause)
    //{
    //    if (onPause) DisableSFX();
    //    else EnableSFX();
    //}

    public void SwitchSFXVolume(Image image)
    {
        _SFXIsOn = !_SFXIsOn;
        if (_SFXIsOn)
        {
            _audioMixer.SetFloat("SFX", _sfxMax);
            image.sprite = _SFXEnabledIcon;
        }
        else
        {
            _audioMixer.SetFloat("SFX", _sfxMin);
            image.sprite = _SFXDisabledIcon;
        }
    }
    public void SwitchMusicVolume(Image image)
    {
        _MusicIsOn = !_MusicIsOn;
        if (_MusicIsOn)
        {
            _audioMixer.SetFloat("Music", -7f);
            image.sprite = _MusicEnabledIcon;
        }
        else
        {
            _audioMixer.SetFloat("Music", -80f);
            image.sprite = _MusicDisabledIcon;
        }
    }

    public void DisableMasterSound()
    {
        //_toggleSFXButton.image.sprite = _SFXDisabledIcon;
        _audioMixer.SetFloat("MasterSound", -80f);
    }

    public void EnableMasterSound()
    {
        //if (!_SFXIsOn) return;
        //_toggleSFXButton.image.sprite = _SFXEnabledIcon;
        _audioMixer.SetFloat("MasterSound", -15f);
    }


    public void FadeOutSFX(float duration)
    {
        StartCoroutine(FadeOutSFXCoroutine(duration));
    }
    IEnumerator FadeOutSFXCoroutine(float duration)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            float value = Mathf.Lerp(1, 0.0001f, t);

            _audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20f);
            yield return null;
        }
        _audioMixer.SetFloat("SFX", _sfxMin);
    }

    public void FadeInSFX(float duration)
    {
        StartCoroutine(FadeOutInCoroutine(duration));
    }
    IEnumerator FadeOutInCoroutine(float duration)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            float value = Mathf.Lerp(0.0001f, 1, t);

            _audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20f);
            yield return null;
        }
        _audioMixer.SetFloat("SFX", _sfxMax);
    }


    void DisableSFX()
    {

    }
    public void EnableSFX()
    {

    }

    void DisableMusic()
    {

    }
    public void EnableMusic()
    {

    }

}
