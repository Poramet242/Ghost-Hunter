using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "ItemDetail", menuName = "ScriptableObjects/ItemDetail", order = 5)]
public class ItemDetail : ScriptableObject
{
    [SerializeField] public string _itemID;
    [SerializeField] public bool isArmor;
    [SerializeField] public bool isWeapon;
    [SerializeField] public int effectTime = 0;
    [SerializeField] public GameObject itemObj;
}
