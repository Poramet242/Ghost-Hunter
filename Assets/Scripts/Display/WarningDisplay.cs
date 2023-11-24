using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningDisplay : MonoBehaviour
{
    public static WarningDisplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Sprite Display")]
    [SerializeField] private Sprite _doubleDamages_spr;
    [SerializeField] private Sprite _armorUp_spr;
    [SerializeField] private Sprite _energy_spr;
    [SerializeField] private Sprite _noneEnergy_spr;
    [SerializeField] private Sprite _errorServer_spr;
    [Header("Warning Display")]
    [SerializeField] private WarningType w_type;
    [SerializeField] private GameObject w_display_obj;
    [SerializeField] private Text w_heard_text;
    [SerializeField] private Text w_info_text;
    [SerializeField] private Image w_img;
    [SerializeField] private Button w_ok;
    [SerializeField] private Color suscces_clr;
    [SerializeField] private Color error_clr;
    [SerializeField] private GameObject close_Btn;
    public void setupWarningDisplay(string heard,string info, WarningType warning)
    {
        w_type = warning;
        switch (warning)
        {
            case WarningType.GetDoubleDamages:
                w_heard_text.color = suscces_clr;
                w_img.sprite = _doubleDamages_spr;
                break;
            case WarningType.GetArmorUpper:
                w_heard_text.color = suscces_clr;
                w_img.sprite = _armorUp_spr;
                break;
            case WarningType.GetEnergy:
                w_heard_text.color = suscces_clr;
                w_img.sprite = _energy_spr;
                break;
            case WarningType.noneStamina:
                w_heard_text.color = error_clr;
                w_img.sprite = _noneEnergy_spr;
                break;
            case WarningType.ErrorServer:
                if (heard == "ตรวจพบการเคลื่อนที่ผิดปกติ")
                {
                    close_Btn.SetActive(false);
                }
                w_heard_text.color = error_clr;
                w_img.sprite = _errorServer_spr;
                break;     
            case WarningType.ErrorUiDisplay:
                w_heard_text.color = suscces_clr;
                w_img.sprite = _errorServer_spr;
                break;
            case WarningType.ServerClose:
                w_heard_text.color = suscces_clr;
                w_img.sprite = _errorServer_spr;
                break;
        }
        w_heard_text.text = heard;
        w_info_text.text = info;
        w_display_obj.SetActive(true);
    }
    public void onclickCloseWarningDispaly()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        if (w_type == WarningType.ErrorServer || w_type == WarningType.ServerClose)
        {
            Application.Quit();
        }
        w_heard_text.text = string.Empty;
        w_info_text.text = string.Empty;
        w_img.sprite = null;
        w_type = WarningType.None;
        w_display_obj.SetActive(false);
    }
}
public enum WarningType
{
    None = 0,
    GetDoubleDamages = 1,
    GetArmorUpper = 2,
    GetEnergy = 3,
    noneStamina = 4,
    ErrorUiDisplay = 5,
    ErrorServer = 6,
    ServerClose = 7,
}
