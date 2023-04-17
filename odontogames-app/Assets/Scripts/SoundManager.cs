using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    AudioSource source;

    public AudioClip[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        source = GetComponent<AudioSource>();
    }

    public void PlaySound(int sound)
    {
        source.clip = sounds[sound];
        source.loop = false;
        source.Play();
    }

    public void PlayMusic()
    {
        source.clip = sounds[0];
        source.loop = true;
        source.Play();
    }
}
