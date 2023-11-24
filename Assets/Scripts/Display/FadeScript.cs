using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    public static FadeScript instance;
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
        DontDestroyOnLoad(gameObject);
    }
    [SerializeField] private CanvasGroup FadeCamvas;
    [SerializeField] public bool fadeIn = false;
    [SerializeField] public bool fadeOut = false;
    private void Update()
    {
        if (fadeIn)
        {
            if (FadeCamvas.alpha < 1)
            {
                FadeCamvas.alpha += Time.deltaTime;
                if (FadeCamvas.alpha >=1)
                {
                    fadeIn = false;
                }
            }
        }
        if (fadeOut)
        {
            if (FadeCamvas.alpha >=0)
            {
                FadeCamvas.alpha -= Time.deltaTime;
                if (FadeCamvas.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }
    public IEnumerator setFadeIN(Action callback)
    {
        while (fadeIn)
        {
            if (FadeCamvas.alpha < 1)
            {
                FadeCamvas.alpha += Time.deltaTime;
                if (FadeCamvas.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
            yield return null;
        }
        callback?.Invoke();
    }
    public IEnumerator setFadeOut(Action callback)
    {
        while (fadeOut)
        {
            if (FadeCamvas.alpha >= 0)
            {
                FadeCamvas.alpha -= Time.deltaTime;
                if (FadeCamvas.alpha == 0)
                {
                    fadeOut = false;
                }
            }
            yield return null;
        }
        callback?.Invoke();
    }
}
