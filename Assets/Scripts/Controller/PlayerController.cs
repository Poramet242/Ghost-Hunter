using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Location Provider")]
    [SerializeField] private bool resetMap;
    [SerializeField] private AbstractMap AbstractMap;
    bool _isInitialized;
    ILocationProvider _locationProvider;
    [SerializeField] private bool isTest;
    [SerializeField] float x = 13.745506150499459f;
    [SerializeField] float y = 100.53253616682058f;
    [Header("Player")]
    [SerializeField] public List<GameObject> ghots = new List<GameObject>();
    private void Start()
    {
        LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;
    }
    public IEnumerator setLocationProviderFactory()
    {
        if (PlayerPrefs.GetString("FirstTime")== "true")
        {
            yield return GameManager.instance.LoadDataGameplayProfile();
            yield return GameManager.instance.GetMonsterInLocation();
            yield break;
        }
        else
        {
            PlayerPrefs.SetString("FirstTime", "true");
            PlayerPrefs.Save();
            SceneManager.LoadScene("BlockupMapDisplay");
            yield break;
        }
    }
    ILocationProvider LocationProvider
    {
        get
        {
            if (_locationProvider == null)
            {
                _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            }

            return _locationProvider;
        }
    }
    Vector3 _targetPosition;
    void LateUpdate()
    {
        #region CTR Player transform
        if (_isInitialized)
        {
            var map = LocationProviderFactory.Instance.mapManager;

            if (isTest)
            {
                transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
            }
            else
            {
#if UNITY_EDITOR
                x = PlayerData.instance._playerCenterLatitude;
                y = PlayerData.instance._playerCenterLongitude;
                Vector2d v2 = new Vector2d(x, y);
                /// V2 => PlayerData.instance._latitudeLongitude
                transform.localPosition = map.GeoToWorldPosition(v2);
#endif

#if !UNITY_EDITOR
                Vector2d v2 = new Vector2d(PlayerData.instance._playerCenterLatitude, PlayerData.instance._playerCenterLongitude);
                transform.localPosition = map.GeoToWorldPosition(v2);
#endif
            }
        }
#endregion
    }
}
