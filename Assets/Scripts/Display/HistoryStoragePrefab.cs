using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryStoragePrefab : MonoBehaviour
{
    [Header("Stage Category")]
    [SerializeField] public CouponHistoryDetail iscoupon;
    [SerializeField] public ShopItemCategory isCategory;
    public Sprite def_spr;
    public Image couponIMG;
    public Text couponNameTXT;
    public Text couponDescriptionTXT;
    public Text couponTimeTXT;
    public Text pointTXT;
    public void SetupHistoryDisplay(Sprite pic, string name, string detail, DateTime finishTime, int point, ShopItemCategory category)
    {
        if (pic ==null)
        {
            pic = def_spr;
        }
        isCategory = category;
        couponIMG.sprite = pic;
        couponNameTXT.text = String.Format("{0}", name);
        couponDescriptionTXT.text = String.Format("{0}", detail);
        couponTimeTXT.text = String.Format("ใช้ได้ถึง {0}.{1}.{2}", finishTime.Day, finishTime.Month, finishTime.Year);
        pointTXT.text = String.Format("{0}", point);
    }
}
