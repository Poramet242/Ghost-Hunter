using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInstance : MonoBehaviour
{
    public AudioSource audioSource;
    public void InitInsance(AudioClip sound, float volume = 1.0f, bool isLoop = false)
    {
        audioSource.loop = isLoop;
        audioSource.volume = volume;
        audioSource.clip = sound;
        audioSource.Play();
        StartCoroutine(PlaySoundProcess());
    }
    IEnumerator PlaySoundProcess()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        if (audioSource.loop)
        {
            yield break;
        }
        Destroy(this.gameObject);
    }
}
