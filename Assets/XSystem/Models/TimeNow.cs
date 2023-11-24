using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class TimeNow : BaseWSResponse
    {
        public DateTime timeNow;

        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            var data = jObj["data"].AsObject;
           
            this.timeNow = Utility.ParseDatetime(data["entities"].Value);

        }

        public static IEnumerator GetTimeNow(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<TimeNow>(
            apiPath: Uri.EscapeUriString("/api/v1/now"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }
}