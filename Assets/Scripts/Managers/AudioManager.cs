using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Used to work with audio that persists through scenes. Sound effect logic should be handled on object that emits them.
/// </summary>
public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] private AudioMixer audioMixer;

    private AudioSource musicSource;
    private AudioSource ambienceSource;

    private Coroutine adjustingMusic;
    private Coroutine adjustingAmbience;

    public AudioClip CurrentMusicClip { get { return musicSource.clip; } }
    public AudioClip CurrentAmbienceClip { get { return ambienceSource.clip; } }

    #region Initialization
    protected override void Awake()
    {
        FindReferences();
        TransferAudioSourceData();
        base.Awake();
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

    private void TransferAudioSourceData()
    {
        if (Instance != null)
        {
            if (CurrentMusicClip == null)
            {
                Instance.StopMusic();
            }
            else if (Instance.CurrentMusicClip == null || Instance.CurrentMusicClip.name != CurrentMusicClip.name)
            {
                Instance.PlayMusic(CurrentMusicClip);
            }

            if (CurrentAmbienceClip == null)
            {
                Instance.StopAmbience();
            }
            else if (Instance.CurrentAmbienceClip == null || Instance.CurrentAmbienceClip.name != CurrentAmbienceClip.name)
            {
                Instance.PlayAmbience(CurrentAmbienceClip);
            }
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
    #endregion

    #region Public Methods
    #region Music
    public void PlayMusic(float fadeInTime = 0)
    {
        if (fadeInTime <= 0)
        {
            musicSource.Play();
        }
        else
        {
            CheckAndStopCoroutine(adjustingMusic);
            adjustingMusic = StartCoroutine(StartAudioSource(musicSource, fadeInTime));
        }
    }

    public void PlayMusic(AudioClip clip, float fadeInTime = 0, float fadeOutTime = 0)
    {
        if (fadeInTime <= 0 && fadeOutTime <= 0)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
        else
        {
            CheckAndStopCoroutine(adjustingMusic);
            adjustingMusic = StartCoroutine(SwapAudioClip(musicSource, clip, fadeInTime, fadeOutTime));
        }
    }

    public void StopMusic(float fadeOutTime = 0)
    {
        if (fadeOutTime <= 0)
        {
            musicSource.Stop();
        }
        CheckAndStopCoroutine(adjustingMusic);
        adjustingMusic = StartCoroutine(StopAudioSource(musicSource, fadeOutTime));
    }

    public void ChangeMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    #endregion

    #region Ambience
    public void PlayAmbience(float fadeInTime = 0)
    {
        if (fadeInTime <= 0)
        {
            ambienceSource.Play();
        }
        else
        {
            CheckAndStopCoroutine(adjustingAmbience);
            adjustingAmbience = StartCoroutine(StartAudioSource(ambienceSource, fadeInTime));
        }
    }

    public void PlayAmbience(AudioClip clip, float fadeInTime = 0, float fadeOutTime = 0)
    {
        if (fadeInTime <= 0 && fadeOutTime <= 0)
        {
            ambienceSource.clip = clip;
            ambienceSource.Play();
        }
        else
        {
            CheckAndStopCoroutine(adjustingAmbience);
            adjustingAmbience = StartCoroutine(SwapAudioClip(ambienceSource, clip, fadeInTime, fadeOutTime));
        }
    }

    public void StopAmbience(float fadeOutTime = 0)
    {
        if (fadeOutTime <= 0)
        {
            ambienceSource.Stop();
        }
        CheckAndStopCoroutine(adjustingAmbience);
        adjustingAmbience = StartCoroutine(StopAudioSource(ambienceSource, fadeOutTime));
    }

    public void ChangeAmbienceVolume(float volume)
    {
        ambienceSource.volume = volume;
    }
    #endregion
    #endregion

    #region Private Methods
    private void CheckAndStopCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator StartAudioSource(AudioSource source, float fadeInTime)
    {
        source.Play();
        if (fadeInTime > 0)
        {
            while (source.volume < 1)
            {
                source.volume += Time.deltaTime / fadeInTime;
                yield return null;
            }
        }
    }

    private IEnumerator StopAudioSource(AudioSource source, float fadeOutTime)
    {
        if (fadeOutTime > 0 && source.isPlaying)
        {
            while (source.volume > 0)
            {
                source.volume -= Time.deltaTime / fadeOutTime;
                yield return null;
            }
        }
    }

    private IEnumerator SwapAudioClip(AudioSource source, AudioClip clip, float fadeOutTime, float fadeInTime)
    {
        yield return StartCoroutine(StopAudioSource(source, fadeOutTime));
        source.clip = clip;
        yield return StartCoroutine(StartAudioSource(source, fadeInTime));
    }
    #endregion
}
