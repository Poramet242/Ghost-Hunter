using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Text_Controller : MonoBehaviour
{
    [Header("Text controller")]
    [SerializeField] Text[] allTextUI;
    //[SerializeField] TMP_Text[] allText;
    private void Awake()
    {
        /*allText = FindObjectsOfType<TMP_Text>();

        for (int i = 0; i < allText.Length; i++)
        {
            allText[i].text = ThaiFontAdjuster.Adjust(allText[i].text);
        }*/


    }
    private void Update()
    {
        allTextUI = FindObjectsOfType<Text>();
        for (int i = 0; i < allTextUI.Length; i++)
        {
            allTextUI[i].text = ThaiFontAdjuster.Adjust(allTextUI[i].text);
        }
    }
}
