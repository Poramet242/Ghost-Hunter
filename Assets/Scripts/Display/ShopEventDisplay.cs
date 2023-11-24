using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopEventDisplay : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] public ShopDetail detail;
    [SerializeField] private Image _icon_img;
    [SerializeField] private Text _nameShop;
    [SerializeField] private Text _infoItem;
    [SerializeField] private Text _priceItem;
    [SerializeField] private Text _expiredOn;
    public void setupDisplay(ShopDetail shopDetail)
    {
        if (!shopDetail._isHighlight)
        {
            _expiredOn.text = shopDetail.expiredOn.ToString("dd.MM.yyyy");
        }
        detail = shopDetail;
        _icon_img.sprite = shopDetail._shop_icon[0];
        _nameShop.text = shopDetail._shopName;
        _infoItem.text = shopDetail._shopDescription;
        if (_priceItem != null)
        {
            _priceItem.text = shopDetail._priceItem.ToString("#,##0");
        }
    }
    public void onClickOpenDisplay()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        if (detail != null)
        {
            if (detail._isHighlight)
            {
                ShopDisplayController.instance.onHightlightView(detail);
            }
            else 
            {
                ShoppingDataObject.instance._selectedShopDisplay = detail;
                ProfileController.instance._shopSelectedDisplay.SetActive(true);
                UIManager.instance.menu.SetActive(false);
                RedeemRewardsDisplay.instance.setupPromotionDisplay(detail);
            }
        }
    }
}
