using FlexGhost.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class RankingController : MonoBehaviour
{
    [Header("Other Display")]
    [SerializeField] public StageUiDisplay uiDisplay;
    [SerializeField] public GameObject other_game;
    [SerializeField] public GameObject other_ui;
    [Header("Player Ranking")]
    [SerializeField] public GameObject _playerRankDisplay_gameplay;
    [SerializeField] public GameObject _playerRankDisplay_uiplay;
    [Header("Top ranking")]
    [SerializeField] public List<GameObject> _topGameObjectList = new List<GameObject>();
    [Header("Other ranking")]
    [SerializeField] public List<GameObject> _otherGameObjectList = new List<GameObject>();
    [SerializeField] public Transform _otherTransform_gameplay;
    [SerializeField] public Transform _otherTransform_uiplay;
    [SerializeField] public GameObject _otherPrefabs;
    [Header("Color")]
    [SerializeField] public Color otherColor;
    private void OnEnable()
    {
        StartCoroutine(setupDataRanking());
    }
    private void OnDisable()
    {
        clearDataRanking();
    }
    public void setupAllPlayerRanking(List<RankerDetail> _allRanking, StageUiDisplay stageUi)
    {
        _allRanking = _allRanking.OrderByDescending((x) => x._pointPlayer).ToList();
        for (int i = 0; i < _allRanking.Count; i++)
        {
            if (i <= 2)
            {
                RankingDataObject.instance._topRankingList.Add(_allRanking[i]);
                continue;
            }
            RankingDataObject.instance._otherRankingList.Add(_allRanking[i]);
        }
        setupTopRanking();
        setupOtherRanking(stageUi);
    }
    public void setupTopRanking()
    {
        for (int i = 0; i < _topGameObjectList.Count; i++)
        {
            _topGameObjectList[i].GetComponent<TopRankDisplay>().setupTopRankDisplay(RankingDataObject.instance._topRankingList[i]);
        }
    }
    public void setupOtherRanking(StageUiDisplay stageUi)
    {
        for (int i = 0; i < RankingDataObject.instance._otherRankingList.Count; i++)
        {
            switch (stageUi)
            {
                case StageUiDisplay.GameDisplay:
                    other_game.SetActive(true);
                    other_ui.SetActive(false);
                    GameObject otherplayer_gaemplay = Instantiate(_otherPrefabs, _otherTransform_gameplay);
                    otherplayer_gaemplay.SetActive(true);
                    _otherGameObjectList.Add(otherplayer_gaemplay);
                    int num = (i + 1) + 3;
                    _otherGameObjectList[i].GetComponent<OtherRankingDisplay>().setupOtherRankDisplay(RankingDataObject.instance._otherRankingList[i], num);
                    break;
                case StageUiDisplay.Ranking:
                    other_game.SetActive(false);
                    other_ui.SetActive(true);
                    GameObject otherplayer_ui = Instantiate(_otherPrefabs, _otherTransform_uiplay);
                    otherplayer_ui.SetActive(true);
                    _otherGameObjectList.Add(otherplayer_ui);
                    int num_ui = (i + 1) + 3;
                    _otherGameObjectList[i].GetComponent<OtherRankingDisplay>().setupOtherRankDisplay(RankingDataObject.instance._otherRankingList[i], num_ui);
                    break;
            }
        }
    }
    public void setupRankingPlayer(RankerDetail rank, StageUiDisplay stageUi)
    {
        switch (stageUi)
        {
            case StageUiDisplay.GameDisplay:
                _playerRankDisplay_gameplay.GetComponent<PlayerRankingDisplay>().setupOtherRankDisplay(rank, rank._rankPlayer);
                break;
            case StageUiDisplay.Ranking:
                _playerRankDisplay_uiplay.GetComponent<PlayerRankingDisplay>().setupOtherRankDisplay(rank, rank._rankPlayer);
                break;
        }
    }
    public IEnumerator setupDataRanking()
    {
        UIManager.instance.loading_display.SetActive(true);
        UIManager.instance._logLoading_text.text = "กำลังโหลดข้อมูล อันดับผู้เล่น";
        IWSResponse response = null;
        yield return HighScore.GetHighScores(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error Data Ranking");
            yield break;
        }
        List<HighScore> highScores = HighScore.ParseToList(response.RawResult().ToString());
        highScores.Select(z => z.uid).Distinct().ToList();
        for (int i = 0; i < highScores.Count; i++)
        {
            RankerDetail ranker = ScriptableObject.CreateInstance<RankerDetail>();
            yield return RankingDataObject.instance.setupDateRanking(highScores[i], ranker, () => 
            {
                RankingDataObject.instance._allRanking.Add(ranker);
            });
        }
        setupAllPlayerRanking(RankingDataObject.instance._allRanking,UIManager.instance.stageUiDisplay);
        yield return HighScore.GetUserHighScore(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error Data Ranking");
            yield break;
        }
        var userScore = response as HighScore;
        RankerDetail rankerUser = ScriptableObject.CreateInstance<RankerDetail>();
        rankerUser._namePlayer = userScore.displayName;
        rankerUser._pointPlayer = userScore.score;
        rankerUser._rankPlayer = userScore.rank;
        rankerUser._iconPlayer = PlayerData.instance._iconplayer;
        RankingDataObject.instance._playerRank = rankerUser;
        setupRankingPlayer(rankerUser,UIManager.instance.stageUiDisplay);
        UIManager.instance.loading_display.SetActive(false);

    }
    public void onClickCloseRanking()
    {
        switch (UIManager.instance.stageUiDisplay)
        {
            case StageUiDisplay.GameDisplay:
                SoundListObject.instance.onPlaySoundSFX(0);
                clearDataRanking();
                UIManager.instance.ranking_Ui.SetActive(false);
                UIManager.instance.onClickGamePlay();
                GameManager.instance.isUiDispaly = false;
                break;
            case StageUiDisplay.Ranking:
                SoundListObject.instance.onPlaySoundSFX(0);
                clearDataRanking();
                UIManager.instance.ranking_Ui.SetActive(false);
                UIManager.instance.onClickLocation_btn();
                UIManager.instance.menu.SetActive(true);
                break;
        }
    }
    public void clearDataRanking()
    {
        if (_otherGameObjectList.Count == 0)
            return;
        for (int i = 0; i < _otherGameObjectList.Count; i++)
        {
            Destroy(_otherGameObjectList[i]);
        }
        _otherGameObjectList.Clear();
        RankingDataObject.instance._allRanking.Clear();
        RankingDataObject.instance._topRankingList.Clear();
        RankingDataObject.instance._otherRankingList.Clear();
    }
}
