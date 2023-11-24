using FlexGhost.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XSystem;

public class EnergyController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] public bool isCheckDone;
    [SerializeField] public LocationDetail locationDetail;
    [Header("Pin Dispaly")]
    [SerializeField] public Material disableLocationPin_mtr;
    [SerializeField] public Material enableLocationPin_mtr;
    [SerializeField] public GameObject pinpoint_obj;
    [Header("Star Display")]
    [SerializeField] public Material disableLocationStar_mtr;
    [SerializeField] public Material enableLocationStar_mtr;
    [SerializeField] public GameObject star_obj;
    private void Update()
    {
        if (isCheckDone)
        {
            pinpoint_obj.GetComponent<MeshRenderer>().material = disableLocationPin_mtr;
            star_obj.GetComponent<MeshRenderer>().material = disableLocationStar_mtr;
        }
        else
        {
            pinpoint_obj.GetComponent<MeshRenderer>().material = enableLocationPin_mtr;
            star_obj.GetComponent<MeshRenderer>().material = enableLocationStar_mtr;
        }
    }
    private void OnMouseDown()
    {
        if (IsPointerOverUIObject()){return;}
        if (isCheckDone)
        {
            SoundListObject.instance.onPlaySoundSFX(2);
            WarningDisplay.instance.setupWarningDisplay("<color=red>" + "ไม่สามารถรับ Energy ได้" + "</color>", "ดูเหมือนว่าคุณจะเติม Energy ไปแล้ว\nโปรดรอจนกว่าจะครบ 1 วัน ถึงจะสามารถเติม \nEnergy ได้อีกครั้ง", WarningType.ErrorUiDisplay);
            return;
        }
        else
        {
            //TODO: Call back to server get energy in this location
            StartCoroutine(RefillEnergyPoint());
        }
    }


    public IEnumerator checkEnergyPos(string pointID)
    {
        IWSResponse response = null;
        yield return CanRefillEnergyByPointResp.CanRefillEnergyByPoint(XCoreManager.instance.mXCoreInstance, pointID, (r) => response = r);
        var check = response as CanRefillEnergyByPointResp;
        Debug.Log(pointID + " => " + check.canRefill);
        isCheckDone = !check.canRefill;
    }
    IEnumerator RefillEnergyPoint()
    {
        IWSResponse response = null;
        yield return CanRefillEnergyByPointResp.CanRefillEnergyByPoint(XCoreManager.instance.mXCoreInstance, locationDetail._locationID, (r) => response = r);
        var check = response as CanRefillEnergyByPointResp;
        if (check.canRefill)
        {
            yield return GameAPI.RefillByPoint(XCoreManager.instance.mXCoreInstance, locationDetail._locationID, PlayerData.instance._playerCenterLatitude, PlayerData.instance._playerCenterLongitude, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                WarningDisplay.instance.setupWarningDisplay("<color=red>" + "ไม่สามารถรับ Energy ได้" + "</color>", "ดูเหมือนว่าคุณจะอยู่ไกลเกิน\nโปรดเดินเข้าใกล้ เสาเติม Energy ถึงจะสามารถเติม \nEnergy ได้", WarningType.ErrorUiDisplay);
                yield break;
            }
            yield return EnergyResp.GetEnergy(XCoreManager.instance.mXCoreInstance, (r) => response = r);
            var energy = response as EnergyResp;
            int num = energy.energy - PlayerData.instance._current_Energy;
            if (energy.energy >= PlayerData.instance._max_Energy)
            {
                PlayerData.instance._max_Energy = energy.energy;
            }
            else
            {
                PlayerData.instance._max_Energy = 100;
            }
            SoundListObject.instance.onPlaySoundSFX(1);
            PlayerData.instance._current_Energy = energy.energy;
            isCheckDone = true;
            WarningDisplay.instance.setupWarningDisplay("ได้รับ Energy", "ว้าว! คุณได้รับ Energy\nเพิ่มจำนวน " + "<color=#1FED65>" + num + " Energy" + "</color>" + "\nตอนนี้เพียงพอที่จะจับผีแล้ว ออกไปจับผีกันเถอะ !", WarningType.GetEnergy);
            EnergyGameplay.instance.setupDisplay(PlayerData.instance._current_Energy, PlayerData.instance._max_Energy);
        }
        else
        {
            SoundListObject.instance.onPlaySoundSFX(2);
            WarningDisplay.instance.setupWarningDisplay("<color=red>" + "ไม่สามารถรับ Energy ได้" + "</color>", "ดูเหมือนว่าคุณจะเติม Energy ไปแล้ว\nโปรดรอจนกว่าจะครบ 1 วัน ถึงจะสามารถเติม \nEnergy ได้อีกครั้ง", WarningType.ErrorUiDisplay);
            yield break;
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
}
