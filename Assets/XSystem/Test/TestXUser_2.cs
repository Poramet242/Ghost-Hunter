using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSystem;

public class TestXUser_2 : MonoBehaviour
{
    XCore mXCoreInstance;

    void Awake()
    {
        Application.runInBackground = true;
        mXCoreInstance = XCore.FromConfig(XAPIConfig.New(
            host: "http://localhost",
            port: 1188,
            version: "0.0.1",
            versionCode: 1));
        XUnityDispatcher.Initialize();
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        //yield return StartCoroutine(Test_NormalLoginFlow());
        yield return StartCoroutine(Test_GuestLoginFlow());

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Test_NormalLoginFlow()
    {
        IWSResponse result = null;

        //string username = System.Guid.NewGuid().ToString().Split('-')[0];
        //string email = $"{username}@gmail.com";
        string email = "test02@gmail.com";
        string password = "my-password-01";

        // 1. Sign up
        /*Debug.LogFormat("begin signup with email:{0} password:{1}", email, password);
        yield return XUser.SignUp(mXCoreInstance, email, password, "Test02 User",
            (r) =>
            {
                Debug.LogFormat("signup finished");
                result = r;
            }
        );
        if (result.Success() == false)
        {
            Debug.LogErrorFormat("signup failed: {0}", result.ErrorsString());
            yield break;
        }
        SignUpResult signUpResult = result as SignUpResult;
        Debug.LogFormat("signup success with email: {0}", signUpResult.user.email);*/


        // 2. Login
        Debug.LogFormat("begin logging in with email:{0} password:{1}", email, password);
        yield return XUser.Login(mXCoreInstance, email, password,
            (r) =>
            {
                result = r;
            }
        );
        if (result.Success() == false)
        {
            Debug.LogErrorFormat("login failed: {0}", result.ErrorsString());
            yield break;
        }

        LoginResult loginResult = result as LoginResult;
        Debug.LogFormat("login success with user: {0} sessionToken: {1}", loginResult.user.username, loginResult.sessionToken);

        Debug.Log(loginResult.sessionToken);

        // 3. Sync user data
        /* XUser me = XUser.CurrentUser();
         bool syncUserSuccess = false;
         Debug.Log("sync user data");
         yield return me.SyncUserData(mXCoreInstance,
             (r) =>
             {
                 syncUserSuccess = r;
             });
         if (!syncUserSuccess)
         {
             Debug.LogError("syncUserData failed!!");
             yield break;
         }
         Debug.LogFormat("login success with user: {0}", XUser.CurrentUser().username);
         Debug.LogFormat("session token: {0}", XSession.Current().Token());*/

        // 4. Logout
        yield return XUser.Logout(mXCoreInstance,
            (r) =>
            {
                result = r;
            });
        if (result.Success() == false)
        {
            Debug.LogErrorFormat("logout failed: {0}", result.ErrorsString());
            yield break;
        }
        Debug.LogFormat("logout success");

        // 5. Test Session is perished after logout
        if (XSession.Current() != null)
        {
            Debug.LogErrorFormat("session is not null after logged out... [failed]");
            yield break;
        }

        // 6. Test SyncUserData after logging out is not allowed
        /* syncUserSuccess = false;
         Debug.Log("sync user data");
         yield return me.SyncUserData(mXCoreInstance, (r) => syncUserSuccess = r);
         if (syncUserSuccess)
         {
             Debug.LogError("After logout SyncUserData must fail!!");
             yield break;
         }*/

        // 7. Done - all cases success
        Debug.Log("<color=green>Test... all passed</color>");
    }

    IEnumerator Test_GuestLoginFlow()
    {
        IWSResponse result = null;


        // 2. Login
        Debug.LogFormat("begin logging in with Guest");
        yield return XUser.GuestRegister(mXCoreInstance,
            (r) =>
            {
                result = r;
            }
        );
        if (result.Success() == false)
        {
            Debug.LogErrorFormat("login failed: {0}", result.ErrorsString());
            yield break;
        }

        LoginResult loginResult = result as LoginResult;
        Debug.LogFormat("login success with user: {0} sessionToken: {1}", loginResult.user.username, loginResult.sessionToken);

        Debug.Log(loginResult.sessionToken);


    }
}


