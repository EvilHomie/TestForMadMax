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
    private void Start()
    {
        _soundIsOn = true;
        _toggleSoundButton.image.sprite = _soundsFullIcon;
        _toggleSoundButton.onClick.AddListener(SwitchSoundVolume);
    }

    void SwitchSoundVolume()
    {
        if (_soundIsOn)
        {
            DisableMasterSound();
            _soundIsOn = false;
        }
        else
        {
            EnableMasterSound();
            _soundIsOn = true;
        }
    }

    public void DisableMasterSound()
    {
        _toggleSoundButton.image.sprite = _soundsOffIcon;
        _audioMixer.SetFloat("MasterSound", -80f);
    }

    public void EnableMasterSound()
    {
        _toggleSoundButton.image.sprite = _soundsFullIcon;
        _audioMixer.SetFloat("MasterSound", 0f);
    }

    public void LerpDisableSound()
    {

    }

    public void LerpEnableSound()
    {

    }

}
