using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CouponHistoryDetail", menuName = "ScriptableObjects/CouponHistoryDetail", order = 6)]
public class CouponHistoryDetail : ScriptableObject
{
    [Header("Data")]
    [SerializeField] public string _shopId;
    [SerializeField] public bool _isUse;
    [SerializeField] public ShopItemCategory _shopType;
    [SerializeField] public string[] _image_URL_icon;
    [SerializeField] public Sprite[] _shop_icon;
    [SerializeField] public string _shopName;
    [SerializeField] public string _shopDescription;
    [SerializeField] public string _shopInfo;
    [SerializeField] public string _couponId;
    [SerializeField] public int _priceItem;
    [SerializeField] public DateTime expiredOn;
}
