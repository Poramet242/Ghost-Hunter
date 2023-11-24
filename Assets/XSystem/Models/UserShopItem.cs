using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class UserShopItem : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string userID;
        public string itemID;
        public string itemCode;
        public bool isUsed;
        public DateTime buyOn;
        public DateTime usedOn;
        public DateTime expiredOn;

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
            this.itemID = data["itemID"].Value;
            this.itemCode = data["itemCode"].Value;
            this.isUsed = data["isUsed"].AsBool;
            this.buyOn = Utility.ParseDatetime(data["buyOn"].Value);
            this.usedOn = Utility.ParseDatetime(data["usedOn"].Value);
            this.expiredOn = Utility.ParseDatetime(data["expiredOn"].Value);
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
            this.itemID = data["itemID"].Value;
            this.itemCode = data["itemCode"].Value;
            this.isUsed = data["isUsed"].AsBool;
            this.buyOn = Utility.ParseDatetime(data["buyOn"].Value);
            this.usedOn = Utility.ParseDatetime(data["usedOn"].Value);
            this.expiredOn = Utility.ParseDatetime(data["expiredOn"].Value);
        }

        public static List<UserShopItem> ParseToList(string jsonString)
        {
            List<UserShopItem> userMonsters = new List<UserShopItem>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new UserShopItem();
                item.ParseFromJSONObject2(itemJson);
                userMonsters.Add(item);
            }

            return userMonsters;

        }

        public static IEnumerator GetAllUserItemCode(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/shop/userCodes"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

        public static IEnumerator GetActiveUserItemCode(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/shop/userCodes/active"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

        public static IEnumerator UseItemCode(XCore xcoreInst, string itemCode, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("itemCode", itemCode);
            yield return xcoreInst.POST<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/shop/use"),
            headers: null,
            postData:formData,
            callback: callback,
            apiTrackCode: -1);

        }
    }
}