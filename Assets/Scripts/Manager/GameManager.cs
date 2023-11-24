using FlexGhost.Models;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    [Header("Bigger Map")]
    [SerializeField] public bool isBig_map = false;
    [SerializeField] public bool isUiDispaly;
    [SerializeField] public Camera cam_ctr;
    private float defView = 25f;
    private float bigView = 70f;
    [Header("Map Data")]
    [SerializeField] private AbstractMap _map;
    [SerializeField][Geocode] string[] _locationStrings;
    private Vector2d[] _locations;
    [Header("Model location")]
    [SerializeField] private GameObject _markerPrefab;
    [SerializeField] private float _spawnScale = 100f;
    [SerializeField] private List<GameObject> _spawnedObjects;
    [Header("Ghost Area")]
    [SerializeField][Geocode] string[] _areaGhostLocationStrings;
    private Vector2d[] _areaGhostlocations;
    [SerializeField] public bool isLocationMap;
    [SerializeField] public Camera _locationCamera;
    [SerializeField] private GameObject _ghostArea_prefab;
    [SerializeField] private List<GameObject> _ghostAreaObjects;
    [Header("RenderTexture")]
    [SerializeField] public RenderTexture _locationRenderTexture_mini;
    [SerializeField] public RenderTexture _locationRenderTexture_big;
    [Header("Player")]
    [SerializeField] public PlayerController currentPlayer;

    private void Start()
    {
        FadeScript.instance.fadeOut = true;
        //TODO: Play sound BGM to main gameplay
        DataHolder.SFX_Volume = PlayerPrefs.GetFloat("SFX_Volume");
        DataHolder.BGM_Volume = PlayerPrefs.GetFloat("BGM_Volume");
        SoundManager.instance.PlaySoundBGM(SoundListObject.instance.all_BGM[0]);
        _locationStrings = new string[MapDataObject.instance.all_Energy_location.Count];
        for (int i = 0; i < MapDataObject.instance.all_Energy_location.Count; i++)
        {
            _locationStrings[i] = MapDataObject.instance.all_Energy_location[i]._locationStrings;
        }
        StartCoroutine(startGameplay());
    }
    IEnumerator startGameplay()
    {
        setupEnergyLocation();
        yield return LocationGPS_Controller.instance.startLocationService();
        yield return currentPlayer.setLocationProviderFactory();
    }
    private void Update()
    {
        int count = _spawnedObjects.Count;
        for (int i = 0; i < count; i++)
        {
            var spawnedObject = _spawnedObjects[i];
            var location = _locations[i];
            spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
            spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            spawnedObject.transform.position = new Vector3(spawnedObject.transform.position.x, 4f, spawnedObject.transform.position.z);
        }
        int countArea = _ghostAreaObjects.Count;
        for (int i = 0; i < countArea; i++)
        {
            var areaSpawnedObject = _ghostAreaObjects[i];
            var location = _areaGhostlocations[i];
            areaSpawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
            areaSpawnedObject.transform.localScale = new Vector3(MapDataObject.instance.GhostArea_location[i]._sizeAreaGhost.x / 10f, 1f, MapDataObject.instance.GhostArea_location[i]._sizeAreaGhost.z / 10f);
            areaSpawnedObject.transform.position = new Vector3(areaSpawnedObject.transform.position.x, 1f, areaSpawnedObject.transform.position.z);
        }
        if (isUiDispaly)
        {
            cam_ctr.gameObject.GetComponent<MouseCameraController>().enabled = false;
        }
        else
        {
            cam_ctr.gameObject.GetComponent<MouseCameraController>().enabled = true;
        }
        setupMapLocationTextureRender(isBig_map);
    }
    public void setupMapLocationTextureRender(bool checkBig)
    {
        if (checkBig)
        {
            _locationCamera.targetTexture = _locationRenderTexture_big;
            _locationCamera.fieldOfView = bigView;
        }
        else
        {
            _locationCamera.targetTexture = _locationRenderTexture_mini;
            _locationCamera.fieldOfView = defView;
        }
    }
    public void setupEnergyLocation()
    {
        _locations = new Vector2d[_locationStrings.Length];
        _spawnedObjects = new List<GameObject>();
        for (int i = 0; i < _locationStrings.Length; i++)
        {
            var locationString = _locationStrings[i];
            _locations[i] = Conversions.StringToLatLon(locationString);
            var instance = Instantiate(_markerPrefab,this.gameObject.transform);
            instance.name = _locationStrings[i];
            instance.GetComponent<EnergyController>().locationDetail = MapDataObject.instance.all_Energy_location[i];
            instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            _spawnedObjects.Add(instance);
        }
        for (int i = 0; i < _spawnedObjects.Count; i++)
        {
            StartCoroutine(_spawnedObjects[i].GetComponent<EnergyController>().checkEnergyPos(_spawnedObjects[i].GetComponent<EnergyController>().locationDetail._locationID));
        }
    }
    public void setupAreaLocation()
    {
        clearAreaGhostLocationOld();
        //Get Location in data map
        _areaGhostLocationStrings = new string[MapDataObject.instance.GhostArea_location.Count];
        for (int i = 0; i < MapDataObject.instance.GhostArea_location.Count; i++)
        {
            _areaGhostLocationStrings[i] = MapDataObject.instance.GhostArea_location[i]._locationStrings;
        }
        //Set Location in gameplay
        _areaGhostlocations = new Vector2d[_areaGhostLocationStrings.Length];
        _ghostAreaObjects = new List<GameObject>();
        for (int i = 0; i < _areaGhostLocationStrings.Length; i++)
        {
            var areaString = _areaGhostLocationStrings[i];
            _areaGhostlocations[i] = Conversions.StringToLatLon(areaString);
            var instanceArea = Instantiate(_ghostArea_prefab, this.gameObject.transform);
            instanceArea.name = MapDataObject.instance.GhostArea_location[i]._nameLocation;
            instanceArea.transform.localPosition = _map.GeoToWorldPosition(_areaGhostlocations[i], true);
            instanceArea.transform.localScale = MapDataObject.instance.GhostArea_location[i]._sizeAreaGhost;
            _ghostAreaObjects.Add(instanceArea);
        }
    }
    public void clearAreaGhostLocationOld()
    {
        for (int i = 0; i < _ghostAreaObjects.Count; i++)
        {
            Destroy(_ghostAreaObjects[i]);
        }
        //MapDataObject.instance.GhostArea_location.Clear();
        _ghostAreaObjects.Clear();
    }
    public IEnumerator LoadDataGameplayProfile()
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
        //--------------------------------------------GET USE MONSTER--------------------------------------------------
        InventoryGhostObject.instance.ghostInventoryList.Clear();
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
        //--------------------------------------------------------------------------------------------------------------------
        UIManager.instance.loading_display.SetActive(false);
    }
    //last update location
    public IEnumerator GetMonsterInLocation()
    {
        IWSResponse response = null;
        //Debug.Log("GetMonsterInLocation (1)");
        while (true)
        {
            while (PlayerData.instance._playerCenterLatitude == 0 || PlayerData.instance._playerCenterLongitude == 0)
            {
                UIManager.instance.loading_display.SetActive(true);
                UIManager.instance._logLoading_text.text = "กำลังโหลดข้อมูล แผนที่และตำแหน่งที่ตั้งของคุณ";
                yield return null;
            }
            UIManager.instance.loading_display.SetActive(false);
            //Debug.Log("GetNearbyMonster (2)");
            GhostDataObject.instance.GhostUnitData_listZone.Clear();
            yield return Monster.GetNearbyMonster(XCoreManager.instance.mXCoreInstance, PlayerData.instance._playerCenterLatitude, PlayerData.instance._playerCenterLongitude, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                Debug.Log(response.InternalErrorsString());
                Debug.Log("Error Get Location Monster");
                WarningDisplay.instance.setupWarningDisplay("ตรวจพบการเคลื่อนที่ผิดปกติ", "คุณไม่ควรเล่นเกมในขณะขับรถ อาจจะทำให้เกิดอุบัติเหตุได้\nกรุณากลับเข้ามาเล่นใหม่ ภายหลัง 15 นาที", WarningType.ErrorServer);
            }
            List<Monster> monsters = Monster.ParseToList(response.RawResult().ToString());
            if (monsters.Count == 0 || monsters == null)
            {
                EvenLocationController .instance._faceDisplay.SetActive(true);
            }
            else
            {
                EvenLocationController.instance._faceDisplay.SetActive(false);
            }
            for (int i = 0; i < monsters.Count; i++)
            {
                for (int a = 0; a < GhostDataObject.instance.all_GhostUnitData_list.Count; a++)
                {
                    if (monsters[i].monsterID == GhostDataObject.instance.all_GhostUnitData_list[a].detail.ghostID)
                    {
                        GhostDataObject.instance.all_GhostUnitData_list[a].detail.AreaName = monsters[i].pointName;
                        GhostDataObject.instance.GhostUnitData_listZone.Add(GhostDataObject.instance.all_GhostUnitData_list[a]);
                    }
                }
            }
            GhostFactory.instance.Initialize();
            if (EvenLocationController.instance.thisObject.active == true)
            {
                EvenLocationController.instance.clearData();
                EvenLocationController.instance.Initialize();
            }
            yield return GetItemInLocation(response);
            //Debug.Log("WaitForSeconds (3)");
            GhostFactory.instance.GenerateStart();
            yield return new WaitForSeconds(25f);
        }
    }
    IEnumerator GetItemInLocation(IWSResponse response)
    {
        ItemDataObject.instance.itemInLocation.Clear();
        yield return Item.GetNearbyItem(XCoreManager.instance.mXCoreInstance, PlayerData.instance._playerCenterLatitude, PlayerData.instance._playerCenterLongitude, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error Get Location Item");
            yield break;
        }
        List<Item> items = Item.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < items.Count; i++)
        {
            ItemDetail item = ScriptableObject.CreateInstance<ItemDetail>();
            item._itemID = items[i].pointID;
            item.isArmor = items[i].isArmor;
            item.isWeapon = items[i].isWeapon;
            item.effectTime = items[i].effectTime;
            if (item.isArmor)
            {
                item.itemObj = LoadUnitObject.instance.GetItemPrefabs("Armer");
                item.itemObj.GetComponent<ItemController>().isArmer = true;
                item.itemObj.GetComponent<ItemController>()._timeCooldown = items[i].effectTime;
            }
            if (item.isWeapon)
            {
                item.itemObj = LoadUnitObject.instance.GetItemPrefabs("Gun");
                item.itemObj.GetComponent<ItemController>().isGun = true;
                item.itemObj.GetComponent<ItemController>()._timeCooldown = items[i].effectTime;
            }
            ItemDataObject.instance.itemInLocation.Add(item);
            ItemFactory.instance.Initialize();
        }
    }
    public IEnumerator getUserEquipment()
    {
        IWSResponse response = null;
        yield return UserEquipment.GetUserEquipment(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserEquipment");
            yield break;
        }
        var equipment = response as UserEquipment;
        if (equipment.activeWeapon)
        {
            ItemDataObject.instance._isDoubleDamages = equipment.activeWeapon;
            ItemDataObject.instance.doubleDamageMaxTime = equipment.weaponEndTimeStamp;
            ItemDataObject.instance._currentDoubleDamages = XTimeManager.instance.TimeUntilSeconds(equipment.weaponEndTimeStamp);
            //Debug.Log("Pase to double: " + ItemDataObject.instance._currentDoubleDamages);
            ItemDataObject.instance._doubleDamagesMax = setTimeNowItem(equipment.weaponEndTimeStamp, equipment.weaponStartTimeStamp);
        }
        else
        {
            ItemDataObject.instance._isDoubleDamages = equipment.activeWeapon;
            ItemDataObject.instance.doubleDamageMaxTime = equipment.weaponEndTimeStamp;
            ItemDataObject.instance._currentDoubleDamages = 0f;
            ItemDataObject.instance._doubleDamagesMax = 0f;
        }
        //-----------------------------------------------------------------------------------------------
        if (equipment.activeArmor)
        {
            ItemDataObject.instance._isArmorUpper = equipment.activeArmor;
            ItemDataObject.instance.armorUpperMaxTime = equipment.armorEndTimeStamp;
            ItemDataObject.instance._currentArmorUpper = XTimeManager.instance.TimeUntilSeconds(equipment.armorEndTimeStamp);
            //Debug.Log("Pase to double: " + ItemDataObject.instance._currentArmorUpper);
            ItemDataObject.instance._armorUpperMax = setTimeNowItem(equipment.armorEndTimeStamp, equipment.armorStartTimeStamp);
        }
        else
        {
            ItemDataObject.instance._isArmorUpper = equipment.activeArmor;
            ItemDataObject.instance.armorUpperMaxTime = equipment.armorEndTimeStamp;
            ItemDataObject.instance._currentArmorUpper = 0f;
            ItemDataObject.instance._armorUpperMax = 0f;
        }
    }
    public double setTimeNowItem(DateTime timeOld, DateTime timeNow)
    {
        TimeSpan timeSpan = timeOld - timeNow;
        double timeDifference = timeSpan.TotalSeconds;// + PlayerData.instance.dateTimeServer;
        //Debug.Log("Pase to double: " + timeDifference);
        return timeDifference;
    }
}
