using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class RankingDataObject : MonoBehaviour
{
    public static RankingDataObject instance;
    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    [SerializeField] public List<RankerDetail> _allRanking = new List<RankerDetail>();
    [Header("Player Ranking")]
    [SerializeField] public RankerDetail _playerRank;
    [Header("Top ranking")]
    [SerializeField] public List<RankerDetail> _topRankingList = new List<RankerDetail>();
    [Header("Other ranking")]
    [SerializeField] public List<RankerDetail> _otherRankingList = new List<RankerDetail>();

    public IEnumerator setupDateRanking(HighScore highScore, RankerDetail ranker, Action callback)
    {
        ranker._namePlayer = highScore.displayName;
        ranker._rankPlayer = highScore.rank;
        ranker._pointPlayer = highScore.score;
        if (string.IsNullOrEmpty(highScore.displayImageID))
        {
            ranker._iconPlayer = LoadUnitObject.instance.PlayerIconDef_spr;
        }
        else
        {
            yield return LoadUnitObject.instance.setLoadSpriteFromURL(highScore.displayImageID, (sprite) =>
            {
                ranker._iconPlayer = sprite;
            });
        }
        callback?.Invoke();
        yield break;
    }

}
