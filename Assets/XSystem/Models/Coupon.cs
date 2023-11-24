using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class Coupon : BaseWSResponse
    {
        public string code;
        public int energyAmount;
        public int armorAmount;
        public int weaponAmount;
        public DateTime usedOn;

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
            this.code = data["code"].Value;
            this.energyAmount = data["energyAmount"].AsInt;
            this.armorAmount = data["armorAmount"].AsInt;
            this.weaponAmount = data["weaponAmount"].AsInt;
            this.usedOn = Utility.ParseDatetime(data["usedOn"].Value);

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
            this.code = data["code"].Value;
            this.energyAmount = data["energyAmount"].AsInt;
            this.armorAmount = data["armorAmount"].AsInt;
            this.weaponAmount = data["weaponAmount"].AsInt;
            this.usedOn = Utility.ParseDatetime(data["usedOn"].Value);

        }

        public static List<Coupon> ParseToList(string jsonString)
        {
            List<Coupon> monsters = new List<Coupon>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new Coupon();
                item.ParseFromJSONObject2(itemJson);
                monsters.Add(item);
            }

            return monsters;

        }

        public static IEnumerator GetRedeemLog(XCore xcoreInst, Action<IWSResponse> callback)
        {

            //  var header = new Dictionary<string, string>();
            // header["X-Session-Token"] = XSession.Current().Token();

            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/coupon/redeemLog"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

    }

    public enum CouponCategory
    {
        Food = 0,
        Entertainment = 1,
        Beauty = 2,
        Shopping = 3

    }
}
