using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using XSystem;
using FlexGhost.Models;
using System.Collections.Generic;
using System;
using Mapbox.Unity.Location;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Mapbox.Map;

public class LocationGPS_Controller : MonoBehaviour
{
    public static LocationGPS_Controller instance;
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
        DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
        PlayerData.instance._playerCenterLatitude = 13.745506150499459f;
        PlayerData.instance._playerCenterLongitude = 100.53253616682058f;
#endif
    }
    [SerializeField] private bool isDemo = false;
    [Header("Data GPS to Gameplay")]
    [SerializeField] private bool isUpdateGPS;
    [SerializeField] float latitude;
    [SerializeField] float longitude;
    [SerializeField] private Vector2d latitudeLongitude;
    [Header("Waring GPS")]
    [SerializeField] public GameObject _waring_panel;
    [SerializeField] public Text _waring_Head_text;
    [SerializeField] public Text _warning_Info_text;
    [Header("Debug MOD")]
    [SerializeField] private bool isDebugGPS;
    [SerializeField] public GameObject _DebugGPS;
    [SerializeField] public Text _latitude_text;
    [SerializeField] public Text _longitude_text;
    [SerializeField] public Text _id_text;
    public IEnumerator startLocationService()
    {
        Debug.Log("startLocationService{1}");
#if UNITY_ANDROID
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
            Debug.Log("startLocationService{2}");
        }
#endif
        
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled GPS");
            setTextWarning("Not enabled GPS", "Cannot get location, please enable GPS");
            PlayerData.instance._checkGPS_Status = false;
            yield break;
        }
        Debug.Log("startLocationService{3}");
        Input.location.Start();
        int maxWait = 30;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            Debug.Log("startLocationService{4}"+ Input.location.status);
            yield return new WaitForSeconds(1f);
            maxWait--;
        }
        if (maxWait < 1)
        {
            Debug.Log("Time Out");
            PlayerData.instance._checkGPS_Status = false;
            setTextWarning("Time Out", "Cannot get location, please enable GPS");
            yield break;
        }
        //-------------------------------------------------------------------
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("startLocationService{5} faild");

            Debug.Log("Unable to determine device location");
            setTextWarning("Unable location", "Unable to determine device location");
            PlayerData.instance._checkGPS_Status = false;
            yield break;
        }
        else
        {
            Debug.Log("startLocationService{5}" + Input.location.status);
            InvokeRepeating("UpdateGPSData", 3f, 1f);
            isUpdateGPS = true;
        }
        if (isDemo)
        {
            yield return setLocationMovePoint(latitude, longitude);
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        if (focus && !isUpdateGPS)
        {
            StartCoroutine(startLocationService());
        }
    }
    private void UpdateGPSData()
    {
        Debug.Log("startLocationService{6}"+ Input.location.status);
        if (Input.location.status == LocationServiceStatus.Running)
        {
            Debug.Log("startLocationService{7}");
            PlayerData.instance._checkGPS_Status = true;
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
#if !UNITY_EDITOR
            latitudeLongitude.Set(latitude, longitude);
            PlayerData.instance._latitudeLongitude = latitudeLongitude;
            PlayerData.instance._playerCenterLatitude = latitude;
            PlayerData.instance._playerCenterLongitude = longitude;
#endif
            Debug.Log("latitude=> " + latitude + "longitude=> " + longitude);
        }
        else
        {
            Input.location.Stop();
            Debug.Log("Location service is not running.");
            setTextWarning("Can not get location", "Location service is not running.");
            PlayerData.instance._checkGPS_Status = false;
        }
        _latitude_text.text = "Latitude : " + latitude;
        _longitude_text.text = "Longitude : " + longitude;
        _id_text.text = "ID : " + PlayerData.instance._playerUID;
    }
    IEnumerator setLocationMovePoint(float lat, float lon)
    {
        IWSResponse response = null;
        yield return GameAPI.MovePoint(XCoreManager.instance.mXCoreInstance, lat, lon, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error Get move point location");
            yield break;
        }
    }
    private void Update()
    {
        if (isDebugGPS)
        {
            _DebugGPS.SetActive(true);
        }
        else
        {
            _DebugGPS.SetActive(false);
        }
    }
    public IEnumerator updateGPSConytroller()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _latitude_text.text = "Latitude: " + latitude;
            _longitude_text.text = "Longitude: " + longitude;
#if !UNITY_EDITOR
            LocationInfo lastLocation = Input.location.lastData;
            latitude = lastLocation.latitude;
            longitude = lastLocation.longitude;

            latitudeLongitude.Set(latitude, longitude);
            PlayerData.instance._latitudeLongitude = latitudeLongitude;
            PlayerData.instance._playerCenterLatitude = latitude;
            PlayerData.instance._playerCenterLongitude = longitude;
            Debug.Log("latitude=> " + latitude + "longitude=> " + longitude);
#endif
        }
    }
    public void setTextWarning(string header, string Info)
    {
        //_waring_panel.SetActive(true);
        _waring_Head_text.text = header;
        _warning_Info_text.text = Info;
    }
    public void onclickCloseWarningDispaly()
    {
        _waring_Head_text.text = string.Empty;
        _warning_Info_text.text = string.Empty;
    }
}
