using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class WalletResp : BaseWSResponse
    {
        public int coin;
        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            Debug.Log(jObj.ToString());

            var data = jObj["data"].AsObject;
            var entities = data["entities"].AsObject;
            this.coin = entities["coin"].AsInt;
        }

        public static IEnumerator GetWallet(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<WalletResp>(
                apiPath: Uri.EscapeUriString("/api/v1/wallet"),
                headers: null,
                callback: callback,
                apiTrackCode: -1);
        }

    }

}
