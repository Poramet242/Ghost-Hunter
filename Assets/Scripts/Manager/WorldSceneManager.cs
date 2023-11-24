using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSceneManager : PacketGhostSceneManager
{
    public override void PlayerTapped(GameObject player)
    {

    }
    public override void GhostTapped(GameObject ghost)
    {
        //SceneManager.LoadScene(PacketGhostConstants.SCENE_CAPTURE, LoadSceneMode.Additive);
        List<GameObject> Ghostlist = new List<GameObject>();
        Ghostlist.Add(ghost);
        GhostDataObject.instance.ghostDatas = ghost.GetComponent<GhostController>().ghostData;
        GhostDataObject.instance.ghostDetail = ghost.GetComponent<GhostController>().ghostDetail;
        GhostDataObject.instance.isARGameplay = true;
        FadeScript.instance.fadeIn = true;
        GhostDataObject.instance.isMainmenu = false;
        MouseCameraController.instance.SavePos();
        StartCoroutine(FadeScript.instance.setFadeIN(() => 
        { 
            SceneTransitionManager.instance.GoToScene(PacketGhostConstants.SCENE_CAPTURE);
        }));
    }
}
