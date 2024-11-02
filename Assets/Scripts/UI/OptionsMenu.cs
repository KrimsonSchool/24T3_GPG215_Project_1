using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private TextMeshProUGUI masterNumber;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicNumber;

    [SerializeField] private Slider sFXSlider;
    [SerializeField] private TextMeshProUGUI sFXNumber;

    [SerializeField] private Slider ambienceSlider;
    [SerializeField] private TextMeshProUGUI ambienceNumber;

    [SerializeField] private Slider deadZoneSlider;
    [SerializeField] private TextMeshProUGUI deadZoneNumber;

    private void Awake()
    {
        SetupVolumeOptions();
        SetupDeadZoneOptions();
    }

    private void SetupVolumeOptions()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float volume = PlayerPrefs.GetFloat("MasterVolume");
            audioMixer.SetFloat("MasterVolume", Mathf.Log(volume) * 20);
            masterNumber.text = $"{(int)(volume * 100)}%";
            masterSlider.value = volume;
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float volume = PlayerPrefs.GetFloat("MusicVolume");
            audioMixer.SetFloat("MusicVolume", Mathf.Log(volume) * 20);
            musicNumber.text = $"{(int)(volume * 100)}%";
            musicSlider.value = volume;
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float volume = PlayerPrefs.GetFloat("SFXVolume");
            audioMixer.SetFloat("SFXVolume", Mathf.Log(volume) * 20);
            sFXNumber.text = $"{(int)(volume * 100)}%";
            sFXSlider.value = volume;
        }
        if (PlayerPrefs.HasKey("AmbienceVolume"))
        {
            float volume = PlayerPrefs.GetFloat("AmbienceVolume");
            audioMixer.SetFloat("AmbienceVolume", Mathf.Log(volume) * 20);
            ambienceNumber.text = $"{(int)(volume * 100)}%";
            ambienceSlider.value = volume;
        }
    }

    private void SetupDeadZoneOptions()
    {
        if (PlayerCombatControls.Instance != null)
        {
            deadZoneSlider.value = PlayerCombatControls.Instance.DeadZone;
        }
        else if (PlayerPrefs.HasKey("DeadZone"))
        {
            deadZoneSlider.value = PlayerPrefs.GetFloat("DeadZone");
        }
        else
        {
            deadZoneSlider.value = 0.01f;
        }
        deadZoneNumber.text = $"{(float)(int)(deadZoneSlider.value * 1000) / 10}%";
    }

    public void ChangeMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("MasterVolume", volume);
        audioMixer.SetFloat("MasterVolume", Mathf.Log(volume) * 20);
        masterNumber.text = $"{(int)(volume * 100)}%";
    }

    public void ChangeMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        audioMixer.SetFloat("MusicVolume", Mathf.Log(volume) * 20);
        musicNumber.text = $"{(int)(volume * 100)}%";
    }

    public void ChangeSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        audioMixer.SetFloat("SFXVolume", Mathf.Log(volume) * 20);
        sFXNumber.text = $"{(int)(volume * 100)}%";
    }

    public void ChangeAmbienceVolume(float volume)
    {
        PlayerPrefs.SetFloat("AmbienceVolume", volume);
        audioMixer.SetFloat("AmbienceVolume", Mathf.Log(volume) * 20);
        ambienceNumber.text = $"{(int)(volume * 100)}%";
    }

    public void ChangeDeadzone(float value)
    {
        if (PlayerCombatControls.Instance != null)
        {
            PlayerCombatControls.Instance.DeadZone = value;
        }
        PlayerPrefs.SetFloat("DeadZone", value);
        deadZoneNumber.text = $"{(float)(int)(value * 1000) / 10}%";
    }

    public void CloseOptions()
    {
        SceneManager.UnloadSceneAsync("Options");
    }
}