using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class ProfileController : MonoBehaviour
{
    public static ProfileController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Ui")]
    [SerializeField] public GameObject _profileDisplay;
    [SerializeField] public GameObject _shopDisplay;
    [SerializeField] public GameObject _shopSelectedDisplay;
    [Header("Rule panel")]
    [SerializeField] public GameObject _Rule_panel;
    [Header("Profile Data")]
    [SerializeField] private Image iconPlayer_img;
    [SerializeField] private Text namePlayer_text;
    [SerializeField] private Text uidPlayer_text;
    [SerializeField] private Text countPoint_text;
    [SerializeField] private Text countGhost_text;
    [SerializeField] private Text countScore_text;
    [Header("Shopping profile")]
    [SerializeField] private Image iconShopping;
    [SerializeField] private Text nameShopping;
    [SerializeField] private Text uidShopping;
    [SerializeField] private Text coineShopping;
    [Header("Summary")]
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _prefabsObj;
    [SerializeField] private List<GameObject> ghostUnitList = new List<GameObject>();

    private void OnEnable()
    {
        StartCoroutine(LoadDataGameplayProfile(() =>
        {
            _profileDisplay.SetActive(true);
            _shopDisplay.SetActive(false);
            _shopSelectedDisplay.SetActive(false);
            setupProfileDate(PlayerData.instance);
            setupSummary();
        }));
    }
    private void OnDisable()
    {
        clearData();
        _profileDisplay.SetActive(true);
        _shopDisplay.SetActive(false);
        _shopSelectedDisplay.SetActive(false);
    }
    public void setupProfileDate(PlayerData playerData)
    {
        namePlayer_text.text = playerData._playerName;
        uidPlayer_text.text = playerData._playerUID;
        iconPlayer_img.sprite = playerData._iconplayer;

        nameShopping.text = playerData._playerName;
        uidShopping.text = playerData._playerUID;
        iconShopping.sprite = playerData._iconplayer;

        countScore_text.text = playerData._totalMaxPoint.ToString("#,##0");
        countPoint_text.text = playerData._totalPoint.ToString("#,##0");
        countGhost_text.text = playerData._totalGhost.ToString("#,##0");

        coineShopping.text = playerData._totalPoint.ToString("#,##0");
        SetupCoinShoppingFlexible(coineShopping);
    }
    public void setupSummary()
    {
        List<string> allSpeciesTypeList = InventoryGhostObject.instance.getAllGhostUnit();
        for (int i = 0; i < allSpeciesTypeList.Count; i++)
        {
            GameObject ghostSummary = Instantiate(_prefabsObj, _content.transform);
            ghostSummary.SetActive(true);
            ghostSummary.GetComponent<GhostDetailSummaryDisplay>()._ghostId = allSpeciesTypeList[i];
            ghostUnitList.Add(ghostSummary);
        }
        for (int i = 0; i < InventoryGhostObject.instance.ghostInventoryList.Count; i++)
        {
            setDataTogameObject(InventoryGhostObject.instance.ghostInventoryList[i]);
        }
        for (int i = 0; i < ghostUnitList.Count; i++)
        {
            ghostUnitList[i].GetComponent<GhostDetailSummaryDisplay>().calculateAllPoint();
        }
    }
    public void setDataTogameObject(GhostUnitData ghostUnitData)
    {
        for (int i = 0; i < ghostUnitList.Count; i++)
        {
            ghostUnitList[i].GetComponent<GhostDetailSummaryDisplay>().setupGhostDetailSummaryDisplay(ghostUnitData);
        }
    }
    public void onClickShopDisplay()
    {
        _shopDisplay.SetActive(true);
        _profileDisplay.SetActive(false);
    }
    public void onClickCloseShopDisplay()
    {

        _shopDisplay.SetActive(false);
        _profileDisplay.SetActive(true);
        //UIManager.instance.onClickProfile_btn();
    }
    public void clearData()
    {
        for (int i = 0; i < ghostUnitList.Count; i++)
        {
            Destroy(ghostUnitList[i]);
        }
        ghostUnitList.Clear();
    }
    IEnumerator LoadDataGameplayProfile(Action callback)
    {
        IWSResponse response = null;
        yield return Account.GetUserProfile(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
        var user = response as Account;
        PlayerData.instance._totalMaxPoint = user.score;
        PlayerData.instance._totalGhost = user.totalCatch;
        yield return WalletResp.GetWallet(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        var wallet = response as WalletResp;
        PlayerData.instance._coineReward = wallet.coin;
        PlayerData.instance._totalPoint = wallet.coin;
        callback?.Invoke();
    }
    public void SetupCoinShoppingFlexible(Text coinText)
    {
        RectTransform coinRec = coinText.GetComponent<RectTransform>();
        LayoutElement coinElement = coinText.GetComponent<LayoutElement>();
        coinElement.enabled = false;

        if (coinRec.sizeDelta.x > 300)
        {
            coinElement.enabled = true;
        }
        else
        {
            coinElement.enabled = false;
        }
    }
    public void onClickOpenRulePanel()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        _Rule_panel.SetActive(true);
    }
    public void onClickCloseRulePanel()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        _Rule_panel.SetActive(false);
    }
}
