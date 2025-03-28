using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using YG;

public class MainAudioManager : MonoBehaviour
{
    public static MainAudioManager Instance;
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] Button _toggleSFXButton;
    [SerializeField] Image _toggleSFXImage;
    [SerializeField] Sprite _SFXEnabledIcon;
    [SerializeField] Sprite _SFXDisabledIcon;
    [SerializeField] Transform _SFXHotKeyImage;

    [SerializeField] Button _toggleMusicButton;
    [SerializeField] Image _toggleMusicImage;
    [SerializeField] Sprite _MusicEnabledIcon;
    [SerializeField] Sprite _MusicDisabledIcon;
    [SerializeField] Transform _MusicHotKeyImage;

    bool _SFXIsOn;
    bool _MusicIsOn;

    float _sfxMax = -1f;
    float _sfxMin = -80f;

    bool _isDesktop;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    public void Init()
    {
        _SFXIsOn = true;
        _MusicIsOn = true;
        _SFXHotKeyImage.gameObject.SetActive(YandexGame.EnvironmentData.isDesktop);
        _MusicHotKeyImage.gameObject.SetActive(YandexGame.EnvironmentData.isDesktop);
        _isDesktop = YandexGame.EnvironmentData.isDesktop;

        _toggleSFXButton.onClick.AddListener(SwitchSFXVolume);
        _toggleMusicButton.onClick.AddListener(SwitchMusicVolume);
    }
    private void Update()
    {
        if(!_isDesktop) return;
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SwitchSFXVolume();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            SwitchMusicVolume();
        }
    }

    public void SwitchSFXVolume()
    {
        _SFXIsOn = !_SFXIsOn;
        if (_SFXIsOn)
        {
            _audioMixer.SetFloat("SFX", _sfxMax);
            _toggleSFXImage.sprite = _SFXEnabledIcon;
        }
        else
        {
            _audioMixer.SetFloat("SFX", _sfxMin);
            _toggleSFXImage.sprite = _SFXDisabledIcon;
        }
    }
    public void SwitchMusicVolume()
    {
        _MusicIsOn = !_MusicIsOn;
        if (_MusicIsOn)
        {
            _audioMixer.SetFloat("Music", -7f);
            _toggleMusicImage.sprite = _MusicEnabledIcon;
        }
        else
        {
            _audioMixer.SetFloat("Music", -80f);
            _toggleMusicImage.sprite = _MusicDisabledIcon;
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
            if (_SFXIsOn)
            {
                _audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20f);
            }
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
            if (_SFXIsOn)
            {
                _audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20f);
            }
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
