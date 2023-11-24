using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TestDeeplink : MonoBehaviour
{
    public static TestDeeplink instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("URL link app")]
    public string Package_name = "";
    public string URL_App_ios = "https://apps.apple.com/th/app/flexconnect/id1562305493";
    public string URL_App_android = "https://play.google.com/store/apps/details?id=net.flexconnect.radio";
    [Header("other")]
    [SerializeField] public Transform content_tf;
    [SerializeField] public GameObject prefab_tf;
    [SerializeField] public InputField URL_InputField;

    public string url = "https://flex-ghost-hunting.onelink.me/6e7T?pid=my_media_source&token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJtZW1iZXJJRCI6IjYwMTY1IiwidGltZVN0YW1wIjoiMjAyMy0wOC0yNSAxMDoxNTo0OCIsImV4cCI6MTY5MjkzMzY0OH0.Z1koIyx-6_j6Z55_kaolTkimFZ64Bfxeke1ejRTRJCI";

    public string[] kop;
    private void Start()
    {
        Debug.Log("Original URL => " + url);
        int tokenIndex = url.IndexOf("token=");
        Debug.Log("INDEXFO =>" + tokenIndex + "Count");
        if (tokenIndex != -1)
        {
            string tokenAndParameters = url.Substring(tokenIndex);
            Debug.Log("Parameters sub => " + tokenAndParameters);
            tokenAndParameters = tokenAndParameters.Substring(6);
            Debug.Log("Token => " + tokenAndParameters);
        }
        else
        {
            Debug.Log("Token not found in the URL.");
        }
        /*for (int i = 0; i < kop.Length; i++)
        {
            ProcessString(kop[i]);
        }*/
    }
    public void ProcessString(string inputString)
    {
        if (inputString.Contains("http"))
        {
            Debug.Log("Processing URL: " + inputString);
        }
        else
        {
            Debug.Log("Processing Key: " + inputString);
        }
    }
}
