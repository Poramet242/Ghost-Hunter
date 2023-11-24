using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XTimeManager : MonoBehaviour
{
    public static XTimeManager instance;
    private double timeDiff;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SyncTime(DateTime serverTime)
    {
        var timeSpanDiff = DateTime.Now.Subtract(serverTime);
        timeDiff = timeSpanDiff.TotalSeconds;

    }

    public DateTime Now()
    {
        return DateTime.Now.AddSeconds(timeDiff * -1);
    }

    public Double TimeSinceSeconds(DateTime targetTime)
    {
        var timeSpanDiff = Now().Subtract(targetTime);
        return timeSpanDiff.TotalSeconds;
    }
    public Double TimeUntilSeconds(DateTime targetTime)
    {
        var timeSpanDiff = targetTime.Subtract(Now());
        return timeSpanDiff.TotalSeconds;
    }
}
