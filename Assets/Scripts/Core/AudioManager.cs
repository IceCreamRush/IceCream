
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Slider volumSlider;
    private void Awake()
    {
        Instance = this;
    }

    public AudioSource bgmSource;
    public AudioSource effectSource;

    public void Init()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (volumSlider)
        {
            SetBGMVolume(volumSlider.value);
            SetEffectVolume(volumSlider.value);
        }
    }

    public void PlayBGM(string name, bool isLoop = true)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/BGM/" + name);
        bgmSource.clip = clip;
        bgmSource.loop = isLoop;
        bgmSource.Play();
    }

    public void PlayEffect(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/Effects/" + name);
        effectSource.PlayOneShot(clip);
    }
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }
    public void SetEffectVolume(float volume)
    {
        effectSource.volume = volume;
    }
    
    public void BGMStart()
    {
        if(!bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
        else
            bgmSource.UnPause();
    }

    public void BGMPause()
    {
        bgmSource.Pause();
    }

    public void BGMStop()
    {
        bgmSource.Stop();
    }
}
