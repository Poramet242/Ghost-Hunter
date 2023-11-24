using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest_ctr : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] private Button[] _quest_btn;
    [Header("Count Ghost")]
    [SerializeField] private int all_ghost = 0;
    [SerializeField] private int ghosts_r = 0;
    [SerializeField] private int ghosts_sr = 0;
    [SerializeField] private int ghost_ssr = 0;
    [SerializeField] private int ghost_legendary = 0;
    private void Start()
    {
        for (int i = 0; i < InventoryGhostObject.instance.ghostInventoryList.Count; i++)
        {
            switch (InventoryGhostObject.instance.ghostInventoryList[i].detail._rarityType)
            {
                case RarityType.none:
                    break;
                case RarityType.R:
                    ghosts_r++;
                    break;
                case RarityType.SR:
                    ghosts_sr++;
                    break;
                case RarityType.SSR:
                    ghost_ssr++;
                    break;
                case RarityType.Legendary:
                    ghost_legendary++;
                    break;
            }
        }
        all_ghost = (ghosts_r + ghosts_sr + ghost_ssr + ghost_legendary);
        for (int i = 0; i < _quest_btn.Length; i++)
        {
            setupQuest(_quest_btn[i].GetComponent<QuestDisplay>());
        }
    }
    public void setupQuest(QuestDisplay questDisplay)
    {
        /*//TODO call back to server get succeed quest to gameplay
        if (ghosts_r >= questDisplay.countGhost && questDisplay.questType == QuestType.R)
        {
            questDisplay.Succeed = true;
        }
        else if (ghosts_sr >= questDisplay.countGhost && questDisplay.questType == QuestType.SR)
        {
            questDisplay.Succeed = true;
        }
        else if (ghost_ssr >= questDisplay.countGhost && questDisplay.questType == QuestType.SSR)
        {
            questDisplay.Succeed = true;
        }
        else if (ghost_legendary >= questDisplay.countGhost && questDisplay.questType == QuestType.Legendary)
        {
            questDisplay.Succeed = true;
        }
        else if (all_ghost >= questDisplay.countGhost && questDisplay.questType == QuestType.All)
        {
            questDisplay.Succeed = true;
        }*/
    }
    public void onClickClaimItems_btn(QuestDisplay display)
    {
        /*for (int i = 0; i < _quest_btn.Length; i++)
        {
            if (_quest_btn[i].GetComponent<QuestDisplay>()._questID == display._questID)
            {
                if (_quest_btn[i].GetComponent<QuestDisplay>().Succeed)
                {
                    _quest_btn[i].GetComponent<QuestDisplay>().ClaimItems = true;
                    _quest_btn[i].GetComponent<QuestDisplay>().Succeed = false;
                    PlayerData.instance._coineReward += display.point;
                }
            }
        }*/
    }
}
