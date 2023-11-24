using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Components")]
    [SerializeField] public SoundInstance soundInstancePrefabs;
    [SerializeField] public Transform soundParent;
    [Header("Setting")]
    [SerializeField] public int MaxSoundInstance;
    [SerializeField] private SoundInstance BGMSoundInstance;
    [Range(0f,1f)]
    [SerializeField] public float volumeMultipler = 1f;

    public void PlaySoundSFX(AudioClip sound, bool isLoop = false)
    {
        if (soundParent.childCount >= MaxSoundInstance) return;
        SoundInstance _instanceSFX = Instantiate(soundInstancePrefabs, soundParent);
        _instanceSFX.InitInsance(sound, DataHolder.SFX_Volume * volumeMultipler, isLoop);
    }
    public void PlaySoundBGM(AudioClip sound)
    {
        if (soundParent.childCount >= MaxSoundInstance) return;
        if (BGMSoundInstance != null)
        {
            GetBGMInstance().Stop();
            Destroy(BGMSoundInstance.gameObject);
        }
        SoundInstance _instanceBGM = Instantiate(soundInstancePrefabs, soundParent);
        _instanceBGM.InitInsance(sound, DataHolder.BGM_Volume * volumeMultipler, true);
        BGMSoundInstance = _instanceBGM;
    }
    public void StopBGM()
    {
        GetBGMInstance().Stop();
        Destroy(BGMSoundInstance.gameObject);
    }

    public AudioSource GetBGMInstance()
    {
        return BGMSoundInstance.audioSource;
    }

    public bool isPlayingBGM()
    {
        return BGMSoundInstance != null;
    }
}
