using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Default Settings")]
    [SerializeField]
    private bool mute;

    [SerializeField]
    private float masterVolume = 0.5f,
                    musicVolume = 1,
                    soundVolume = 1,
                    voiceVolume = 1;

    [SerializeField]
    private Toggle  muteToggle;

    [SerializeField]
    private Slider  masterVolumeSlider,
                    musicVolumeSlider,
                    soundVolumeSlider,
                    voiceVolumeSlider;

    [SerializeField]
    private AudioSource[]   musicSources,
                            soundSources,
                            voiceSources;

    private void Awake()
    {
        LoadPlayerPrefs();
        InitializeUI();
        InitializeComponents();
    }

    private void LoadPlayerPrefs()
    {
        mute            = PlayerPrefs.GetInt    ("Mute",    mute ? 1 : 0) == 1;
        masterVolume    = PlayerPrefs.GetFloat  ("Master",  masterVolume);
        musicVolume     = PlayerPrefs.GetFloat  ("Music",   musicVolume);
        soundVolume     = PlayerPrefs.GetFloat  ("Sound",   soundVolume);
        voiceVolume     = PlayerPrefs.GetFloat  ("Voice",   voiceVolume);
    }

    private void InitializeUI()
    {
        muteToggle.isOn             = mute;
        masterVolumeSlider.value    = masterVolume;
        musicVolumeSlider.value     = musicVolume;
        soundVolumeSlider.value     = soundVolume;
        voiceVolumeSlider.value     = voiceVolume;
    }

    private void InitializeComponents()
    {
        AdjustAudio(musicSources, mute);
        AdjustAudio(soundSources, mute);
        AdjustAudio(voiceSources, mute);

        AdjustAudio(musicSources, masterVolume * musicVolume);
        AdjustAudio(soundSources, masterVolume * soundVolume);
        AdjustAudio(voiceSources, masterVolume * voiceVolume);
    }

    private void AdjustAudio(AudioSource[] audioSources, bool mute)
    {
        foreach (AudioSource source in audioSources)
            source.mute = mute;
    }

    private void AdjustAudio(AudioSource[] audioSources, float volume)
    {
        foreach (AudioSource source in audioSources)
            source.volume = volume;
    }

    public void SetMute(bool value)
    {
        mute = value;

        AdjustAudio(musicSources, mute);
        AdjustAudio(soundSources, mute);
        AdjustAudio(voiceSources, mute);

        PlayerPrefs.SetInt("Mute", mute ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = value;

        AdjustAudio(musicSources, masterVolume * musicVolume);
        AdjustAudio(soundSources, masterVolume * soundVolume);
        AdjustAudio(voiceSources, masterVolume * voiceVolume);

        PlayerPrefs.SetFloat("Master", masterVolume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;

        AdjustAudio(musicSources, masterVolume * musicVolume);

        PlayerPrefs.SetFloat("Music", musicVolume);
        PlayerPrefs.Save();
    }

    public void SetSoundVolume(float value)
    {
        soundVolume = value;

        AdjustAudio(soundSources, masterVolume * soundVolume);

        PlayerPrefs.SetFloat("Sound", soundVolume);
        PlayerPrefs.Save();
    }

    public void SetVoiceVolume(float value)
    {
        voiceVolume = value;

        AdjustAudio(voiceSources, masterVolume * voiceVolume);

        PlayerPrefs.SetFloat("Voice", voiceVolume);
        PlayerPrefs.Save();
    }
}
