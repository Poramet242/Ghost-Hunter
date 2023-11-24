using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class EnergyResp : BaseWSResponse
    {
        public int energy;
        public DateTime timeStamp;
        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            Debug.Log(jObj.ToString());

            var data = jObj["data"].AsObject;
            var entities = data["entities"].AsObject;
            this.energy = entities["energy"].AsInt;
            this.timeStamp = Utility.ParseDatetime(entities["timeStamp"].Value);

        }

        public static IEnumerator GetEnergy(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<EnergyResp>(
                apiPath: Uri.EscapeUriString("/api/v1/energy"),
                headers: null,
                callback: callback,
                apiTrackCode: -1);
        }
    }

}
