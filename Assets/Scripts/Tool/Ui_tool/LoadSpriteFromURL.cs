using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class LoadSpriteFromURL : MonoBehaviour
{
    public static LoadSpriteFromURL instance;
    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    [SerializeField] public StorageContainerManager storageCacheManager;

    public void getDataToLoadSpriteFromURL(string url, string nameItem, Action<Sprite> onComplete)
    {
        StartCoroutine(setLoadSpriteFromURL(url, nameItem, onComplete));
    }
    public IEnumerator setLoadSpriteFromURL(string url, string cacheKey, Action<Sprite> onComplete)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                WarningDisplay.instance.setupWarningDisplay("เกิดข้อผิดพลาด", "" + "เกิดข้อผิดพลาดขณะเรียกข้อมูลจากเซิร์ฟเวอร์\r\nกรุณาลองใหม่อีก", WarningType.ErrorServer);
                Debug.LogError("Error while downloading sprite: " + request.isNetworkError);
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

                // Save the downloaded sprite to cache
                storageCacheManager.SaveSpriteToCache(cacheKey, sprite);

                onComplete?.Invoke(sprite); // Use the callback to pass the loaded sprite
            }
        }
    }
}
