using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGhostObject : MonoBehaviour
{
    public static InventoryGhostObject instance;
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
    [Header("Inventory")]
    [SerializeField] public List<GhostUnitData> ghostInventoryList = new List<GhostUnitData>();

    public List<string> getAllGhostUnit()
    {
        List<string> temp = new List<string>();
        for (int i = 0; i < ghostInventoryList.Count; i++)
        {
            if (!temp.Exists(o=>o== ghostInventoryList[i].detail.ghostID))
            {
                temp.Add(ghostInventoryList[i].detail.ghostID);
            }
        }
        return temp;
    }
}
