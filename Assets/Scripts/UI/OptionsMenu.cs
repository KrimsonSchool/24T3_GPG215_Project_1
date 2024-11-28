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

    [SerializeField] private Slider fpsSlider;
    [SerializeField] private TextMeshProUGUI fpsNumber;

    private void Awake()
    {
        SetupVolumeOptions();
        SetupDeadZoneOptions();
        SetupFPSOptions();
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
            deadZoneSlider.value = PlayerCombatControls.defaultDeadZone;
        }
        deadZoneNumber.text = $"{(deadZoneSlider.value * 100).ToString("F1")}%";
    }

    private void SetupFPSOptions()
    {
        if (PlayerPrefs.HasKey("FrameRate"))
        {
            Application.targetFrameRate = (int)PlayerPrefs.GetFloat("FrameRate");
            switch (PlayerPrefs.GetFloat("FrameRate"))
            {
                case 15:
                    fpsSlider.value = 0;
                    fpsNumber.text = "15.0";
                    break;
                case 30:
                    fpsSlider.value = 1;
                    fpsNumber.text = "30.0";
                    break;
                case 60:
                    fpsSlider.value = 2;
                    fpsNumber.text = "60.0";
                    break;
                case 75:
                    fpsSlider.value = 3;
                    fpsNumber.text = "75.0";
                    break;
                case 120:
                    fpsSlider.value = 4;
                    fpsNumber.text = "120.0";
                    break;
            }
        }
        else
        {
            Application.targetFrameRate = 60;
            PlayerPrefs.SetFloat("FrameRate", 60);
            fpsSlider.value = 2;
            fpsNumber.text = "60.0";
        }
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
        deadZoneNumber.text = $"{(value * 100).ToString("F1")}%";
    }

    public void ChangeFrameRate(float sliderStep)
    {
        switch (sliderStep)
        {
            case 0:
                Application.targetFrameRate = 15;
                PlayerPrefs.SetFloat("FrameRate", 15);
                fpsNumber.text = "15.0";
                break;
            case 1:
                Application.targetFrameRate = 30;
                PlayerPrefs.SetFloat("FrameRate", 30);
                fpsNumber.text = "30.0";
                break;
            case 2:
                Application.targetFrameRate = 60;
                PlayerPrefs.SetFloat("FrameRate", 60);
                fpsNumber.text = "60.0";
                break;
            case 3:
                Application.targetFrameRate = 75;
                PlayerPrefs.SetFloat("FrameRate", 75);
                fpsNumber.text = "75.0";
                break;
            case 4:
                Application.targetFrameRate = 120;
                PlayerPrefs.SetFloat("FrameRate", 120);
                fpsNumber.text = "120.0";
                break;
        }
    }

    public void CloseOptions()
    {
        SceneManager.UnloadSceneAsync("Options");
    }
}