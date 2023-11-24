using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "GhostDetail", menuName = "ScriptableObjects/GhostDetail", order = 1)]
public class GhostDetail : ScriptableObject
{
    [Header("GhostData")]
    [SerializeField] public string ghostName = "";
    [SerializeField] public string ghostID = "";
    [SerializeField] public string AreaName = "";
    [Header("Detail")]
    [SerializeField] public Sprite _iconGhost;
    [SerializeField] public Sprite _iconeGhostAR;
    [SerializeField] public RarityType _rarityType;
    [SerializeField] public GameObject _prefabsGhost;
    [SerializeField] public GhostController _ghost_ctr;
    [SerializeField] public AudioClip _ghost_sound;
}
public enum RarityType 
{
    none = 0,
    R = 1,
    SR = 2,
    SSR = 3,
    Legendary = 4,
    All = 5,
}
