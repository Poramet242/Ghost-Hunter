using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class ItemCodeDisplay : MonoBehaviour
{
    public static ItemCodeDisplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Display")]
    [SerializeField] public GameObject _itemCodeDisplayBar;
    [SerializeField] public InputField _itemCode_input;
    [SerializeField] private string _itemCode_str;
    [Header("SuccessDisplay")]
    [SerializeField] private GameObject Redeem_success_display;
    [SerializeField] private GameObject noneRedeem_success_display;
    private void Update()
    {
        _itemCode_str = _itemCode_input.text;
    }
    public void onClickShowDisplayItemCode()
    {
        _itemCode_input.text = string.Empty;
        _itemCodeDisplayBar.SetActive(true);
    }
    public void onClickRedeemItemCode()
    {
        if (_itemCode_str != null && _itemCode_str != string.Empty && _itemCode_str != "" && _itemCode_str.Length == 13) 
        {
            StartCoroutine(setupItemCode(_itemCode_str));
        }
        else
        {
            noneRedeem_success_display.SetActive(true);
        }
    }
    IEnumerator setupItemCode(string itemCode)
    {
        IWSResponse response = null;
        yield return GameAPI.RedeemCode(XCoreManager.instance.mXCoreInstance, itemCode, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error Get History item code");
            noneRedeem_success_display.SetActive(true);
            yield break;
        }
        Redeem_success_display.SetActive(true);
    }
    public void onClickCancel(bool check)
    {
        StartCoroutine(Canceldispaly(check));

    }
    IEnumerator Canceldispaly(bool check)
    {
        yield return ItemCodeController.instance.setEnergyDataDispaly();
        _itemCode_input.text = string.Empty;
        Redeem_success_display.SetActive(false);
        noneRedeem_success_display.SetActive(false);
        _itemCodeDisplayBar.SetActive(false);
        if (check)
        {
            ItemCodeController.instance.clearData();
            ItemCodeController.instance.setupHistoryDisplay();
        }
    }
}
