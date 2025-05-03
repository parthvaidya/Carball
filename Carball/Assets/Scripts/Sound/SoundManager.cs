using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    public SoundType[] Sounds;
    public AudioSource soundEffect;
    public AudioSource soundMusic;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }



    }
    private void Start()
    {
        PlayMusic(global::Sounds.Music);
    }

    public void PlayMusic(Sounds sound)
    {
        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            soundMusic.clip = clip;
            soundMusic.Play();
        }

        else { Debug.Log("Clip not found"); }
    }

    public void Play(Sounds sound)
    {
        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            soundEffect.PlayOneShot(clip);
        }

        else { Debug.Log("Clip not found"); }
    }

    private AudioClip getSoundClip(Sounds sound)
    {
        SoundType item = Array.Find(Sounds, i => i.soundType == sound);
        if (item != null)
        {
            return item.soundClip;
        }
        return null;
    }

    [Serializable]
    public class SoundType
    {
        public Sounds soundType;
        public AudioClip soundClip;
    }
}

public enum Sounds
{
    ButtonClick,
    CarSound,
    BallKick,
    Goal,
    Music,
    Shoot,
    AsteroidBreak,
    collectItem
}