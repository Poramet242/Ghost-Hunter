using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using XSystem;

public class XEditorCore
{
    const string kAdminKey = "s25CZkFu7n";

    public static void POST(string apiPath,
        WWWForm postData,
        Dictionary<string, string> headers,
        Action<WWW> callback) {
        POST(apiPath, postData.data, headers, callback);
    }

    public static void POST(string url,
        byte[] postData,
        Dictionary<string, string> headers,
        Action<WWW> callback) {
        
        if (headers == null) {
            headers = new Dictionary<string, string>();
        }

        // Clean up and set SessionToken
        string sessionToken = XSession.Current() != null ? XSession.Current().Token() : "";
        if (sessionToken == null) {
            sessionToken = "";
        }
        headers["sessionToken"] = sessionToken;

        if (!headers.ContainsKey("X-AdminKey")) {
            headers["X-AdminKey"] = kAdminKey;
        }

        var edWWW = new EditorWWW();
        edWWW.StartWWW(url, headers, postData, (www) => {
#if UNITY_EDITOR
            Debug.LogFormat("[XEditorCore] WWW POST url:{0} response: {1}", url, www.text);
#endif
            callback?.Invoke(www);
        });
    }

    public static void GET(string url,
        Dictionary<string, string> headers,
        Action<WWW> callback) {

        if (headers == null) {
            headers = new Dictionary<string, string>();
        }
        if (!headers.ContainsKey("sessionToken")) {
            headers["sessionToken"] = XSession.Current() != null ? XSession.Current().Token() : "";
        }
        if (!headers.ContainsKey("X-AdminKey")) {
            headers["X-AdminKey"] = kAdminKey;
        }

        var edWWW = new EditorWWW();
        edWWW.StartWWW(url, headers, null, (www) => {
#if UNITY_EDITOR
            Debug.LogFormat("[XEditorCore] WWW GET url: {0} response: {1}", url, www.text);
#endif
            callback?.Invoke(www);
        });
    }


    public static void GET<R>(string apiPath, Dictionary<string, string> headers, Action<IWSResponse> callback)
        where R : BaseWSResponse, new() {
        GET(apiPath, headers, (www) => {
            if (!string.IsNullOrEmpty(www.error)) {
                if (callback != null) {
                    callback(Utility.SimpleWWWError(www));
                    www.Dispose();
                    return;
                }
            }

            if (callback == null) {
                www.Dispose();
                return;
            }

            var resp = new R();
            resp.ParseFromJsonString(www.text);
            if (callback != null) {
                callback(resp);
            }
            www.Dispose();
        });
    }

    public static void POST<R>(string apiPath, WWWForm form, Dictionary<string, string> headers, Action<IWSResponse> callback)
        where R : BaseWSResponse, new() {
        POST<R>(apiPath, form.data, headers, callback);
    }

    public static void POST<R>(string apiPath, byte[] postData, Dictionary<string, string> headers, Action<IWSResponse> callback)
        where R : BaseWSResponse, new() {
        POST(apiPath, postData, headers, (www) => {
            if (!string.IsNullOrEmpty(www.error)) {
                if (callback != null) {
                    callback(Utility.SimpleWWWError(www));
                    www.Dispose();
                    return;
                }
            }

            if (callback == null) {
                www.Dispose();
                return;
            }
            
            var resp = new R();
            resp.ParseFromJsonString(www.text);
            if (callback != null) {
                callback(resp);
            }
            www.Dispose();
        });
    }

    public class EditorWWW {
        private WWW mWWW;
        private Action<WWW> mCallback;
        
        public void StartWWW(string path, Dictionary<string, string> headers, byte[] postData, Action<WWW> callback, params object[] arguments) {
            if (headers == null) {
                headers = new Dictionary<string, string>();
            }
            mCallback = callback;
            if (postData != null) {
                mWWW = new WWW(path, postData, headers);
            } else {
                mWWW = new WWW(path, null, headers);
            }
            EditorApplication.update += Tick;
        }

        public void Tick() {
            if (mWWW.isDone) {
                EditorApplication.update -= Tick;
                mCallback?.Invoke(mWWW);
            }
        }

        public void StopWWW() {
            EditorApplication.update -= Tick;
            if (mWWW != null) mWWW.Dispose();
        }
    }
}
