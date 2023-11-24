using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XSystem;

public class UiLoginDisplay : MonoBehaviour
{
    public static UiLoginDisplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Data")]
    [SerializeField] private VersionController versionController;
    [Header("Login Display")]
    [SerializeField] public bool fadeLogin = false;
    [SerializeField] public GameObject login_obj;
    [SerializeField] public CanvasGroup icon_login_img;
    [SerializeField] public GameObject mainLogin_obj;
    [Header("Button Login")]
    [SerializeField] public GameObject start_btn;
    [SerializeField] public GameObject login_btn;
    [Header("Loading Display")]
    [SerializeField] public GameObject loading_obj;
    [SerializeField] public GameObject startLoading_obj;
    [SerializeField] public Text loading_text;
    [SerializeField] public Image loading_image;
    [SerializeField] public int currentLoading = 0;
    [Header("Start Game Dispaly")]
    [SerializeField] public GameObject startGame_obj;
    [Header("Warning Display")]
    [SerializeField] public WarningDisplay warning_obj;
    [SerializeField] public GameObject faceBack_panel;

    [Header("Setting Login")]
    [SerializeField] public GameObject _Flex_Login;
    [SerializeField] public GameObject _Guest_Login;
    private void Start()
    {
        StartCoroutine(versionController.checkVersionAPP(() => 
        {
            if (PlayerPrefs.GetFloat("SFX_Volume") == 0f && PlayerPrefs.GetFloat("BGM_Volume") == 0f)
            {
                PlayerPrefs.SetFloat("SFX_Volume", 1f);
                PlayerPrefs.SetFloat("BGM_Volume", 1f);
            }
            DataHolder.SFX_Volume = PlayerPrefs.GetFloat("SFX_Volume");
            DataHolder.BGM_Volume = PlayerPrefs.GetFloat("BGM_Volume");
            SoundManager.instance.PlaySoundBGM(SoundListObject.instance.all_BGM[0]);
            fadeLogin = true;
            icon_login_img.alpha = 0;
            StartCoroutine(setStartLading(() =>
            {
                login_obj.SetActive(false);
                startLoading_obj.SetActive(true);
                StartCoroutine(LoginController.instance.XLogin());
                //mainLogin_obj.SetActive(true);
            }));
        }));

    }
    public void setupLoginGuest(bool check)
    {
        if (check)
        {
            _Flex_Login.SetActive(false);
            _Guest_Login.SetActive(true);
        }
        else
        {
            _Guest_Login.SetActive(false);
            _Flex_Login.SetActive(true);
        }
    }
    public void Update()
    {
        loading_text.text = currentLoading.ToString() + "%";
    }
    IEnumerator setStartLading(Action callback)
    {
        while (fadeLogin)
        {
            if (icon_login_img.alpha < 1)
            {
                icon_login_img.alpha += Time.deltaTime;
                if (icon_login_img.alpha >= 1)
                {
                    fadeLogin = false;
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        callback?.Invoke();
    }
    public void setLoadingDisplay()
    {
        loading_obj.SetActive(true);
        mainLogin_obj.SetActive(false);
        StartCoroutine(setReloadDisplay(() => 
        {
            loading_obj.SetActive(false);
            startGame_obj.SetActive(true);
        }));
    }
    public void setGuestDisplay()
    {
        loading_obj.SetActive(true);
        mainLogin_obj.SetActive(false);
        StartCoroutine(setReloadDisplay(() =>
        {
            //loading_obj.SetActive(false);
            GoToScene("Location_basedGameplay");
        }));
    }
    public void GoToScene(string sceneName)
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        SceneManager.LoadScene(sceneName);
        //StartCoroutine(LoadScene(sceneName, objectToMove));
    }
    IEnumerator setReloadDisplay(Action callback)
    {
        yield return LoginController.instance.IsLogin();
        currentLoading = 10;
        yield return LoginController.instance.IsLoadGhostData();
        currentLoading = 60;
        yield return LoginController.instance.LoadShopping();
        callback?.Invoke();
    }
}
