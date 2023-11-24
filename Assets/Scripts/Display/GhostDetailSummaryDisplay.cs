using Assets.Mapbox.Unity.MeshGeneration.Modifiers.MeshModifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostDetailSummaryDisplay : MonoBehaviour
{
    public static GhostDetailSummaryDisplay Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    [Header("Dispaly")]
    [SerializeField] public string _ghostId;
    [SerializeField] private Image _iconGhost_img;
    [SerializeField] private Text _nameGhost_text;
    [SerializeField] private Text _countGhost_text;
    [SerializeField] private Text _allPointGhost_text;
    [SerializeField] private List<GhostUnitData> ghostUnitDatas = new List<GhostUnitData>();
    
    public void setupGhostDetailSummaryDisplay(GhostUnitData data)
    {
        if (data.detail.ghostID == _ghostId)
        {
            ghostUnitDatas.Add(data);
            setupGhostDisplay(ghostUnitDatas[0]);
        }
    }
    public void setupGhostDisplay(GhostUnitData ghostUnitData)
    {
        _iconGhost_img.sprite = ghostUnitData.detail._iconGhost;
        _nameGhost_text.text = ghostUnitData.detail.ghostName;
        _countGhost_text.text = "Area: " + ghostUnitData.detail.AreaName;
    }

    public void calculateAllPoint()
    {
        int point = 0;
        for (int i = 0; i < ghostUnitDatas.Count; i++)
        {
            point += ghostUnitDatas[i].unitData.ghostPoint;
        }
        _allPointGhost_text.text = point.ToString("#,##0") + " เหรียญ";
    }
}
