using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class RedeemRewardsDisplay : MonoBehaviour
{
    public static RedeemRewardsDisplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Ui Display")]
    [SerializeField] public ShopDetail _thisPromotion;
    [SerializeField] public GameObject _thisGameObject;
    [SerializeField] private Image _iconPromotion_img;
    [SerializeField] private Text _nameShop_text;
    [SerializeField] private Text _pricePromotion_text;
    [SerializeField] private Text _info_text;
    [SerializeField] private Text _dateTime_text;
    [SerializeField] private GameObject RedeemNow_btn;
    [SerializeField] private GameObject UnShowRedeem_btn;
    [Header("TEMP DISPLAY")]
    [SerializeField] private Text Nomel_tem_text;
    [SerializeField] private Text PN_tem_text;
    [SerializeField] private Text Entertainment_text;
    [Header("Redem Reward Warning")]
    [SerializeField] public GameObject _redeem_panel;
    [SerializeField] public GameObject _redeemRewardWarning;
    [SerializeField] public GameObject _warning_panel;
    [SerializeField] public GameObject _fail_panel;
    [SerializeField] public GameObject _success_panel;
    [SerializeField] public GameObject _concertFailWarningPanel;
    [SerializeField] public GameObject _waitForSystem_panel;
    [SerializeField] public Text _warning_text;

    [Header("Terms and Conditions")]
    [SerializeField] public GameObject TermsAndConditions_obj;
    [SerializeField] public Text info_text;
    [SerializeField] public Text PNinfo_text;
    [SerializeField] public Text Entertainmentinfo_text;
    [SerializeField] public string info_str;
    [Header("CouponStorage")]
    [SerializeField] public GameObject Coupon_obj;
    public void setupPromotionDisplay(ShopDetail detail)
    {
        _thisPromotion = detail;
        UiDisplayPromotion(detail);
    }
    public void UiDisplayPromotion(ShopDetail detail)
    {
        _iconPromotion_img.sprite = detail._shop_icon[0];
        _nameShop_text.text = detail._shopName;
        _pricePromotion_text.text = detail._priceItem.ToString("#,##0");
        _info_text.text = detail._shopDescription;
        if (detail._shopOffset == "Flex Station @ Lido Connect")
        {
            PN_tem_text.gameObject.SetActive(true);
            Nomel_tem_text.gameObject.SetActive(false);
            Entertainment_text.gameObject.SetActive(false);
            PN_tem_text.text = setUpTermsPartner(detail._shopOffset);
        }
        else
        {
            switch (detail._shopOffset)
            {
                case "Lido Connect Hall 1":
                    Entertainment_text.gameObject.SetActive(true);
                    Nomel_tem_text.gameObject.SetActive(false);
                    PN_tem_text.gameObject.SetActive(false);
                    Entertainment_text.text = setupEntertainment(detail._shopOffset);
                    break;                
                case "บูธ Flex 104.5 บริเวณลานกิจกรรม Centerpoint Siam Square":
                    Entertainment_text.gameObject.SetActive(true);
                    Nomel_tem_text.gameObject.SetActive(false);
                    PN_tem_text.gameObject.SetActive(false);
                    Entertainment_text.text = setupEntertainment(detail._shopOffset);
                    break;                
                default:
                    Nomel_tem_text.gameObject.SetActive(true);
                    PN_tem_text.gameObject.SetActive(false);
                    Entertainment_text.gameObject.SetActive(false);
                    Nomel_tem_text.text = setUpTermsAndConditions(detail._shopOffset);
                    break;
            }
            if (_thisPromotion._shopType == ShopItemCategory.Entertainment)
            {
                Entertainment_text.gameObject.SetActive(true);
                Nomel_tem_text.gameObject.SetActive(false);
                PN_tem_text.gameObject.SetActive(false);
                Entertainment_text.text = setupEntertainment(detail._shopOffset);
            }
        }
        _dateTime_text.text = "ตั้งแต่วันที่ " + detail.startedOn.ToString("dd.MM.yyyy") + " ถึง " + detail.expiredOn.ToString("dd.MM.yyyy");
        RedeemNow_btn.SetActive(true);
        StartCoroutine(CheckItemRedeem(detail));
    }
    IEnumerator CheckItemRedeem(ShopDetail isShop)
    {
        IWSResponse response = null;
        yield return ShopItem.GetAllShopItem(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log(response.InternalErrorsString());
            Debug.Log("Error Get All Shop item");
            yield break;
        }
        List<ShopItem> shopTemp = ShopItem.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < shopTemp.Count; i++)
        {
            if (shopTemp[i].itemID == isShop._shopId)
            {
                //Debug.Log(shopTemp[i].itemID + "AlreabyBuy =>" + shopTemp[i].alreadyBuy);
                //Debug.Log(shopTemp[i].itemID + "CanRedeem =>" + shopTemp[i].canRedeem);
                if (shopTemp[i].alreadyBuy)
                {
                    RedeemNow_btn.SetActive(false);
                    UnShowRedeem_btn.SetActive(true);
                }
                else if (!shopTemp[i].canRedeem)
                {
                    _redeemRewardWarning.SetActive(true);
                    _concertFailWarningPanel.SetActive(true);
                }
                else
                {
                    RedeemNow_btn.SetActive(true);
                    UnShowRedeem_btn.SetActive(false);
                }
            }
        }
    }
    public void onClickRedeemRewards_btn()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        _redeemRewardWarning.SetActive(true);
        _warning_panel.SetActive(true);
        if (_thisPromotion._shopType == ShopItemCategory.WelcomeReward)
        {
            _warning_text.text = "หากใช้งานแล้วคูปองจะหายไป โปรดใช้งานคูปองของคุณ\r\nเมื่ออยู่ต่อหน้าพนักงานเท่านั้น\r\n<color=#FF5858>สามารถใช้คูปองได้ภายในวันที่ 31 ต.ค. 2566</color>";
        }
        else
        {
            _warning_text.text = "หากใช้งานแล้วคูปองจะหายไป โปรดใช้งานคูปองของคุณ\r\nเมื่ออยู่ต่อหน้าพนักงานเท่านั้น\r\n<color=#FF5858>คูปองอยู่ได้ 7 วันหากไม่มีการใช้งาน</color>";
        }
        //_warning_text.text = "คุณต้องการแลกคูปองใช่หรือไม่ หากแลกคูปองงานแล้ว\nคุณจะไม่สามารถแลกคูปองนี้ได้อีกครั้ง เนื่องจากจำกัด 1 คนต่อ 1 สิทธิ์ในการแลก\n " + "<color=red>" + "คูปองมีอายุการใช้งานได้ 7 วันเมื่อทำงานแลกแล้ว" + "</color>";
    }
    public void onClikConfirm()
    {
        //TODO call back to server
        SoundListObject.instance.onPlaySoundSFX(0);
        StartCoroutine(getItemCodeDispaly());
    }
    IEnumerator getItemCodeDispaly()
    {
        _waitForSystem_panel.SetActive(true);
        IWSResponse response = null;
        yield return BuyShopItemResp.BuyShopItem(XCoreManager.instance.mXCoreInstance, _thisPromotion._shopId,(r)=> response =r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error not get Reward");
            SoundListObject.instance.onPlaySoundSFX(2);
            _waitForSystem_panel.SetActive(false);
            _warning_panel.SetActive(false);
            _fail_panel.SetActive(true);
            yield break;
        }
        yield return WalletResp.GetWallet(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        var wallet = response as WalletResp;
        PlayerData.instance._coineReward = wallet.coin;
        PlayerData.instance._totalPoint = wallet.coin;
        _waitForSystem_panel.SetActive(false);
        SoundListObject.instance.onPlaySoundSFX(1);
        _success_panel.SetActive(true);
    }
    public void onClickTermsAndConditions()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        TermsAndConditions_obj.SetActive(true);
        if (_thisPromotion._isPartner)
        {
            info_text.gameObject.SetActive(false);
            PNinfo_text.gameObject.SetActive(true);
            Entertainmentinfo_text.gameObject.SetActive(false);
            PNinfo_text.text = setUpTermsPartner(_thisPromotion._shopOffset);
        }
        else
        {
            switch (_thisPromotion._shopOffset)
            {
                case "Lido Connect Hall 1":
                    Entertainmentinfo_text.gameObject.SetActive(true);
                    info_text.gameObject.SetActive(false);
                    PNinfo_text.gameObject.SetActive(false);
                    Entertainment_text.text = setupEntertainment(_thisPromotion._shopOffset);
                    break;
                case "บูธ Flex 104.5 บริเวณลานกิจกรรม Centerpoint Siam Square":
                    Entertainmentinfo_text.gameObject.SetActive(true);
                    info_text.gameObject.SetActive(false);
                    PNinfo_text.gameObject.SetActive(false);
                    Entertainment_text.text = setupEntertainment(_thisPromotion._shopOffset);
                    break;
                default:
                    info_text.gameObject.SetActive(true);
                    PNinfo_text.gameObject.SetActive(false);
                    Entertainmentinfo_text.gameObject.SetActive(false);
                    info_text.text = setUpTermsAndConditions(_thisPromotion._shopOffset);
                    break;
            }
            if (_thisPromotion._shopType == ShopItemCategory.Entertainment)
            {
                Entertainmentinfo_text.gameObject.SetActive(true);
                info_text.gameObject.SetActive(false);
                PNinfo_text.gameObject.SetActive(false);
                Entertainment_text.text = setupEntertainment(_thisPromotion._shopOffset);
            }
        }
    }
    public string setUpTermsAndConditions(string shopname)
    {
        info_str = "1. รับสิทธิ์และใช้งาน ผ่านแอปพลิเคชัน Flex Hunter ในช่วงวันที่กำหนดเท่านั้น\r" +
            "\n2. ใช้สิทธิได้ที่ "+ "<color=#1EE488>" + shopname + "</color>" + "\r" +
            "\n3. ลูกค้าต้องแสดงสิทธิ์ก่อนใช้บริการ โดยกดรับสิทธิพิเศษ ผ่านแอปพลิเคชัน Flex Hunter และแสดงรหัสที่ร้านค้า ไม่อนุญาตให้บันทึกภาพหน้าจอโทรศัพท์ หรือภาพถ่ายทุกกรณี\r" +
            "\n4. สิทธิพิเศษนี้ไม่สามารถโอนสิทธิให้ผู้อื่น ซื้อขาย หรือแลกเปลี่ยนเป็นเงินสด ส่วนลด หรือสิ่งของอื่นได้\r" +
            "\n5. หากบริษัทฯ ตรวจพบว่ามีการทุจริตหรือทำผิดเงื่อนไข และกรณีมีข้อพิพาท จะถือว่าคำตัดสินของบริษัทฯ เป็นที่สิ้นสุด\r" +
            "\n6. บริษัทขอสงวนสิทธิ์ในการเปลี่ยนแปลงเงื่อนไข โดยไม่ต้องแจ้งให้ทราบล่วงหน้า\r" +
            "\n7. หากมีข้อสงสัยเกี่ยวกับแคมเปญ ติดต่อ www.facebook.com/SiamHalloween  ในวันจันทร์ - ศุกร์ เวลา 10:00-19:00 น. ยกเว้นวันหยุดและวันหยุดนักขัตฤกษ์\r";
        return info_str;
    }
    public string setUpTermsPartner(string shopname)
    {
        info_str = "1. ลูกค้าสามารถกดรับสิทธิ์และรับของรางวัล ณ " + "<color=#1EE488>" + shopname + "</color>" + " ในช่วงวันที่กำหนดเท่านั้น \r" +
            "\n2. ลูกค้าต้องแสดงสิทธิ์ก่อนรับของรางวัล โดยกดรับสิทธิพิเศษ ผ่านแอปพลิเคชัน Flex Hunter ไม่อนุญาตให้บันทึกภาพหน้าจอโทรศัพท์ หรือภาพถ่ายทุกกรณี\r" +
            "\n3. ระยะเวลาการใช้สิทธิ์และรับของรางวัล 1 กันยายน 2566 - 31 ตุลาคม 2566 เฉพาะวันทำการ จันทร์ - ศุกร์ เวลา 11:00-18:00 น. ยกเว้นวันหยุดและวันหยุดนักขัตฤกษ์\r" +
            "\n4. หากไม่มาใช้สิทธิ์และรับของรางวัลตามวันและ เวลาที่กำหนดจะถือว่าสละสิทธิ์\r" +
            "\n5. สิทธิพิเศษนี้ไม่สามารถโอนสิทธิให้ผู้อื่น ซื้อขาย หรือแลกเปลี่ยนเป็นเงินสด ส่วนลด หรือสิ่งของอื่นได้\r" +
            "\n6. หากบริษัทฯ ตรวจพบว่ามีการทุจริตหรือทำผิดเงื่อนไข และกรณีมีข้อพิพาท จะถือว่าคำตัดสินของบริษัทฯ เป็นที่สิ้นสุด\r" +
            "\n7. บริษัทขอสงวนสิทธิ์ในการเปลี่ยนแปลงเงื่อนไข โดยไม่ต้องแจ้งให้ทราบล่วงหน้า\r" +
            "\n8. หากมีข้อสงสัยเกี่ยวกับแคมเปญ ติดต่อ www.facebook.com/SiamHalloween ในวันจันทร์ - ศุกร์ เวลา 10:00-19:00 น. ยกเว้นวันหยุดและวันหยุดนักขัตฤกษ์";
        return info_str;
    }
    public string setupEntertainment(string shopname)
    {
        info_str = "1. รับสิทธิ์และใช้งาน ผ่านแอปพลิเคชัน Flex Hunter ในวันและเวลาที่กำหนด โดยจะระบุไว้ที่หน้าแลกของรางวัลเท่านั้น\r" +
            "\n2. ใช้สิทธิ์ได้ที่ " + "<color=#1EE488>" + shopname + "</color>" + "\r" +
            "\n3. ลูกค้าต้องแสดงสิทธิ์ก่อนรับของรางวัล โดยกดรับสิทธิ์พิเศษ ผ่านแอปพลิเคชัน Flex Hunter ไม่อนุญาตให้บันทึกภาพหน้าจอโทรศัพท์ หรือภาพถ่ายทุกกรณี\r" +
            "\n4. หากไม่มาใช้สิทธิ์และรับของรางวัลตามวันและเวลาที่กำหนด จะถือว่าสละสิทธิ์\r" +
            "\n5. สิทธิพิเศษนี้ไม่สามารถโอนสิทธิให้ผู้อื่น ซื้อขาย หรือแลกเปลี่ยนเป็นเงินสด ส่วนลด หรือสิ่งของอื่นได้\r" +
            "\n6. หากบริษัทฯ ตรวจพบว่ามีการทุจริตหรือทำผิดเงื่อนไข และกรณีมีข้อพิพาท จะถือว่าคำตัดสินของบริษัทฯ เป็นที่สิ้นสุด\r" +
            "\n7. บริษัทขอสงวนสิทธิ์ในการเปลี่ยนแปลงเงื่อนไข โดยไม่ต้องแจ้งให้ทราบล่วงหน้า\r" +
            "\n8. หากมีข้อสงสัยเกี่ยวกับแคมเปญ ติดต่อ www.facebook.com/SiamHalloween ในวันจันทร์ - ศุกร์ เวลา 10:00-19:00 น. ยกเว้นวันหยุดและวันหยุดนักขัตฤกษ์";
        return info_str;
    }
    public void onCloseTermsAndConditions()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        TermsAndConditions_obj.SetActive(false);
        info_text.text = string.Empty;
    }
    public void onClickClose_btn()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        _warning_panel.SetActive(false);
        _fail_panel.SetActive(false);
        _success_panel.SetActive(false);
        _redeemRewardWarning.SetActive(false);
        _concertFailWarningPanel.SetActive(false);
        this.gameObject.SetActive(false);
        UIManager.instance.menu.SetActive(true);
    }
    public void onClickShowRedeem()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        _redeemRewardWarning.SetActive(true);
        _success_panel.SetActive(true);
    }
    public void onClickGoStorage()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        _warning_panel.SetActive(false);
        _fail_panel.SetActive(false);
        _success_panel.SetActive(false);
        _redeemRewardWarning.SetActive(false);
        _concertFailWarningPanel.SetActive(false);
        Coupon_obj.SetActive(true);
        CouponStorageController.instance.uiIsReweed = true;
        ShopDisplayController.instance.isMain = false;
    }
    public void onClkickCloseWarning()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        _warning_panel.SetActive(false);
        _fail_panel.SetActive(false);
        _success_panel.SetActive(false);
        _redeemRewardWarning.SetActive(false);
        _concertFailWarningPanel.SetActive(false);
    }
}
