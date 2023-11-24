using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndThrow : MonoBehaviour
{
    public static DragAndThrow instance;
    private void Awake()
    {
        if (instance ==null)
        {
            instance = this;
        }
    }
    [Header("Catch Ball")]
    [SerializeField] private bool dragging = false;
    private float distance;
    [SerializeField] private float throwSpeed;
    [SerializeField] private float archSpeed;
    [SerializeField] private float _Speed;
    [SerializeField] private float throwForce = 10f;
    [Header("Object Position")]
    [SerializeField] public bool isTime;
    [SerializeField] private float _isTime = 5f;
    [Header("VFX")]
    [SerializeField] public TrailRenderer ball_trail;
    private void Start()
    {
        ball_trail.enabled = false;
    }
    private void OnMouseDown()
    {
        if (IsPointerOverUIObject())
        {
            return;
        }
        if (ARGameManager.instance.isAR)
        {
            distance = Vector3.Distance(transform.position, ARGameManager.instance.ARCamera.transform.position);
            ARGameManager.instance.OverrideOrb_AR.SetActive(false);
            ARGameManager.instance.isGun_AR.SetActive(false);
            ARGameManager.instance.isAim.SetActive(false);
            ARUiManager.instance.backMane.enabled = false;
            ARUiManager.instance.AR_on.enabled = false;
            ARUiManager.instance.AR_off.enabled = false;
            ball_trail.enabled = true;
        }
        else
        {
            distance = Vector3.Distance(transform.position,Camera.main.transform.position);
            ARGameManager.instance.OverrideOrb_cam.SetActive(false);
            ARGameManager.instance.isGun_cam.SetActive(false);
            ARGameManager.instance.isAim.SetActive(false);
            ARUiManager.instance.backMane.enabled = false;
            ARUiManager.instance.AR_on.enabled = false;
            ARUiManager.instance.AR_off.enabled = false;
            ball_trail.enabled = true;
        }
        dragging = true;
    }
    private void OnMouseUp()
    {
        if (IsPointerOverUIObject())
        {
            return;
        }
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().velocity += this.transform.forward * throwSpeed;
        this.GetComponent<Rigidbody>().velocity += this.transform.up * archSpeed;
        isTime = false;
        dragging = false;
        SoundListObject.instance.onPlaySoundSFX(6);
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
    private void Update()
    {
        if (dragging)
        {
            if (ARGameManager.instance.isAR)
            {
                Ray ray = ARGameManager.instance.ARCamera.ScreenPointToRay(Input.mousePosition);
                Vector3 rayPoint = ray.GetPoint(distance);
                transform.position = Vector3.Lerp(this.transform.position, rayPoint, (_Speed * Time.deltaTime) + 2);
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 rayPoint = ray.GetPoint(distance);
                transform.position = Vector3.Lerp(this.transform.position, rayPoint, (_Speed * Time.deltaTime) + 2);
            }

        }
        if (!isTime)
        {
            _isTime -= Time.deltaTime;
            if (_isTime <= 0 && !ARUiManager.instance.isCatchGhost)
            {
                ARGameManager.instance.BallGameplay(this.gameObject);
            }
        }
    }
}
