using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class LoadUnitObject : MonoBehaviour
{

    public static LoadUnitObject instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] public Sprite RewardDef_spr;
    [SerializeField] public Sprite PlayerIconDef_spr;
    public Sprite GetLocalIconMonster(string IconId)
    {
        string basePath = "Gameplay/IconMonster/Frame/" + IconId;
        Sprite sprite = Resources.Load<Sprite>(basePath);
        return sprite;
    }
    public Sprite GetLocalIconMonsterAR(string IconId)
    {
        string basePath = "Gameplay/IconMonster/" + IconId;
        Sprite sprite = Resources.Load<Sprite>(basePath);
        return sprite;
    }

    public GameObject GetMonsterPrefaabs(string nameGhost)
    {
        string basePath = "Gameplay/MonsterPrefabs/" + nameGhost;
        GameObject sprite = Resources.Load<GameObject>(basePath);
        return sprite;
    }
    public GhostController GetMonsterController(string nameGhost)
    {
        string basePath = "Gameplay/MonsterPrefabs/" + nameGhost;
        GhostController sprite = Resources.Load<GhostController>(basePath);
        return sprite;
    }
    public AudioClip GetSoundMonster(string nameGhost)
    {
        string basePath = "Sound/Monster/" + nameGhost;
        AudioClip sound = Resources.Load<AudioClip>(basePath);
        return sound;
    }
    public GameObject GetItemPrefabs(string nameItem)
    {
        string basePath = "Gameplay/Weapon/Potion/" + nameItem;
        GameObject sprite = Resources.Load<GameObject>(basePath);
        return sprite;
    }
    public IEnumerator setLoadSpriteFromURL(string url, Action<Sprite> onComplete)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError("Error while downloading sprite: " + request.isNetworkError);
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                onComplete?.Invoke(sprite); // Use the callback to pass the loaded sprite
            }
        }
    }
}
