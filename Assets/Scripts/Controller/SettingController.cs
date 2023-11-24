using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : MonoBehaviour
{
    public static SettingController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Display")]
    [SerializeField] public GameObject _thisGaemObject;
    [SerializeField] private GameObject SettingDisplay;
    [SerializeField] private GameObject ItemCodeDisplay;
    [SerializeField] private GameObject HelpAndSupportDisplay;
    [SerializeField] private GameObject PolicyDisplay;
    [SerializeField] private GameObject RuleDisplay;
    [SerializeField] private GameObject SoundSettingDisplay;
    [SerializeField] private GameObject mainSettingDisplay;
    [SerializeField] private GameObject sideTermsAndPolicyDisplay;

    public void onClickSettingEnable()
    {
        SettingDisplay.SetActive(true);
        ItemCodeDisplay.SetActive(false);
        HelpAndSupportDisplay.SetActive(false);
        PolicyDisplay.SetActive(false);
        RuleDisplay.SetActive(false);
        SoundSettingDisplay.SetActive(false);
    }

    public void openURL(string url)
    {
        Application.OpenURL(url);
    }
    #region onClick_Btn

    public void onClickItemCode_btn()
    {
        ItemCodeDisplay.SetActive(true);
        HelpAndSupportDisplay.SetActive(false);
        PolicyDisplay.SetActive(false);
        RuleDisplay.SetActive(false);
        SoundSettingDisplay.SetActive(false);
    }
    public void onClickHelpAndSupport_btn()
    {
        ItemCodeDisplay.SetActive(false);
        HelpAndSupportDisplay.SetActive(true);
        PolicyDisplay.SetActive(false);
        RuleDisplay.SetActive(false);
        SoundSettingDisplay.SetActive(false);
    }
    public void onClickPolicy_btn()
    {
        ItemCodeDisplay.SetActive(false);
        HelpAndSupportDisplay.SetActive(false);
        PolicyDisplay.SetActive(true);
        RuleDisplay.SetActive(false);
        SoundSettingDisplay.SetActive(false);
    }
    public void onClickTerms_btn()
    {
        ItemCodeDisplay.SetActive(false);
        HelpAndSupportDisplay.SetActive(false);
        PolicyDisplay.SetActive(false);
        RuleDisplay.SetActive(true);
        SoundSettingDisplay.SetActive(false);
    }
    public void onClickSoundSetting_btn()
    {
        ItemCodeDisplay.SetActive(false);
        HelpAndSupportDisplay.SetActive(false);
        PolicyDisplay.SetActive(false);
        RuleDisplay.SetActive(false);
        SoundSettingDisplay.SetActive(true);
    }

    public void onClickOpenAnotherRuleSetting_btn()
    {
        RuleDisplay.SetActive(true);
        sideTermsAndPolicyDisplay.SetActive(false);
    }
    public void onClickTermsPolicy()
    {
        sideTermsAndPolicyDisplay.SetActive(true);
        mainSettingDisplay.SetActive(false);
    }
    public void onClickOpenMainetting_btn()
    {
        sideTermsAndPolicyDisplay.SetActive(false);
        mainSettingDisplay.SetActive(true);
    }
    #endregion
}
