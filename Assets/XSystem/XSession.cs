using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using JSONLib = SimpleJSON;

namespace XSystem {

    public class XSession {
        internal const string kHeaderKeyXSessionToken = "X-Session-Token";

        static XSession s_currentSession;
        public static XSession Current() {
            return s_currentSession;
        }
        
        string mSessionToken;
        public string Token() {
            return mSessionToken;
        }

        private XSession(string sessionToken) {
            mSessionToken = sessionToken;
        }

        public bool IsIdentical(XSession otherSession) {
            return mSessionToken == otherSession.mSessionToken;
        }

        public static void SaveCurrentSession(string sessionToken) {
            s_currentSession = new XSession(sessionToken);
        }

        public static XSession FromSessionToken(string sessionToken) {
            return new XSession(sessionToken);
        }

        public static void Clear() {
            s_currentSession = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator ValidateSession(XCore xcoreInst, Action<IWSResponse> callback) {
            yield return xcoreInst.POST<ValidateSessionResult>(
                apiPath: $"api/v1/me/validate",
                postData : new WWWForm(),
                headers: new Dictionary<string, string>() { { kHeaderKeyXSessionToken, mSessionToken } },
                callback: callback,
                apiTrackCode: XAPITrackCode.ValidateSession
            );
        }
    }

    public class ValidateSessionResult : BaseWSResponse {
        public bool valid;
        public string reason;

        public override void ParseFromJSONObject(JSONLib.JSONObject jObj) {
            base.ParseFromJSONObject(jObj);
            if (!this.success) return;

            var data = jObj["data"].AsObject;
            valid = data["valid"].AsBool;
            reason = data["reason"].Value;
        }
    }

}