using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XSystem.Test {

    public class TestXUser : MonoBehaviour {
        XCore mXCoreInstance;

        void Awake() {
            Application.runInBackground = true;
            mXCoreInstance = XCore.FromConfig(XAPIConfig.New(
                host: "http://localhost",
                port: 8080,
                version: "0.0.1",
                versionCode: 1));
            XUnityDispatcher.Initialize();
        }
        
        // Use this for initialization
        IEnumerator Start() {
            Debug.Log("Begin test case #1: Test_NormalLoginFlow...");
            Debug.Log("========================================");
            yield return StartCoroutine(Test_NormalLoginFlow());
            
            Debug.Log("Begin test case #2: Test_RestoreLoginFromSessionToken...");
            Debug.Log("========================================");
            yield return StartCoroutine(Test_RestoreLoginFromSessionToken());
            
            Debug.Log("Tearing down");
            Debug.Log("========================================");
        }


        IEnumerator Test_NormalLoginFlow() {
            IWSResponse result = null;

            string username = System.Guid.NewGuid().ToString().Split('-')[0];
            string email = $"{username}@gmail.com";
            string password = "my-password-01";

            // 1. Sign up
            Debug.LogFormat("begin signup with email:{0} password:{1}", email, password);
            yield return XUser.SignUp(mXCoreInstance, email, password,"Test User",
                (r) => {
                    Debug.LogFormat("signup finished");
                    result = r;
                }
            );
            if (result.Success() == false) {
                Debug.LogErrorFormat("signup failed: {0}", result.ErrorsString());
                yield break;
            }
            SignUpResult signUpResult = result as SignUpResult;
            Debug.LogFormat("signup success with email: {0}", signUpResult.user.email);


            // 2. Login
            Debug.LogFormat("begin logging in with email:{0} password:{1}", email, password);
            yield return XUser.Login(mXCoreInstance, email, password,
                (r) => {
                    result = r;
                }
            );
            if (result.Success() == false) {
                Debug.LogErrorFormat("login failed: {0}", result.ErrorsString());
                yield break;
            }
            
            LoginResult loginResult = result as LoginResult;
            Debug.LogFormat("login success with user: {0} sessionToken: {1}", loginResult.user.username, loginResult.sessionToken);

            // 3. Sync user data
            XUser me = XUser.CurrentUser();
            bool syncUserSuccess = false;
            Debug.Log("sync user data");
            yield return me.SyncUserData(mXCoreInstance,
                (r) => {
                    syncUserSuccess = r;
                });
            if (!syncUserSuccess) {
                Debug.LogError("syncUserData failed!!");
                yield break;
            }
            Debug.LogFormat("login success with user: {0}", XUser.CurrentUser().username);
            Debug.LogFormat("session token: {0}", XSession.Current().Token());

            // 4. Logout
            yield return XUser.Logout(mXCoreInstance,
                (r) => {
                    result = r;
                });
            if (result.Success() == false) {
                Debug.LogErrorFormat("logout failed: {0}", result.ErrorsString());
                yield break;
            }
            Debug.LogFormat("logout success");

            // 5. Test Session is perished after logout
            if (XSession.Current() != null) {
                Debug.LogErrorFormat("session is not null after logged out... [failed]");
                yield break;
            }

            // 6. Test SyncUserData after logging out is not allowed
            syncUserSuccess = false;
            Debug.Log("sync user data");
            yield return me.SyncUserData(mXCoreInstance, (r) => syncUserSuccess = r);
            if (syncUserSuccess) {
                Debug.LogError("After logout SyncUserData must fail!!");
                yield break;
            }

            // 7. Done - all cases success
            Debug.Log("<color=green>Test... all passed</color>");
        }

        /// <summary>
        /// Test step:
        /// 1. Signup a new user
        /// 2. Log in
        /// 3. Keep the session token
        /// 4. Restore login
        /// 5. Log out
        /// 6. If Restore login again, it will fail because session is invalid since last logout
        /// </summary>
        /// <returns></returns>
        IEnumerator Test_RestoreLoginFromSessionToken() {
            IWSResponse result = null;

            string username = System.Guid.NewGuid().ToString().Split('-')[0];
            string email = $"{username}@gmail.com";
            string password = "my-password-01";

            // 1. Sign up
            Debug.LogFormat("begin signup with email:{0} password:{1}", email, password);
            yield return XUser.SignUp(mXCoreInstance, email, password,"Test User",
                (r) => {
                    Debug.LogFormat("signup finished");
                    result = r;
                }
            );
            if (result.Success() == false) {
                Debug.LogErrorFormat("signup failed: {0}", result.ErrorsString());
                yield break;
            }
            SignUpResult signUpResult = result as SignUpResult;
            Debug.LogFormat("signup success with email: {0}", signUpResult.user.email);


            // 2. Login
            Debug.LogFormat("begin logging in with email:{0} password:{1}", username, password);
            yield return XUser.Login(mXCoreInstance, email, password, (r) => result = r);
            if (result.Success() == false) {
                Debug.LogErrorFormat("login failed: {0}", result.ErrorsString());
                yield break;
            }

            LoginResult loginResult = result as LoginResult;
            Debug.LogFormat("login success with user: {0} sessionToken: {1}", loginResult.user.username, loginResult.sessionToken);


            // 3. Keep Session token
            string savedSessionToken = XSession.Current().Token();


            // 4. Restore login with session token. First, w have to force current user logout (locally) and clear session 
            // to simulate situation loke user just launch the application
            XUser.CurrentUser().__tst__simulate_logout_locally__();
            XSession.Clear();

            yield return XUser.RestoreSession(mXCoreInstance, savedSessionToken,
                (r) => {
                    result = r;
                });
            if (result.Success() == false) {
                Debug.LogErrorFormat("cannot restore login with session token due to error: {0}", result.ErrorsString());
                yield break;
            }

            // check fail cases
            if (XUser.CurrentUser() == null) {
                Debug.LogErrorFormat("restore login user with session token: {0}... failed CurrentUser is null", savedSessionToken);
                yield break;
            }
            if (XSession.Current() == null) {
                Debug.LogErrorFormat("restore login user with session token: {0}... failed CurrentSession is null", savedSessionToken);
                yield break;
            }
            if (XUser.CurrentUser().email != email) {
                Debug.LogErrorFormat("restore login user with session token: {0}... failed username not match : {1} <=> {2}", savedSessionToken, XUser.CurrentUser().email, email);
                yield break;
            }
            if (XSession.Current().Token() != savedSessionToken) {
                Debug.LogErrorFormat("restore login user with session token: {0}... failed session token not match", savedSessionToken);
                yield break;
            }

            // 5. Logout
            yield return XUser.Logout(mXCoreInstance, (r) => result = r );
            if (result.Success() == false) {
                Debug.LogErrorFormat("logout failed: {0}", result.ErrorsString());
                yield break;
            }
            Debug.LogFormat("logout success");


            // 6. Restore login with session token, but this time, it should FAIL. 
            // Because user intentionally logged out before, so session (on server) was already marked as invalid
            yield return XUser.RestoreSession(mXCoreInstance, savedSessionToken, (r) => result = r );
            if (result.Success() == true) {
                Debug.LogErrorFormat("this cound not happen, because he was already logged out. So the given session token {0} must be invalid", savedSessionToken);
                yield break;
            }
            
            // success
            Debug.Log("<color=green>Test... all passed</color>");
        }
    }
}