using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class EnergyGameplay : MonoBehaviour
{
    public static EnergyGameplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Data")]
    [SerializeField] public float _current_Energy;
    [SerializeField] public float _max_Energy;
    [Header("Dispaly")]
    [SerializeField] public Text _energy_txt;
    [SerializeField] public Image _energy_img;

    private void Start()
    {
        StartCoroutine(setEnergyDataDispaly());
    }
    public void setupDisplay(float _current, float _max)
    {
        _current_Energy = _current;
        _max_Energy = _max;

        _energy_txt.text = _current.ToString() + "/" + _max.ToString();
        _energy_img.fillAmount = _current / _max;
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
        setupDisplay(PlayerData.instance._current_Energy, PlayerData.instance._max_Energy);
    }
}
