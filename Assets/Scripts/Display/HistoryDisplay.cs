using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryDisplay : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] public string history_str;
    [Header("GUI Dispaly")]
    [SerializeField] public Text history_txt;
    [SerializeField] private Color daily_cr;
    [SerializeField] private Color energy_cr;


    public void setupHistoryDisplay(string str)
    {
        history_str = str;

        string[] parts = str.Split(' ');

        if (parts.Length >= 4)
        {
            Debug.Log("Parts string=> "+parts.Length);
            string datePart = parts[0] + " " + parts[1]; // "16/08/2566 16:06"
            string codePart = parts[2] + " " + parts[3]; // "ใช้รหัส L6M238E5O8NC1"
            string energyPart = "";
            for (int i = 4; i < parts.Length; i++)
            {
                energyPart = energyPart + parts[i]; // "ได้รับ Energy 5"
                if (i < parts.Length)
                {
                    energyPart = energyPart + " ";
                }
            }
            if (parts.Length > 9)
            {
                energyPart = "\n" + energyPart;
            }
            string formattedText = "<color=#F178B6>" + datePart + "</color>" + " " + codePart + " " + "<color=#1EE488>" + energyPart + "</color>";
            history_txt.text = formattedText;
        }
        else
        {
            Debug.LogWarning("Invalid input string format.");
        }
    }
}
