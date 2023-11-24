using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class ShopItemWithCode : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string itemID;
        public ShopItemCategory category;
        public bool isHighlight;
        public string title;
        public string description;
        public int price;
        public bool canRedeem;
        public List<string> images;
        public List<string> codes;

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
            this.itemID = data["itemID"].Value;
            this.category = (ShopItemCategory)data["category"].AsInt;
            this.isHighlight = data["isHighlight"].AsBool;
            this.title = data["title"].Value;
            this.description = data["description"].Value;
            this.price = data["price"].AsInt;
            this.canRedeem = data["canRedeem"].AsBool;
            this.images = new List<string>();
            var items = data["images"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                this.images.Add(items[i].Value);
            }
            this.codes = new List<string>();
            items = data["codes"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                this.codes.Add(items[i].Value);
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
            this.itemID = data["itemID"].Value;
            this.category = (ShopItemCategory)data["category"].AsInt;
            this.isHighlight = data["isHighlight"].AsBool;
            this.title = data["title"].Value;
            this.description = data["description"].Value;
            this.price = data["price"].AsInt;
            this.canRedeem = data["canRedeem"].AsBool;
            this.images = new List<string>();
            var items = data["images"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                this.images.Add(items[i].Value);
            }
            this.codes = new List<string>();
            items = data["codes"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                this.codes.Add(items[i].Value);
            }
        }

        public static List<ShopItemWithCode> ParseToList(string jsonString)
        {
            List<ShopItemWithCode> monsters = new List<ShopItemWithCode>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new ShopItemWithCode();
                item.ParseFromJSONObject2(itemJson);
                monsters.Add(item);
            }

            return monsters;

        }

        public static IEnumerator GetAllShopItem(XCore xcoreInst, Action<IWSResponse> callback)
        {

            //  var header = new Dictionary<string, string>();
            // header["X-Session-Token"] = XSession.Current().Token();

            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/shop/items/withCode"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

    }
}
