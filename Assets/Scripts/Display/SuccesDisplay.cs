using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuccesDisplay : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GhostUnitData GhostUnitData;
    [SerializeField] private GameObject _success_img;
    [SerializeField] private GameObject _success_bar;
    [Header("GUI display")]
    [SerializeField] private Text _header_text;
    [SerializeField] private Text _info_text;
    [SerializeField] private Image _icone_img;
    bool playsound = false;
    public void setupSuccessDisplay(GhostUnitData ghostdata)
    {
        StartCoroutine(setSuccessBar(2f));
        GhostUnitData.unitData = ghostdata.unitData;
        GhostUnitData.detail = ghostdata.detail;
        //use text color
        _info_text.text = "คุณจับ " + "<color=red>" + ghostdata.detail.ghostName + "</color>" + " ได้";
        _icone_img.sprite = ghostdata.detail._iconeGhostAR;
    }
    public void setUpFailDisplay(GhostUnitData ghostdata)
    {
        if (!playsound)
        {
            SoundListObject.instance.onPlaySoundSFX(3);
            playsound = true;
        }
        _icone_img.sprite = ghostdata.detail._iconeGhostAR;
    }
    IEnumerator setSuccessBar(float count)
    {
        _success_img.SetActive(true);
        yield return new WaitForSeconds(count);
        _success_img.SetActive(false);
        _success_bar.SetActive(true);
    }
    public void onClickGoToMenu()
    {
        FadeScript.instance.fadeIn = true;
        GhostDataObject.instance.isARGameplay = false;
        SceneTransitionManager.instance.GoToScene(PacketGhostConstants.SCENE_WORD);
    }
}
