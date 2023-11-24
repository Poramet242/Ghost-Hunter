using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class regExSearchDataController : MonoBehaviour
{
    public static regExSearchDataController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] public ButtonType stateDisplay;
    [SerializeField] public List<ShopDetail> datalist = new List<ShopDetail>();
    [SerializeField] public List<GameObject> itemDisplay_obj = new List<GameObject>();
    [SerializeField] public InputField inputField_Promotion;
    [SerializeField] public InputField inputField_viewAll;
    [SerializeField] private List<ShopDetail> matchedDataList = new List<ShopDetail>();

    private void Start()
    {
        inputField_Promotion.onValueChanged.AddListener(OnInputValueChanged);
        inputField_viewAll.onValueChanged.AddListener(OnInputValueChanged);
    }
    public static void clear(InputField inputfield)
    {
        inputfield.text = string.Empty;
    }
    public void setupFieldDisplay(ButtonType buttonType, List<ShopDetail> shopDetails, List<GameObject> tempList)
    {
        datalist = shopDetails;
        itemDisplay_obj = tempList; 
        stateDisplay = buttonType;
        switch (buttonType)
        {
            case ButtonType.Main:
                clear(inputField_Promotion);
                clear(inputField_viewAll);
                break;
            case ButtonType.Promotion:
                clear(inputField_Promotion);
                break;
            case ButtonType.ViewAll:
                clear(inputField_viewAll);
                break;
        }
    }
    public void OnInputValueChanged(string searchString)
    {
        matchedDataList.Clear();
        if (datalist.Count == 0 || string.IsNullOrEmpty(searchString))
        {
            switch (stateDisplay)
            {
                case ButtonType.Promotion:
                    FieldViewAllShopDisplay.instance.Promoutuon_ViewAll_Search.SetActive(true);
                    FieldViewAllShopDisplay.instance.ViewAll_SearchNot.SetActive(false);
                    break;
                case ButtonType.ViewAll:
                    FieldViewAllShopDisplay.instance.ViewAll_Search.SetActive(true);
                    FieldViewAllShopDisplay.instance.ViewAll_SearchNot.SetActive(false);
                    break;
            }
            for (int i = 0; i < itemDisplay_obj.Count; i++)
            {
                itemDisplay_obj[i].SetActive(true);
            }
            Debug.Log("string is null or empty");
            return;
        }
        //string text = ThaiFontAdjuster.Adjust(searchString);
        string escapedSearchString = "(" + searchString + @"\w?)";
        Regex regex = new Regex(escapedSearchString, RegexOptions.IgnoreCase);
        foreach (ShopDetail item in datalist)
        {
            if (!string.IsNullOrEmpty(item._shopName))
            {
                MatchCollection match = regex.Matches(item._shopName);
                if (match.Count > 0) 
                {
                    matchedDataList.Add(item);
                }
            }
            if (!string.IsNullOrEmpty(item._shopDescription))
            {
                MatchCollection match = regex.Matches(item._shopDescription);
                if (match.Count > 0)
                {
                    if(matchedDataList.Contains(item))
                    {
                        continue;
                    }
                    matchedDataList.Add(item);
                }
            }
        }
        ShowSearchResults(matchedDataList);
    }
    public void ShowSearchResults(List<ShopDetail> matches)
    {
        if (matches.Count > 0)
        {
            switch (stateDisplay)
            {
                case ButtonType.Promotion:
                    FieldViewAllShopDisplay.instance.Promoutuon_ViewAll_Search.SetActive(true);
                    break;
                case ButtonType.ViewAll:
                    FieldViewAllShopDisplay.instance.ViewAll_Search.SetActive(true);
                    break;
            }
            FieldViewAllShopDisplay.instance.ViewAll_SearchNot.SetActive(false);
            for (int i = 0; i < itemDisplay_obj.Count; i++)
            {
                itemDisplay_obj[i].SetActive(false);
            }
            for (int i = 0; i < matches.Count; i++)
            {
                Debug.Log("Results Matches =>" + " {" + i + "} " + matches[i]._shopName);
                OpenDisplayResults(matches[i]);
            }
        }
        else
        {
            switch (stateDisplay)
            {
                case ButtonType.Promotion:
                    FieldViewAllShopDisplay.instance.Promoutuon_ViewAll_Search.SetActive(false);
                    break;
                case ButtonType.ViewAll:
                    FieldViewAllShopDisplay.instance.ViewAll_Search.SetActive(false);
                    break;
            }
            FieldViewAllShopDisplay.instance.ViewAll_SearchNot.SetActive(true);
            Debug.Log("No matches found.");
        }
    }
    public void OpenDisplayResults(ShopDetail shopDetail)
    {
        for (int i = 0; i < itemDisplay_obj.Count; i++)
        {
            if (itemDisplay_obj[i].GetComponent<ShopEventDisplay>().detail == shopDetail)
            {
                itemDisplay_obj[i].SetActive(true);
            }
        }
    }
    #region Test API CALL
    public IEnumerator getShoppingData()
    {
        IWSResponse response = null;
        yield return ShopItem.GetAllShopItem(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log(response.InternalErrorsString());
            Debug.Log("Error Get All Shop item");
            yield break;
        }
        List<ShopItem> shopTemp = ShopItem.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < shopTemp.Count; i++)
        {
            ShopDetail shopDetail = ScriptableObject.CreateInstance<ShopDetail>();
            ShoppingDataObject.instance.setupShopDetail(shopTemp[i], shopDetail,() =>
            {
                ShoppingDataObject.instance._allShopDisplay.Add(shopDetail);
                datalist.Add(shopDetail);

                GameObject Temp = Instantiate(TestLoadSprite.instance._prefabDisplay, TestLoadSprite.instance.pointCount.transform);
                Temp.SetActive(true);
                Temp.GetComponent<ShopEventDisplay>().detail = shopDetail;
                Temp.GetComponent<ShopEventDisplay>().setupDisplay(shopDetail);
            });

        }
    }
    #endregion
}
