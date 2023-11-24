using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LocationDetail", menuName = "ScriptableObjects/LocationDetail", order = 4)]
public class LocationDetail : ScriptableObject
{
    [SerializeField] public string _nameLocation;
    [SerializeField] public string _locationID;
    [SerializeField][Geocode] public string _locationStrings;
    [SerializeField] public Vector3 _sizeAreaGhost;
}
