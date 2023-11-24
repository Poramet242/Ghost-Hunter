using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class MonsterSpawnPoint : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string pointID;
        public string pointName;
        public float lat;
        public float lon;
        public float radiusLat;
        public float radiusLon;
        public List<string> monsters;


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
            this.pointID = data["pointID"].Value;
            this.pointName = data["pointName"].Value;
            this.lat = data["lat"].AsFloat;
            this.lon = data["long"].AsFloat;
            this.radiusLat = data["radiusLat"].AsFloat;
            this.radiusLon = data["radiusLong"].AsFloat;
            this.monsters = new List<string>();
            var items = data["monsters"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                this.monsters.Add(items[i].Value);
            }
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
            this.pointID = data["pointID"].Value;
            this.pointName = data["pointName"].Value;
            this.lat = data["lat"].AsFloat;
            this.lon = data["long"].AsFloat;
            this.radiusLat = data["radiusLat"].AsFloat;
            this.radiusLon = data["radiusLong"].AsFloat;
            this.monsters = new List<string>();
            var items = data["monsters"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                this.monsters.Add(items[i].Value);
            }
        }
        public static List<MonsterSpawnPoint> ParseToList(string jsonString)
        {
            List<MonsterSpawnPoint> monsterSpawnPoints = new List<MonsterSpawnPoint>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new MonsterSpawnPoint();
                item.ParseFromJSONObject2(itemJson);
                monsterSpawnPoints.Add(item);
            }

            return monsterSpawnPoints;

        }

        public static IEnumerator GetMonsterSpawnPoint(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/monsterSpawnPoint"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }
}