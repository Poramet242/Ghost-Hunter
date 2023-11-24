using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSystem;

public class TestShopData : MonoBehaviour
{
    public static TestShopData instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] public List<ShopDetail> shopData = new List<ShopDetail>();

    IEnumerator Start()
    {
        IWSResponse response = null;

        ////////////////////USE THIS PART///////////////////////////

        yield return XUser.RestoreSession(XCoreManager.instance.mXCoreInstance, "Hd36QwkgCeAXM1WP",
            (r) =>
            {
                response = r;
            });
        if (response.Success() == false)
        {
            Debug.LogErrorFormat("cannot restore login with session token due to error: {0}", response.ErrorsString());
            Debug.Log(response.RawResult().ToString());
            yield break;
        }

        //yield return regExSearchDataController.instance.getShoppingData();
        //yield return new WaitForSeconds(2f);
        //yield return ItemCodeController.instance.setupItemCodeDisplay();
        yield return TestLoadSprite.instance.setupCouponStorage();
    }
}
