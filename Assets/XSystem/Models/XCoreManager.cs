using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSystem;

public class XCoreManager : MonoBehaviour
{
    public static XCoreManager instance;
    public XCore mXCoreInstance;
    [SerializeField]
    private string hostUrl = "http://18.143.141.228";
    [SerializeField]
    private int port = 8095;
    [SerializeField]
    private HostType hostType;
    // Start is called before the first frame update
    void Awake()
    {
        switch (hostType){
            case HostType.LocalHost:
            hostUrl = "http://localhost";
            break;
            case HostType.TestServer:
            hostUrl = "http://api.prod.flexhunting.innova.happygocoding.com";
            break;
            case HostType.BataServer:
            hostUrl = "http://18.143.141.228";
            break;
        }



        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        Application.runInBackground = true;
        mXCoreInstance = XCore.FromConfig(XAPIConfig.New(
            host: hostUrl,
            port: port,
            version: "0.0.1",
            versionCode: 1));
        XUnityDispatcher.Initialize();
    }

    enum HostType
    {
        LocalHost, TestServer, BataServer,Other
    }
}
