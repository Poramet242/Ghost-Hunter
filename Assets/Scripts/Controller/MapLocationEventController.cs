using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapLocationEventController : MonoBehaviour
{
    public static MapLocationEventController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("MAP DATA")]
    [SerializeField] private string accessToken;
    [SerializeField] public float centerLatitude = 13.75249f;
    [SerializeField] public float centerLongitude = 100.4935f;
    [SerializeField] private float zoom = 12.0f;
    [SerializeField] private int bearing = 0;
    [SerializeField] private int pitch = 0;

    [SerializeField] private enum style {Light,Dark,streets,Outdoors, Satellite,SatelliteStreets};
    [SerializeField] private style mapStyle = style.streets;
    [SerializeField] private enum resolution {low =1,high = 2 };
    [SerializeField] private resolution mapResolution = resolution.low;

    private int mapWidth = 800;
    private int mapHeight = 600;
    private string[] styleStr = new string[] { "light-v10","dark-v10","streets-v11","outdoors-v11","satellite-v9","satellite-streets-v11"};
    private string url = "";
    private bool mapIsloading = false;
    private Rect rects;
    private bool updateMap = true;


    private string accessTokenLast;
    private float centerLatitudeLast;
    private float centerLongitudeLast;
    private float zoomLast;
    private int bearingLast;
    private int pitchLast;
    private style mapStyleLast = style.streets;
    private resolution mapResolutionLast = resolution.low;

    private void Update()
    {
        if (updateMap && (accessTokenLast != accessToken || !Mathf.Approximately(centerLatitudeLast,centerLatitude) || !Mathf.Approximately(centerLongitudeLast, centerLongitude)
            || zoomLast != zoom || bearingLast != bearing || pitchLast != pitch || mapStyleLast != mapStyle || mapResolutionLast != mapResolution))
        {
            rects = gameObject.GetComponent<RawImage>().rectTransform.rect;
            mapWidth = (int)Mathf.Round(rects.width);
            mapHeight = (int)Mathf.Round(rects.height);
            StartCoroutine(GetMapbox());
            updateMap = false;
        }
    }
    public void setUpGetMapImages()
    {
        StartCoroutine(GetMapbox());
        rects = gameObject.GetComponent<RawImage>().rectTransform.rect;
        mapWidth = (int)Mathf.Round(rects.width);
        mapHeight = (int)Mathf.Round(rects.height);
    }
    IEnumerator GetMapbox()
    {
        url = "https://api.mapbox.com/styles/v1/mapbox/dark-v10/static/" + centerLongitude + "," + centerLatitude + "," + zoom + "," + bearing + "," + pitch + "/" +
            +mapWidth + "x" + mapHeight + "?" + "access_token=" + accessToken;
        mapIsloading = true;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.responseCode != 200)
        {
            Debug.LogError("www Error" + www.error);
        }
        else
        {
            mapIsloading = false;
            gameObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            accessTokenLast = accessToken;
            centerLatitudeLast =centerLatitude;
            centerLongitudeLast = centerLongitude;
            zoomLast = zoom;
            bearingLast = bearing;
            pitchLast = pitch;
            mapStyleLast = mapStyle;
            mapResolutionLast = mapResolution;
            updateMap = true;
        }
    }

}
