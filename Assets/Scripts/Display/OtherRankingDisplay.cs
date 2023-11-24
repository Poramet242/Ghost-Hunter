using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherRankingDisplay : MonoBehaviour
{
    [Header("Ui Display")]
    [SerializeField] private Text _rankText;
    [SerializeField] private Image _icon;
    [SerializeField] private Text _name;
    [SerializeField] private Text _point;

    public void setupOtherRankDisplay(RankerDetail ranker, int countRank)
    {
        _rankText.text = countRank.ToString();
        _icon.sprite = ranker._iconPlayer;
        _name.text = ranker._namePlayer;
        _point.text = ranker._pointPlayer.ToString();
    }
}
