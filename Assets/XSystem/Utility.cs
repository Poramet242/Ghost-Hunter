using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace XSystem {
    public class Utility {
        /// <summary>
        /// Make a simple WWW Error such as 404, 402, 502...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="www"></param>
        /// <returns></returns>
        public static IWSResponse SimpleWWWError(WWW www) {
            var result = new BaseWSResponse();
            result.success = false;
            result.errors = new string[] { www.error };
            result.rawResult = www.text;
            return result;
        }

        /// <summary>
        /// Make a simple WWW Error such as 404, 402, 502...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="www"></param>
        /// <returns></returns>
        public static IWSResponse SimpleUnityWebRequestError(UnityWebRequest req) {
            var result = new BaseWSResponse();
            result.success = false;
            result.errors = new string[] { req.error };
            result.rawResult = req.downloadHandler.text;
            result.apiError = new APIError() { errorID = (int)req.responseCode };
            return result;
        }

        public static IWSResponse SimpleNetworkError(UnityWebRequest req) {
            var result = new NetworkErrorResult();
            result.success = false;
            result.errors = new string[] { req.error };
            if (req.downloadHandler != null) {
                result.rawResult = req.downloadHandler.text;
            }
            return result;
        }

        public static string DateTimeRFC3339ToString(DateTime dateTime) {
            return string.Format("{0}T{1}", 
                dateTime.ToString("yyyy-MM-dd"),
                dateTime.ToString("HH:mm:sszzz"));
        }

        public static DateTime ParseDatetime(string dateTimeString) {
            string cleanedDateString = dateTimeString;

            if (dateTimeString.Contains(".")) {
                string[] p = dateTimeString.Split('.');
                char utcSign = '\0';
                if (p[1].Contains("Z")) {
                    utcSign = 'Z';
                }
                else if (p[1].Contains("+")) {
                    utcSign = '+';
                }
                else if (p[1].Contains("-")) {
                    utcSign = '-';
                }

                string ms = "";
                if (utcSign != '\0') {
                    string[] q = p[1].Split(utcSign);
                    ms = q[0];
                }
                else {
                    ms = p[1];
                }

                while (ms.Length != 6) {
                    if (ms.Length > 6) {
                        ms = ms.Substring(0, 6);
                    }
                    else if (ms.Length < 6) {
                        ms += "0";
                    }
                }
                cleanedDateString = p[0] + "." + ms;

                if (utcSign != '\0') {
                    cleanedDateString += utcSign + p[1].Substring(p[1].IndexOf(utcSign) + 1);
                }
            }

            return DateTime.Parse(cleanedDateString);
        }
    }

}