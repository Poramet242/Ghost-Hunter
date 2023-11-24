using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSystem;

public class VersionController : MonoBehaviour
{
    [Header("URL store")]
    [SerializeField] private string appStore_ulr = "https://apps.apple.com/th/app/flex-hunter/id6462846997";
    [SerializeField] private string playStore_url = "https://play.google.com/store/apps/details?id=net.flexconnect.ghosthunting";
    [SerializeField] private string oneLink_url = "https://flex-hunter.onelink.me/6e7T/hqsbc92f";
    [Header("Version")]
    [SerializeField] private string IOS_version = "";
    [SerializeField] private string Android_version = "";
    [SerializeField] public float Guest_version;

    //TO USE: set after start login and start game call get version to server
    public IEnumerator checkVersionAPP(Action callback)
    {
        IWSResponse response = null;
#if UNITY_IOS
        yield return GameConfig.GetGameConfig(XCoreManager.instance.mXCoreInstance, "IOS_version", (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            WarningDisplay.instance.setupWarningDisplay("<color=red>" + "ขณะนี้เซิร์ฟเวอร์ ได้ทำการปิดให้บริการ" + "</color>", "ทางทีมงานต้องขอขอบคุณ ทุกๆ ท่านที่ได้มาเข้าร่วมเล่นกิจกรรมในครั้งนี้", WarningType.ErrorUiDisplay);
            yield break;
        }
        var gameConfig = response as GameConfig;
        if (float.Parse(IOS_version) >= float.Parse(gameConfig.value))
        {
            callback?.Invoke();
            yield break;
        }    
        else if (float.Parse(IOS_version) < float.Parse(gameConfig.value))
        {
            LoadSpriteFromURL.instance.storageCacheManager.ClearCache(() => 
            {
                Application.OpenURL(oneLink_url);
            });
            yield break;
        }
#endif
#if UNITY_ANDROID
        yield return GameConfig.GetGameConfig(XCoreManager.instance.mXCoreInstance, "Android_version", (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            WarningDisplay.instance.setupWarningDisplay("<color=red>" + "ขณะนี้เซิร์ฟเวอร์ ได้ทำการปิดให้บริการ" + "</color>", "ทางทีมงานต้องขอขอบคุณ ทุกๆ ท่านที่ได้มาเข้าร่วมเล่นกิจกรรมในครั้งนี้", WarningType.ServerClose);
            yield break;
        }
        var gameConfig = response as GameConfig;
        if (float.Parse(Android_version) >= float.Parse(gameConfig.value))
        {
            callback?.Invoke();
            yield break;
        }    
        else if (float.Parse(Android_version) < float.Parse(gameConfig.value))
        {
            LoadSpriteFromURL.instance.storageCacheManager.ClearCache(() => 
            {
                Application.OpenURL(oneLink_url);
            });
            yield break;
        }
#endif
    }

}
