using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "GhostData", menuName = "ScriptableObjects/GhostData", order = 0)]
public class GhostData : ScriptableObject
{
    [Header("GhostData")]
    [SerializeField] public string ghostName = "";
    [SerializeField] public string ghostID = "";
    [Header("Ghot status")]
    [SerializeField] public int ghostPoint = 0;
    [SerializeField] public int hp = 0;
    [SerializeField] public int energy_Consumption = 0;
    //[SerializeField] public float playTime = 0;
}
