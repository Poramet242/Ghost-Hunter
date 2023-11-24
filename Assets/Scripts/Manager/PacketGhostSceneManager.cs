using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PacketGhostSceneManager : MonoBehaviour
{
    public abstract void PlayerTapped(GameObject player);
    public abstract void GhostTapped(GameObject ghots);

}
