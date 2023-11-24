using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONLib = SimpleJSON;

namespace XSystem {
    public class XModelBase {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;

        public virtual void ParseFromJSONString(string json) {
            this.ParseFromJSONObject(JSONLib.JSON.Parse(json).AsObject);
        }

        public virtual void ParseFromJSONObject(JSONLib.JSONObject jObj) {
            this.id = jObj["id"].Value;
            if (jObj.HasKey("createdOn") && !jObj["createdOn"].IsNull) {
                this.createdOn = Utility.ParseDatetime(jObj["createdOn"].Value);
            }
            if (jObj.HasKey("updatedOn") && !jObj["updatedOn"].IsNull) {
                this.updatedOn = Utility.ParseDatetime(jObj["updatedOn"].Value);
            }
        }
    }

}