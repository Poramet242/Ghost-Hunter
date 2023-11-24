using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "RankerDetail", menuName = "ScriptableObjects/RankerDetail", order = 3)]
public class RankerDetail : ScriptableObject
{
    [Header("Leader")]
    [SerializeField] public string _namePlayer;
    [SerializeField] public int _rankPlayer;
    [SerializeField] public int _pointPlayer;
    [SerializeField] public string _Icon_url;
    [SerializeField] public Sprite _iconPlayer;
}
