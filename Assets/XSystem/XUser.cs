using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using JSONLib = SimpleJSON;

namespace XSystem
{

    public partial class XUser : XModelBase
    {
        public string name;
        public string username;
        public string email;

        private static XUser s_currentUser;
        public static XUser CurrentUser()
        {
            return s_currentUser;
        }

        private bool mHasAuthenticated;
        public bool HasAuthenticated()
        {
            return mHasAuthenticated;
        }

        public XUser()
        {
            name = "";
            username = "";
            email = "";
            mHasAuthenticated = false;
        }

        public override void ParseFromJSONObject(JSONLib.JSONObject jObj)
        {
            this.name = jObj["name"].Value;
            this.username = jObj["username"].Value;
            this.email = jObj["email"].Value;
        }

        public static IEnumerator SignUp(XCore xcoreInst, string email, string password, string username, Action<IWSResponse> callback)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("password", password);
            form.AddField("username", username);

            yield return xcoreInst.POST<SignUpResult>(
                apiPath: "api/v1/signup",
                postData: form,
                headers: null,
                callback: callback,
                apiTrackCode: XAPITrackCode.SignUp);
        }

        public static IEnumerator Login(XCore xcoreInst, string email, string password, Action<IWSResponse> callback)
        {
            WWWForm form = new WWWForm();
            form.AddField("id", email);
            form.AddField("password", password);

            IWSResponse result = null;
            yield return xcoreInst.POST<LoginResult>(
                apiPath: "api/v1/signin",
                postData: form,
                headers: null,
                callback: (r) => result = r,
                apiTrackCode: XAPITrackCode.LogIn
            );

            if (!result.Success())
            {
                callback?.Invoke(result);
                yield break;
            }

            var loginResult = result as LoginResult;

            s_currentUser = loginResult.user;
            s_currentUser.mHasAuthenticated = true;

            // save session
            XSession.SaveCurrentSession(loginResult.sessionToken);
            PlayerPrefs.SetString("sessionToken", loginResult.sessionToken);
            callback?.Invoke(result);
        }

        public static IEnumerator GuestRegister(XCore xcoreInst, Action<IWSResponse> callback)
        {
            WWWForm form = new WWWForm();

            IWSResponse result = null;
            yield return xcoreInst.POST<LoginResult>(
                apiPath: "api/v1/signin/guest",
                postData: form,
                headers: null,
                callback: (r) => result = r,
                apiTrackCode: XAPITrackCode.SignUp
            );

            if (!result.Success())
            {
                callback?.Invoke(result);
                yield break;
            }

            var loginResult = result as LoginResult;

            s_currentUser = loginResult.user;
            s_currentUser.mHasAuthenticated = true;

            // save session
            XSession.SaveCurrentSession(loginResult.sessionToken);
            PlayerPrefs.SetString("sessionToken", loginResult.sessionToken);

            callback?.Invoke(result);
        }

        /// <summary>
        /// [Please Note]
        /// RetoreSession will take the given session token, verify it with server.
        /// If session is valid (user has logged in). It will login with that session's user automatically.
        /// Please note if you call this function while the current user still log in, he will be logged out before.
        /// </summary>
        /// <param name="sessionToken"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static IEnumerator RestoreSession(XCore xcoreInst, string sessionToken, Action<IWSResponse> callback)
        {
            const string kLogTag = "[XUser::RestoreSession]";

            IWSResponse result = null;
            XSession session = XSession.FromSessionToken(sessionToken);

            // check if session is identical to the current one. skip the process
            if (XSession.Current() != null && XSession.Current().IsIdentical(session))
            {
                callback?.Invoke(new BaseWSResponse()
                {
                    success = true,
                    errors = new string[] { },
                    rawResult = ""
                });
                yield break;
            }

            // validate session with server, if session is invalid, calback return success as false
            yield return session.ValidateSession(xcoreInst, (r) => result = r);
            if (!result.Success())
            {
                callback?.Invoke(result);
                yield break;
            }

            callback(new BaseWSResponse()
            {
                success = true,
                rawResult = ""
            });

            /*var validateSessionResult = result as ValidateSessionResult;
            if (!validateSessionResult.valid)
            {
                callback(new BaseWSResponse()
                {
                    success = false,
                    errors = new string[] { $"session {sessionToken} is invalid with reason: {validateSessionResult}" },
                    rawResult = ""
                });
                yield break;
            }*/

            // try to logout the current user
            if (XUser.CurrentUser() != null && XUser.CurrentUser().HasAuthenticated())
            {
                Debug.Log($"{kLogTag} preparing to log out the current user...");
                yield return XUser.Logout(xcoreInst, callback: (r) => result = r);
                if (!result.Success())
                {
                    callback?.Invoke(result);
                    yield break;
                }
            }

            // save current session
            XSession.SaveCurrentSession(sessionToken);

            // then sync user
            /*XUser user = new XUser();
            bool syncSuccess = false;
            yield return XCore.S(user.SyncUserData(xcoreInst, callback: r => syncSuccess = r));
            if (!syncSuccess)
            {
                XSession.Clear();
                callback(new BaseWSResponse()
                {
                    success = false,
                    errors = new string[] { "sync user failed" },
                    rawResult = ""
                });
                yield break;
            }

            // set current authenticate user
            s_currentUser = user;
            s_currentUser.mHasAuthenticated = true;*/

            callback?.Invoke(new BaseWSResponse()
            {
                success = true,
                errors = new string[] { },
                rawResult = ""
            });
        }

        public static IEnumerator Logout(XCore xcoreInst, Action<IWSResponse> callback)
        {
            /*if (s_currentUser == null)
            {
                throw new InvalidOperationException("Cannot logout, You have not signed in");
            }*/
            if (XSession.Current() == null)
            {
                throw new InvalidOperationException("Invalid Session");
            }

            IWSResponse result = null;
            var formData = new WWWForm();
            formData.AddField("dummy", "");
            yield return xcoreInst.POST<LogoutResult>(
                apiPath: "api/v1/signout",
                postData: formData,
                headers: null,
                callback: (r) => result = r,
                apiTrackCode: XAPITrackCode.Logout
            );

            if (!result.Success())
            {
                callback?.Invoke(result);
                yield break;
            }

            LogoutResult logoutResult = result as LogoutResult;

            //s_currentUser.mHasAuthenticated = false;
            //s_currentUser = null;
            XSession.Clear();
            callback?.Invoke(result);
        }

        public static IEnumerator FacebookAuth(XCore xcoreInst, string accessToken, Action<IWSResponse> callback)
        {
            var header = new Dictionary<string, string>();
            header["Authorization"] = accessToken;

            var formData = new WWWForm();
            //formData.AddField("accessToken", accessToken);

            IWSResponse result = null;
            yield return xcoreInst.POST<LoginResult>(
                apiPath: "/api/v1/signin/social?social=facebook",
                headers: header,
                postData: formData,
                callback: (r) => result = r,
                apiTrackCode: XAPITrackCode.LogIn);

            if (!result.Success())
            {
                callback?.Invoke(result);
                yield break;
            }

            var loginResult = result as LoginResult;

            s_currentUser = loginResult.user;
            s_currentUser.mHasAuthenticated = true;

            // save session
            XSession.SaveCurrentSession(loginResult.sessionToken);
            PlayerPrefs.SetString("sessionToken", loginResult.sessionToken);

            callback?.Invoke(result);

        }

        public static IEnumerator GoogleAuth(XCore xcoreInst, string accessToken, Action<IWSResponse> callback)
        {
            var header = new Dictionary<string, string>();
            header["Authorization"] = accessToken;

            var formData = new WWWForm();
            //formData.AddField("accessToken", accessToken);

            IWSResponse result = null;
            yield return xcoreInst.POST<LoginResult>(
                apiPath: "/api/v1/signin/social?social=google",
                headers: header,
                postData: formData,
                callback: (r) => result = r,
                apiTrackCode: XAPITrackCode.LogIn);

            if (!result.Success())
            {
                callback?.Invoke(result);
                yield break;
            }

            var loginResult = result as LoginResult;

            s_currentUser = loginResult.user;
            s_currentUser.mHasAuthenticated = true;

            // save session
            XSession.SaveCurrentSession(loginResult.sessionToken);
            PlayerPrefs.SetString("sessionToken", loginResult.sessionToken);

            callback?.Invoke(result);

        }

        public static IEnumerator AppleAuth(XCore xcoreInst, string idToken, Action<IWSResponse> callback)
        {
            var header = new Dictionary<string, string>();
             header["Authorization"] = idToken;

            var formData = new WWWForm();
           // formData.AddField("idToken", idToken);

            IWSResponse result = null;
            yield return xcoreInst.POST<LoginResult>(
                apiPath: "/api/v1/signin/social?social=apple",
                headers: header,
                postData: formData,
                callback: (r) => result = r,
                apiTrackCode: XAPITrackCode.LogIn);

            if (!result.Success())
            {
                callback?.Invoke(result);
                yield break;
            }

            var loginResult = result as LoginResult;

            s_currentUser = loginResult.user;
            s_currentUser.mHasAuthenticated = true;

            // save session
            XSession.SaveCurrentSession(loginResult.sessionToken);
            PlayerPrefs.SetString("sessionToken", loginResult.sessionToken);
            

            callback?.Invoke(result);

        }

        public static IEnumerator FlexAuth(XCore xcoreInst, string idToken, Action<IWSResponse> callback)
        {
            var header = new Dictionary<string, string>();
             header["Authorization"] = idToken;

            var formData = new WWWForm();
           // formData.AddField("idToken", idToken);

            IWSResponse result = null;
            yield return xcoreInst.POST<LoginResult>(
                apiPath: "/api/v1/signin/flex",
                headers: header,
                postData: formData,
                callback: (r) => result = r,
                apiTrackCode: XAPITrackCode.LogIn);

            if (!result.Success())
            {
                callback?.Invoke(result);
                yield break;
            }

            var loginResult = result as LoginResult;

            s_currentUser = loginResult.user;
            s_currentUser.mHasAuthenticated = true;

            // save session
            XSession.SaveCurrentSession(loginResult.sessionToken);
            PlayerPrefs.SetString("sessionToken", loginResult.sessionToken);
            

            callback?.Invoke(result);

        }

        /*public static IEnumerator TokenAuth(XCore xcoreInst, string walletAddress, Action<IWSResponse> callback)
        {
            var header = new Dictionary<string, string>();
            header["Authorization"] = walletAddress;


            var formData = new WWWForm();

            IWSResponse result = null;
            yield return xcoreInst.POST<LoginResult>(
                apiPath: "/api/v1/auth/wallet_login",
                headers: header,
                postData: formData,
                callback: (r) => result = r,
                apiTrackCode: XAPITrackCode.LogIn);

            if (!result.Success())
            {
                callback?.Invoke(result);
                yield break;
            }

            var loginResult = result as LoginResult;

            s_currentUser = loginResult.user;
            s_currentUser.mHasAuthenticated = true;

            // save session
            XSession.SaveCurrentSession(loginResult.sessionToken);
            PlayerPrefs.SetString("sessionToken", loginResult.sessionToken);

            callback?.Invoke(result);

        }*/

        public static IEnumerator FacebookLink(XCore xcoreInst, string accessToken, Action<IWSResponse> callback)
        {
            var header = new Dictionary<string, string>();
            header["X-Session-Token"] = XSession.Current().Token();

            var formData = new WWWForm();
            formData.AddField("accessToken", accessToken);

            IWSResponse result = null;
            yield return xcoreInst.POST<LoginResult>(
                apiPath: "/api/v1/auth/fbLink",
                headers: header,
                postData: formData,
                callback: (r) => result = r,
                apiTrackCode: XAPITrackCode.LogIn);

            if (!result.Success())
            {
                callback?.Invoke(result);
                yield break;
            }

            var loginResult = result as LoginResult;

            s_currentUser = loginResult.user;
            s_currentUser.mHasAuthenticated = true;

            // save session
            XSession.SaveCurrentSession(loginResult.sessionToken);
            PlayerPrefs.SetString("sessionToken", loginResult.sessionToken);

            callback?.Invoke(result);

        }

        /// <summary>
        /// SyncData will query user's data from server, and update all properties
        /// [Beaware] call this function when user has authentiucated
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator SyncUserData(XCore xcoreInst, Action<bool> callback)
        {
            IWSResponse result = null;
            yield return xcoreInst.GET<SyncUserDataResult>(
                apiPath: "api/v1/me",
                headers: null,
                callback: (r) => result = r,
                apiTrackCode: XAPITrackCode.SyncUser
            );

            if (!result.Success())
            {
                callback?.Invoke(false);
                yield break;
            }

            var syncUserResult = result as SyncUserDataResult;

            var u = syncUserResult.user;
            this.email = u.email;
            this.name = u.name;
            this.username = u.username;
            callback?.Invoke(true);
        }

        public static IEnumerator ConvertGuestAccountWithSignup(XCore xcoreInst, string email, string password, string username,
                                                                Action<IWSResponse> callback)
        {

            var header = new Dictionary<string, string>();
            header["X-Session-Token"] = XSession.Current().Token();

            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("password", password);
            form.AddField("username", username);

            yield return xcoreInst.POST<SignUpResult>(
                apiPath: "api/v1/auth/register",
                postData: form,
                headers: header,
                callback: callback,
                apiTrackCode: XAPITrackCode.SignUp);
        }



        #region Test function
        public void __tst__simulate_logout_locally__()
        {
            s_currentUser = null;
        }
        #endregion

    }


    #region Result Models

    public class SignUpResult : BaseWSResponse
    {
        public XUser user;

        public override void ParseFromJSONObject(JSONLib.JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            if (!this.success)
            {
                return;
            }

            jObj = jObj["data"].AsObject;
            if (jObj == null)
            {
                return;
            }
            if (this.user == null)
            {
                this.user = new XUser();
            }
            this.user.ParseFromJSONObject(jObj);
        }
    }

    [Serializable]
    public class LoginResult : BaseWSResponse
    {
        public string sessionToken;
        public XUser user;

        public override void ParseFromJSONObject(JSONLib.JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            if (!this.success) return;

            var data = jObj["data"].AsObject;
            this.sessionToken = data["token"];
            if (this.user == null)
            {
                this.user = new XUser();
            }
            this.user.ParseFromJSONObject(data["user"].AsObject);
        }
    }

    [Serializable]
    public class LogoutResult : BaseWSResponse { }

    [Serializable]
    public class LinkWithFacebookResult : BaseWSResponse { }

    [Serializable]
    public class SyncUserDataResult : BaseWSResponse
    {
        public XUser user;

        public override void ParseFromJSONObject(JSONLib.JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            if (!this.success) return;

            var data = jObj["data"].AsObject;
            if (this.user == null)
            {
                this.user = new XUser();
            }
            this.user.ParseFromJSONObject(data["user"].AsObject);
        }
    }

    #endregion

}
