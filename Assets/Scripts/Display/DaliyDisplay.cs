using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class DaliyDisplay : MonoBehaviour
{
    public static DaliyDisplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("MOD")]
    [SerializeField] public bool isEnergy;
    [SerializeField] public bool isDamage;
    [SerializeField] public bool isArmor;
    [SerializeField] public bool isFlex;
    [Header("Ui dispaly")]
    [SerializeField] public StageDaily stageDaily;
    [SerializeField] public bool isCheckDaily;
    [SerializeField] public bool isOldDaily;
    [SerializeField] public int day_count;
    [SerializeField] public GameObject DailyCheck_in;
    [SerializeField] public Image DailyCheck_img;
    [SerializeField] public Image _checkIn;
    [SerializeField] public Text _day_text;

    public void onClickCheck_in()
    {
        if (day_count == (DaliyCheckIn_controller.instance.countLogin + 1))
        {
            DaliyCheckIn_controller.instance.setActiveNextDay(day_count);
            StartCoroutine(setClaimRewardDaily());
        }
        else if (day_count < (DaliyCheckIn_controller.instance.countLogin + 1))
        {
            WarningDisplay.instance.setupWarningDisplay("ล็อคอิน", "คุณได้ทำการรับของรางวัล ของวันนี้ไปแล้ว !!!", WarningType.ErrorUiDisplay);
        }
        else if(day_count > (DaliyCheckIn_controller.instance.countLogin + 1))
        {
            for (int i = 0; i < DaliyCheckIn_controller.instance.dailyDisplayslist.Count; i++)
            {
                if (DaliyCheckIn_controller.instance.dailyDisplayslist[i].GetComponent<DaliyDisplay>().day_count == (DaliyCheckIn_controller.instance.countLogin + 1))
                {
                    DaliyCheckIn_controller.instance.dailyDisplayslist[i].GetComponent<DaliyDisplay>().onClickCheck_in();
                }
            }
        }
    }
    public void LastCheckIN()
    {
        //_checkIn.gameObject.SetActive(true);
        isCheckDaily = true;
        isOldDaily = true;
        stageDaily = StageDaily.Past;
    }
    IEnumerator setClaimRewardDaily()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        IWSResponse response = null;
        yield return CanClaimRewardResp.GetCanClaimReward(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error Can Reward Daily");
            yield break;
        }
        var canClaim = response as CanClaimRewardResp;
        if (canClaim.canClaim)
        {
            yield return GameAPI.ClaimDailyReward(XCoreManager.instance.mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                Debug.Log("Error Claim Reward Daily");
                yield break;
            }
            yield return EnergyResp.GetEnergy(XCoreManager.instance.mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                Debug.Log("Error GetEnergy");
                yield break;
            }
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
            yield return GameManager.instance.getUserEquipment();
            _checkIn.gameObject.SetActive(true);
            isCheckDaily = true;
            yield return DaliyCheckIn_controller.instance.setLoginDailyReward();
        }
        else
        {
            WarningDisplay.instance.setupWarningDisplay("ล็อคอิน", "คุณได้ทำการรับของรางวัล ของวันนี้ไปแล้ว !!!", WarningType.ErrorUiDisplay);
            //DaliyCheckIn_controller.instance._thisObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (isCheckDaily)
        {
            if (isOldDaily)
            {
                DailyCheck_img.sprite = setDisplayDaily(stageDaily, DailyCheck_img.sprite);
            }
            else
            {
                DailyCheck_img.sprite = setDisplayDaily(stageDaily, DailyCheck_img.sprite);
            }
        }
    }
    public Sprite setDisplayDaily(StageDaily stageDaily, Sprite temp_sp)
    {
        switch (stageDaily)
        {
            case StageDaily.Past:
                if (isEnergy)
                {
                    temp_sp = DaliyCheckIn_controller.instance.past_energy_spr;
                }
                else if (isDamage)
                {
                    temp_sp = DaliyCheckIn_controller.instance.past_doubleDamage_spr;
                }
                else if (isArmor)
                {
                    temp_sp = DaliyCheckIn_controller.instance.past_armorUp_spr;
                }
                break;
            case StageDaily.Present:
                if (isEnergy)
                {
                    temp_sp = DaliyCheckIn_controller.instance.present_energy_spr;
                }
                else if (isDamage)
                {
                    temp_sp = DaliyCheckIn_controller.instance.present_doubleDamage_spr;
                }
                else if (isArmor)
                {
                    temp_sp = DaliyCheckIn_controller.instance.present_armorUp_spr;
                }
                else if (isFlex)
                {
                    temp_sp = DaliyCheckIn_controller.instance.present_flexPoint_spr;
                }
                break;
        }
        return temp_sp;
    }
}
public enum StageDaily
{
    Past = 0,
    Present = 1,
    Future = 2,
}
