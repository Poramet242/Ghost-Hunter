using FlexGhost.Models;
using Mapbox.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class ARUiManager : MonoBehaviour
{
    public static ARUiManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Ui Gameplay")]
    [SerializeField] public bool isCatchGhost;
    [SerializeField] public float time_count;
    [SerializeField] public float HP_count;
    [SerializeField] private Text time_text;
    [SerializeField] private Text nameGhost_text;
    [SerializeField] private Image time_value;
    [SerializeField] private Image hp_value;
    [SerializeField] private GameObject hp_bar;
    [SerializeField] public Button backMane;
    [SerializeField] public Button AR_on;
    [SerializeField] public Button AR_off;
    [Header("Ui Panel")]
    [SerializeField] public GameObject fail_panel;
    [SerializeField] private GameObject success_panel;
    [SerializeField] private GameObject AR_On;
    [SerializeField] private GameObject AR_Off;
    [SerializeField] private Gradient gradient_TimeColor;
    [Header("HP Images")]
    [SerializeField] private Sprite MaxHp_spr;
    [SerializeField] private Sprite SamHP_spr;
    [SerializeField] private Sprite MinHp_spr;

    [SerializeField] public float max;
    public void Initialize(GhostUnitData ghostUnitData)
    {
        HP_count = Mathf.FloorToInt(ghostUnitData.unitData.hp);
        max = ghostUnitData.unitData.hp;
        time_count = 30;
        nameGhost_text.text = ghostUnitData.detail.ghostName;
    }
    private void Start()
    {
       //HP_count = UnityEngine.Random.Range(10, 30);
       //max = HP_count;
       //time_count = 30;
    }
    private void Update()
    {
        if (isCatchGhost)
        {
            return;
        }
        else
        {
            time_count -= Time.deltaTime;
            if (time_count <= 0)
            {
                if (ARGameManager.instance.isCatch)
                {
                    time_count = 0;
                    return;
                }
                else
                {
                    if (ARGameManager.instance.isAR)
                    {
                        if (ARGameManager.instance.Ghost_AR == null)
                        {
                            return;
                        }
                        ARGameManager.instance.Ghost_AR.GetComponent<GhostController>().Is_ghostFade = true;
                        StartCoroutine(ARGameManager.instance.Ghost_AR.GetComponent<GhostController>().setAlphaModel(() =>
                        {
                            Debug.Log("Ghost RUN !!!");
                            time_count = 0;
                            fail_panel.SetActive(true);
                            fail_panel.GetComponent<SuccesDisplay>().setUpFailDisplay(ARGameManager.instance.ghostUnit);
                            return;
                        }));
                    }
                    else
                    {
                        if (ARGameManager.instance.Ghost_cam == null)
                        {
                            return;
                        }
                        ARGameManager.instance.Ghost_cam.GetComponent<GhostController>().Is_ghostFade = true;
                        StartCoroutine(ARGameManager.instance.Ghost_cam.GetComponent<GhostController>().setAlphaModel(() =>
                        {
                            Debug.Log("Ghost RUN !!!");
                            time_count = 0;
                            fail_panel.SetActive(true);
                            fail_panel.GetComponent<SuccesDisplay>().setUpFailDisplay(ARGameManager.instance.ghostUnit);
                            return;
                        }));
                    }
                }
            }
            else
            {
                time_text.text = Mathf.FloorToInt(time_count).ToString();
                time_value.fillAmount = time_count / 30f;
                time_value.color = gradient_TimeColor.Evaluate(time_value.fillAmount);
            }
        }
    }
    public void setCalculateHp(int hit)
    {
        HP_count -= hit;
        if (HP_count <= 0)
        {
            SoundListObject.instance.onPlaySoundSFX(7);
            ARGameManager.instance.isCatch = true;
            hp_bar.SetActive(false);
            if (ARGameManager.instance.isAR)
            {
                ARGameManager.instance.OverrideOrb_AR.SetActive(true);
                ARGameManager.instance.Ghost_AR.GetComponent<GhostController>().GhostPlayStunVFX();
            }
            else
            {
                ARGameManager.instance.OverrideOrb_cam.SetActive(true);
                ARGameManager.instance.Ghost_cam.GetComponent<GhostController>().GhostPlayStunVFX();
            }
            HP_count = 0;
        }
        hp_value.fillAmount = HP_count / max;
        setupHpImages(hp_value.fillAmount);
    }
    public void setupHpImages(float amount)
    {
        if (amount < 0.5f && amount > 0.3f)
        {
            hp_value.sprite = SamHP_spr;
        }
        else if (amount <= 0.3f)
        {
            hp_value.sprite = MinHp_spr;
        }
        else
        {
            hp_value.sprite = MaxHp_spr;
        }
    }
    public void onClickRun()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        GhostDataObject.instance.isARGameplay = false;
        FadeScript.instance.fadeIn = true;
        StartCoroutine(FadeScript.instance.setFadeIN(() =>
        {
            StartCoroutine(setEndGame(false));
        }));
    }
    public void onClickFail(bool isMainmenu)
    {
        if (isMainmenu)
        {
            GhostDataObject.instance.isMainmenu = true;
        }
        else
        {
            GhostDataObject.instance.isMainmenu = false;
        }
        GhostDataObject.instance.isARGameplay = false;
        FadeScript.instance.fadeIn = true;
        StartCoroutine(FadeScript.instance.setFadeIN(() =>
        {
            StartCoroutine(setEndGame(false));
        }));
    }
    public void onClickSuccess(bool isMainmenu)
    {
        if (isMainmenu)
        {
            GhostDataObject.instance.isMainmenu = true;
        }
        else
        {
            GhostDataObject.instance.isMainmenu = false;
        }
        GhostDataObject.instance.isARGameplay = false;
        FadeScript.instance.fadeIn = true;
        StartCoroutine(FadeScript.instance.setFadeIN(() =>
        {
            StartCoroutine(setEndGame(true));
        }));
    }
    public void onClickAR_ON(bool check)
    {
        Debug.Log("AR: " + check);
        SoundListObject.instance.onPlaySoundSFX(0);
        ARGameManager.instance.isAR = check;
        if (check)
        {
            AR_On.SetActive(false);
            AR_Off.SetActive(true);
            ARGameManager.instance.CloseObjectMainCammera(true);
            ARGameManager.instance._gun_AR.GetComponent<GunController>().isShotting = false;
            aimController.instance.gameObject.transform.position = Vector3.zero;
            GhostDataObject.instance.isOpenAR = check;
            if (ARGameManager.instance._doubleDamage)
            {
                ARGameManager.instance.frameAR_ptc.Play();
                ARGameManager.instance.frameAR_ptc.gameObject.SetActive(true);
                ARGameManager.instance.frameCam_ptc.Stop();
                ARGameManager.instance.frameCam_ptc.gameObject.SetActive(false);
                ARGameManager.instance._item_img.SetActive(true);
                
            }
        }
        else
        {
            AR_On.SetActive(true);
            AR_Off.SetActive(false);
            ARGameManager.instance.CloseObjectMainCammera(false);
            ARGameManager.instance._gun_Main.GetComponent<GunController>().isShotting = false;
            GhostDataObject.instance.isOpenAR = check;
            if (ARGameManager.instance._doubleDamage)
            {
                ARGameManager.instance.frameAR_ptc.Stop();
                ARGameManager.instance.frameAR_ptc.gameObject.SetActive(false);
                ARGameManager.instance.frameCam_ptc.Play();
                ARGameManager.instance.frameCam_ptc.gameObject.SetActive(true);
                ARGameManager.instance._item_img.SetActive(true);
            }
        }
    }
    public void onClickCatch(bool check)
    {
        if (ARGameManager.instance.isCatch && check)
        {
            SoundListObject.instance.onPlaySoundSFX(2);
            ARGameManager.instance.OverrideOrb_AR.SetActive(false);
            ARGameManager.instance.OverrideOrb_cam.SetActive(false);
            //InventoryGhostObject.instance.ghostInventoryList.Add(ARGameManager.instance.ghostUnit);
            success_panel.SetActive(true);
            success_panel.GetComponent<SuccesDisplay>().setupSuccessDisplay(ARGameManager.instance.ghostUnit);
        }
    }
    IEnumerator setEndGame(bool success)
    {
        IWSResponse response = null;
        yield return GameAPI.EndBattle(XCoreManager.instance.mXCoreInstance, ARGameManager.instance.ghostUnit.detail.ghostID,
                                       success, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.LogError(response.InternalErrorsString());
            Debug.Log("Error End Game AR");
            yield break;
        }
        SceneTransitionManager.instance.GoToScene(PacketGhostConstants.SCENE_WORD);
    }
}
