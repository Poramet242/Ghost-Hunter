using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataObject : MonoBehaviour
{
    public static MapDataObject instance;
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
    [Header("Location")]
    [SerializeField] public List<LocationDetail> all_Energy_location = new List<LocationDetail>();
    [SerializeField] public List<LocationDetail> GhostArea_location = new List<LocationDetail>();
}
