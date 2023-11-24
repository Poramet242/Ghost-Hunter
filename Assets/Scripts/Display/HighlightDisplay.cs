using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightDisplay : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] public ShopDetail ShopDetail;
    [Header("Display Show")]
    [SerializeField] private Text _heard_1;
    [SerializeField] private Text _heard_2;
    [SerializeField] private Text _heard_3;
    [SerializeField] private Text _info_1;
    [SerializeField] private Text _info_2;
    [SerializeField] private Text _info_3;
    [SerializeField] private Image _image_1;
    [SerializeField] private Image _image_2;

    public void setUpHighlightDisplay(ShopDetail detail)
    {
        ShopDetail = detail;
        _heard_1.text = detail._shopName;
    }
    public void CloseHighlightDisplay()
    {
        ShopDetail = new ShopDetail();

        _heard_1.text = string.Empty;
        _heard_2.text = string.Empty;
        _heard_3.text = string.Empty;

        _info_1.text = string.Empty;
        _info_2.text = string.Empty;
        _info_3.text = string.Empty;

        _image_1.sprite = null;
        _image_2.sprite = null;
    }
}
