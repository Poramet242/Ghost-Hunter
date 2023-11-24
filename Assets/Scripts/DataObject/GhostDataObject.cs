using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GhostUnitData
{
    public GhostDetail detail;
    public GhostData unitData;
}
public class GhostDataObject : MonoBehaviour
{
    public static GhostDataObject instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [Header("Ghost Spawn data")]
    [SerializeField] public float waitTime = 180.0f;
    [SerializeField] public float minRange = 5.0f;
    [SerializeField] public float maxRange = 50.0f;
    [Header("Stage Gameplay")]
    [SerializeField] public bool isMainmenu;
    [Header("Ghost Selected")]
    [SerializeField] public bool isOpenAR;
    [SerializeField] public bool isARGameplay;
    [SerializeField] public GhostData ghostDatas;
    [SerializeField] public GhostDetail ghostDetail;
    [Header("Ghost in zone")]
    [SerializeField] public List<GhostUnitData> GhostUnitData_listZone = new List<GhostUnitData>();
    [Header("All Ghost")]
    [SerializeField] public List<GhostUnitData> all_GhostUnitData_list = new List<GhostUnitData>();

    public void setUpDataGhostInfo(Monster monster,GhostData ghostData, GhostDetail ghostDetail)
    {
        //Detail
        ghostDetail.ghostName = monster.name;
        ghostDetail.ghostID = monster.monsterID;
        ghostDetail._rarityType = monster.rarity;
        ghostDetail.AreaName = monster.pointName;
        //ghostDetail._iconGhost = LoadUnitObject.instance.GetLocalIconMonster(monster.monsterID);

        ghostDetail._prefabsGhost = LoadUnitObject.instance.GetMonsterPrefaabs(monster.monsterID);
        ghostDetail._ghost_ctr = LoadUnitObject.instance.GetMonsterController(monster.monsterID);
        ghostDetail._iconGhost = LoadUnitObject.instance.GetLocalIconMonster(monster.monsterID);
        ghostDetail._iconeGhostAR = LoadUnitObject.instance.GetLocalIconMonsterAR(monster.monsterID);
        ghostDetail._ghost_sound = LoadUnitObject.instance.GetSoundMonster(monster.monsterID);
        //Data
        ghostData.ghostName = monster.name;
        ghostData.ghostID = monster.monsterID;
        ghostData.hp = monster.hp;
        //TODO: call back get energy consumption
        ghostData.energy_Consumption = monster.energyCost;
        ghostData.ghostPoint = monster.reward;
    }
}
