using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] Button _toggleSoundButton;
    [SerializeField] Sprite _soundsFullIcon;
    [SerializeField] Sprite _soundsOffIcon;

    bool _soundIsOn;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    public void Init()
    {
        _soundIsOn = true;
        _toggleSoundButton.image.sprite = _soundsFullIcon;
        _toggleSoundButton.onClick.AddListener(SwitchSoundVolume);
        //GameFlowManager.GameFlowChangeStateOnPause += ConfigAudioOnChangeFlowState;
    }

    void ConfigAudioOnChangeFlowState(bool onPause)
    {
        if (onPause) DisableSFX();
        else EnableSFX();
    }

    void SwitchSoundVolume()
    {
        if (_soundIsOn)
        {
            _soundIsOn = false;
            DisableMasterSound();
        }
        else
        {
            _soundIsOn = true;
            EnableMasterSound();
        }
    }

    public void DisableMasterSound()
    {
        _toggleSoundButton.image.sprite = _soundsOffIcon;
        _audioMixer.SetFloat("MasterSound", -80f);
    }

    public void EnableMasterSound()
    {
        if (!_soundIsOn) return;
        _toggleSoundButton.image.sprite = _soundsFullIcon;
        _audioMixer.SetFloat("MasterSound", -15f);
    }

    void DisableSFX()
    {
        _audioMixer.SetFloat("SFX", -80f);
    }
    public void EnableSFX() 
    {
        _audioMixer.SetFloat("SFX", -1f);
    }

}
