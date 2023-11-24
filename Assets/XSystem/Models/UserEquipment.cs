using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace FlexGhost.Models
{
    public class UserEquipment : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string userID;
        public bool activeArmor;
        public DateTime armorEndTimeStamp;
        public DateTime armorStartTimeStamp;
        public bool activeWeapon;
        public DateTime weaponEndTimeStamp;
        public DateTime weaponStartTimeStamp;




        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            Debug.Log(jObj.ToString());
            if (jObj["success"].AsBool == false)
            {
                return;
            }
            var data = jObj["data"].AsObject;
            data = data["entities"].AsObject;
            if (data == null || data.ToString() == string.Empty || data.ToString() == "{}")
            {
                data = jObj;
            }
            this.id = data["id"].Value;
            this.createdOn = Utility.ParseDatetime(data["createdOn"].Value);
            this.updatedOn = Utility.ParseDatetime(data["updatedOn"].Value);
            this.userID = data["userID"].Value;
            this.activeArmor = data["activeArmor"].AsBool;
            this.armorEndTimeStamp = Utility.ParseDatetime(data["armorEndTimeStamp"].Value);
            this.armorStartTimeStamp = Utility.ParseDatetime(data["armorStartTimeStamp"].Value);
            this.activeWeapon = data["activeWeapon"].AsBool;
            this.weaponEndTimeStamp = Utility.ParseDatetime(data["weaponEndTimeStamp"].Value);
            this.weaponStartTimeStamp = Utility.ParseDatetime(data["weaponStartTimeStamp"].Value);


        }
        public void ParseFromJSONObject2(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            var data = jObj["data"].AsObject;
            data = data["entities"].AsObject;
            if (data == null || data.ToString() == string.Empty || data.ToString() == "{}")
            {
                data = jObj;
            }
            this.id = data["id"].Value;
            this.createdOn = Utility.ParseDatetime(data["createdOn"].Value);
            this.updatedOn = Utility.ParseDatetime(data["updatedOn"].Value);
            this.userID = data["userID"].Value;
            this.activeArmor = data["activeArmor"].AsBool;
            this.armorEndTimeStamp = Utility.ParseDatetime(data["armorEndTimeStamp"].Value);
            this.armorStartTimeStamp = Utility.ParseDatetime(data["armorStartTimeStamp"].Value);
            this.activeWeapon = data["activeWeapon"].AsBool;
            this.weaponEndTimeStamp = Utility.ParseDatetime(data["weaponEndTimeStamp"].Value);
            this.weaponStartTimeStamp = Utility.ParseDatetime(data["weaponStartTimeStamp"].Value);
        }
        public static List<UserEquipment> ParseToList(string jsonString)
        {
            List<UserEquipment> userBattleSessions = new List<UserEquipment>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new UserEquipment();
                item.ParseFromJSONObject2(itemJson);
                userBattleSessions.Add(item);
            }

            return userBattleSessions;

        }

        public static IEnumerator GetUserEquipment(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<UserEquipment>(
                apiPath: Uri.EscapeUriString("/api/v1/gamedata/userEquipment"),
                headers: null,
                callback: callback,
                apiTrackCode: -1);
        }
    }
}