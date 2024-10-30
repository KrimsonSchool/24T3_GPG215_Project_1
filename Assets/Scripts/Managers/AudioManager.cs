using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to work with audio that persists through scenes. Sound effect logic should be handled on object that emits them.
/// </summary>
public class AudioManager : PersistentSingleton<AudioManager>
{
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
}
