using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class Account : BaseWSResponse
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

        public override void ParseFromJSONObject(JSONObject jObj)
        {
            Debug.Log(jObj.ToString());
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
        }

        public static List<Account> ParseToList(string jsonString)
        {
            List<Account> accounts = new List<Account>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new Account();
                item.ParseFromJSONObject(itemJson);
                accounts.Add(item);
            }

            return accounts;

        }

        public static IEnumerator GetUserProfile(XCore xcoreInst, Action<IWSResponse> callback)
        {

            //  var header = new Dictionary<string, string>();
            // header["X-Session-Token"] = XSession.Current().Token();

            yield return xcoreInst.GET<Account>(
            apiPath: Uri.EscapeUriString("/api/v1/account"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

        public static IEnumerator SetName(XCore xcoreInst, string name, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("name", name);

            yield return xcoreInst.POST<Account>(
                apiPath: Uri.EscapeUriString("/api/v1/account/setName"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }


        /* public static IEnumerator setProfileImage(XCore xcoreInst, string imageID, Action<IWSResponse> callback)
         {
             var formData = new WWWForm();
             formData.AddField("imageID", imageID);

             yield return xcoreInst.POST<Account>(
                 apiPath: Uri.EscapeUriString("/api/v1/account/setProfileImage"),
                 headers: null,
                 postData: formData,
                 callback: callback,
                 apiTrackCode: -1);
         }

         public static IEnumerator ResetAccount(XCore xcoreInst, Action<IWSResponse> callback)
         {
             var formData = new WWWForm();

             yield return xcoreInst.POST<LoginResult>(
                 apiPath: Uri.EscapeUriString("/api/v1/auth/reset"),
                 headers: null,
                 postData: formData,
                 callback: callback,
                 apiTrackCode: -1);
         }

         public static IEnumerator DeleteAccount(XCore xcoreInst, Action<IWSResponse> callback)
         {
             var formData = new WWWForm();

             yield return xcoreInst.POST<BaseWSResponse>(
                 apiPath: Uri.EscapeUriString("/api/v1/auth/delete"),
                 headers: null,
                 postData: formData,
                 callback: callback,
                 apiTrackCode: -1);
         }

         public static IEnumerator UnbindAccount(XCore xcoreInst, Action<IWSResponse> callback)
         {
             var formData = new WWWForm();

             yield return xcoreInst.POST<BaseWSResponse>(
                 apiPath: Uri.EscapeUriString("/api/v1/auth/unbind"),
                 headers: null,
                 postData: formData,
                 callback: callback,
                 apiTrackCode: -1);
         }*/

        public static IEnumerator SetTutorialPlayed(XCore xcoreInst, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/account/setTutorialPlayed"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }
    }
}
