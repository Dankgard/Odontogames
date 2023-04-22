using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip[] sounds;

    private Slider soundSlider;
    private AudioSource source;

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
        soundSlider = GameObject.FindWithTag("sound").GetComponent<Slider>();

    }

    public void SetSoundVolume()
    {
        source.volume = soundSlider.value;
    }

    public void PlaySound(int sound)
    {
        source.PlayOneShot(sounds[sound]);
    }

    public void PlayMusic()
    {
        source.clip = sounds[0];
        source.loop = true;
        source.Play();
    }
}
