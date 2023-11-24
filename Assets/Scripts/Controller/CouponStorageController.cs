using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class CouponStorageController : MonoBehaviour
{
    public static CouponStorageController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Panel")]
    public bool uiIsReweed;
    public GameObject storagePanel;
    [SerializeField] public List<GameObject> coupons = new List<GameObject>();
    [SerializeField] public List<GameObject> historys = new List<GameObject>();
    [Header("Data storage list")]
    [SerializeField] public List<CouponHistoryDetail> activeCouponsList = new List<CouponHistoryDetail>();
    [SerializeField] public List<CouponHistoryDetail> historyCouponsList = new List<CouponHistoryDetail>();
    [Header("UI")]
    public Button backBTN;
    [Header("Type Button Group")]
    public Button couponTypeBTN;
    public GameObject lineCoupon;
    public Button historyTypeBTN;
    public GameObject lineHistory;
    [Header("Coupon Container")]
    public GameObject couponDisplay;
    public Transform couponContainer;
    public CouponStoragePrefab couponStoragePrefab;
    [Header("History Container")]
    public GameObject historyDisplay;
    public Transform historyContainer;
    public HistoryStoragePrefab historyStoragePrefab;
    private void OnEnable()
    {
        if (ShopDisplayController.instance.isMain)
        {
            UIManager.instance.menu.SetActive(false);
        }
        setupDataCouponStorage();
    }
    private void OnDisable()
    {
        if (ShopDisplayController.instance.isMain)
        {
            UIManager.instance.menu.SetActive(true);
        }
        if (uiIsReweed)
        {
            RedeemRewardsDisplay.instance.setupPromotionDisplay(RedeemRewardsDisplay.instance._thisPromotion);
        }
        clearDataCouponStorage();
    }
    public void setupDataCouponStorage()
    {
        StartCoroutine(setupCouponStorage(() =>
        {
            SetupCouponStorage();
            SetupHistoryStorage();
        }));
    }
    public void clearDataCouponStorage()
    {
        for (int i = 0; i < coupons.Count; i++)
        {
            Destroy(coupons[i]);
        }
        for (int i = 0; i < historys.Count; i++)
        {
            Destroy(historys[i]);
        }
        coupons.Clear();
        historys.Clear();
        activeCouponsList.Clear();
        historyCouponsList.Clear();
    }
    public void OpenStorage()
    {
        storagePanel.SetActive(true);
        // coupons = 
        // historys = 
        // OpenCouponDisplay();
        // OpenHistoryDisplay();
    }

    public void OpenCouponDisplay()
    {
        clearDataCouponStorage();
        setupDataCouponStorage();
        SoundListObject.instance.onPlaySoundSFX(0);
        lineCoupon.SetActive(true);
        lineHistory.SetActive(false);
        couponDisplay.SetActive(true);
        historyDisplay.SetActive(false);
    }

    public void OpenHistoryDisplay()
    {
        clearDataCouponStorage();
        setupDataCouponStorage();
        SoundListObject.instance.onPlaySoundSFX(0);
        lineCoupon.SetActive(false);
        lineHistory.SetActive(true);
        couponDisplay.SetActive(false);
        historyDisplay.SetActive(true);
    }

    public void SetupCouponStorage()
    {
        for (int i = 0; i < activeCouponsList.Count; i++)
        {
            CouponStoragePrefab coupon = Instantiate(couponStoragePrefab, couponContainer);
            coupon.gameObject.SetActive(true); ;
            coupon.SetupCouponDisplay(activeCouponsList[i]._shop_icon[0], activeCouponsList[i]._shopName, activeCouponsList[i]._shopDescription,
                                      activeCouponsList[i].expiredOn, activeCouponsList[i]._priceItem, activeCouponsList[i]._shopType,
                                      activeCouponsList[i]);
            coupons.Add(coupon.gameObject);
        }
    }

    public void SetupHistoryStorage()
    {
        for (int i = 0; i < historyCouponsList.Count; i++)
        {
            HistoryStoragePrefab history = Instantiate(historyStoragePrefab, historyContainer);
            history.gameObject.SetActive(true);
            history.SetupHistoryDisplay(historyCouponsList[i]._shop_icon[0], historyCouponsList[i]._shopName, historyCouponsList[i]._shopDescription,
                                        historyCouponsList[i].expiredOn, historyCouponsList[i]._priceItem, historyCouponsList[i]._shopType);
            historys.Add(history.gameObject);
        }
    }

    public IEnumerator setupCouponStorage(Action callback)
    {
        IWSResponse response = null;
        yield return UserShopItem.GetAllUserItemCode(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error Get AllUserItemCode");
            yield break;
        }
        List<UserShopItem> userAllCouponStorage = UserShopItem.ParseToList(response.RawResult().ToString());
        Debug.Log("userAllCouponStorage=> " + userAllCouponStorage.Count);
        for (int i = 0; i < userAllCouponStorage.Count; i++)
        {
            CouponHistoryDetail coupons = ScriptableObject.CreateInstance<CouponHistoryDetail>();
            coupons = setDataCoupon(userAllCouponStorage[i], coupons);
            if ((coupons._isUse) || (coupons.expiredOn < ItemDataObject.instance.timeNowServer))
            {
                historyCouponsList.Add(coupons);
            }
            else
            {
                activeCouponsList.Add(coupons);
            }
        }
        callback?.Invoke();
    }
    public CouponHistoryDetail setDataCoupon(UserShopItem shopItem , CouponHistoryDetail coupon)
    {
        coupon._shopId = shopItem.itemID;
        coupon._isUse = shopItem.isUsed;
        coupon._couponId = shopItem.itemCode;
        coupon.expiredOn= shopItem.expiredOn;
        //---------------------------------------------------------------------------

        for (int i = 0; i < ShoppingDataObject.instance._allShopDisplay.Count; i++)
        {
            if (ShoppingDataObject.instance._allShopDisplay[i]._shopId == shopItem.itemID)
            {
                coupon._shopType = ShoppingDataObject.instance._allShopDisplay[i]._shopType;

                coupon._shop_icon = new Sprite[ShoppingDataObject.instance._allShopDisplay[i]._shop_icon.Length];
                coupon._shop_icon[0] = ShoppingDataObject.instance._allShopDisplay[i]._shop_icon[0];


                coupon._shopName = ShoppingDataObject.instance._allShopDisplay[i]._shopName;
                coupon.name = ShoppingDataObject.instance._allShopDisplay[i]._shopName;
                coupon._shopDescription = ShoppingDataObject.instance._allShopDisplay[i]._shopDescription;
                coupon._shopInfo = ShoppingDataObject.instance._allShopDisplay[i]._shopInfo;
                coupon._priceItem = ShoppingDataObject.instance._allShopDisplay[i]._priceItem;
            }
        }
        //-------------------------------------------------------------------------------
        return coupon;
    }
}
