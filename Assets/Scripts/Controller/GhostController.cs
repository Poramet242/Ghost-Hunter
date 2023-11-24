using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using XSystem;

public class GhostController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] public GhostData ghostData;
    [SerializeField] public GhostDetail ghostDetail;
    [SerializeField] public GameObject GhostObject;
    [Header("Gameplay")]
    [SerializeField] public Animator Ghost_animator;
    [SerializeField] public Collider Ghost_collider;
    [Header("Material")]
    [SerializeField] public bool Is_ghostFade;
    [SerializeField] public GameObject ObjMain_mtr;
    [SerializeField] public Material Ghost_Run;
    [Header("Spawn rate")]
    [SerializeField] public float timeToAlive = 0f;
    [SerializeField] public float maxTimeAlive = 20f;
    [Header("Effect")]
    [SerializeField] public GameObject particlePos;
    [SerializeField] public ParticleSystem spawn_ptc;
    [SerializeField] public ParticleSystem stun_ptc;
    [Header("Particle Instant")]
    [SerializeField] public ParticleSystem die_ptc;
    private void Start()
    {
        if (GhostDataObject.instance.isARGameplay)
        {
            Ghost_Run.color = new Color(0, 0, 0, 1f);
            Ghost_collider.enabled = false;
        }
        else
        {
            Ghost_collider.enabled = true;
            spawn_ptc.gameObject.SetActive(false);
            die_ptc.gameObject.SetActive(false);
        }
    }
    private void OnMouseDown()
    {
        if (IsPointerOverUIObject())
        {
            return;
        }
        else
        {
            if(GhostDataObject.instance.isARGameplay)
            { return; }
            if(PlayerData.instance._current_Energy < ghostData.energy_Consumption)
            {
                WarningDisplay.instance.setupWarningDisplay("<color=red>" + "Energy ไม่เพียงพอ" + "</color>", "ดูเหมือนว่า Energy ของคุณจะไม่เพียงพอต่อการ\nจับผี โปรดรอจนกว่า Energy ของคุณจะเพียงพอ\nต่อการจับผี", WarningType.noneStamina);
                return;
            }
            else
            {
                setupEnnergyToGameplay();
            }
        }
    }
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult r in results)
        {
            if (r.gameObject.GetComponent<RectTransform>() != null)
                return true;
        }
        return false;
    }
    public void setupEnnergyToGameplay()
    {
        StartCoroutine(setStartARGameplay());
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //GhostPlayAnimation("Idle");
            spawn_ptc.Play();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            stun_ptc.gameObject.SetActive(true);
            stun_ptc.Play();
            Ghost_animator.SetInteger("State", 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GhostPlayAnimation(2, false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            int num = UnityEngine.Random.Range(1, 4);
            GhostPlayAnimation(num, true);
        }
#endif
    }
    public void GhostPlayStunVFX()
    {
        stun_ptc.gameObject.SetActive(true);
        stun_ptc.Play();
    }
    public void GhostPlayAnimation(int state,bool isDamage)
    {
        if (isDamage)
        {
            switch (state)
            {
                case 1:
                    Ghost_animator.SetTrigger("DM1");
                    break;

                case 2:
                    Ghost_animator.SetTrigger("DM2");
                    break;

                case 3:
                    Ghost_animator.SetTrigger("DM3");
                    break;
            }
        }
        else
        {
            Ghost_animator.SetInteger("State", state);
        }
    }
    public IEnumerator setAlphaModel(Action callback)
    {
        Material x = Ghost_Run;
        ObjMain_mtr.GetComponent<SkinnedMeshRenderer>().material = Ghost_Run;
        float original = Ghost_Run.color.a;
        while (Is_ghostFade)
        {
            original -= Time.deltaTime;
            Ghost_Run.color = new Color(0, 0, 0, original);
            if (original <= 0)
            {
                Ghost_Run = x;
                Is_ghostFade = false;
                callback?.Invoke();
            }
            else
            {
                yield return null;
            }
        }
        //Debug.Log("Is_ghostFade: "+Is_ghostFade);
        //callback?.Invoke();
    }
    IEnumerator setStartARGameplay()
    {
        IWSResponse response = null;
        yield return GameAPI.StartBattle(XCoreManager.instance.mXCoreInstance, ghostDetail.ghostID, ghostDetail.AreaName,
                                         PlayerData.instance._playerCenterLatitude, PlayerData.instance._playerCenterLongitude, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log(response.InternalErrorsString());
            Debug.Log("Error Strat Game AR");
            yield break;
        }
        PacketGhostSceneManager[] managers = FindObjectsOfType<PacketGhostSceneManager>();
        foreach (PacketGhostSceneManager item in managers)
        {
            if (item.gameObject.activeSelf)
            {
                item.GhostTapped(this.gameObject);
            }
        }
    }
}
