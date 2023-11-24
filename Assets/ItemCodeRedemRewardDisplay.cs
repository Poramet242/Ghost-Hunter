using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using XSystem;
using ZXing;
using ZXing.QrCode;

public class ItemCodeRedemRewardDisplay : MonoBehaviour
{
    public static ItemCodeRedemRewardDisplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Data Code")]
    [SerializeField] public CouponHistoryDetail iscoupon;
    [SerializeField] public GameObject concertWarning_bar;
    [SerializeField] public GameObject _codeTextPrefab;
    [SerializeField] public GameObject thisObject;
    [Header("Display Shop")]
    [SerializeField] public GameObject showItemShopCode_bar;
    [SerializeField] public Transform containerItemShop;
    [Header("Display Concert")]
    [SerializeField] public GameObject showItemConcertCode_bar;
    [SerializeField] public Transform containerConcert;
    [Header("QRCode Display")]
    [SerializeField] public GameObject showItemQRCode_bar;
    [SerializeField] public Image _qrCode_img;
    [SerializeField] public GameObject nomel_obj;
    [SerializeField] public GameObject molto_obj;
    [Header("Redeem Warning")]
    [SerializeField] public GameObject RedeemWarning_bar;
    [SerializeField] public Text _redeemWarningText;

    public void setupItemCodeDisplay(CouponHistoryDetail coupon)
    {
        thisObject.SetActive(true);
        if (iscoupon._shopType == ShopItemCategory.Entertainment)
        {
            showItemConcertCode_bar.SetActive(true);
            ShowConcertCode(coupon._couponId, containerConcert);
        }
        else
        {
            if (coupon._couponId.Contains("http") || coupon._couponId.Contains("HTTP"))
            {
                showItemQRCode_bar.SetActive(true);
                StartCoroutine(setLoadSpriteFromURL(coupon._couponId, (sprite) => 
                {
                    _qrCode_img.sprite = sprite;
                    _qrCode_img.gameObject.SetActive(true);
                    if (iscoupon._shopId.Contains("molto"))
                    {
                        nomel_obj.SetActive(false);
                        molto_obj.SetActive(true);
                    }
                    else
                    {
                        nomel_obj.SetActive(true);
                        molto_obj.SetActive(false);
                    }
                }));
            }
            else
            {
                showItemShopCode_bar.SetActive(true);
                ShowConcertCode(coupon._couponId, containerItemShop);
            }
        }
    }
    public void ShowConcertCode(string code,Transform barDisplay)
    {
        var codeChars = code.ToCharArray();
        foreach (Transform codeChar in barDisplay)
        {
            Destroy(codeChar.gameObject);
        }

        for (int i = 0; i < codeChars.Length; i++)
        {
            GameObject codePrefab = Instantiate(_codeTextPrefab, barDisplay);
            Text codeTxt = codePrefab.GetComponentInChildren<Text>();
            codeTxt.text = codeChars[i].ToString();
        }
    }
    public IEnumerator setLoadSpriteFromURL(string url, Action<Sprite> onComplete)
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
                onComplete?.Invoke(sprite); // Use the callback to pass the loaded sprite
            }
        }
    }
    public void closeThisBar()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        StartCoroutine(setIsUseCoupon(() => 
        {
            showItemShopCode_bar.SetActive(false);
            showItemConcertCode_bar.SetActive(false);
            showItemQRCode_bar.SetActive(false);
            concertWarning_bar.SetActive(false);
            thisObject.SetActive(false);
        }));
    }
    IEnumerator setIsUseCoupon(Action callback)
    {
        IWSResponse response = null;
        yield return UserShopItem.UseItemCode(XCoreManager.instance.mXCoreInstance, iscoupon._couponId, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error set use coupon");
            yield break;
        }
        CouponStorageController.instance.clearDataCouponStorage();
        CouponStorageController.instance.setupDataCouponStorage();
        callback?.Invoke();
    }
    public void opneRewarningBar()
    {
        RedeemWarning_bar.SetActive(true);
        if (iscoupon._shopType == ShopItemCategory.WelcomeReward)
        {
            _redeemWarningText.text = "หากใช้งานแล้วคูปองจะหายไป โปรดใช้งานคูปองของคุณ\r\nเมื่ออยู่ต่อหน้าพนักงานเท่านั้น\r\n<color=#FF5858>สามารถใช้คูปองได้ภายในวันที่ 31 ต.ค. 2566</color>";
        }
        else
        {
            _redeemWarningText.text = "หากใช้งานแล้วคูปองจะหายไป โปรดใช้งานคูปองของคุณ\r\nเมื่ออยู่ต่อหน้าพนักงานเท่านั้น\r\n<color=#FF5858>คูปองอยู่ได้ 7 วันหากไม่มีการใช้งาน</color>";
        }
    }
    public void closeRewarningBar(bool confirm)
    {
        if (confirm)
        {
            RedeemWarning_bar.SetActive(false);
            setupItemCodeDisplay(iscoupon);
        }
        else 
        { 
            RedeemWarning_bar.SetActive(false);
        }
    }
}
