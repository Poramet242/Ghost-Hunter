using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class ItemSpawnPoint : BaseWSResponse
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
        public bool dropArmor;
        public bool dropWeapon;
        public int cooldown;


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
            this.radiusLon = data["radiusLon"].AsFloat;
            this.dropArmor = data["dropArmor"].AsBool;
            this.dropWeapon = data["dropWeapon"].AsBool;
            this.cooldown = data["cooldown"].AsInt;
           
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
            this.radiusLon = data["radiusLon"].AsFloat;
            this.dropArmor = data["dropArmor"].AsBool;
            this.dropWeapon = data["dropWeapon"].AsBool;
            this.cooldown = data["cooldown"].AsInt;
        }
        public static List<ItemSpawnPoint> ParseToList(string jsonString)
        {
            List<ItemSpawnPoint> itemSpawnPoints = new List<ItemSpawnPoint>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new ItemSpawnPoint();
                item.ParseFromJSONObject2(itemJson);
                itemSpawnPoints.Add(item);
            }

            return itemSpawnPoints;

        }

        public static IEnumerator GetItemSpawnPoint(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/itemSpawnPoint"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }
}