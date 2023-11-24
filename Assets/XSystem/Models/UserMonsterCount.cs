using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class UserMonsterCount : BaseWSResponse
    {
        public string monsterID;
        public int amount;


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
            this.monsterID = data["monsterID"].Value;
            this.amount = data["amount"].AsInt;
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
            this.monsterID = data["monsterID"].Value;
            this.amount = data["amount"].AsInt;
        }
        public static List<UserMonsterCount> ParseToList(string jsonString)
        {
            List<UserMonsterCount> userUserMonsterCounts = new List<UserMonsterCount>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new UserMonsterCount();
                item.ParseFromJSONObject2(itemJson);
                userUserMonsterCounts.Add(item);
            }

            return userUserMonsterCounts;

        }

        public static IEnumerator GetUserMonsterCount(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/userMonster/count"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }
}