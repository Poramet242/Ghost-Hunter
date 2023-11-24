using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] public StageUiDisplay stageUiDisplay;
    [Header("Ui Display")]
    [SerializeField] public GameObject menu;
    [SerializeField] public GameObject daily_Ui;
    [SerializeField] public GameObject location_Ui;
    [SerializeField] public GameObject quest_Ui;
    [SerializeField] public GameObject profile_ui;
    [SerializeField] public GameObject setting_Ui;
    [SerializeField] public GameObject ranking_Ui;
    [SerializeField] public GameObject tutorial_Ui;
    [SerializeField] public GameObject gameplayDisplay;
    [Header("Button Display")]
    [SerializeField] public Button _ranking_btn;
    [Header("Loading Display")]
    [SerializeField] public GameObject loading_display;
    [SerializeField] public Text _logLoading_text;

    private void Start()
    {
        if (GhostDataObject.instance.isMainmenu)
        {
            StartCoroutine(setLocationdisplay());
        }
        else
        {
            //Loading Display
            loading_display.SetActive(true);
            //Open get data to gameplay
            location_Ui.SetActive(true);
            daily_Ui.SetActive(true);
            quest_Ui.SetActive(false);
            profile_ui.SetActive(false);
            setting_Ui.SetActive(false);
            ranking_Ui.SetActive(false);
            tutorial_Ui.SetActive(false);
            menu.SetActive(false);
            gameplayDisplay.SetActive(true);
            _ranking_btn.gameObject.SetActive(false);
        }
    }
    IEnumerator setLocationdisplay()
    {
        //Loading Display
        loading_display.SetActive(true);
        //Open get data to gameplay
        location_Ui.SetActive(true);
        gameplayDisplay.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        onClickLocation_btn();
    }
    public void onclickTutorial_btn()
    {
        GameManager.instance.isUiDispaly = true;
        stageUiDisplay = StageUiDisplay.GameDisplay;
        tutorial_Ui.SetActive(true);
        location_Ui.SetActive(false);
        daily_Ui.SetActive(false);
        quest_Ui.SetActive(false);
        profile_ui.SetActive(false);
        setting_Ui.SetActive(false);
        ranking_Ui.SetActive(false);

    }
    public void onClickLocation_btn()
    {
        GameManager.instance.isUiDispaly = true;
        stageUiDisplay = StageUiDisplay.Event_Location;
        location_Ui.SetActive(true);
        daily_Ui.SetActive(false);
        quest_Ui.SetActive(false);
        profile_ui.SetActive(false);
        setting_Ui.SetActive(false);
        ranking_Ui.SetActive(false);
        gameplayDisplay.SetActive(false);
        _ranking_btn.gameObject.SetActive(true);
        menu.SetActive(true);
        MenuDisplay.instance.onClickLocationBtn();
        PlayerData.instance._checkDaily = true;
    }
    public void onClickQuest_btn()
    {
        stageUiDisplay = StageUiDisplay.Quest;
        location_Ui.SetActive(false);
        daily_Ui.SetActive(false);
        quest_Ui.SetActive(true);
        profile_ui.SetActive(false);
        setting_Ui.SetActive(false);
        ranking_Ui.SetActive(false);
        _ranking_btn.gameObject.SetActive(false);
    }
    public void onClickProfile_btn()
    {
        stageUiDisplay = StageUiDisplay.Profile;
        location_Ui.SetActive(false);
        daily_Ui.SetActive(false);
        quest_Ui.SetActive(false);
        profile_ui.SetActive(true);
        setting_Ui.SetActive(false);
        ranking_Ui.SetActive(false);
        _ranking_btn.gameObject.SetActive(false);
    }
    public void onClickSetting_btn()
    {
        stageUiDisplay = StageUiDisplay.Setting;
        location_Ui.SetActive(false);
        daily_Ui.SetActive(false);
        quest_Ui.SetActive(false);
        profile_ui.SetActive(false);
        setting_Ui.SetActive(true);
        SettingController.instance.onClickSettingEnable();
        ranking_Ui.SetActive(false);
        _ranking_btn.gameObject.SetActive(false);
    }
    public void onClickRanking_btn(bool isMainmenu)
    {
        GameManager.instance.isUiDispaly = true;
        if (isMainmenu)
        {
            stageUiDisplay = StageUiDisplay.Ranking;
            menu.SetActive(true);
        }
        else
        {
            stageUiDisplay = StageUiDisplay.GameDisplay;
            menu.SetActive(false);
        }
        location_Ui.SetActive(false);
        daily_Ui.SetActive(false);
        quest_Ui.SetActive(false);
        profile_ui.SetActive(false);
        setting_Ui.SetActive(false);
        ranking_Ui.SetActive(true);
    }
    public void onClickGamePlay()
    {
        GameManager.instance.isUiDispaly = false;
        stageUiDisplay = StageUiDisplay.GameDisplay;
        gameplayDisplay.SetActive(true);
        location_Ui.SetActive(false);
        daily_Ui.SetActive(false);
        quest_Ui.SetActive(false);
        profile_ui.SetActive(false);
        setting_Ui.SetActive(false);
        ranking_Ui.SetActive(false);
        menu.SetActive(false);
        _ranking_btn.gameObject.SetActive(false);
        if (PlayerPrefs.GetString("Tutorial") == "true")
        {
            return;
        }
        else
        {
            tutorial_Ui.SetActive(true);
        }
    }

    public void OpenNavigationMenu()
    {
        menu.SetActive(true);
    }
    public void CloseNavigationMenu()
    {
        menu.SetActive(false);
    }

    IEnumerator setReloadDisplay(Action callback,int count)
    {
        loading_display.SetActive(true);
        yield return new WaitForSeconds(count);
        loading_display.SetActive(false);
        callback?.Invoke();
    }
}
public enum StageUiDisplay
{
    GameDisplay = 0,
    Event_Location = 1,
    Quest = 2,
    Profile = 3,
    Shopping = 4,
    Setting = 5,
    Ranking = 6,
}
