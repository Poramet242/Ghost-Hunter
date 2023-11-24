using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldViewAllShopDisplay : MonoBehaviour
{
    public static FieldViewAllShopDisplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] public ButtonType stateDisplay;
    [Header("Field View All Panel")]
    [SerializeField] public GameObject _prefabViewAllDisplay;
    [SerializeField] public List<ShopDetail> fieldShopDetail_list = new List<ShopDetail>();
    [SerializeField] public List<GameObject> fieldViewObj_list = new List<GameObject>();
    [Header("View all")]
    [SerializeField] public InputField InputField_Promotion;
    [SerializeField] public InputField InputField_viewAll;
    [SerializeField] public GameObject ViewAll_btn;
    [Header("Promotion")]
    [SerializeField] public Text header_text;
    [SerializeField] public ShopItemCategory category;
    [SerializeField] public GameObject Promoutuon_ViewAll_Search;
    [SerializeField] public GameObject pointContPromotion;
    [Header("View All Search")]
    [SerializeField] public GameObject ViewAll_Search;
    [SerializeField] public GameObject pointContViewAll_Search;
    [Header("Not Show all View")]
    [SerializeField] public GameObject ViewAll_SearchNot;

    public void setupPromotionFieldViewAllShopDisplay()
    {
        //Set active field view promotion
        ViewAll_btn.SetActive(false);
        InputField_viewAll.gameObject.SetActive(false);
        InputField_Promotion.gameObject.SetActive(true);
        //--------------------------------------------------------------------
        regExSearchDataController.instance.setupFieldDisplay(stateDisplay, fieldShopDetail_list, fieldViewObj_list);
        Debug.Log("Open Field view all (3)");
        category = fieldShopDetail_list[0]._shopType;
        header_text.text = ShoppingDataObject.instance.convetString(category);
        for (int i = 0; i < fieldShopDetail_list.Count; i++)
        {
            GameObject promotionTemp = Instantiate(_prefabViewAllDisplay, pointContPromotion.transform);
            promotionTemp.SetActive(true);
            promotionTemp.name = fieldShopDetail_list[i]._shopName;
            promotionTemp.GetComponent<ShopEventDisplay>().setupDisplay(fieldShopDetail_list[i]);
            fieldViewObj_list.Add(promotionTemp);
        }
    }
    public void setupFieldViewAllShopDisplay()
    {
        //Set active field view all
        ViewAll_btn.SetActive(false);
        InputField_viewAll.gameObject.SetActive(true);
        InputField_Promotion.gameObject.SetActive(false);
        //--------------------------------------------------------------------
        regExSearchDataController.instance.setupFieldDisplay(stateDisplay, fieldShopDetail_list, fieldViewObj_list);
        Debug.Log("Open Field view all (3)");
        category = fieldShopDetail_list[0]._shopType;
        header_text.text = ShoppingDataObject.instance.convetString(category);
        for (int i = 0; i < fieldShopDetail_list.Count; i++)
        {
            GameObject promotionTemp = Instantiate(_prefabViewAllDisplay, pointContViewAll_Search.transform);
            promotionTemp.SetActive(true);
            promotionTemp.name = fieldShopDetail_list[i]._shopName;
            promotionTemp.GetComponent<ShopEventDisplay>().setupDisplay(fieldShopDetail_list[i]);
            fieldViewObj_list.Add(promotionTemp);
        }
    }
    public void ClearDataShopping()
    {
        for (int i = 0; i < fieldViewObj_list.Count; i++)
        {
            Destroy(fieldViewObj_list[i]);
        }
        fieldViewObj_list.Clear();
        fieldShopDetail_list.Clear();
    }
}
