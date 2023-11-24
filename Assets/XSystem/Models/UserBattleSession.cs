using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class UserBattleSession : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string userID;
        public string monsterID;
        public bool isActive;
        public DateTime startTime;
        public float lat;
        public float lon;
        public bool isWin;
        public int energyUsed;
        public string capturedMonsterID;



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
            this.id = data["id"].Value;
            this.createdOn = Utility.ParseDatetime(data["createdOn"].Value);
            this.updatedOn = Utility.ParseDatetime(data["updatedOn"].Value);
            this.userID = data["userID"].Value;
            this.monsterID = data["monsterID"].Value;
            this.isActive = data["isActive"].AsBool;
            this.startTime = Utility.ParseDatetime(data["startTime"].Value);
            this.lat = data["lat"].AsFloat;
            this.lon = data["long"].AsFloat;
            this.isWin = data["isWin"].AsBool;
            this.energyUsed = data["energyUsed"].AsInt;
            this.capturedMonsterID = data["capturedMonsterID"].Value;

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
            this.id = data["id"].Value;
            this.createdOn = Utility.ParseDatetime(data["createdOn"].Value);
            this.updatedOn = Utility.ParseDatetime(data["updatedOn"].Value);
            this.userID = data["userID"].Value;
            this.monsterID = data["monsterID"].Value;
            this.isActive = data["isActive"].AsBool;
            this.startTime = Utility.ParseDatetime(data["startTime"].Value);
            this.lat = data["lat"].AsFloat;
            this.lon = data["long"].AsFloat;
            this.isWin = data["isWin"].AsBool;
            this.energyUsed = data["energyUsed"].AsInt;
            this.capturedMonsterID = data["capturedMonsterID"].Value;
        }
        public static List<UserBattleSession> ParseToList(string jsonString)
        {
            List<UserBattleSession> userBattleSessions = new List<UserBattleSession>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new UserBattleSession();
                item.ParseFromJSONObject2(itemJson);
                userBattleSessions.Add(item);
            }

            return userBattleSessions;

        }
    }
}