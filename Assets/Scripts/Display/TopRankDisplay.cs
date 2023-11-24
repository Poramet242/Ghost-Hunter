using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopRankDisplay : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Text _name;
    [SerializeField] private Text _point;

    public void setupTopRankDisplay(RankerDetail ranker)
    {
        _icon.sprite = ranker._iconPlayer;
        _name.text = ranker._namePlayer;
        _point.text = ranker._pointPlayer.ToString();
    }
}
