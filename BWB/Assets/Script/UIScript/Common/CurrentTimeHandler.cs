﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;

public class CurrentTimeHandler
{
    static private CurrentTimeHandler instance = null;
    private int iInterval;

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
        iInterval = 60;
        Timers.inst.Add(1, 0, CountDown);
    }

    public void StopTimer()
    {
        Timers.inst.Remove(CountDown);
    }

    /*
     * 每分钟存储一次客户端的技能经验变化
     */
    private void CountDown(object param)
    {
        long iServerTime = DataManager.Instance.ServerTime;
        iServerTime -= 1000;
        DataManager.Instance.ServerTime = iServerTime;

        iInterval--;
        if (iInterval <= 0)
        {
            NetManager.Instance.SkillExpSave();
            iInterval = 60;
        }
    }
}