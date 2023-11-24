using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class GameConfig : BaseWSResponse
    {
        public string value;

        public override void ParseFromJSONObject(JSONObject jObj)
        {
            Debug.Log(jObj.ToString());
            base.ParseFromJSONObject(jObj);
            if (jObj["success"] == "false")
            {
                return;
            }
            var data = jObj["data"].AsObject;
            this.value = data["entities"].Value;

        }

        public void ParseFromJSONObject2(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            var data = jObj["data"].AsObject;
            this.value = data["entities"].Value;

        }

        public static List<GameConfig> ParseToList(string jsonString)
        {
            List<GameConfig> configs = new List<GameConfig>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new GameConfig();
                item.ParseFromJSONObject2(itemJson);
                configs.Add(item);
            }

            return configs;

        }

        public static IEnumerator GetGameConfig(XCore xcoreInst,string key, Action<IWSResponse> callback)
        {

            //  var header = new Dictionary<string, string>();
            // header["X-Session-Token"] = XSession.Current().Token();

            yield return xcoreInst.GET<GameConfig>(
            apiPath: Uri.EscapeUriString("/api/v1/gamedata/gameConfig?key="+key),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

    }
}
