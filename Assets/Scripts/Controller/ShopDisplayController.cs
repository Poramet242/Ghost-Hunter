using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class ShopDisplayController : MonoBehaviour
{
    public static ShopDisplayController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Shopp object")]
    [SerializeField] public ButtonType stateBtn;
    [SerializeField] public GameObject _prefabDisplay;
    [SerializeField] public GameObject _prefabHightlightDisplay;
    [SerializeField] public GameObject pointCount;
    [SerializeField] public InputField _headField;
    [Header("Close Button")]
    [SerializeField] public GameObject _Main_Close_btn;
    [SerializeField] public GameObject _PromotionViewAll_Close_btn;
    [SerializeField] public GameObject _ViewAll_Close_btn;
    [SerializeField] public GameObject _highlight_Close_btn;
    [Header("Hightlight List")]
    [SerializeField] bool isSetHighlight = false;
    [SerializeField] GameObject _contentHightlight;
    [SerializeField] GameObject _contentPoint;
    [SerializeField] GameObject _highlight_bar;
    [SerializeField] public List<GameObject> _hightlightObj_list = new List<GameObject>();
    [SerializeField] public List<GameObject> _pointHightlightObj_list = new List<GameObject>();
    [Header("Promotion")]
    [SerializeField] GameObject _prefabPromotionDisplay;
    [SerializeField] GameObject _contenPromotion;
    [SerializeField] public List<PromotionController> promotionsData_list = new List<PromotionController>();
    [SerializeField] public List<GameObject> promotionsDisplay_list = new List<GameObject>();
    [Header("promotion Dispaly")]
    [SerializeField] public RectTransform _rectTransform_Main;
    [SerializeField] public RectTransform _rectTransform_def;
    [SerializeField] public RectTransform _rectTransform_Mak;
    [Header("View all")]
    [SerializeField] public GameObject Field_viewAll;
    [SerializeField] private ScrollSnapRect scrollSnap;
    [Header("CouponStorage")]
    public bool isMain;
    [SerializeField] public GameObject CouponStorage;
    [Header("Other")]
    [SerializeField] public Text coins_text;
    private void OnEnable()
    {
        ClearDataShopping();
        StartCoroutine(getShoppingData());
    }
    private void OnDisable()
    {
        ClearDataShopping();
    }
    public void onClickOpenStorageCouponDisplay()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        isMain = true;
        CouponStorage.SetActive(true);
        CouponStorageController.instance.uiIsReweed = false;
    }
    private void Update()
    {
        switch (stateBtn)
        {
            case ButtonType.Main:
                _Main_Close_btn.SetActive(true);
                _PromotionViewAll_Close_btn.SetActive(false);
                _ViewAll_Close_btn.SetActive(false);
                _highlight_Close_btn.SetActive(false);
                break;
            case ButtonType.Promotion:
                _Main_Close_btn.SetActive(false);
                _PromotionViewAll_Close_btn.SetActive(true);
                _ViewAll_Close_btn.SetActive(false);
                _highlight_Close_btn.SetActive(false);
                break;
            case ButtonType.ViewAll:
                _Main_Close_btn.SetActive(false);
                _PromotionViewAll_Close_btn.SetActive(false);
                _ViewAll_Close_btn.SetActive(true);
                _highlight_Close_btn.SetActive(false);
                break;
            case ButtonType.Hightlight:
                _highlight_Close_btn.SetActive(true);
                _Main_Close_btn.SetActive(false);
                _PromotionViewAll_Close_btn.SetActive(false);
                _ViewAll_Close_btn.SetActive(false);
                break;
        }
        coins_text.text = PlayerData.instance._coineReward.ToString();
    }
    public void onAllViewPromotion(List<ShopDetail> shopDetails)
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        Debug.Log("Open Field view all (2)");
        stateBtn = ButtonType.Promotion;
        Field_viewAll.SetActive(true);
        FieldViewAllShopDisplay.instance.stateDisplay = stateBtn;
        FieldViewAllShopDisplay.instance.Promoutuon_ViewAll_Search.SetActive(true);
        FieldViewAllShopDisplay.instance.ViewAll_Search.SetActive(false);
        FieldViewAllShopDisplay.instance.ViewAll_SearchNot.SetActive(false);
        //Promotion
        for (int i = 0; i < shopDetails.Count; i++)
        {
            FieldViewAllShopDisplay.instance.fieldShopDetail_list.Add(shopDetails[i]);
        }
        FieldViewAllShopDisplay.instance.setupPromotionFieldViewAllShopDisplay();
    }
    public void onSeaechAllView()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        stateBtn = ButtonType.ViewAll;
        Field_viewAll.SetActive(true);
        FieldViewAllShopDisplay.instance.stateDisplay = stateBtn;
        FieldViewAllShopDisplay.instance.ViewAll_Search.SetActive(true);
        FieldViewAllShopDisplay.instance.Promoutuon_ViewAll_Search.SetActive(false);
        FieldViewAllShopDisplay.instance.ViewAll_SearchNot.SetActive(false);
        //ViewAll
        for (int i = 0; i < ShoppingDataObject.instance._allShopDisplay.Count; i++)
        {
            FieldViewAllShopDisplay.instance.fieldShopDetail_list.Add(ShoppingDataObject.instance._allShopDisplay[i]);
        }
        FieldViewAllShopDisplay.instance.setupFieldViewAllShopDisplay();
    }
    public void onClickClosePromotion()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        stateBtn = ButtonType.Main;
        FieldViewAllShopDisplay.instance.stateDisplay = stateBtn;
        FieldViewAllShopDisplay.instance.Promoutuon_ViewAll_Search.SetActive(false);
        FieldViewAllShopDisplay.instance.ViewAll_btn.SetActive(true);
        FieldViewAllShopDisplay.instance.InputField_Promotion.gameObject.SetActive(false);
        FieldViewAllShopDisplay.instance.InputField_viewAll.gameObject.SetActive(false);
        FieldViewAllShopDisplay.instance.ViewAll_btn.SetActive(true);
        FieldViewAllShopDisplay.instance.ClearDataShopping();
        Field_viewAll.SetActive(false);
    }   
    public void onClickCloseViewAll()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        stateBtn = ButtonType.Main;
        FieldViewAllShopDisplay.instance.stateDisplay = stateBtn;
        FieldViewAllShopDisplay.instance.ViewAll_Search.SetActive(false);
        FieldViewAllShopDisplay.instance.InputField_Promotion.gameObject.SetActive(false);
        FieldViewAllShopDisplay.instance.InputField_viewAll.gameObject.SetActive(false);
        FieldViewAllShopDisplay.instance.ViewAll_btn.SetActive(true);
        FieldViewAllShopDisplay.instance.ClearDataShopping();
        Field_viewAll.SetActive(false);
    }
    public void onClickCloseHightlight()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        stateBtn = ButtonType.Main;
        //TODO : set text in highlight display
        //_highlight_bar.GetComponent<HighlightDisplay>().CloseHighlightDisplay();
        _highlight_bar.SetActive(false);
        UIManager.instance.menu.SetActive(true);
    }
    public void onHightlightView(ShopDetail shopDetail)
    {
        stateBtn = ButtonType.Hightlight;
        _highlight_bar.SetActive(true);
        _highlight_bar.GetComponent<HighlightDisplay>().setUpHighlightDisplay(shopDetail);
        UIManager.instance.menu.SetActive(false);
    }
    public void ClearDataShopping()
    {
        /*for (int i = 0; i < _hightlightObj_list.Count; i++)
        {
            Destroy(_hightlightObj_list[i]);
        }
        for (int i = 0; i < _pointHightlightObj_list.Count; i++)
        {
            Destroy(_pointHightlightObj_list[i]);
        }*/
        for (int i = 0; i < promotionsData_list.Count; i++)
        {
            promotionsData_list[i].ClearDataShopping();
        }
        for (int i = 0; i < promotionsDisplay_list.Count; i++)
        {
            Destroy(promotionsDisplay_list[i]);
        }
        promotionsData_list.Clear();
        //_hightlightObj_list.Clear();
        //_pointHightlightObj_list.Clear();
        promotionsDisplay_list.Clear();
        //ShoppingDataObject.instance._allHighlightShop_list.Clear();
    }
    public IEnumerator getShoppingData()
    {
        yield return setupShopDisplay();
    }
    IEnumerator setupShopDisplay()
    {
        yield return setUpHightlight();
        List<ShopItemCategory> allTypeList = ShoppingDataObject.instance.getAllShopUnit();
        for (int i = 0; i < allTypeList.Count; i++)
        {
            GameObject promotionTemp = Instantiate(_prefabPromotionDisplay, _contenPromotion.transform);
            promotionTemp.SetActive(true);
            promotionTemp.name = allTypeList[i].ToString();
            promotionTemp.GetComponent<PromotionController>().shopType = allTypeList[i];
            promotionsDisplay_list.Add(promotionTemp);
            promotionsData_list.Add(promotionTemp.GetComponent<PromotionController>());
        }
        for (int i = 0; i < promotionsData_list.Count; i++)
        {
            for (int s = 0; s < ShoppingDataObject.instance._allNomalShop_list.Count; s++)
            {
                if (ShoppingDataObject.instance._allNomalShop_list[s]._shopType == promotionsData_list[i].shopType && ShoppingDataObject.instance._allNomalShop_list[s].expiredOn > ItemDataObject.instance.timeNowServer)
                {
                    if (ShoppingDataObject.instance._allNomalShop_list[s].startedOn > ItemDataObject.instance.timeNowServer)
                    {
                        continue;
                    }
                    promotionsData_list[i].shopDetailsPromotion_list.Add(ShoppingDataObject.instance._allNomalShop_list[s]);
                }
                else
                {
                    continue;
                }
            }
        }
        for (int i = 0; i < promotionsData_list.Count; i++)
        {
            promotionsData_list[i].setupPromotionDisplay();
        }
        yield break;
    }
    public IEnumerator setUpHightlight()
    {
        if (isSetHighlight || ShoppingDataObject.instance._allHighlightShop_list.Count == 0)
        {
            _rectTransform_Main = setRectSize(_rectTransform_Main, _rectTransform_Mak);
            scrollSnap.gameObject.SetActive(false);
            yield break;
        }
        for (int i = 0; i < ShoppingDataObject.instance._allHighlightShop_list.Count; i++)
        {
            GameObject highlightTemp = Instantiate(_prefabHightlightDisplay,_contentHightlight.transform);
            highlightTemp.SetActive(true);
            highlightTemp.name = ShoppingDataObject.instance._allHighlightShop_list[i]._shopName;
            highlightTemp.GetComponent<ShopEventDisplay>().setupDisplay(ShoppingDataObject.instance._allHighlightShop_list[i]);
            _hightlightObj_list.Add(highlightTemp);
            GameObject pointHighlight = Instantiate(pointCount, _contentPoint.transform);
            pointHighlight.SetActive(true);
            pointHighlight.name = "Point" + (i + 1);
            _pointHightlightObj_list.Add(pointHighlight);
        }
        _rectTransform_Main = setRectSize(_rectTransform_Main, _rectTransform_def);
        isSetHighlight = true;
        scrollSnap.enabled = true;
        yield break;
    }
    public RectTransform setRectSize(RectTransform A,RectTransform B)
    {
        A.anchoredPosition = B.anchoredPosition;
        A.sizeDelta = B.sizeDelta;
        A.anchorMin = B.anchorMin;
        A.anchorMax = B.anchorMax;
        A.pivot = B.pivot;
        A.rotation = B.rotation;
        return A;
    }
}
public enum ButtonType
{
    Main=0,
    Promotion = 1,
    ViewAll=2,
    Hightlight= 3,
}