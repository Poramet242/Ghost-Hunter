using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class UserQuest : BaseWSResponse
    {
        public string questID;
        public string description;
        public QuestType questType;
        public QuestAction questAction;
        public int targetRarity; //TODO : Change to enum
        public int targetAmount;
        public int rewardAmount;
        public string userID;
        public int progress;
        public bool isCompleted;
        public bool isClaimed;
        public DateTime startedOn;
        public DateTime claimedOn;

        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            Debug.Log(jObj.ToString());
            if (jObj["success"].AsBool == false)
            {
                return;
            }
            var data = jObj["data"].AsObject;
            data = data["entities"].AsObject;
            if (data == null || data.ToString() == string.Empty || data.ToString() == "{}")
            {
                data = jObj;
            }
            this.questID = data["questID"].Value;
            this.description = data["description"].Value;
            this.questType = (QuestType)data["questType"].AsInt;
            this.questAction = (QuestAction)data["questAction"].AsInt;
            this.targetRarity = data["targetRarity"].AsInt;
            this.targetAmount = data["targetAmount"].AsInt;
            this.rewardAmount = data["rewardAmount"].AsInt;
            this.userID = data["userID"].Value;
            this.progress = data["progress"].AsInt;
            this.isCompleted = data["isCompleted"].AsBool;
            this.isClaimed = data["isClaimed"].AsBool;
            this.startedOn = Utility.ParseDatetime(data["startedOn"].Value);
            this.claimedOn = Utility.ParseDatetime(data["claimedOn"].Value);

        }
        public void ParseFromJSONObject2(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            var data = jObj["data"].AsObject;
            data = data["entities"].AsObject;
            if (data == null || data.ToString() == string.Empty || data.ToString() == "{}")
            {
                data = jObj;
            }
            this.questID = data["questID"].Value;
            this.description = data["description"].Value;
            this.questType = (QuestType)data["questType"].AsInt;
            this.questAction = (QuestAction)data["questAction"].AsInt;
            this.targetRarity = data["targetRarity"].AsInt;
            this.targetAmount = data["targetAmount"].AsInt;
            this.rewardAmount = data["rewardAmount"].AsInt;
            this.userID = data["userID"].Value;
            this.progress = data["progress"].AsInt;
            this.isCompleted = data["isCompleted"].AsBool;
            this.isClaimed = data["isClaimed"].AsBool;
            this.startedOn = Utility.ParseDatetime(data["startedOn"].Value);
            this.claimedOn = Utility.ParseDatetime(data["claimedOn"].Value);
        }
        public static List<UserQuest> ParseToList(string jsonString)
        {
            List<UserQuest> userQuests = new List<UserQuest>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new UserQuest();
                item.ParseFromJSONObject2(itemJson);
                userQuests.Add(item);
            }

            return userQuests;

        }

        public static IEnumerator GetUserQuest(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/userQuest"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }

    public enum QuestType
    {
        DailyQuest = 0,
        WeeklyQuest = 1,
        MonthlyQuest = 2
    }

    public enum QuestAction
    {
        CollectByRarity = 0,
        CollectAny = 1,
        CompleteAllSameType = 2,
        CompleteQuest = 3
    }


}