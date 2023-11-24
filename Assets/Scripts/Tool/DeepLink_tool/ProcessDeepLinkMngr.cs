using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XSystem;
using ZXing.Aztec.Internal;

public class ProcessDeepLinkMngr : MonoBehaviour
{
    public static ProcessDeepLinkMngr instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Deep link to get token")]
    [SerializeField] private string _deeplinkURL;
    [SerializeField] public string _tokenDeeplink;
    public void DeepLinkActiveted()
    {
        Application.deepLinkActivated += onDeepLinkActivated;
        if (!string.IsNullOrEmpty(Application.absoluteURL))
        {
            // Cold start and Application.absoluteURL not null so process Deep Link.
            onDeepLinkActivated(Application.absoluteURL);
        }
        // Initialize DeepLink Manager global variable.
        else
        {
            _deeplinkURL = "[none]";
        }
    }
    private void onDeepLinkActivated(string url)
    {
        // Update DeepLink Manager global variable, so URL can be accessed from anywhere.
        _deeplinkURL = url;
        Debug.Log("Token {1} =>" + url);
        // Decode the URL to determine action. 
        int tokenIndex = url.IndexOf("token=");
        if (tokenIndex != -1)
        {
            string tokenAndParameters = url.Substring(tokenIndex);
            tokenAndParameters = tokenAndParameters.Substring(6);
            _tokenDeeplink = tokenAndParameters;
            Debug.Log("Token {2} =>"+ _tokenDeeplink);

            StartCoroutine(setToken(_tokenDeeplink));
        }
        else
        {
            Debug.Log("Token {2} => Error");
            WarningDisplay.instance.setupWarningDisplay("เกิดข้อผิดพลาด", "" + "เกิดข้อผิดพลาดขณะเรียกล็อคอิน\r\nกรุณาลองใหม่อีก", WarningType.ErrorServer);
            Debug.Log("Token not found in the URL.");
        }
       
    }
    IEnumerator setToken(string token)
    {
        IWSResponse response = null;
        yield return XUser.FlexAuth(XCoreManager.instance.mXCoreInstance, token, (r) => response = r);
        if (!response.Success())
        {
            //UiLoginDisplay.instance.faceBack_panel.SetActive(false);
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            WarningDisplay.instance.setupWarningDisplay("เกิดข้อผิดพลาด", "" + "เกิดข้อผิดพลาดในการดึงข้อมูลล็อคอินจากเซิร์ฟเวอร์\r\nกรุณาล็อคอินใหม่อีกครั้ง", WarningType.ErrorServer);
            yield break;
        }
        var userLogin = response as LoginResult;
        PlayerPrefs.SetString("sessionToken", userLogin.sessionToken);
        Debug.Log("Token {4} =>" + userLogin.sessionToken);
        //-----------------------------------------------
        //UiLoginDisplay.instance.faceBack_panel.SetActive(false);
        UiLoginDisplay.instance.setLoadingDisplay();
    }
}
