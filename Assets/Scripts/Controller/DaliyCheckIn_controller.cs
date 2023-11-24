using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSystem;

public class DaliyCheckIn_controller : MonoBehaviour
{
    public static DaliyCheckIn_controller instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Data")]
    [SerializeField] public int countLogin;
    [SerializeField] public GameObject _thisObject;
    [SerializeField] private GameObject _content_object;
    [SerializeField] private GameObject _dailyCheck_in;
    [SerializeField] public List<GameObject> dailyDisplayslist = new List<GameObject>();
    [Header("Energy Display")]
    [SerializeField] public Sprite past_energy_spr;
    [SerializeField] public Sprite present_energy_spr;
    [SerializeField] public Sprite future_energy_spr;
    [Header("Damage Display")]
    [SerializeField] public Sprite past_doubleDamage_spr;
    [SerializeField] public Sprite present_doubleDamage_spr;
    [SerializeField] public Sprite future_doubleDamage_spr;
    [Header("Armor Display")]
    [SerializeField] public Sprite past_armorUp_spr;
    [SerializeField] public Sprite present_armorUp_spr;
    [SerializeField] public Sprite future_armorUp_spr;
    [Header("Flex point Display")]
    [SerializeField] public Sprite present_flexPoint_spr;
    [SerializeField] public Sprite future_flexPoint_spr;
    

    private void Start()
    {
        if (PlayerData.instance._checkDaily)
        {
            this.gameObject.SetActive(false);
            EvenLocationController.instance.thisObject.SetActive(false);
        }
        else
        {
            Initialize(30);
            this.gameObject.SetActive(true);
            EvenLocationController.instance.thisObject.SetActive(true);
        }
    }
    public void Initialize(int day)
    {
        for (int daily = 0; daily < day; daily++)
        {
            GameObject dailys = Instantiate(_dailyCheck_in, _content_object.transform);
            dailys.SetActive(true);
            dailys.GetComponent<DaliyDisplay>()._day_text.text = "Energy\nวันที่ "+(daily + 1).ToString();
            dailys.GetComponent<DaliyDisplay>().isEnergy = true;
            dailys.GetComponent<DaliyDisplay>().stageDaily = StageDaily.Future;
            dailys.GetComponent<DaliyDisplay>().DailyCheck_img.sprite = future_energy_spr;
            dailys.name = "Daily_checkIn_" + (daily + 1);
            dailys.GetComponent<DaliyDisplay>().day_count = daily + 1;
            switch (daily+1)
            {
                case 5:
                    dailys.GetComponent<DaliyDisplay>()._day_text.text = "Plasma Bomb\n5 นาที\nวันที่ " + (daily + 1).ToString();
                    dailys.GetComponent<DaliyDisplay>().isEnergy = false;
                    dailys.GetComponent<DaliyDisplay>().isDamage = true;
                    dailys.GetComponent<DaliyDisplay>().DailyCheck_img.sprite = future_doubleDamage_spr;
                    break;
                case 10:
                    dailys.GetComponent<DaliyDisplay>()._day_text.text = "Plasma Bomb\n10 นาที\nวันที่ " + (daily + 1).ToString();
                    dailys.GetComponent<DaliyDisplay>().isEnergy = false;
                    dailys.GetComponent<DaliyDisplay>().isDamage = true;
                    dailys.GetComponent<DaliyDisplay>().DailyCheck_img.sprite = future_doubleDamage_spr;
                    break;
                case 15:
                    dailys.GetComponent<DaliyDisplay>()._day_text.text = "Energy Vial\n10 นาที\nวันที่ " + (daily + 1).ToString();
                    dailys.GetComponent<DaliyDisplay>().isEnergy = false;
                    dailys.GetComponent<DaliyDisplay>().isArmor = true;
                    dailys.GetComponent<DaliyDisplay>().DailyCheck_img.sprite = future_armorUp_spr;
                    break;
                case 20:
                    dailys.GetComponent<DaliyDisplay>()._day_text.text = "Plasma Bomb\n10 นาที\nวันที่ " + (daily + 1).ToString();
                    dailys.GetComponent<DaliyDisplay>().isEnergy = false;
                    dailys.GetComponent<DaliyDisplay>().isDamage = true;
                    dailys.GetComponent<DaliyDisplay>().DailyCheck_img.sprite = future_doubleDamage_spr;
                    break;
                case 25:
                    dailys.GetComponent<DaliyDisplay>()._day_text.text = "Energy Vial\n15 นาที\nวันที่ " + (daily + 1).ToString();
                    dailys.GetComponent<DaliyDisplay>().isEnergy = false;
                    dailys.GetComponent<DaliyDisplay>().isArmor = true;
                    dailys.GetComponent<DaliyDisplay>().DailyCheck_img.sprite = future_armorUp_spr;
                    break;
                case 30:
                    dailys.GetComponent<DaliyDisplay>()._day_text.text = "All Stack\nวันที่ " + (daily + 1).ToString();
                    dailys.GetComponent<DaliyDisplay>().isEnergy = false;
                    dailys.GetComponent<DaliyDisplay>().isFlex = true;
                    dailys.GetComponent<DaliyDisplay>().DailyCheck_img.sprite = future_flexPoint_spr;
                    break;
            }
            dailyDisplayslist.Add(dailys);
        }
        StartCoroutine(setLoginDailyReward());
    }
    public IEnumerator setLoginDailyReward()
    {
        IWSResponse response = null;
        yield return DailyRewardCountResp.GetDailyRewardCount(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error Get Daily Reward");
            yield break;
        }
        var rewardCount = response as DailyRewardCountResp;
        countLogin = rewardCount.count;
        setupCheckin(countLogin);
    }
    public void setupCheckin(int DayCheckIn)
    {
        if (DayCheckIn == 0)
        {
            return;
        }
        for (int i = 0; i < DayCheckIn; i++)
        {
            dailyDisplayslist[i].GetComponent<DaliyDisplay>().LastCheckIN();
            dailyDisplayslist[i].GetComponent<DaliyDisplay>()._checkIn.gameObject.SetActive(false);
        }
        dailyDisplayslist[DayCheckIn - 1].GetComponent<DaliyDisplay>()._checkIn.gameObject.SetActive(true);
        //dailyDisplayslist[DayCheckIn - 1].GetComponent<DaliyDisplay>().stageDaily = StageDaily.Present;
    }
    public void setActiveNextDay(int DayCheckIn)
    {
        if (DayCheckIn == 0)
        {
            return;
        }
        dailyDisplayslist[DayCheckIn - 1].GetComponent<DaliyDisplay>()._checkIn.gameObject.SetActive(false);
    }
}
