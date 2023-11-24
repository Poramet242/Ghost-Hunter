using Mapbox.ProbeExtractorCs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARGameManager : MonoBehaviour
{
    public static ARGameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Buf")]
    [SerializeField] public bool isDragball;
    [SerializeField] public bool _doubleDamage;
    [SerializeField] public bool _DontStamina;
    [Header("Mod")]
    [SerializeField] public bool isAR;
    [SerializeField] public Camera mainCamera;
    [SerializeField] public Camera ARCamera;
    [SerializeField] public GameObject[] AR_MainCamera;
    [Header("Data Gameplay")]
    [SerializeField] public bool isCatch;
    [SerializeField] public GhostUnitData ghostUnit;
    [SerializeField] private Transform content_ghost;
    [Header("Gun")]
    [SerializeField] public GameObject _gun_Main;
    [SerializeField] public GameObject _gun_AR;
    [Header("Camera Mod")]
    [SerializeField] public GameObject Ghost_cam;
    [SerializeField] public GameObject plane_down;
    [SerializeField] public GameObject plane_up;
    [SerializeField] public GameObject isGun_cam;
    [SerializeField] public GameObject isAim;
    [SerializeField] public GameObject isBall_cam;
    [SerializeField] public GameObject pef_ball_cam;
    [SerializeField] public Transform posBall_cam;
    [SerializeField] public GameObject OverrideOrb_cam;
    [Header("AR Mod")]
    [SerializeField] public GameObject Ghost_AR;
    [SerializeField] public float minRange_AR = 5.0f;
    [SerializeField] public float maxRange_AR = 50.0f;
    [SerializeField] public GameObject isGun_AR;
    [SerializeField] public GameObject isBall_AR;
    [SerializeField] public GameObject pef_ball_AR;
    [SerializeField] public Transform posBall_AR;
    [SerializeField] public GameObject OverrideOrb_AR;
    [SerializeField] public GameObject plane_AR;
    [Header("Target")]
    [SerializeField] public Transform AR_Target;
    [SerializeField] public Transform Cam_Target;
    [Header("Particle display")]
    [SerializeField] public ParticleSystem dieCam_ptc;
    [SerializeField] public ParticleSystem dieAR_ptc;
    [Header("Frame Particle")]
    [SerializeField] public GameObject _item_img;
    [SerializeField] public ParticleSystem frameCam_ptc;
    [SerializeField] public ParticleSystem frameAR_ptc;
    private void Start()
    {
        setupStartARGameplay(() => 
        { 
            ARUiManager.instance.onClickAR_ON(GhostDataObject.instance.isOpenAR);
        });
    }
    void setupStartARGameplay(Action callback)
    {
        FadeScript.instance.fadeOut = true;
        SoundListObject.instance.all_SFX.Add(GhostDataObject.instance.ghostDetail._ghost_sound);
        SoundManager.instance.PlaySoundBGM(SoundListObject.instance.all_BGM[0]);
        SoundListObject.instance.onPlaySoundSFX(4);
        ghostUnit = new GhostUnitData();
        ghostUnit.unitData = GhostDataObject.instance.ghostDatas;
        ghostUnit.detail = GhostDataObject.instance.ghostDetail;
        setupGameplayNoneAR(ghostUnit);
        setupGameplayAR_ON(ghostUnit);
        //StartCoroutine(LookAtCamera(Ghost_AR));
        Ghost_cam.GetComponent<GhostController>().spawn_ptc.gameObject.SetActive(true);
        dieCam_ptc = Ghost_cam.GetComponent<GhostController>().die_ptc;
        dieAR_ptc = Ghost_cam.GetComponent<GhostController>().die_ptc;
        Ghost_AR.GetComponent<GhostController>().GhostPlayAnimation(2, false);
        Ghost_cam.GetComponent<GhostController>().GhostPlayAnimation(2, false);
        SoundListObject.instance.onPlaySoundSFX(SoundListObject.instance.all_SFX.Count - 1);
        if (ItemDataObject.instance._isDoubleDamages)
        {
            _doubleDamage = true;
        }
        callback?.Invoke();
    }
    private void Update()
    {
        if (isAR)
        {
            mainCamera.gameObject.SetActive(false);
            for (int i = 0; i < AR_MainCamera.Length; i++)
            {
                AR_MainCamera[i].SetActive(true);
            }
            if (Ghost_AR == null || Ghost_cam == null)
            {
                return;
            }
            Ghost_AR.SetActive(true);
            isBall_AR.SetActive(true);
            isBall_cam.SetActive(false);
            Ghost_cam.SetActive(false);
            if (isCatch)
            {
                ActiceBall(true);
                Ghost_AR.GetComponent<GhostController>().Ghost_animator.SetInteger("State", 1);
            }
            setPlantAR(ARCamera);
        }
        else
        {
            mainCamera.gameObject.SetActive(true);
            for (int i = 0; i < AR_MainCamera.Length; i++)
            {
                AR_MainCamera[i].SetActive(false);
            }
            if (Ghost_AR == null || Ghost_cam == null)
            {
                return;
            }
            Ghost_cam.SetActive(true);
            isBall_cam.SetActive(true);
            isBall_AR.SetActive(false);
            Ghost_AR.SetActive(false);
            if (isCatch)
            {
                ActiceBall(true);
                Ghost_cam.GetComponent<GhostController>().Ghost_animator.SetInteger("State", 1);
            }
        }
    }
    public void ActiceBall(bool Check)
    {
        isAim.SetActive(!Check);
        if (isAR)
        {
            isBall_AR.SetActive(Check);
            isGun_AR.SetActive(!Check);
            isBall_cam.SetActive(false);
            plane_AR.SetActive(true);
        }
        else
        {
            isBall_cam.SetActive(Check);
            isGun_cam.SetActive(!Check);
            isBall_AR.SetActive(false);
            plane_AR.SetActive(false);
        }
    }
    public void setupGameplayNoneAR(GhostUnitData ghostUnitData)
    {
        GameObject ghostObject = Instantiate(ghostUnitData.detail._prefabsGhost, content_ghost);
        ghostObject.name = ghostUnitData.detail.ghostName;
        Ghost_cam = ghostObject;
        ghostObject.SetActive(false);
        ARUiManager.instance.Initialize(ghostUnitData);
    }
    public void setupGameplayAR_ON(GhostUnitData ghostUnitData)
    {
        float x = ARCamera.transform.position.x + GennerateRange();
        float z = ARCamera.transform.position.z + GennerateRange();
        float y = ARCamera.transform.position.y;
        GameObject ghostObject = Instantiate(ghostUnitData.detail._prefabsGhost, new Vector3(x, y, z), Quaternion.identity, content_ghost);
        ghostObject.name = ghostUnitData.detail.ghostName;
        ghostObject.SetActive(false);
        Ghost_AR = ghostObject;
        TranformToCamera(Ghost_AR);
    }
    IEnumerator LookAtCamera(GameObject ARObj)
    {
        while (true)
        {
            if (ARObj == null)
            {
                yield break;
            }
            TranformToCamera(ARObj);
            yield return new WaitForSeconds(3f);
        }
    }
    public void TranformToCamera(GameObject ARObj)
    {
        Vector3 arObjDirection = ARCamera.transform.position - ARObj.transform.position;
        Quaternion arObjRotation = Quaternion.LookRotation(arObjDirection);
        ARObj.transform.rotation = Quaternion.Lerp(ARObj.transform.rotation, arObjRotation, Time.deltaTime);
        ARObj.transform.LookAt(ARCamera.transform);
    }
    public void CloseObjectMainCammera(bool check)
    {
        if (check)
        {
            Ghost_AR.SetActive(true);
            Ghost_cam.SetActive(false);
            plane_down.SetActive(false);
            plane_up.SetActive(false);
        }
        else
        {
            Ghost_AR.SetActive(false);
            Ghost_cam.SetActive(true);
            plane_down.SetActive(true);
            plane_up.SetActive(true);
        }
    }
    public float GennerateRange()
    {
        float range = UnityEngine.Random.Range(minRange_AR, maxRange_AR);
        bool isPositive = UnityEngine.Random.Range(0, 10) < 5;
        return range * (isPositive ? 1 : -1);
    }
    public void BallGameplay(GameObject ball)
    {
        if (isAR)
        {
            if (isCatch)
            {
                Destroy(ball);
                GameObject ballTemp = Instantiate(pef_ball_AR, posBall_AR);
                isBall_AR = ballTemp;
                isBall_AR.transform.position = posBall_AR.position;
                isBall_AR.GetComponent<DragAndThrow>().isTime = true;
            }
            else
            {
                Destroy(ball);
                GameObject ballTemp = Instantiate(pef_ball_AR, posBall_AR);
                isBall_AR = ballTemp;
                isBall_AR.transform.position = posBall_AR.position;
                isBall_AR.GetComponent<DragAndThrow>().isTime = true;
                instance.isGun_AR.SetActive(true);
                instance.isAim.SetActive(true);
            }
            //OverrideOrb_AR.SetActive(true);
        }
        else
        {

            //OverrideOrb_cam.SetActive(true);
            if (isCatch)
            {
                Destroy(ball);
                GameObject ballTemp = Instantiate(pef_ball_cam, posBall_cam);
                isBall_cam = ballTemp;
                isBall_cam.transform.position = posBall_cam.position;
                isBall_cam.GetComponent<DragAndThrow>().isTime = true;
            }
            else
            {
                Destroy(ball);
                GameObject ballTemp = Instantiate(pef_ball_cam, posBall_cam);
                isBall_cam = ballTemp;
                isBall_cam.transform.position = posBall_cam.position;
                isBall_cam.GetComponent<DragAndThrow>().isTime = true;
                isGun_cam.SetActive(true);
                isAim.SetActive(true);
            }
        }
        ARUiManager.instance.backMane.enabled = true;
        ARUiManager.instance.AR_on.enabled = true;
        ARUiManager.instance.AR_off.enabled = true;
    }
    public void HitGhost(int hit)
    {
        ARUiManager.instance.setCalculateHp(hit);
        int num = UnityEngine.Random.Range(1, 4);
        if (isAR)
        {
            Ghost_AR.GetComponent<GhostController>().GhostPlayAnimation(num, true);
        }
        else
        {
            Ghost_cam.GetComponent<GhostController>().GhostPlayAnimation(num, true);
        }
    }
    public void setPlantAR(Camera ARcam)
    {
        if (ARcam.transform.position.y == plane_AR.transform.position.y)
        {
            plane_AR.transform.position = new Vector3(plane_AR.transform.position.x, plane_AR.transform.position.y + (-10), plane_AR.transform.position.z);
        }
    }
    public IEnumerator SpawnDieParticleDisplay(GameObject ghostDie , bool isAR)
    {
        if (isAR)
        {
            ParticleSystem particle = Instantiate(dieAR_ptc, ghostDie.transform);
            particle.gameObject.SetActive(true);
            particle.Play();
            yield break; 
        }
        else
        {
            ParticleSystem particle = Instantiate(dieCam_ptc, content_ghost);
            particle.gameObject.SetActive(true);
            particle.Play();
            yield break;
        }
    }
}
