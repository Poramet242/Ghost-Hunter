using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class Item : BaseWSResponse
    {
        public string pointID;
        public bool isArmor;
        public bool isWeapon;
        public int effectTime;

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
            this.pointID = data["pointID"].Value;
            this.isArmor = data["isArmor"].AsBool;
            this.isWeapon = data["isWeapon"].AsBool;
            this.effectTime = data["effectTime"].AsInt;

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
            this.pointID = data["pointID"].Value;
            this.isArmor = data["isArmor"].AsBool;
            this.isWeapon = data["isWeapon"].AsBool;
            this.effectTime = data["effectTime"].AsInt;
        }

        public static List<Item> ParseToList(string jsonString)
        {
            List<Item> itemms = new List<Item>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var itemJson = items[i].AsObject;
                    var item = new Item();
                    item.ParseFromJSONObject2(itemJson);
                    itemms.Add(item);
                }
            }
            return itemms;

        }

        public static IEnumerator GetNearbyItem(XCore xcoreInst, float lat, float lon, Action<IWSResponse> callback)
        {
            //  var header = new Dictionary<string, string>();
            // header["X-Session-Token"] = XSession.Current().Token();

            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gamedata/item/near?lat=" + lat + "&long=" + lon),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

    }
}
