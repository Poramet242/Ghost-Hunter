using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace XSystem {

    public class HealthCheckAPI {
        public static IEnumerator GetHealthStatus(XCore xcoreInst, Action<IWSResponse> callback) {
            yield return xcoreInst.GET<GetHealthStatusResponse>(
                apiPath: "/api/v1/health/info",
                headers: null,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator TryPostingData(XCore xcoreInst, string data, Action<IWSResponse> callback) {
            var formData = new WWWForm();
            formData.AddField("data", data);
            yield return xcoreInst.POST<TryPostingDataResponse>(
                apiPath: "xapilib/healthcheck/postdata",
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator ListAllRoutes(XCore xcoreInst, int offset, int limit, Action<IWSResponse> callback) {
            yield return xcoreInst.GET<ListAllRoutesResponse>(
                apiPath: $"xapilib/healthcheck/routes?offset={offset}&limit={limit}",
                headers: null,
                callback: callback,
                apiTrackCode: -1);
        }

        public class GetHealthStatusResponse: BaseWSResponse {
            public string status;

            public override void ParseFromJSONObject(JSONObject jObj) {
                base.ParseFromJSONObject(jObj);
                if (!this.success) return;
                var data = jObj["data"].AsObject;
                this.status = data["status"].Value;
            }
        }

        public class TryPostingDataResponse : BaseWSResponse {
            public string postData;

            public override void ParseFromJSONObject(JSONObject jObj) {
                base.ParseFromJSONObject(jObj);
                if (!this.success) return;
                var data = jObj["data"].AsObject;
                this.postData = data["postData"].Value;
            }
        }

        public class ListAllRoutesResponse : BaseWSResponse {
            public SimpleJSON.JSONArray routes;
            public int totalRoutes;

            public override void ParseFromJSONObject(JSONObject jObj) {
                base.ParseFromJSONObject(jObj);
                if (!this.success) return;
                var data = jObj["data"].AsObject;
                this.routes = data["routes"].AsArray;
                this.totalRoutes = data["totalRoutes"].AsInt;
            }
        }


    }

}