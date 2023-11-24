using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class Monster : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string monsterID;
        public string name;
        public RarityType rarity;
        public int hp;
        public int energyCost;
        public int reward;
        public string pointName;

        public override void ParseFromJSONObject(JSONObject jObj)
        {
            Debug.Log(jObj.ToString());
            base.ParseFromJSONObject(jObj);
            if (jObj["success"] == "false")
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
            this.monsterID = data["monsterID"].Value;
            this.name = data["name"].Value;
            //TODO : Change to Enum
            this.rarity = (RarityType)data["rarity"].AsInt;
            this.hp = data["hp"].AsInt;
            this.energyCost = data["energyCost"].AsInt;
            this.reward = data["reward"].AsInt;
            this.pointName = data["pointName"].Value;
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
            this.monsterID = data["monsterID"].Value;
            this.name = data["name"].Value;
            //TODO : Change to Enum
            this.rarity = (RarityType)data["rarity"].AsInt;
            this.hp = data["hp"].AsInt;
            this.energyCost = data["energyCost"].AsInt;
            this.reward = data["reward"].AsInt;
            this.pointName = data["pointName"].Value;
        }

        public static List<Monster> ParseToList(string jsonString)
        {
            List<Monster> monsters = new List<Monster>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            if (items == null)
            {
                return monsters;
            }
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new Monster();
                item.ParseFromJSONObject2(itemJson);
                monsters.Add(item);
            }

            return monsters;

        }

        public static IEnumerator GetAllMonster(XCore xcoreInst, Action<IWSResponse> callback)
        {

            //  var header = new Dictionary<string, string>();
            // header["X-Session-Token"] = XSession.Current().Token();

            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gamedata/monster/all"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

        public static IEnumerator GetNearbyMonster(XCore xcoreInst,float lat,float lon, Action<IWSResponse> callback)
        {
            //  var header = new Dictionary<string, string>();
            // header["X-Session-Token"] = XSession.Current().Token();
            Debug.Log(lat + " : " + lon);
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gamedata/monster/near?lat="+lat+"&long="+lon),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

    }
}
