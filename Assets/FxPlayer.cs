using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FxPlayer : MonoBehaviour
{

    AudioSource audS;
    public AudioClip[] allClips;
    // Start is called before the first frame update
    void Start()
    {
        audS = GetComponent<AudioSource>();
        
        allClips = Resources.LoadAll<AudioClip>("SFX");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string name)
    {
        int index = -1;
        if (allClips.Length > 0)
        {
            for (int i = 0; i < allClips.Length; i++)
            {
                if (allClips[i].name == name)
                {
                    index = i; break;
                }
                else
                {
                    print("name was [" + allClips[i].name + "]");
                }
            }
            if (index > -1)
            {
                audS.clip = allClips[index];
                audS.Play();
            }
            else
            {
                Debug.LogError("No audio clip with name [" + name + "] found.");
            }
        }
    }
}
