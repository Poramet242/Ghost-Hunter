using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataObject : MonoBehaviour
{
    public static ItemDataObject instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [Header("Item Spawn data")]
    [SerializeField] public float waitTime = 180.0f;
    [SerializeField] public float minRange = 5.0f;
    [SerializeField] public float maxRange = 50.0f;
    [SerializeField] public DateTime timeNowServer;
    [Header("Object Gameplay")]
    [SerializeField] public GameObject _gunObject;
    [SerializeField] public GameObject _armerObject;
    [SerializeField] public List<ItemDetail> itemInLocation = new List<ItemDetail>();
    [Header("DoubleDamages")]
    [SerializeField] public bool _isDoubleDamages;
    [SerializeField] public double _currentDoubleDamages;
    [SerializeField] public double _doubleDamagesMax;
    [SerializeField] public DateTime doubleDamageMaxTime;
    [Header("ArmorUpper")]
    [SerializeField] public bool _isArmorUpper;
    [SerializeField] public double _currentArmorUpper;
    [SerializeField] public double _armorUpperMax;
    [SerializeField] public DateTime armorUpperMaxTime;
}
