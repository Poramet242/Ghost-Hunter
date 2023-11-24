using FlexGhost.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSystem;

public class TestLoadSprite : MonoBehaviour
{
    public static TestLoadSprite instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] public GameObject _prefabDisplay;
    [SerializeField] public GameObject pointCount;




    public IEnumerator setupCouponStorage()
    {
        IWSResponse response = null;
        yield return UserShopItem.GetAllUserItemCode(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error Get AllUserItemCode");
            yield break;
        }
        List<UserShopItem> userAllCouponStorage = UserShopItem.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < userAllCouponStorage.Count; i++)
        {
            Debug.Log(userAllCouponStorage[i].itemCode);
        }
    }
}
