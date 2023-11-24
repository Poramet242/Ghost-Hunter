using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
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
    }
    [Header("Daily")]
    [SerializeField] public bool _isPlayTutorial;
    [SerializeField] public bool _checkDaily;
    [Header("Location Player")]
    [SerializeField] public bool _checkGPS_Status;
    [SerializeField] public float _playerCenterLatitude;
    [SerializeField] public float _playerCenterLongitude;
    [Header("Vector2d")]
    [SerializeField] public Vector2d _latitudeLongitude;
    [Header("Player Info")]
    [SerializeField] public string _playerName = "";
    [SerializeField] public string _playerUID = "";
    [SerializeField] public string _iconPlayerURL = "";
    [SerializeField] public Sprite _iconplayer;
    [Header("Total point")]
    [SerializeField] public int _totalPoint = 0; //=>coine wall
    [SerializeField] public int _totalMaxPoint = 0; //=>score acc
    [SerializeField] public int _totalGhost = 0;
    [Header("Energy")]
    [SerializeField] public int _current_Energy;
    [SerializeField] public int _max_Energy = 100;
    [SerializeField] public int dateTimeServer = 0;
    [Header("Coine")]
    [SerializeField] public int _coineReward = 0;
    [Header("Save Pos Cam")]
    [SerializeField] public Vector3 savedPosition;
    [SerializeField] public Quaternion savedRotation;
    [SerializeField] public Vector3 savedOffset;
}
