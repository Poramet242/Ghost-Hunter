using System;
using System.Collections;
using System.Collections.Generic;
using FlexGhost.Models;
using UnityEngine;
using XSystem;

namespace XSystem.Test
{
    public class TestTime : MonoBehaviour
    {
        XCore mXCoreInstance;

        private void Awake()
        {
            Application.runInBackground = true;
            mXCoreInstance = XCore.FromConfig(XAPIConfig.New(
                host: "http://13.229.48.21",
                port: 8095,
                version: "0.0.1",
                versionCode: 1));
            XUnityDispatcher.Initialize();
        }

        // Start is called before the first frame update
        IEnumerator Start()
        {
            IWSResponse response = null;

            ////////////////////USE THIS PART///////////////////////////

            yield return XUser.RestoreSession(mXCoreInstance, "mHlR-eAZ4HFD5x38",
                (r) =>
                {
                    response = r;
                });
            if (response.Success() == false)
            {
                Debug.LogErrorFormat("cannot restore login with session token due to error: {0}", response.ErrorsString());
                Debug.Log(response.RawResult().ToString());
                yield break;
            }

            yield return TimeNow.GetTimeNow(mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            var serverTime = response as TimeNow;
            Debug.Log(serverTime.timeNow);
            XTimeManager.instance.SyncTime(serverTime.timeNow);

            Debug.Log("Server Time : " + serverTime.timeNow);
            Debug.Log("X Time : " + XTimeManager.instance.Now());

            yield return new WaitForSeconds(2);

            yield return TimeNow.GetTimeNow(mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            serverTime = response as TimeNow;
            Debug.Log("Server Time : " + serverTime.timeNow);
            Debug.Log("X Time : " + XTimeManager.instance.Now());

            DateTime testTime_1 = new DateTime(2023, 07, 05, 14, 30, 00);
            DateTime testTime_2 = new DateTime(2023, 07, 05, 18, 30, 00);

            Debug.Log(testTime_1);
            Debug.Log(testTime_2);

            Debug.Log("Time Since Before : "+XTimeManager.instance.TimeSinceSeconds(testTime_1));
            Debug.Log("Time Since After : "+XTimeManager.instance.TimeSinceSeconds(testTime_2));

            Debug.Log("Time Until Before : "+XTimeManager.instance.TimeUntilSeconds(testTime_1));
            Debug.Log("Time Until After : "+XTimeManager.instance.TimeUntilSeconds(testTime_2));


        }
    }
}
