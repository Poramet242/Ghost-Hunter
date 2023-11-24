using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class DailyRewardCountResp : BaseWSResponse
    {
        public int count;
        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            Debug.Log(jObj.ToString());

            var data = jObj["data"].AsObject;
            count = data["entities"].AsInt;

        }

        public static IEnumerator GetDailyRewardCount(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<DailyRewardCountResp>(
                apiPath: Uri.EscapeUriString("/api/v1/dailyReward/count"),
                headers: null,
                callback: callback,
                apiTrackCode: -1);
        }


    }

}
