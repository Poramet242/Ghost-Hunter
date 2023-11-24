using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace XSystem
{

    public interface IWSResponse
    {
        bool Success();
        string ErrorsString();
        string InternalErrorsString();
        APIError APIError();
        string RawResult();
        bool IsNetworkError();
    }

    public class APIError
    {
        public int errorID;
        public int httpStatus;
        public string message;
        public string internalErrorMessage;
    }

    public class BaseWSResponse : IWSResponse
    {
        public bool success;
        public APIError apiError;
        public string[] errors;
        public string rawResult;
        protected bool mIsNetworkError;

        public BaseWSResponse()
        {
            mIsNetworkError = false;
        }

        public virtual string ErrorsString()
        {
            if (this.apiError != null)
            {
                return this.apiError.message;
            }

            if (this.errors == null)
            {
                return "";
            }

            string errorMsg = "";
            for (int i = 0; i < this.errors.Length; i++)
            {
                errorMsg += this.errors[i];
            }
            return errorMsg;
        }

        public virtual string InternalErrorsString()
        {
            if (this.apiError != null)
            {
                return this.apiError.internalErrorMessage;
            }

            if (this.errors == null)
            {
                return "";
            }

            string errorMsg = "";
            for (int i = 0; i < this.errors.Length; i++)
            {
                errorMsg += this.errors[i];
            }
            return errorMsg;
        }

        public void ParseFromJsonString(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            // keep raw result
            this.rawResult = json;

            var jObj = SimpleJSON.JSON.Parse(json).AsObject;
            this.ParseFromJSONObject(jObj);
        }

        public virtual void ParseFromJSONObject(SimpleJSON.JSONObject jObj)
        {
            // parse success
            this.success = jObj["success"].AsBool;

            // parse errors
            var errorArray = jObj["errors"].AsArray;
            int errorCount = 0;
            if (errorArray != null)
            {
                errorCount = errorArray.Count;
            }
            this.errors = new string[errorCount];
            for (int i = 0; i < errorCount; i++)
            {
                this.errors[i] = errorArray[i].ToString();
            }

            // parse api error
            if (!success)
            {
                jObj = jObj["error"].AsObject;
                if (!jObj.IsNull)
                {
                    var err = new APIError();
                    err.errorID = jObj["id"].AsInt;
                    err.message = jObj["message"].Value;
                    err.httpStatus = jObj["httpStatus"].AsInt;
                    err.internalErrorMessage = jObj["internalErrorMessage"].Value;
                    this.apiError = err;
                }
            }
            else
            {
                this.apiError = null;
            }
        }

        public bool Success()
        {
            return this.success;
        }

        public string[] Errors()
        {
            return this.errors;
        }

        public APIError APIError()
        {
            return this.apiError;
        }

        public string RawResult()
        {
            return this.rawResult;
        }

        public bool IsNetworkError()
        {
            return mIsNetworkError;
        }
    }

    public class NetworkErrorResult : BaseWSResponse
    {
        public NetworkErrorResult()
        {
            mIsNetworkError = true;
        }
    }

    public class XCore
    {
        private const string kHeaderKeyPlatform = "X-Platform";
        private string apiURLEndpoint;
        private static int kRetryLimit = 5;

        private Func<int, IEnumerator> mNetworkErrorFirstOrderHandler;
        public void SetNetworkErrorHandler(Func<int, IEnumerator> handler)
        {
            mNetworkErrorFirstOrderHandler = handler;
        }

        private static void AppendPlatformHeader(Dictionary<string, string> headers)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            // if runnign in editor, use server platform by default
            headers[kHeaderKeyPlatform] = "client";
#elif UNITY_IOS
            headers["platform"] = "ios";
#elif UNITY_ANDROID
            headers["platform"] = "android";
#endif
        }

        // hidden public constructor
        private XCore() { }

        public static XCore New(string urlEndpoint)
        {
            var inst = new XCore();
            inst.apiURLEndpoint = urlEndpoint;
            return inst;
        }

        public static XCore FromConfig(IXAPIConfig cfg)
        {
            var inst = new XCore();
            inst.apiURLEndpoint = cfg.URLEndPoint();
            //            Debug.Log($"set apiURLEndPoint: {inst.apiURLEndpoint}");
            return inst;
        }

        private IEnumerator processRequest(string apiPath,
            Dictionary<string, string> headers,
            Action<UnityWebRequest> callback,
            int apiTrackCode,
            Func<string, UnityWebRequest> requestBuilder)
        {

            while (apiPath.StartsWith("/"))
            {
                apiPath = apiPath.Remove(0, 1);
            }
            string url = string.Format("{0}/{1}", apiURLEndpoint, apiPath);
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }

            // Clean up and set SessionToken
            string sessionToken = XSession.Current() != null ? XSession.Current().Token() : "";
            if (sessionToken == null)
            {
                sessionToken = "";
            }

            if (sessionToken != "")
            {
                headers[XSession.kHeaderKeyXSessionToken] = sessionToken;
            }


            // platform header
            AppendPlatformHeader(headers);

            UnityWebRequest req = null;
            yield return ProcessWebRequestWithRetry(() =>
            {
                var r = requestBuilder(url);
                foreach (var kvp in headers)
                {
                    r.SetRequestHeader(kvp.Key, kvp.Value);
                }
                return r;
            }, kRetryLimit,
            (req_) => req = req_);

            // handle network error
            if (req.isNetworkError)
            {
                if (mNetworkErrorFirstOrderHandler != null)
                {
                    yield return mNetworkErrorFirstOrderHandler(apiTrackCode);
                }
            }

            callback.Invoke(req);
        }

        public IEnumerator GET(string apiPath,
            Dictionary<string, string> headers,
            Action<UnityWebRequest> callback,
            int apiTrackCode)
        {
            yield return processRequest(
                apiPath: apiPath,
                headers: headers,
                callback: callback,
                apiTrackCode: apiTrackCode,
                requestBuilder: (url) => UnityWebRequest.Get(url));
        }

        public IEnumerator POST(string apiPath,
            WWWForm postData,
            Dictionary<string, string> headers,
            Action<UnityWebRequest> callback,
            int apiTrackCode)
        {

            yield return processRequest(
                apiPath: apiPath,
                headers: headers,
                callback: callback,
                apiTrackCode: apiTrackCode,
                requestBuilder: (url) => UnityWebRequest.Post(url, postData));
        }

        public IEnumerator DELETE(string apiPath,
            Dictionary<string, string> headers,
            Action<UnityWebRequest> callback,
            int apiTrackCode)
        {

            yield return processRequest(
                apiPath: apiPath,
                headers: headers,
                callback: callback,
                apiTrackCode: apiTrackCode,
                requestBuilder: (url) => UnityWebRequest.Delete(url));
        }

        public IEnumerator PUT(string apiPath,
            WWWForm putData,
            Dictionary<string, string> headers,
            Action<UnityWebRequest> callback,
            int apiTrackCode)
        {

            yield return processRequest(
                apiPath: apiPath,
                headers: headers,
                callback: callback,
                apiTrackCode: apiTrackCode,
                requestBuilder: (url) => UnityWebRequest.Put(url, putData.data));
        }

        /// <summary>
        /// Helper functin for Starting Coroutine by XUnityDispatcher
        /// </summary>
        /// <param name="routine"></param>
        /// <returns></returns>
        public static Coroutine S(IEnumerator routine)
        {
            return XUnityDispatcher.CurrentDispatcher().StartCoroutine(routine);
        }

        public static void StopCoroutine(Coroutine coroutine)
        {
            XUnityDispatcher.CurrentDispatcher().StopCoroutine(coroutine);
        }

        public IEnumerator GET<R>(string apiPath, Dictionary<string, string> headers, Action<IWSResponse> callback, int apiTrackCode)
            where R : BaseWSResponse, new()
        {
            UnityWebRequest req = null;
            yield return GET(apiPath, headers, (req_) => req = req_, apiTrackCode);
            yield return postProcessRequest<R>(req, callback);
        }

        public IEnumerator POST<R>(string apiPath, WWWForm postData, Dictionary<string, string> headers, Action<IWSResponse> callback, int apiTrackCode)
            where R : BaseWSResponse, new()
        {

            UnityWebRequest req = null;
            yield return POST(apiPath, postData, headers, (req_) => req = req_, apiTrackCode);
            yield return postProcessRequest<R>(req, callback);
        }

        public IEnumerator DELETE<R>(string apiPath, Dictionary<string, string> headers, Action<IWSResponse> callback, int apiTrackCode)
            where R : BaseWSResponse, new()
        {
            UnityWebRequest req = null;
            yield return DELETE(apiPath, headers, (req_) => req = req_, apiTrackCode);
            yield return postProcessRequest<R>(req, callback);
        }

        public IEnumerator PUT<R>(string apiPath, WWWForm putData, Dictionary<string, string> headers, Action<IWSResponse> callback, int apiTrackCode)
            where R : BaseWSResponse, new()
        {

            UnityWebRequest req = null;
            yield return PUT(apiPath, putData, headers, (req_) => req = req_, apiTrackCode);
            yield return postProcessRequest<R>(req, callback);
        }

        private IEnumerator postProcessRequest<R>(UnityWebRequest req, Action<IWSResponse> callback)
            where R : BaseWSResponse, new()
        {
            R resp;
            if (req.isNetworkError)
            {
                callback?.Invoke(Utility.SimpleNetworkError(req));
                yield break;
            }
            else if (req.isHttpError)
            {
                try
                {
                    resp = new R();
                    if (req.method == "DELETE")
                    {
                        resp.success = false;
                        resp.apiError = new APIError()
                        {
                            errorID = 0,
                            httpStatus = (int)req.responseCode,
                            message = "",
                        };
                        resp.rawResult = "Unity [Delete] method could not get response from http response";
                    }
                    else
                    {
                        resp.ParseFromJsonString(req.downloadHandler.text);
                    }
                    callback?.Invoke(resp);
                }
                catch (Exception)
                {
                    var respAsHttpError = Utility.SimpleUnityWebRequestError(req);
                    callback?.Invoke(respAsHttpError);
                }
                finally
                {
                    req.Dispose();
                }
                yield break;
            }

            if (callback == null)
            {
                req.Dispose();
                yield break;
            }

            resp = new R();
            if (req.method == "DELETE")
            {
                resp.success = true;
                resp.rawResult = "Unity [Delete] method could not get response from http response";
            }
            else
            {
                resp.ParseFromJsonString(req.downloadHandler.text);
            }
            callback?.Invoke(resp);
            req.Dispose();
        }

        private IEnumerator ProcessWebRequestWithRetry(Func<UnityWebRequest> reqGenerator, int retryLimit, Action<UnityWebRequest> callback)
        {
            const string kLogTag = "[WebService::ProcessWebRequestWithRetry]";
            var retry = 0;
            UnityWebRequest req = null;
            DateTime retryOn = DateTime.Now;

            while (true)
            {
                req = reqGenerator();
                yield return req.SendWebRequest();
                if (req.isNetworkError)
                {
                    retry++;
                    if (retry <= retryLimit)
                    {
                        Debug.LogWarning($"{kLogTag} failed to request api: {req.url} method: {req.method} due to error: {req.error}... retry {retry}/{kRetryLimit}");
                    }
                    else
                    {
                        Debug.LogWarning($"{kLogTag} failed to request api: {req.url} method: {req.method} due to error: {req.error}... after retried for {kRetryLimit} times");
                        break;
                    }

                    // add more wait time before retry...
                    var diff = DateTime.Now.Subtract(retryOn);
                    if (diff.TotalSeconds < 1)
                    {
                        yield return new WaitForSeconds(1f - (float)diff.TotalSeconds);
                    }
                    continue;
                }
                break;
            }
            callback?.Invoke(req);
        }

    }
}
