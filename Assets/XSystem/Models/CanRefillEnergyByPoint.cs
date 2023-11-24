using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class CanRefillEnergyByPointResp : BaseWSResponse
    {
        public bool canRefill;
        public DateTime nextRefillTime;
        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            Debug.Log(jObj.ToString());

            var data = jObj["data"].AsObject;
            canRefill = data["canRefill"].AsBool;
            nextRefillTime = Utility.ParseDatetime(data["nextRefillTime"].Value);

        }

        public static IEnumerator CanRefillEnergyByPoint(XCore xcoreInst,string pointID, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<CanRefillEnergyByPointResp>(
                apiPath: Uri.EscapeUriString("/api/v1/energy/canRefillByPoint?pointID="+pointID),
                headers: null,
                callback: callback,
                apiTrackCode: -1);
        }


    }

}
