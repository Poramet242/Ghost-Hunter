using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class GameAPI
    {

        public static IEnumerator StartBattle(XCore xcoreInst, string monsterID,string pointName, float lat, float lon, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("monsterID", monsterID);
            formData.AddField("pointName", pointName);
            formData.AddField("lat", lat.ToString());
            formData.AddField("long", lon.ToString());

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/battle/start"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator EndBattle(XCore xcoreInst, string monsterID, bool isCapture, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("monsterID", monsterID);
            //formData.AddField("energyUsed", energyUsed);
            formData.AddField("isCapture", isCapture.ToString());

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/battle/end"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator RefillByPoint(XCore xcoreInst, string pointID, float lat, float lon, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("pointID", pointID);
            formData.AddField("lat", lat.ToString());
            formData.AddField("long", lon.ToString());

            yield return xcoreInst.POST<UserEnergyResp>(
                apiPath: Uri.EscapeUriString("/api/v1/energy/refillByPoint"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator ClaimDailyReward(XCore xcoreInst, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/dailyReward/claim"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator RedeemCode(XCore xcoreInst, string code, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("code", code);

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/coupon/redeem"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator PickItem(XCore xcoreInst, float lat, float lon, Item item, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("lat", lat.ToString());
            formData.AddField("long", lon.ToString());
            formData.AddField("pointID", item.pointID);
            formData.AddField("isArmor", item.isArmor.ToString());
            formData.AddField("isWeapon", item.isWeapon.ToString());
            formData.AddField("effectTime", item.effectTime);

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/item/pick"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator ForceCompleteQuest(XCore xcoreInst,string questID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("questID", questID);
            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/quest/forceComplete"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

         public static IEnumerator ClaimQuestReward(XCore xcoreInst,string questID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("questID", questID);
            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/quest/claimReward"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator MovePoint(XCore xcoreInst, float lat, float lon, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("pointID", "point_test");
            formData.AddField("lat", lat.ToString());
            formData.AddField("long", lon.ToString());
            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/test/movePoint"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }




    }

}
