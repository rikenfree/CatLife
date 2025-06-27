using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SoundManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public AudioSource audioSource;

    public Slider musicSldr;
    public Slider soundSldr;
    public Slider hapticSldr;
    public Slider clipsoundSldr;

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        musicSldr.value = Music;
        soundSldr.value = Sound;
        hapticSldr.value = Haptic;
        clipsoundSldr.value = ClipSound;

        VideoController.instance.videoPlayer.SetDirectAudioMute(0, ClipSound == 0);
    }

    public int Music
    {
        get
        {
           return PlayerPrefs.GetInt("Music", 1);
        }
        set
        {
            PlayerPrefs.SetInt("Music", value);
        }
    }

    public int Sound
    {
        get
        {
            return PlayerPrefs.GetInt("Sound", 1);
        }
        set
        {
            PlayerPrefs.SetInt("Sound", value);
        }
    }

    public int ClipSound
    {
        get
        {
            return PlayerPrefs.GetInt("ClipSound", 1);
        }
        set
        {
            PlayerPrefs.SetInt("ClipSound", value);
        }
    }

    public int Haptic
    {
        get
        {
            return PlayerPrefs.GetInt("Haptic", 1);
        }
        set
        {
            PlayerPrefs.SetInt("Haptic", value);
        }
    }

    private void Start()
    {

    }

    void Update()
    {
        
    }

    public void HandleMusic()
    {
        //Music ^= 1;

        //Music = Mathf.Abs(Music - 1);

        Music = (Music == 0) ? 1 : 0;
    }

    public void HandleSound()
    {
        Sound = (Sound == 0) ? 1 : 0;
    }

    public void HandleHaptic()
    {
        Haptic = (Haptic == 0) ? 1 : 0;
    }

    public void HandleClipSound()
    {
        ClipSound = (ClipSound == 0) ? 1 : 0;

        VideoController.instance.videoPlayer.SetDirectAudioMute(0, ClipSound == 0);
    }

    public void PlayMusic()
    {
        if (Music == 1)
            videoPlayer.SetDirectAudioMute(0, false);
        else
            videoPlayer.SetDirectAudioMute(0, true);
    }
    public void PlayBtnSound()
    {
        if (Sound == 1)
            audioSource.Play();
    }

    public void PlayHaptic()
    {
        if(Haptic == 1)
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
    }
}
