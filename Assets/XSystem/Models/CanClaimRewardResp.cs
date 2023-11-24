using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class CanClaimRewardResp : BaseWSResponse
    {
        public bool canClaim;
        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            Debug.Log(jObj.ToString());

            var data = jObj["data"].AsObject;
            canClaim = data["entities"].AsBool;

        }

        public static IEnumerator GetCanClaimReward(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<CanClaimRewardResp>(
                apiPath: Uri.EscapeUriString("/api/v1/dailyReward/canClaim"),
                headers: null,
                callback: callback,
                apiTrackCode: -1);
        }


    }

}
