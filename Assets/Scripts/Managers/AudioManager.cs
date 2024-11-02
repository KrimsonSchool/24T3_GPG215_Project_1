using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to work with audio that persists through scenes. Sound effect logic should be handled on object that emits them.
/// </summary>
public class AudioManager : PersistentSingleton<AudioManager>
{
    [Header("Audio Mixers")]
    [SerializeField] private AudioMixer audioMixer;
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambienceSource;

    protected override void Awake()
    {
        base.Awake(); // Run singleton code
        FindReferences();
    }

    private void FindReferences()
    {
        if (musicSource == null)
        {
            musicSource = transform.Find("MusicPlayer").GetComponent<AudioSource>();
        }
        if (ambienceSource == null)
        {
            ambienceSource = transform.Find("AmbiencePlayer").GetComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += AmbienceHandler;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AmbienceHandler;
    }

    private void AmbienceHandler(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Start")
        {
            ambienceSource.Stop();
        }
        else if (!ambienceSource.isPlaying && (scene.name == "DefaultRoom" || scene.name == "BossRoom"))
        {
            ambienceSource.Play();
        }
    }

    private void Start()
    {
        SetupVolumeLevels();
    }

    private void SetupVolumeLevels()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float volume = PlayerPrefs.GetFloat("MasterVolume");
            audioMixer.SetFloat("MasterVolume", Mathf.Log(volume) * 20);
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float volume = PlayerPrefs.GetFloat("MusicVolume");
            audioMixer.SetFloat("MusicVolume", Mathf.Log(volume) * 20);
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float volume = PlayerPrefs.GetFloat("SFXVolume");
            audioMixer.SetFloat("SFXVolume", Mathf.Log(volume) * 20);
        }
        if (PlayerPrefs.HasKey("AmbienceVolume"))
        {
            float volume = PlayerPrefs.GetFloat("AmbienceVolume");
            audioMixer.SetFloat("AmbienceVolume", Mathf.Log(volume) * 20);
        }
    }
}
