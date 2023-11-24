using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSystem;

public class GameplayDisplay : MonoBehaviour
{
    public static GameplayDisplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        StartCoroutine(setEnergyRefile());
    }
    [Header("MOD")]
    [SerializeField] public GameObject _thisGameObject;
    [Header("Calculate Energy Display")]
    [SerializeField] public float timered = 0f;
    [SerializeField] private float refillInterval = 5f * 60f; //5minutes
    private void Update()
    {
        if (ItemDataObject.instance._isDoubleDamages)
        {
            ItemDataObject.instance._currentDoubleDamages -= Time.deltaTime;
            if (ItemDataObject.instance._currentDoubleDamages <= 0)
            {
                ItemDataObject.instance._currentDoubleDamages = 0;
                ItemDataObject.instance._doubleDamagesMax = 0;
                ItemDataObject.instance._isDoubleDamages = false;

            }
        }
        if (ItemDataObject.instance._isArmorUpper)
        {
            ItemDataObject.instance._currentArmorUpper -= Time.deltaTime;
            if (ItemDataObject.instance._currentArmorUpper <= 0)
            {
                ItemDataObject.instance._currentArmorUpper = 0;
                ItemDataObject.instance._armorUpperMax = 0;
                ItemDataObject.instance._isArmorUpper = false;
            }
        }
        calculatedTimeEnergy(PlayerData.instance._max_Energy, PlayerData.instance._current_Energy);
    }
    public void calculatedTimeEnergy(int max, int min)
    {
        if (min < max)
        {
            timered += Time.deltaTime;
            if (timered == refillInterval)
            {
                //PlayerData.instance._current_Energy++; // Increase energy by 1
                StartCoroutine(setEnergyRefile());
                Debug.Log($"Energy refilled! Current energy: {PlayerData.instance._current_Energy}");
                timered = 0f;
            }
        }
    }
    public IEnumerator setEnergyRefile()
    {
        IWSResponse response = null;
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
        double timeDifference = setTimeNow(energy.timeStamp, DateTime.Now);
        if (timered < 0)
        {
            timered = 0f;
        }
        else
        {
            timered = (float)timeDifference;
        }
    }
    public double setTimeNow(DateTime timeOld, DateTime timeNow)
    {
        TimeSpan timeSpan = timeNow - timeOld;
        double timeDifference = timeSpan.TotalSeconds + PlayerData.instance.dateTimeServer;
        Debug.Log("Time Difference in seconds: " + timeDifference);
        return timeDifference;
    }
}
