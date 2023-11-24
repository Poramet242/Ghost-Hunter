using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchManager : MonoBehaviour
{
    [SerializeField] public bool isHit;
    [SerializeField] public GameObject Ghost_main;
    [SerializeField] public GameObject Ghost_AR;
    private void Update()
    {
        Ghost_main = ARGameManager.instance.Ghost_cam;
        Ghost_AR = ARGameManager.instance.Ghost_AR;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ghost")
        {
            if (isHit)
            {
                return;
            }
            StartCoroutine("CatchGhost", other.gameObject);

            /*if (ARGameManager.instance.isCatch)
            {
                StartCoroutine("CatchGhost", other.gameObject);
            }
            else
            {
                ARUiManager.instance.onClickCatch(false);
            }*/
        }
    }

    IEnumerator CatchGhost(GameObject ghost)
    {
        Debug.Log("Catch Ghost !!!!");
        isHit = true;
        this.gameObject.GetComponent<DragAndThrow>().ball_trail.enabled = false;
        transform.Translate(Vector3.up * 1, Space.World);
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (ARGameManager.instance.isAR)
        {
            Ghost_AR.SetActive(false);
            float cal = (float)(((1 - (ARUiManager.instance.HP_count / ARUiManager.instance.max)) + 0.2) / 1.2);
            //Random cal => 100 to gameplay in capture Gameplay
            float ran = Random.Range(cal, 1);
            if (ran >= 0.9f)
            {
                //TODO: Show onClickCatch
                SoundListObject.instance.onPlaySoundSFX(8);
                yield return ARGameManager.instance.SpawnDieParticleDisplay(ghost, ARGameManager.instance.isAR);
                Destroy(Ghost_AR);
                yield return new WaitForSeconds(1f);
                this.GetComponent<Rigidbody>().useGravity = true;
                yield return new WaitForSeconds(1f);
                ARGameManager.instance.isCatch = true;
                ARUiManager.instance.isCatchGhost = true;
                ARUiManager.instance.onClickCatch(true);
            }
            else
            {
                //TODO: Show fail_panel
                ARGameManager.instance.Ghost_AR.GetComponent<GhostController>().Is_ghostFade = true;
                yield return ARGameManager.instance.Ghost_AR.GetComponent<GhostController>().setAlphaModel(() =>
                {
                    Debug.Log("Ghost RUN !!!");
                    Destroy(Ghost_AR);
                });
                yield return new WaitForSeconds(1f);
                SoundListObject.instance.onPlaySoundSFX(3);
                ARUiManager.instance.fail_panel.SetActive(true);
                ARUiManager.instance.fail_panel.GetComponent<SuccesDisplay>().setUpFailDisplay(ARGameManager.instance.ghostUnit);
            }
        }
        else
        {
            Ghost_main.SetActive(false);
            float cal = (float)(((1 - (ARUiManager.instance.HP_count / ARUiManager.instance.max)) + 0.2) / 1.2);
            float ran = Random.Range(cal, 1);
            Debug.Log("ran = " + ran);
            if (ran >= 0.9f)
            {
                //TODO: Show onClickCatch
                SoundListObject.instance.onPlaySoundSFX(8);
                yield return ARGameManager.instance.SpawnDieParticleDisplay(ghost, ARGameManager.instance.isAR);
                Destroy(Ghost_main);
                yield return new WaitForSeconds(1f);
                this.GetComponent<Rigidbody>().useGravity = true;
                yield return new WaitForSeconds(1f);
                ARGameManager.instance.isCatch = true;
                ARUiManager.instance.isCatchGhost = true;
                ARUiManager.instance.onClickCatch(true);
            }
            else
            {
                //TODO: Show fail_panel
                ARGameManager.instance.Ghost_cam.GetComponent<GhostController>().Is_ghostFade = true;
                yield return ARGameManager.instance.Ghost_cam.GetComponent<GhostController>().setAlphaModel(() =>
                {
                    Debug.Log("Ghost RUN !!!");
                    Destroy(Ghost_main);
                });
                yield return new WaitForSeconds(1f);
                SoundListObject.instance.onPlaySoundSFX(3);
                ARUiManager.instance.fail_panel.SetActive(true);
                ARUiManager.instance.fail_panel.GetComponent<SuccesDisplay>().setUpFailDisplay(ARGameManager.instance.ghostUnit);
            }
        }
        // ------------------------- Zoom in Monster -----------------------------------------------------------
        //|yield return new WaitForSeconds(1f);                                                                 |
        //|this.GetComponent<Rigidbody>().useGravity = true;                                                    |
        //|yield return new WaitForSeconds(1f);                                                                 |
        //|if (ARGameManager.instance.isAR)                                                                     |
        //|{                                                                                                    |
        //|    ARGameManager.instance.ARCamera.transform.LookAt(this.transform);                                |
        //|    ARGameManager.instance.ARCamera.gameObject.GetComponent<Camera>().fieldOfView = 0.2f;            |
        //|}                                                                                                    |
        //|else                                                                                                 |
        //|{                                                                                                    |
        //|    ARGameManager.instance.mainCamera.transform.LookAt(this.transform);                              |
        //|    ARGameManager.instance.mainCamera.gameObject.GetComponent<Camera>().fieldOfView = 0.2f;          |
        //|}                                                                                                    |
        //|ARUiManager.instance.isCatchGhost = true;                                                            |
        //|ARUiManager.instance.onClickCatch(true);                                                             |
        // -----------------------------------------------------------------------------------------------------
    }
}
