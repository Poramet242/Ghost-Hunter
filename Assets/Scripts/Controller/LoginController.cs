using FlexGhost.Models;
using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSystem;

#if UNITY_IOS
// Include the IosSupport namespace if running on iOS:
using Unity.Advertisement.IosSupport;
#endif

public class LoginController : MonoBehaviour
{
    public static LoginController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        ProcessDeepLinkMngr.instance.DeepLinkActiveted();
#if UNITY_IOS
        // Check the user's consent status.
        // If the status is undetermined, display the request request:
        if(ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED) {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
#endif
    }
    [Header("Data")]
    public string secenGameplay = "<Secen Name>";
    public string token;
    [SerializeField] private VersionController version;
    #region Login
    public IEnumerator XLogin()
    {
        IWSResponse response = null;
#if UNITY_EDITOR
        //PlayerPrefs.SetString("sessionToken", token);
#endif
        if (PlayerPrefs.HasKey("sessionToken"))
        {
            string sessionToken = PlayerPrefs.GetString("sessionToken");
            //Debug.Log("Token: " + sessionToken);
            yield return XUser.RestoreSession(XCoreManager.instance.mXCoreInstance, sessionToken, (r) =>
            {
                response = r;
            });
            if (response.Success() ==false)
            {
                if (!response.Success())
                {
                    yield return settingLogin();
                    Debug.LogError(response.ErrorsString());
                    yield break;
                }
                //Ui login
                UiLoginDisplay.instance.startLoading_obj.SetActive(false);
                yield return settingLogin();
            }
            else
            {
                //Ui Loading
                UiLoginDisplay.instance.startLoading_obj.SetActive(false);
                UiLoginDisplay.instance.setLoadingDisplay();
            }
        }
        else
        {
            //Ui Login
            UiLoginDisplay.instance.startLoading_obj.SetActive(false);
            yield return settingLogin();
        }
    }
    IEnumerator settingLogin()
    {
        UiLoginDisplay.instance.startLoading_obj.SetActive(true);
        IWSResponse response = null;
        yield return GameConfig.GetGameConfig(XCoreManager.instance.mXCoreInstance, "Guest_version", (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        var gameConfig = response as GameConfig;
        if (float.Parse(gameConfig.value) == version.Guest_version)
        {
            UiLoginDisplay.instance.startLoading_obj.SetActive(false);
            UiLoginDisplay.instance.mainLogin_obj.SetActive(true);
            UiLoginDisplay.instance.setupLoginGuest(true);
        }
        else
        {
            UiLoginDisplay.instance.startLoading_obj.SetActive(false);
            UiLoginDisplay.instance.mainLogin_obj.SetActive(true);
            UiLoginDisplay.instance.setupLoginGuest(false);
        }
    }
    public void onClickLoginFlex()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        StartCoroutine(IsLoginFlex());
    }
    public void onClickGuestLogin()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        StartCoroutine(IsLoginGuest());
    }
    IEnumerator IsLoginGuest()
    {
        IWSResponse result = null;
        Debug.LogFormat("begin logging in with Guest");
        yield return XUser.GuestRegister(XCoreManager.instance.mXCoreInstance,(r) =>{result = r;});
        if (result.Success() == false)
        {
            Debug.LogErrorFormat("login failed: {0}", result.ErrorsString());
            yield break;
        }
        LoginResult loginResult = result as LoginResult;
        //Debug.LogFormat("login success with user: {0} sessionToken: {1}", loginResult.user.username, loginResult.sessionToken);
       // Debug.Log(loginResult.sessionToken);
        PlayerPrefs.SetString("sessionToken", loginResult.sessionToken);
        UiLoginDisplay.instance.setGuestDisplay();
    }
    public IEnumerator IsLoginFlex()
    {
        //UiLoginDisplay.instance.faceBack_panel.SetActive(true);
        //UiLoginDisplay.instance.mainLogin_obj.SetActive(false);
        yield return IsAppInstalled_Mngr.instance.OpenAppOrURL();
    }
    public IEnumerator IsLogin()
    {
        yield return LoadDataGameplayProfile();
    }
    public IEnumerator IsLoadGhostData()
    {
        yield return LoadDataGameplayGhost();
        yield return LoadEnergyLocation();
        yield return LoadMonsterSpawnPoint();
    }
#endregion

#region Load Player Data
    IEnumerator LoadDataGameplayProfile()
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
        PlayerData.instance._playerName = user.displayName;
        PlayerData.instance._playerUID = user.uid;
        PlayerData.instance._iconPlayerURL = user.displayImageID;
        PlayerData.instance._totalMaxPoint = user.score;
        PlayerData.instance._totalGhost = user.totalCatch;
        //TODO: open to loading icon from url to show in game
        if (string.IsNullOrEmpty(user.displayImageID))
        {
            PlayerData.instance._iconplayer = LoadUnitObject.instance.PlayerIconDef_spr;
        }
        else
        {
            yield return LoadUnitObject.instance.setLoadSpriteFromURL(user.displayImageID, (sprite) =>
            {
                PlayerData.instance._iconplayer = sprite;
            });
        }
        yield return WalletResp.GetWallet(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        var wallet = response as WalletResp;
        PlayerData.instance._coineReward = wallet.coin;
        PlayerData.instance._totalPoint = wallet.coin;
        yield return TimeNow.GetTimeNow(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        var timeNow = response as TimeNow;
        ItemDataObject.instance.timeNowServer = timeNow.timeNow;
        PlayerData.instance.dateTimeServer = (int)(timeNow.timeNow - DateTime.Now).TotalSeconds;
        XTimeManager.instance.SyncTime(timeNow.timeNow);
    }
#endregion

#region Load Ghost Data
    //TODO: get data Ghost
    IEnumerator LoadDataGameplayGhost()
    {
        IWSResponse response = null;
        yield return Monster.GetAllMonster(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetMonster");
            yield break;
        }
        List<Monster> monsters = Monster.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < monsters.Count; i++)
        {
            GhostDetail ghostDetail = ScriptableObject.CreateInstance<GhostDetail>();
            GhostData ghostData = ScriptableObject.CreateInstance<GhostData>();
            ghostDetail.name = monsters[i].name;
            ghostData.name = monsters[i].name;
            GhostDataObject.instance.setUpDataGhostInfo(monsters[i], ghostData, ghostDetail);
            GhostUnitData ghostUnitData = new GhostUnitData();
            ghostUnitData.unitData = ghostData;
            ghostUnitData.detail = ghostDetail;
            GhostDataObject.instance.all_GhostUnitData_list.Add(ghostUnitData);
        }
        yield return UserMonster.GetUserMonster(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetMonster");
            yield break;
        }
        List<UserMonster> userMonsters = UserMonster.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < userMonsters.Count; i++)
        {
            for (int a = 0; a < GhostDataObject.instance.all_GhostUnitData_list.Count; a++)
            {
                if (userMonsters[i].monsterID == GhostDataObject.instance.all_GhostUnitData_list[a].detail.ghostID)
                {
                    GhostDataObject.instance.all_GhostUnitData_list[a].detail.AreaName = userMonsters[i].pointName;
                    InventoryGhostObject.instance.ghostInventoryList.Add(GhostDataObject.instance.all_GhostUnitData_list[a]);
                }
            }
        }
    }
#endregion

#region LoadEnergyLocation
    IEnumerator LoadEnergyLocation()
    {
        IWSResponse response = null;
        yield return EnergyRefillPoint.GetEnergyRefillPoint(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error Energy Location");
            yield break;
        }
        List<EnergyRefillPoint> energyRefillPoints = EnergyRefillPoint.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < energyRefillPoints.Count; i++)
        {
            LocationDetail locationDetail = ScriptableObject.CreateInstance<LocationDetail>();
            locationDetail.name = energyRefillPoints[i].pointName;
            locationDetail._nameLocation = energyRefillPoints[i].pointName;
            locationDetail._locationID = energyRefillPoints[i].pointID;
            locationDetail._locationStrings = energyRefillPoints[i].lat + "," + energyRefillPoints[i].lon;
            MapDataObject.instance.all_Energy_location.Add(locationDetail);
        }
    }
#endregion

#region LoadMonsterLocation
    IEnumerator LoadMonsterSpawnPoint()
    {
        IWSResponse response = null;
        yield return MonsterSpawnPoint.GetMonsterSpawnPoint(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error Monster spawn Location");
            yield break;
        }
        List<MonsterSpawnPoint> monsterSpawns = MonsterSpawnPoint.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < monsterSpawns.Count; i++)
        {
            LocationDetail locationDetail = ScriptableObject.CreateInstance<LocationDetail>();
            locationDetail.name = monsterSpawns[i].pointName;
            locationDetail._nameLocation = monsterSpawns[i].pointName;
            locationDetail._locationID = monsterSpawns[i].pointID;
            locationDetail._locationStrings = monsterSpawns[i].lat + "," + monsterSpawns[i].lon;
            locationDetail._sizeAreaGhost = new Vector3(monsterSpawns[i].radiusLat, 1f, monsterSpawns[i].radiusLon);
            MapDataObject.instance.GhostArea_location.Add(locationDetail);
        }
    }
#endregion

#region Shopping Item
    public IEnumerator LoadShopping()
    {
        IWSResponse response = null;
        yield return ShopItem.GetAllShopItem2(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log(response.InternalErrorsString());
            Debug.Log("Error Get All Shop item");
            yield break;
        }
        Debug.Log("get Shopping Data");
        List<ShopItem> shopTemp = ShopItem.ParseToList(response.RawResult().ToString());
        Debug.Log("cout shop item=> " + shopTemp.Count);
        for (int i = 0; i < shopTemp.Count; i++)
        {
            int increment = Math.Min((100 - 60) / shopTemp.Count, 80);
            UiLoginDisplay.instance.currentLoading += increment;
            ShopDetail shopDetail = ScriptableObject.CreateInstance<ShopDetail>();
            yield return ShoppingDataObject.instance.setupShopDetail(shopTemp[i], shopDetail, () =>
            {
                ShoppingDataObject.instance._allShopDisplay.Add(shopDetail);
                if (shopTemp[i].isHighlight)
                {
                    ShoppingDataObject.instance._allHighlightShop_list.Add(shopDetail);
                }
                else
                {
                    ShoppingDataObject.instance._allNomalShop_list.Add(shopDetail);
                }
            });
        }
    }
#endregion
}
