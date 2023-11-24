using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSystem;

public class QuestsController : MonoBehaviour
{
    public static QuestsController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Daily")]
    [SerializeField] private Quest_ctr dailyQuests_Ctr;
    [SerializeField] private List<QuestDisplay> dailyQuests_List = new List<QuestDisplay>();
    [Header("Weekly")]
    [SerializeField] private Quest_ctr weeklyQuests_Ctr;
    [SerializeField] private List<QuestDisplay> weeklyQuests_List = new List<QuestDisplay>();
    [Header("Monthly")]
    [SerializeField] private Quest_ctr monthlyQuests_Ctr;
    [SerializeField] private List<QuestDisplay> monthlyQuests_List = new List<QuestDisplay>();
    [Header("Loading")]
    [SerializeField] public GameObject loading;
    private void OnEnable()
    {
        StartCoroutine(setupUserQuest());
    }
    IEnumerator setupUserQuest()
    {
        IWSResponse response = null;
        yield return UserQuest.GetUserQuest(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error Set Quest");
            yield break;
        }
        List<UserQuest> userQuests = UserQuest.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < userQuests.Count; i++)
        {
            switch (userQuests[i].questType)
            {
                case QuestType.DailyQuest:
                    yield return setupQuestDailyQuest(userQuests[i]);
                    break;
                case QuestType.WeeklyQuest:
                    yield return setupQuestWeeklyQuest(userQuests[i]);
                    break;
                case QuestType.MonthlyQuest:
                    yield return setupQuestMonthlyQuest(userQuests[i]);
                    break;
            }
        }
    }
    public IEnumerator setupQuestDailyQuest(UserQuest quest)
    {
        for (int i = 0; i < dailyQuests_List.Count; i++)
        {
            if (quest.questID == dailyQuests_List[i]._questID)
            {
                dailyQuests_List[i].point = quest.rewardAmount;
                dailyQuests_List[i].Succeed = quest.isCompleted;
                dailyQuests_List[i].ClaimItems = quest.isClaimed;
                dailyQuests_List[i]._descriptionQuest = quest.description;
            }
        }
        yield break;
    }
    public IEnumerator setupQuestWeeklyQuest(UserQuest quest)
    {
        for (int i = 0; i < weeklyQuests_List.Count; i++)
        {
            if (quest.questID == weeklyQuests_List[i]._questID)
            {
                weeklyQuests_List[i].point = quest.rewardAmount;
                weeklyQuests_List[i].Succeed = quest.isCompleted;
                weeklyQuests_List[i].ClaimItems = quest.isClaimed;
                weeklyQuests_List[i]._descriptionQuest = quest.description;

            }
        }
        yield break;

    }
    public IEnumerator setupQuestMonthlyQuest(UserQuest quest)
    {
        for (int i = 0; i < monthlyQuests_List.Count; i++)
        {
            if (quest.questID == monthlyQuests_List[i]._questID)
            {
                monthlyQuests_List[i].point = quest.rewardAmount;
                monthlyQuests_List[i].Succeed = quest.isCompleted;
                monthlyQuests_List[i].ClaimItems = quest.isClaimed;
                monthlyQuests_List[i]._descriptionQuest = quest.description;

            }
        }
        yield break;
    }
}
