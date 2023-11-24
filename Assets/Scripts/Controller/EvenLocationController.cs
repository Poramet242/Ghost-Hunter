using FlexGhost.Models;
using Mapbox.Platform;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class EvenLocationController : MonoBehaviour
{
    public static EvenLocationController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] public GameObject thisObject;
    [Header("Map")]
    [SerializeField] public GameObject BiggerMap;
    [Header("Player Detail")]
    [SerializeField] private Image _energy_img;
    [SerializeField] private Text _energy_text;
    [SerializeField] private Image _atk_img;
    [SerializeField] private Text _atk_text;
    [SerializeField] private Image _armer_img;
    [SerializeField] private Text _armer_text;

    [Header("Ghost Detail")]
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _ghostDetailEven;
    [SerializeField] private List<GameObject> _ghostDetailEvenList = new List<GameObject>();
    [Header("Level Ghost")]
    [SerializeField] private GameObject _gohstLevel_panel;
    [SerializeField] private ScrollSnapRect scrollSnap;
    [Header("Other Display")]
    [SerializeField] public GameObject _faceDisplay;
    private void OnEnable()
    {
        StartCoroutine(setEnergyDataDispaly(() => 
        {
            Initialize();
            setupMapLocationEvent();
        }));
    }
    private void OnDisable()
    {
        clearData();
        GameManager.instance.clearAreaGhostLocationOld();
    }
    private void Update()
    {
        setupPlayerDetail();
    }
    public void Initialize()
    {
        List<GhostUnitData> allGhostUnitData = getAllUnitSperity();
        for (int i = 0; i < allGhostUnitData.Count; i++)
        {
            GameObject _ghosts = Instantiate(_ghostDetailEven, _content.transform);
            _ghosts.SetActive(true);
            _ghosts.name = GhostDataObject.instance.GhostUnitData_listZone[i].detail.ghostName;
            _ghostDetailEvenList.Add(_ghosts);
            _ghostDetailEvenList[i].GetComponent<GhostDetailEventDisplay>().setup(GhostDataObject.instance.GhostUnitData_listZone[i]);
        }
        StartCoroutine(GameManager.instance.getUserEquipment());
    }
    public List<GhostUnitData> getAllUnitSperity()
    {
        List<GhostUnitData> temp = new List<GhostUnitData>();
        for (int i = 0; i < GhostDataObject.instance.GhostUnitData_listZone.Count; i++)
        {
            if (!temp.Exists(o => o.detail.ghostName == GhostDataObject.instance.GhostUnitData_listZone[i].detail.ghostName))
            {
                temp.Add(GhostDataObject.instance.GhostUnitData_listZone[i]);
            }
        }
        return temp;
    }
    public void setupPlayerDetail()
    {
        _energy_text.text = PlayerData.instance._current_Energy.ToString() + "/" + PlayerData.instance._max_Energy.ToString();
        _energy_img.fillAmount = (float)PlayerData.instance._current_Energy / PlayerData.instance._max_Energy;
        TimeSpan spanDamage = TimeSpan.FromSeconds(ItemDataObject.instance._currentDoubleDamages);
        setupDoubleDamages(spanDamage);
        TimeSpan spanArmerUpper = TimeSpan.FromSeconds(ItemDataObject.instance._currentArmorUpper);
        setupArmerUpper(spanArmerUpper);
    }
    public void setupDoubleDamages(TimeSpan spanDamage)
    {
        string hours = spanDamage.Hours.ToString();
        string minutes = spanDamage.Minutes.ToString();
        string seconds = spanDamage.Seconds.ToString();
        _atk_text.text = setDisplayTimeShowInfo(hours, minutes, seconds);
        _atk_img.fillAmount = (float)(ItemDataObject.instance._currentDoubleDamages / ItemDataObject.instance._doubleDamagesMax);
    }
    public void setupArmerUpper(TimeSpan spanArmerUpper)
    {
        string hours = spanArmerUpper.Hours.ToString();
        string minutes = spanArmerUpper.Minutes.ToString();
        string seconds = spanArmerUpper.Seconds.ToString();
        _armer_text.text = setDisplayTimeShowInfo(hours, minutes, seconds);
        _armer_img.fillAmount = (float)(ItemDataObject.instance._currentArmorUpper / ItemDataObject.instance._armorUpperMax);
    }
    public string setDisplayTimeShowInfo(string hours, string minutes, string seconds)
    {
        string time = "";
        if (hours == "0" && minutes == "0") time = seconds + "s";
        else if (hours == "0" && seconds == "0") time = minutes + "m";
        else if (minutes == "0" && seconds == "0") time = hours + "h";
        else if (hours == "0") time = minutes + "m" + seconds + "s";
        else if (seconds == "0") time = hours + "h" + minutes + "m";
        else if (minutes == "0") time = hours + "h" + seconds + "s";
        else time = hours + "h" + minutes + "m" + seconds + "s";
        return time;
    }
    public void setupMapLocationEvent()
    {
        GameManager.instance.setupAreaLocation();
    }
    public void onClickCloseBiggerMap(bool isCheck)
    {
        GameManager.instance.isBig_map = isCheck;
        UIManager.instance.menu.SetActive(!isCheck);
        BiggerMap.SetActive(isCheck);
    }
    public void clearData()
    {
        for (int i = 0; i < _ghostDetailEvenList.Count; i++)
        {
            Destroy(_ghostDetailEvenList[i]);
        }
        _ghostDetailEvenList.Clear();
    }
    public IEnumerator setEnergyDataDispaly(Action callback)
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
        callback?.Invoke();
    }
    public void onClickOpneLevelGhost()
    {
        UIManager.instance.menu.SetActive(false);
        _gohstLevel_panel.SetActive(true);
    }
    public void onClickClosrLevelGhost()
    {
        scrollSnap.onClickScreen(0);
        UIManager.instance.menu.SetActive(true);
        _gohstLevel_panel.SetActive(false);
    }
}
