using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class HighScore : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string userID;
        public string uid;
        public string displayName;
        public string displayImageID;
        public int score;
        public int totalCatch;
        public bool tutorialPlayed;
        public int rank;

        public override void ParseFromJSONObject(JSONObject jObj)
        {
//            Debug.Log(jObj.ToString());
            base.ParseFromJSONObject(jObj);
            if (jObj["success"] == "false")
            {
                return;
            }
            var data = jObj["data"].AsObject;
            if (data == null)
            {
                return;
            }
            data = data["entities"].AsObject;
            if (data == null || data.ToString() == string.Empty || data.ToString() == "{}")
            {
                data = jObj;
            }
            this.id = data["id"].Value;
            this.createdOn = Utility.ParseDatetime(data["createdOn"].Value);
            this.updatedOn = Utility.ParseDatetime(data["updatedOn"].Value);
            this.userID = data["userID"].Value;
            this.uid = data["uid"].Value;
            this.displayName = data["displayName"].Value;
            this.displayImageID = data["displayImageID"].Value;
            this.score = data["score"].AsInt;
            this.totalCatch = data["totalCatch"].AsInt;
            this.tutorialPlayed = data["tutorialPlayed"].AsBool;
            this.rank = data["rank"].AsInt;
        }

        public static List<HighScore> ParseToList(string jsonString)
        {
            List<HighScore> highScores = new List<HighScore>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new HighScore();
                item.ParseFromJSONObject(itemJson);
                highScores.Add(item);
            }

            return highScores;

        }
        
        public static IEnumerator GetHighScores(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/highScore"),
                headers: null,
                callback: callback,
                apiTrackCode: -1);
        }

         public static IEnumerator GetUserHighScore(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<HighScore>(
                apiPath: Uri.EscapeUriString("/api/v1/highScore/my"),
                headers: null,
                callback: callback,
                apiTrackCode: -1);
        }


    }
}
