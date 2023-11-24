using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class QuestDisplay : MonoBehaviour
{
    [Header("Quest")]
    //[SerializeField] public int countGhost;
    [SerializeField] public QuestType questType;
    [Header("Data")]
    [SerializeField] public string _questID = "";
    [SerializeField] public string _descriptionQuest = "";
    [SerializeField] public int point = 0;
    [SerializeField] public bool ClaimItems;
    [SerializeField] public bool Succeed;
    [SerializeField] public GameObject thisObject;
    [SerializeField] public Text _text_btn;
    [Header("Sprite")]
    [SerializeField] private Color c_claim;
    [SerializeField] private Sprite claim_spr;
    [SerializeField] private Sprite get_spr;
    [SerializeField] private Sprite none_spr;

    private void Start()
    {
        thisObject = this.gameObject;
    }
    private void Update()
    {
        if (Succeed)
        {
            if (ClaimItems)
            {
                gameObject.GetComponent<Button>().enabled = false;
                gameObject.GetComponent<Image>().sprite = get_spr;
                _text_btn.text = "รับแล้ว";
            }
            else
            {
                gameObject.GetComponent<Button>().enabled = true;
                gameObject.GetComponent<Image>().sprite = claim_spr;
                _text_btn.text = "พร้อมรับ";
            }
        }
        else
        {
            gameObject.GetComponent<Button>().enabled = false;
            _text_btn.text = point + " เหรียญ";
            /*if (ClaimItems)
            {
                gameObject.GetComponent<Button>().enabled = false;
                gameObject.GetComponent<Image>().color = Color.gray;
                gameObject.GetComponent<Image>().sprite = none_spr;
                _text_btn.text = "รับแล้ว";
            }
            else
            {
                gameObject.GetComponent<Button>().enabled = false;
                _text_btn.text = point + " คะแนน";
            }*/
        }
    }
    public void onClickClaimQuest()
    {
        SoundListObject.instance.onPlaySoundSFX(0);
        StartCoroutine(setClaimQuest());
    }
    IEnumerator setClaimQuest()
    {
        QuestsController.instance.loading.SetActive(true);
        IWSResponse response = null;
        yield return GameAPI.ClaimQuestReward(XCoreManager.instance.mXCoreInstance, _questID, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error Claim Quest");
            yield break;
        }
        yield return WalletResp.GetWallet(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        var wallet = response as WalletResp;
        PlayerData.instance._coineReward = wallet.coin;
        ClaimItems = true;
        QuestsController.instance.loading.SetActive(false);
    }
}

public enum QuestTypeNone
{
    none = 0,
    R = 1,
    SR = 2,
    SSR = 3,
    Legendary = 4,
    All = 5,
    AllQuest = 6,
}
