using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class CommonHandler
{
    static public string TimeTransform(int iTime)
    {
        int iHour = (int)(iTime / 3600);
        int iMinute = (int)(iTime % 3600 / 60);
        int iSecond = iTime % 60;
        string szTime = iHour < 10 ? ("0" + iHour) : iHour.ToString();
        szTime += ":";
        szTime += iMinute < 10 ? ("0" + iMinute) : iMinute.ToString();
        szTime += ":";
        szTime += iSecond < 10 ? ("0" + iSecond) : iSecond.ToString();
        return szTime;
    }

    static public string GetUniqueID()
    {
        long i = 1;
        foreach (byte b in Guid.NewGuid().ToByteArray())
        {
            i *= ((int)b + 1);
        }
        return string.Format("{0:x}", i - DateTime.Now.Ticks);
    }
}