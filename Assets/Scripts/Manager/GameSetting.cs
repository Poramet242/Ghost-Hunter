using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
    #region Sound Header
    [Header("SoundSFX setting")]
    [SerializeField] private Slider _SFXSlider;
    [SerializeField] private float _SFXValue;
    [Header("SoundBGM setting")]
    [SerializeField] private Slider _BGMSlider;
    [SerializeField] private float _BGMValue;
    #endregion
    private void Start()
    {
        setupStartSetting();
    }
    private void Update()
    {
        //setCloseSoundSFX_volme(_SFXSlider.value);
        //setCloseSoundBGM_volme(_SFXSlider.value);
    }
    public void setupStartSetting()
    {
        _SFXValue = DataHolder.SFX_Volume;
        _SFXSlider.value = _SFXValue;
        _BGMValue = DataHolder.BGM_Volume;
        _BGMSlider.value = _BGMValue;
    }
    public void SaveSetting()
    {
        PlayerPrefs.SetFloat("SFX_Volume", _SFXValue);
        PlayerPrefs.SetFloat("BGM_Volume", _BGMValue);

        this.gameObject.SetActive(false);
    }
    public void setDefaultSetting()
    {
        _SFXValue = 1f;
        _BGMValue = 1f;
        _SFXSlider.value = _SFXValue;
        _BGMSlider.value = _BGMValue;
        PlayerPrefs.SetFloat("SFX_Volume", _SFXValue);
        PlayerPrefs.SetFloat("BGM_Volume", _BGMValue);
    }
    public void setSoundSFX_Volume()
    {
        _SFXValue = _SFXSlider.value;
        DataHolder.SFX_Volume = _SFXValue;
        float numVolSFX = _SFXValue * 100;
        //_SFXtext.text = Mathf.FloorToInt(numVolSFX).ToString();
    }
    public void setSoundBGM_volume()
    {
        _BGMValue = _BGMSlider.value;
        DataHolder.BGM_Volume = _BGMValue;
        float numVolBGM = _BGMValue * 100;
        //_BGMtext.text = Mathf.FloorToInt(numVolBGM).ToString();
        if (SoundManager.instance != null)
        {
            if (SoundManager.instance.isPlayingBGM())
            {
                SoundManager.instance.GetBGMInstance().volume = _BGMValue;
            }
        }
    }
}
