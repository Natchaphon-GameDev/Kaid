using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public enum Sound
{
    Laser,
    Missile,
    MachineGun,
    BGM,
    ButtonClicked,
    OpenUI,
    Evo,
    Upgrade,
    GameOver,
    StartGame,
    FireWork,
    Cookie,
    Candy,
    Sell,
    ChooseCard,
    TimeBeforeOver,
    Notification,
    EndWave
}

public class SoundClip
{
    //Init of every SoundClip
    public Sound Sound;
    public AudioClip AudioClip;
    [Range(0, 2)] public float SoundVolume;
    public bool Loop = false;
    [HideInInspector] public AudioSource AudioSource;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    [SerializeField] private List<SoundClip> soundClips;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private SoundClip GetSoundClip(Sound sound)
    {
        //find sound in list
        foreach (var soundClip in soundClips)
        {
            if (soundClip.Sound == sound)
            {
                return soundClip;
            }
        }
        return null;
    }

    private void Start()
    {
        UiManager.Instance.OnButtonClicked += ButtonClickedSound;
    }

    public void ButtonClickedSound()
    {
        Play(Sound.ButtonClicked);
    }
    
    public void Play(Sound sound)
    {
        //Play Sound system
        var soundClip = GetSoundClip(sound);
        if (soundClip.AudioSource == null)
        {
            soundClip.AudioSource = gameObject.AddComponent<AudioSource>();
        }
        soundClip.AudioSource.clip = soundClip.AudioClip;
        soundClip.AudioSource.volume = soundClip.SoundVolume;
        soundClip.AudioSource.loop = soundClip.Loop;
        soundClip.AudioSource.Play();
    }
    
    public void Stop(Sound sound)
    {
        //Stop Sound system
        var soundClip = GetSoundClip(sound);
        if (soundClip.AudioSource == null)
        {
            soundClip.AudioSource = gameObject.AddComponent<AudioSource>();
        }
        soundClip.AudioSource.volume = soundClip.SoundVolume;
        soundClip.AudioSource.loop = soundClip.Loop;
        soundClip.AudioSource.Stop();
    }
    
    public void Pause(Sound sound,bool toggle)
    {
        //Stop and resume Sound system
        var soundClip = GetSoundClip(sound);
        soundClip.AudioSource.clip = soundClip.AudioClip;
        soundClip.AudioSource.volume = soundClip.SoundVolume;
        soundClip.AudioSource.loop = soundClip.Loop;
        if (toggle)
        {
            soundClip.AudioSource.Pause();
        }
        else
        {
            soundClip.AudioSource.UnPause();
        }
    }
}


