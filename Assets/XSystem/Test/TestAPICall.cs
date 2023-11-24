using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FlexGhost.Models;
using UnityEngine;

namespace XSystem.Test
{
    public class TestAPICall : MonoBehaviour
    {
        XCore mXCoreInstance;

        private void Awake()
        {
            Application.runInBackground = true;
            mXCoreInstance = XCore.FromConfig(XAPIConfig.New(
                host: "http://localhost",
                port: 1188,
                version: "0.0.1",
                versionCode: 1));
            XUnityDispatcher.Initialize();
        }

        // Start is called before the first frame update
        IEnumerator Start()
        {
            IWSResponse response = null;

            ////////////////////USE THIS PART///////////////////////////

            yield return XUser.RestoreSession(mXCoreInstance, "ytxXBqRbLFp9QDRc",
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

             yield return GameConfig.GetGameConfig(mXCoreInstance,"guestButton", (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            var gameConfig = response as GameConfig;
            Debug.Log(gameConfig.value);

            yield return ShopItemWithCode.GetAllShopItem(mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            var allShopItems = ShopItemWithCode.ParseToList(response.RawResult().ToString());
            Debug.Log(allShopItems[0].codes[0]);

            yield return UserShopItem.GetAllUserItemCode(mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            var allUserShopItems = UserShopItem.ParseToList(response.RawResult().ToString());
            Debug.Log(allUserShopItems[0].itemCode);
            yield return UserShopItem.GetActiveUserItemCode(mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            allUserShopItems = UserShopItem.ParseToList(response.RawResult().ToString());
            Debug.Log(allUserShopItems[0].itemCode);

            /* yield return DailyRewardCountResp.GetDailyRewardCount(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }
              var count = response as DailyRewardCountResp;
              Debug.Log(count.count);

             yield return HighScore.GetHighScores(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }
             var allHighScores = HighScore.ParseToList(response.RawResult().ToString());
             Debug.Log(allHighScores[0].displayName);

             yield return CanRefillEnergyByPointResp.CanRefillEnergyByPoint(mXCoreInstance,"point_2", (r) => response = r);
              if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }
             var canRefill = response as CanRefillEnergyByPointResp;
             Debug.Log(canRefill.canRefill);
             Debug.Log(canRefill.nextRefillTime);

            /* yield return ShopItem.GetAllShopItem(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }
             var allShopItems = ShopItem.ParseToList(response.RawResult().ToString());
             Debug.Log(allShopItems[0].images[0]);

             yield return ShopItem.GetShopItemByCategory(mXCoreInstance, ShopItemCategory.Shopping, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }
             var shopItems = ShopItem.ParseToList(response.RawResult().ToString());
             Debug.Log(shopItems[0].itemID);

             yield return Coupon.GetRedeemLog(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }
             var redeemLog = Coupon.ParseToList(response.RawResult().ToString());
             Debug.Log(redeemLog[0].code);

             yield return BuyShopItemResp.BuyShopItem(mXCoreInstance, "kfc_1", (r) => response = r);
             var r = response as BuyShopItemResp;
             Debug.Log(r.rawResult);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 Debug.LogError(response.InternalErrorsString());
                 yield break;
             }
             Debug.Log(r.code);


             /*yield return Account.GetUserProfile(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }
             var accountData = response as Account;
             Debug.Log(accountData.displayName);

             Item item = new Item();
             item.pointID = "point_2";
             item.isArmor = false;
             item.isWeapon = true;
             item.effectTime = 5;

             yield return GameAPI.PickItem(mXCoreInstance, 13.8371669f, 100.5827545f, item, (r) => response = r);
             var r = response as BaseWSResponse;
             Debug.Log(r.rawResult);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 Debug.LogError(response.InternalErrorsString());
                 yield break;
             }


              yield return Account.SetName(mXCoreInstance, "test1234", (r) => response = r);
              if (!response.Success())
              {
                  Debug.LogError(response.ErrorsString());
                  yield break;
              }
              accountData = response as Account;
              Debug.Log(accountData.displayName);*/

            /*yield return CanClaimRewardResp.GetCanClaimReward(mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            var canClaim = response as CanClaimRewardResp;
            Debug.Log(canClaim.canClaim);

            if (canClaim.canClaim)
            {
                yield return GameAPI.ClaimDailyReward(mXCoreInstance, (r) => response = r);
                if (!response.Success())
                {
                    Debug.LogError(response.ErrorsString());
                    yield break;
                }
                var r = response as BaseWSResponse;
                Debug.Log(r.rawResult);
            }

            yield return GameAPI.StartBattle(mXCoreInstance, "monster_3", 13.8354284f, 100.5807157f, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            var resp = response as BaseWSResponse;
            Debug.Log(resp.rawResult);

            yield return GameAPI.EndBattle(mXCoreInstance, "monster_3", 20, true, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            resp = response as BaseWSResponse;
            Debug.Log(resp.rawResult);

            yield return Monster.GetAllMonster(mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            resp = response as BaseWSResponse;
            var monsters = Monster.ParseToList(response.RawResult().ToString());
            Debug.Log(monsters[0].monsterID);*/



        }
    }
}