using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromotionController : MonoBehaviour
{
    [Header("Promotion Data")]
    [SerializeField] public ShopItemCategory shopType;
    [SerializeField] GameObject _contentPromotion;
    [SerializeField] GameObject _prefabShopDisplay;
    [SerializeField] Text _headerShopType;
    [SerializeField] public List<GameObject> promotionObj_list = new List<GameObject>();
    [SerializeField] public List <ShopDetail> shopDetailsPromotion_list = new List<ShopDetail>();

    public void setupPromotionDisplay()
    {
        setHeaderText(shopType);
        for (int i = 0; i < shopDetailsPromotion_list.Count; i++)
        {
            GameObject shopTemp = Instantiate(_prefabShopDisplay, _contentPromotion.transform);
            shopTemp.SetActive(true);
            shopTemp.GetComponent<ShopEventDisplay>().detail = shopDetailsPromotion_list[i];
            shopTemp.GetComponent<ShopEventDisplay>().setupDisplay(shopDetailsPromotion_list[i]);
            promotionObj_list.Add(shopTemp);
        }
    }
    public void setHeaderText(ShopItemCategory shopType)
    {
        _headerShopType.text = ShoppingDataObject.instance.convetString(shopType);
    }
    public void onClickAllViewPromotion()
    {
        Debug.Log("Open Field view all (1)");
        ShopDisplayController.instance.onAllViewPromotion(shopDetailsPromotion_list);
    }
    public void ClearDataShopping()
    {
        for (int i = 0; i < promotionObj_list.Count; i++)
        {
            Destroy(promotionObj_list[i]);
        }
        promotionObj_list.Clear();
        shopDetailsPromotion_list.Clear();
    }
}
