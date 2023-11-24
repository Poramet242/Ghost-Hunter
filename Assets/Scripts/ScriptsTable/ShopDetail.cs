using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ShopDetail", menuName = "ScriptableObjects/ShopDetail", order = 2)]
public class ShopDetail : ScriptableObject
{
    [Header("Open")]
    [SerializeField] public bool _isPartner;
    [Header("Data")]
    [SerializeField] public string _shopId;
    [SerializeField] public bool _isCanRedeem;
    [SerializeField] public bool _isHighlight;
    [SerializeField] public bool alreadyBuy;
    [SerializeField] public ShopItemCategory _shopType;
    [SerializeField] public string[] _image_URL_icon;
    [SerializeField] public Sprite[] _shop_icon;
    [SerializeField] public string _shopName;
    [SerializeField] public string _shopOffset;
    [SerializeField] public string _shopDescription;
    [SerializeField] public string _shopInfo;
    [SerializeField] public int _priceItem;
    [SerializeField] public DateTime expiredOn;
    [SerializeField] public DateTime startedOn;
}
