using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDisplay : MonoBehaviour
{
    public static MenuDisplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Location Btn")]
    [SerializeField] private Image _location_img;
    [SerializeField] private Sprite _location_selected;
    [SerializeField] private Sprite _location_unSelected;
    [Header("Quest Btn")]
    [SerializeField] private Image _quest_img;
    [SerializeField] private Sprite _quest_selected;
    [SerializeField] private Sprite _quest_unSelected;
    [Header("Profile Btn")]
    [SerializeField] private Image _profile_img;
    [SerializeField] private Sprite _profile_selected;
    [SerializeField] private Sprite _profile_unSelected;
    [Header("Setting Btn")]
    [SerializeField] private Image _setting_img;
    [SerializeField] private Sprite _setting_selected;
    [SerializeField] private Sprite _setting_unSelected;


    public void onClickLocationBtn()
    {
        _location_img.sprite = _location_selected;
        _quest_img.sprite = _quest_unSelected;
        _profile_img.sprite = _profile_unSelected;
        _setting_img.sprite = _setting_unSelected;
    }
    public void onClickQuestBtn()
    {
        _quest_img.sprite = _quest_selected;
        _location_img.sprite = _location_unSelected;
        _profile_img.sprite = _profile_unSelected;
        _setting_img.sprite = _setting_unSelected;
    }
    public void onClickProfileBtn()
    {
        _profile_img.sprite = _profile_selected;
        _quest_img.sprite = _quest_unSelected;
        _location_img.sprite = _location_unSelected;
        _setting_img.sprite = _setting_unSelected;
    }
    public void onClickSettingBtn()
    {
        _setting_img.sprite = _setting_selected;
        _profile_img.sprite = _profile_unSelected;
        _quest_img.sprite = _quest_unSelected;
        _location_img.sprite = _location_unSelected;
    }
}
