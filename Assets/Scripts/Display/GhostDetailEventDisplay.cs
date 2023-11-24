using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostDetailEventDisplay : MonoBehaviour
{
    public static GhostDetailEventDisplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Detail Dispaly")]
    [SerializeField] private Image _iocn_img;
    [SerializeField] private Text _nameDisplay_text;
    [SerializeField] private Text _areaDisplay_text;
    [SerializeField] private Text _point_text;
    //[SerializeField] private Image _frame_img;

    public void setup(GhostUnitData ghostUnitData)
    {
        _iocn_img.sprite = ghostUnitData.detail._iconGhost;
        //_frame_img.sprite = ghostUnitData.detail._framIconeGhost;
        _nameDisplay_text.text = ghostUnitData.detail.ghostName;
        _areaDisplay_text.text = "Area: " + ghostUnitData.detail.AreaName;
        _point_text.text = ghostUnitData.unitData.ghostPoint.ToString() + " เหรียญ";
    }
}
