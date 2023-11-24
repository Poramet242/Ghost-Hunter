using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ShoppingDataObject : MonoBehaviour
{
    public static ShoppingDataObject instance;
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
    [Header("Selecte")]
    [SerializeField] public ShopDetail _selectedShopDisplay;
    [Header("Highlight Shop")]
    [SerializeField] public List<ShopDetail> _allHighlightShop_list;
    [Header("Nomal Shop")]
    [SerializeField] public List<ShopDetail> _allNomalShop_list;
    [Header("All Data Shop detail")]
    [SerializeField] public List<ShopDetail> _allShopDisplay;

    public IEnumerator setupShopDetail(ShopItem shopItem , ShopDetail detail, Action callback)
    {
        //LoadSpriteFromURL.instance.storageCacheManager.ClearCache();
        detail.name = shopItem.title;
        detail._shopId = shopItem.itemID;
        detail._shopType = shopItem.category;
        detail._isHighlight = shopItem.isHighlight;
        detail._shopName = shopItem.title;
        detail._shopOffset = shopItem.shopName;
        detail._shopDescription = shopItem.description;
        detail._priceItem = shopItem.price;
        detail.alreadyBuy = shopItem.alreadyBuy;
        detail.expiredOn = shopItem.expiredOn;
        detail.startedOn = shopItem.startedOn;
        //TODO: sag
        detail._isPartner = shopItem.isPartner;
        //load image from URL
        detail._image_URL_icon = new string[shopItem.images.Count];
        detail._shop_icon = new Sprite[shopItem.images.Count];
        if (shopItem.images.Count == 0)
        {
            detail._shop_icon = new Sprite[1];
            detail._shop_icon[0] = LoadUnitObject.instance.RewardDef_spr;
            callback?.Invoke();
        }
        for (int i = 0; i < detail._image_URL_icon.Length; i++)
        {
            int index = i; // Create a local variable to capture the current index
            Sprite cachedSprite = LoadSpriteFromURL.instance.storageCacheManager.LoadSpriteFromCache(shopItem.itemID + "_" + index);
            if (cachedSprite != null)
            {
                // Sprite was found in cache, use it
                //Debug.Log("Using cached sprite || cachekey => " + shopItem.itemID);
                detail._image_URL_icon[index] = shopItem.images[index];
                detail._shop_icon[index] = cachedSprite;
                callback?.Invoke();
            }
            else
            {
                // Sprite not found in cache, download it
                //Debug.Log("Sprite not found in cache, download it");
                yield return LoadSpriteFromURL.instance.setLoadSpriteFromURL(shopItem.images[i], shopItem.itemID + "_" + index, (sprite) =>
                {
                    detail._image_URL_icon[index] = shopItem.images[index];
                    detail._shop_icon[index] = sprite;
                    callback?.Invoke();
                });
            }
        }
    }
    public List<ShopItemCategory> getAllShopUnit()
    {
        List<ShopItemCategory> temp = new List<ShopItemCategory>();
        for (int i = 0; i < _allNomalShop_list.Count; i++)
        {
            if (_allNomalShop_list[i].expiredOn < ItemDataObject.instance.timeNowServer)
            {
                continue;
            }
            else if (_allNomalShop_list[i].startedOn > ItemDataObject.instance.timeNowServer)
            {
                continue;
            }
            else
            {
                if (!temp.Exists(o => o == _allNomalShop_list[i]._shopType))
                {
                    temp.Add(_allNomalShop_list[i]._shopType);
                }
            }
        }
        return temp;
    }
    public string convetString(ShopItemCategory @enum)
    {
        string str = "";
        switch (@enum)
        {
            case ShopItemCategory.Shopping_Food:
                str = "Shopping อาหารและเครื่องดื่ม";
                break;
            case ShopItemCategory.Entertainment:
                str = "สิทธิพิเศษ คอนเสิร์ตและภาพยนตร์";
                break;
            case ShopItemCategory.Beauty:
                str = "การท่องเที่ยวและความงาม";
                break;
            case ShopItemCategory.Other:
                str = "อื่นๆ";
                break;
            case ShopItemCategory.ConcertTickets:
                str = "Concert";
                break;
            case ShopItemCategory.WelcomeReward:
                str = "Welcome Reward";
                break;
        }
        return str;
    }
}
