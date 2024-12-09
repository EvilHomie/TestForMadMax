using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
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
            _audioMixer.SetFloat("SFX", -1f);
            image.sprite = _SFXEnabledIcon;
        }
        else
        {
            _audioMixer.SetFloat("SFX", -80f);
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
