using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Globalization;
using XSystem;
using FlexGhost.Models;

public class ItemCodeController : MonoBehaviour
{
    public static ItemCodeController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] public GameObject thisObject;
    [Header("History Display")]
    [SerializeField] private GameObject history_prf;
    [SerializeField] private Transform content_history;
    [SerializeField] private List<GameObject> all_History_obj = new List<GameObject>();
    [SerializeField] private List<string> dateTimeStrings = new List<string>();
    [Header("Other Display")]
    [SerializeField] private GameObject noneHistory_display;
    [SerializeField] private GameObject Historys_display;

    private void OnEnable()
    {
        clearData();
        setupHistoryDisplay();
    }
    public void setupHistoryDisplay()
    {
        //TODO: call to setupHistory display in server
        StartCoroutine(setupItemCodeDisplay());
    }
    private void Update()
    {
        if (all_History_obj.Count > 0)
        {
            noneHistory_display.SetActive(false);
            Historys_display.SetActive(true);
        }
        else
        {
            noneHistory_display.SetActive(true);
            Historys_display.SetActive(false);
        }
    }
    public void sortHistoryDiusplay()
    {
        //var sortedDateTimeStrings = dateTimeStrings.OrderBy(dateTimeString => DateTime.ParseExact(dateTimeString, "dd/MM/yy hh:mm tt", CultureInfo.InvariantCulture));
        var sortedItems = dateTimeStrings.OrderBy(item => DateTime.ParseExact(item.Substring(0, 16), "dd/MM/yyyy HH:mm",
                                                                              CultureInfo.InvariantCulture));
        foreach (var item in sortedItems)
        {
            GameObject history_tmp = Instantiate(history_prf, content_history);
            history_tmp.SetActive(true);
            history_tmp.name = item;
            history_tmp.GetComponent<HistoryDisplay>().setupHistoryDisplay(item);
            all_History_obj.Add(history_tmp);
        }
    }


    public IEnumerator setupItemCodeDisplay()
    {
        IWSResponse response = null;
        yield return Coupon.GetRedeemLog(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error Get History item code");
            yield break;
        }
        List<Coupon> coupons = Coupon.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < coupons.Count; i++)
        {
            string str = coupons[i].usedOn.ToString("dd/MM/yyy HH:mm") + " ใช้รหัส " + coupons[i].code + " ได้รับ ";
            if (coupons[i].energyAmount > 0)
            {
                str = str + "Energy " + coupons[i].energyAmount + " ea. ";
            }
            if (coupons[i].weaponAmount > 0)
            {
                str = str + "Plasma Bomb " + coupons[i].weaponAmount + " min. ";
            }
            if (coupons[i].armorAmount > 0)
            {
                str = str + "Energy Vial " + coupons[i].armorAmount + " min. ";
            }
            dateTimeStrings.Add(str);
        }
        sortHistoryDiusplay();
    }
    public void clearData()
    {
        for (int i = 0; i < all_History_obj.Count; i++)
        {
            Destroy(all_History_obj[i]);
        }
        all_History_obj.Clear();
        dateTimeStrings.Clear();
    }
    public IEnumerator setEnergyDataDispaly()
    {
        IWSResponse response = null;
        yield return EnergyResp.GetEnergy(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        var energy = response as EnergyResp;
        if (energy.energy >= PlayerData.instance._max_Energy)
        {
            PlayerData.instance._max_Energy = energy.energy;
        }
        else
        {
            PlayerData.instance._max_Energy = 100;
        }
        PlayerData.instance._current_Energy = energy.energy;
        EnergyGameplay.instance.setupDisplay(PlayerData.instance._current_Energy, PlayerData.instance._max_Energy);
    }
}
