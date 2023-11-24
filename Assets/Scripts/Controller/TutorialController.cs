using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private ScrollSnapRect scrollSnap;
    [SerializeField] private int current;
    [SerializeField] private int count;
    [Header("GUI")]
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private GameObject _perv_btn;
    [SerializeField] private GameObject _next_btn;
    [SerializeField] private GameObject _star_btn;
    private void OnEnable()
    {
        _next_btn.SetActive(true);
        _perv_btn.SetActive(true);
        _tutorial.SetActive(false);
        _star_btn.SetActive(false);
    }
    private void Update()
    {
        current = scrollSnap._currentPage;
        count = scrollSnap._pageCount;
        if ((current+1) == count)
        {
            //_next_btn.SetActive(false);
            //_star_btn.SetActive(true);
            _star_btn.SetActive(true);
        }
    }
    public void onClickplayGame()
    {
        GameManager.instance.isUiDispaly = false;
        scrollSnap.onClickScreen(0);
        _next_btn.SetActive(true);
        _perv_btn.SetActive(true);
        _tutorial.SetActive(false);
        _star_btn.SetActive(false);
        PlayerPrefs.SetString("Tutorial", "true");
    }
}
