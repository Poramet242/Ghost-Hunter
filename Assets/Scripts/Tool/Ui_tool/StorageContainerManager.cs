using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StorageContainerManager : MonoBehaviour
{
    public void SaveSpriteToCache(string cacheKey,Sprite sprite)
    {
        if (sprite == null)
        {
            Debug.LogWarning("Cannot save null sprite to cache.");
            return;
        }

        Texture2D texture = sprite.texture;
        byte[] bytes = texture.EncodeToPNG(); // Encode the texture as PNG bytes

        string cachePath = Path.Combine(Application.temporaryCachePath, cacheKey);
        File.WriteAllBytes(cachePath, bytes);

        Debug.Log("Sprite saved to cache: " + cacheKey);
    }
    public Sprite LoadSpriteFromCache(string cacheKey)
    {
        string cachePath = Path.Combine(Application.temporaryCachePath, cacheKey);
        if (!File.Exists(cachePath))
        {
            Debug.LogWarning("Sprite not found in cache: " + cacheKey);
            return null;
        }

        byte[] bytes = File.ReadAllBytes(cachePath);
        Texture2D texture = new Texture2D(2, 2); // Create a new Texture2D to load the image
        texture.LoadImage(bytes); // Load the image from the bytes

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f); // Use the center of the image as the pivot

        Sprite sprite = Sprite.Create(texture, rect, pivot);

        Debug.Log("Sprite loaded from cache: " + cacheKey);
        Debug.Log("cachePath => " + cachePath);
        return sprite;
    }
    public void ClearCache(Action callback)
    {
        string cacheDirectory = Application.temporaryCachePath;

        if (Directory.Exists(cacheDirectory))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(cacheDirectory);
            FileInfo[] cacheFiles = directoryInfo.GetFiles();

            foreach (FileInfo file in cacheFiles)
            {
                file.Delete();
            }

            Debug.Log("Cache cleared.");
        }
        else
        {
            Debug.Log("Cache directory not found.");
        }
        callback?.Invoke();
    }
}
