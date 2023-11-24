using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayCastGameplayController : MonoBehaviour
{
    [SerializeField] private Camera Cam;
    [SerializeField] private int hit_count;
    [Header("Laser Controller")]
    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject laser_prf;
    [SerializeField] private GameObject _instance;
    [SerializeField] private EGA_Laser laserScript;
    public float MaxLength;
    private Ray RayMouse;
    private Vector3 direction;
    private Quaternion rotation;
    [SerializeField] private bool isMouseButtonDown = false;
    //[SerializeField] private GameObject aim_obj;
    private void Start()
    {
        hit_count = 0;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() || ARGameManager.instance.isCatch || IsPointerOverUIObject())
            {
                Debug.Log("Clicked on UI element! Exiting...");
                GunController.instance.isShotting = false;
                return; // Return out of the code block
            }
            if (!isMouseButtonDown)
            {
                Debug.Log("Clicked on laser");
                EnableLaser();
                Invoke("DisableLaser", 0.08f);
                isMouseButtonDown = true;
            }
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            calculateRaycastHitGhost(ray);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() || ARGameManager.instance.isCatch || IsPointerOverUIObject())
            {
                Debug.Log("Clicked on UI element! Exiting...");
                GunController.instance.isShotting = false;
                return; // Return out of the code block
            }
            isMouseButtonDown = false;
            GunController.instance.isShotting = false;
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
    public void EnableLaser()
    {
        Destroy(_instance);
        _instance = Instantiate(laser_prf, firePoint.transform.position, firePoint.transform.rotation);
        _instance.transform.parent = firePoint.transform;
        _instance.transform.localRotation = Quaternion.Euler(Vector3.zero);
        laserScript = _instance.GetComponent<EGA_Laser>();
    }
    public void DisableLaser()
    {
        laserScript.DisablePrepare();
        Destroy(_instance, 1);
    }
    public void RotateToMouseDirection(GameObject obj, Vector3 destination)
    {
        direction = destination - obj.transform.position;
        rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }
    public void calculateRaycastHitGhost(Ray ray)
    {
        SoundListObject.instance.onPlaySoundSFX(5);
        RaycastHit hitGhost;
        if (Physics.Raycast(ray, out hitGhost, float.MaxValue))
        {
            //Debug.Log(hitGhost.point);
            Vector3 lookRotation = hitGhost.point - transform.GetChild(0).position;
            transform.GetChild(0).rotation = Quaternion.LookRotation(lookRotation);

            if (hitGhost.collider.tag == "Ghost")
            {
                hit_count = 1;
                if (ItemDataObject.instance._isDoubleDamages)
                {
                    hit_count = (hit_count * 2);
                }
                ARGameManager.instance.HitGhost(hit_count);
                GunController.instance.isShotting = true;
                GunController.instance._animtor.Play("Shotting");
                Debug.Log("Hit Ghost!!!");
            }
            else
            {
                GunController.instance.isShotting = false;
                GunController.instance._animtor.Play("Idle");
                Debug.Log("Dont Hit Ghost!!!");
            }
        }
    }
}
