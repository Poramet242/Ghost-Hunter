using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class UserMonster : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string userID;
        public string monsterID;
        public DateTime capturedOn;
        public float lat;
        public float lon;
        public string pointName;
        public int captureScore;

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
            this.capturedOn = Utility.ParseDatetime(data["capturedOn"].Value);
            this.lat = data["lat"].AsFloat;
            this.lon = data["long"].AsFloat;
            this.pointName = data["pointName"].Value;
            this.captureScore = data["captureScore"].AsInt;
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
            this.capturedOn = Utility.ParseDatetime(data["capturedOn"].Value);
            this.lat = data["lat"].AsFloat;
            this.lon = data["long"].AsFloat;
            this.pointName = data["pointName"].Value;
            this.captureScore = data["captureScore"].AsInt;
        }
        
        public static List<UserMonster> ParseToList(string jsonString)
        {
            List<UserMonster> userMonsters = new List<UserMonster>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new UserMonster();
                item.ParseFromJSONObject2(itemJson);
                userMonsters.Add(item);
            }

            return userMonsters;

        }

        public static IEnumerator GetUserMonster(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/userMonster"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }
}