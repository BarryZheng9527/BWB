using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;

public class CurrentTimeHandler
{
    static private CurrentTimeHandler instance = null;

    public static CurrentTimeHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CurrentTimeHandler();
            }
            return instance;
        }
    }

    public void StartTimer()
    {
        Timers.inst.Add(1, 0, CountDown);
    }

    public void StopTimer()
    {
        Timers.inst.Remove(CountDown);
    }

    private void CountDown(object param)
    {
        long iServerTime = DataManager.Instance.ServerTime;
        iServerTime -= 1000;
        DataManager.Instance.ServerTime = iServerTime;
    }
}