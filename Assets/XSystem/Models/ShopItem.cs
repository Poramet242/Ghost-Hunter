using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class ShopItem : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string itemID;
        public ShopItemCategory category;
        public bool isHighlight;
        public string title;
        public string description;
        public string shopName;
        public int price;
        public bool canRedeem;//player use item
        public bool alreadyBuy;//player buy
        public bool officeHourOnly;//player buy
        public bool isPartner;//player buy
        public List<string> images;
        public DateTime expiredOn;
        public DateTime startedOn;

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
            this.expiredOn = Utility.ParseDatetime(data["expiredOn"].Value);
            this.startedOn = Utility.ParseDatetime(data["startedOn"].Value);
            this.itemID = data["itemID"].Value;
            this.category = (ShopItemCategory)data["category"].AsInt;
            this.isHighlight = data["isHighlight"].AsBool;
            this.title = data["title"].Value;
            this.description = data["description"].Value;
            this.shopName = data["shopName"].Value;
            this.price = data["price"].AsInt;
            this.canRedeem = data["canRedeem"].AsBool;
            this.alreadyBuy = data["alreadyBuy"].AsBool;
            this.officeHourOnly = data["officeHourOnly"].AsBool;
            this.isPartner = data["isPartner"].AsBool;
            this.images = new List<string>();
            var items = data["images"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                this.images.Add(items[i].Value);
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
            this.expiredOn = Utility.ParseDatetime(data["expiredOn"].Value);
            this.startedOn = Utility.ParseDatetime(data["startedOn"].Value);
            this.itemID = data["itemID"].Value;
            this.category = (ShopItemCategory)data["category"].AsInt;
            this.isHighlight = data["isHighlight"].AsBool;
            this.title = data["title"].Value;
            this.description = data["description"].Value;
            this.shopName = data["shopName"].Value;
            this.price = data["price"].AsInt;
            this.canRedeem = data["canRedeem"].AsBool;
            this.alreadyBuy = data["alreadyBuy"].AsBool;
            this.officeHourOnly = data["officeHourOnly"].AsBool;
            this.isPartner = data["isPartner"].AsBool;
            this.images = new List<string>();
            var items = data["images"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                this.images.Add(items[i].Value);
            }
        }

        public static List<ShopItem> ParseToList(string jsonString)
        {
            List<ShopItem> monsters = new List<ShopItem>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new ShopItem();
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
            apiPath: Uri.EscapeUriString("/api/v1/shop/items"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }       
        
        public static IEnumerator GetAllShopItem2(XCore xcoreInst, Action<IWSResponse> callback)
        {

            //  var header = new Dictionary<string, string>();
            // header["X-Session-Token"] = XSession.Current().Token();

            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/shop/items/all"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

        public static IEnumerator GetShopItemByCategory(XCore xcoreInst,ShopItemCategory category, Action<IWSResponse> callback)
        {
            //  var header = new Dictionary<string, string>();
            // header["X-Session-Token"] = XSession.Current().Token();

            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/shop/items/byCategory?category="+(int)category),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

    }

    public enum ShopItemCategory
    {
        Shopping_Food = 1,
        Entertainment = 2,
        Beauty = 3,
        Other = 4,
        WelcomeReward = 5,
        ConcertTickets = 6,
    }
}
