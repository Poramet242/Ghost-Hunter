using FlexGhost.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XSystem;

public class ItemController : MonoBehaviour
{
    [Header("Object item")]
    [SerializeField] public bool isGun;
    [SerializeField] public bool isArmer;
    [SerializeField] public float _timeCooldown = 10f;
    [SerializeField] public ItemDetail _itemDetail;
    [Header("Spawn rate")]
    [SerializeField] public float timeToAlive = 0f;
    [SerializeField] public float maxTimeAlive = 30f;
    private void OnMouseDown()
    {
        if (IsPointerOverUIObject())
        {
            return;
        }
        else
        {
            if (isGun)
            {
                /*ItemDataObject.instance._isDoubleDamages = true;
                ItemDataObject.instance._currentDoubleDamages += _timeCooldown;
                ItemDataObject.instance._doubleDamagesMax += _timeCooldown;*/
                Item item = new Item();
                item.pointID = _itemDetail._itemID;
                item.isArmor = _itemDetail.isArmor;
                item.isWeapon = _itemDetail.isWeapon;
                item.effectTime = _itemDetail.effectTime;
                SoundListObject.instance.onPlaySoundSFX(1);
                StartCoroutine(getItemGameplay(item));


            }
            if (isArmer)
            {
                /*ItemDataObject.instance._isArmorUpper = true;
                ItemDataObject.instance._currentArmorUpper += _timeCooldown;
                ItemDataObject.instance._armorUpperMax += _timeCooldown;*/
                Item item = new Item();
                item.pointID = _itemDetail._itemID;
                item.isArmor = _itemDetail.isArmor;
                item.isWeapon = _itemDetail.isWeapon;
                item.effectTime = _itemDetail.effectTime;
                SoundListObject.instance.onPlaySoundSFX(1);
                StartCoroutine(getItemGameplay(item));

            }
        }
    }
    IEnumerator getItemGameplay(Item item)
    {
        IWSResponse response = null;
        yield return GameAPI.PickItem(XCoreManager.instance.mXCoreInstance, PlayerData.instance._playerCenterLatitude, PlayerData.instance._playerCenterLongitude, item, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log(response.InternalErrorsString());
            Debug.Log("Error Pick Item");
            yield break;
        }
        if (item.isWeapon)
        {
            yield return GameManager.instance.getUserEquipment();
            Destroy(this.gameObject);
            ItemFactory.instance.AliveItemList.Remove(this.gameObject);
            WarningDisplay.instance.setupWarningDisplay("ได้รับ Bomb", "ว้าวดูเหมือนว่าคุณจะเก็บไอเท็ม Bomb ได้\nมาลองใช้ไอเท็มนี้ในการจับผีกันเถอะ !!", WarningType.GetDoubleDamages);
        }
        else if (item.isArmor)
        {
            yield return GameManager.instance.getUserEquipment();
            Destroy(this.gameObject);
            ItemFactory.instance.AliveItemList.Remove(this.gameObject);
            WarningDisplay.instance.setupWarningDisplay("ได้รับ Antibiotics", "ว้าวดูเหมือนว่าคุณจะเก็บไอเท็ม Antibiotics ได้\nมาลองใช้ไอเท็มนี้ในการจับผีกันเถอะ !!", WarningType.GetArmorUpper);
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
