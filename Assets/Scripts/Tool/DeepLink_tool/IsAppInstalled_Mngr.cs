using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAppInstalled_Mngr : MonoBehaviour
{
    public static IsAppInstalled_Mngr instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("App Data")]
    private string appPackageName = "net.flexconnect.radio"; // Replace with the actual app's package name
    private string appScheme = "net.flexconnect.radio"; // Replace with the actual app's URL scheme
    private string webURL = "https://play.google.com/store/apps/details?id=net.flexconnect.radio&hl=en-TH"; // URL to open if the app is not installed
    [Header("Deep link to open other app")]
    [SerializeField] private string deeplinkURL = "";
    public IEnumerator OpenAppOrURL()
    {
        Application.OpenURL(deeplinkURL);
        /*if (Application.platform == RuntimePlatform.Android)
        {
            if (IsAppInstalled(appPackageName))
            {
                //if to have open other app to use function OpenApp(get app scheme or packageName)
                //TODO: open deeplink Android
                OpenWebURL(deeplinkURL);
            }
            else
            {
                OpenWebURL(webURL);
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (CanOpenURL(appScheme))
            {
                //if to have open other app to use function OpenApp(get app scheme or packageName)
                //TODO: open deeplink Iphone
                OpenWebURL(deeplinkURL);
            }
            else
            {
                OpenWebURL(webURL);
            }
        }
        else
        {
            OpenWebURL(webURL);
        }*/
        yield break;
    }

    private bool IsAppInstalled(string packageName)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");

        try
        {
            packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
            return true;
        }
        catch (AndroidJavaException e)
        {
            Debug.LogError("Error checking app installation: " + e.Message);
            return false;
        }
    }

    private bool CanOpenURL(string url)
    {
        return Application.CanStreamedLevelBeLoaded(url);
    }

    private void OpenApp(string scheme)
    {
        //Application.OpenURL(scheme);
        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getPackageManager")
                                                     .Call<AndroidJavaObject>("getLaunchIntentForPackage", scheme);
            currentActivity.Call("startActivity", intent);
        }
        catch (Exception e)
        {
            Debug.LogError("Error opening app: " + e);
        }
    }

    private void OpenWebURL(string url)
    {
        Debug.Log(url);
        url = Application.absoluteURL;
        Debug.Log("Absolute =>"+url);
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
    }
}
