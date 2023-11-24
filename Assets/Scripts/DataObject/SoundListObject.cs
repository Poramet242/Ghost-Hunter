using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundListObject : MonoBehaviour
{
    public static SoundListObject instance;
    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    [Header("Sound SFX")]
    [SerializeField] public List<AudioClip> all_SFX;
    [Header("Sound BGM")]
    [SerializeField] public List<AudioClip> all_BGM;
    public void onPlaySoundSFX(int sfx_index)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySoundSFX(all_SFX[sfx_index]);
        }
    }
}
