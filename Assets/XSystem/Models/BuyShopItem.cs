using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class BuyShopItemResp : BaseWSResponse
    {
        public string code;
        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            Debug.Log(jObj.ToString());
            if (jObj["success"] == "false")
            {
                return;
            }
            var data = jObj["data"].AsObject;
            code = data["entities"].Value;

        }

        public static IEnumerator BuyShopItem(XCore xcoreInst,string itemID, Action<IWSResponse> callback)
        {
             var formData = new WWWForm();
            formData.AddField("itemID", itemID);
            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/shop/buy"),
                headers: null,
                postData:formData,
                callback: callback,
                apiTrackCode: -1);
        }


    }

}
