using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class CommonHandler
{
    /*
     * 时间转换
     */
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

    /*
     * 生成唯一ID
     */
    static public string GetUniqueID()
    {
        long i = 1;
        foreach (byte b in Guid.NewGuid().ToByteArray())
        {
            i *= ((int)b + 1);
        }
        return string.Format("{0:x}", i - DateTime.Now.Ticks);
    }

    /*
     * 时间戳和DateTime互转
     */
    static public DateTime ConvertIntDateTime(double d)
    {
        DateTime time = DateTime.MinValue;
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        time = startTime.AddMilliseconds(d);
        return time;
    }

    static public long ConvertDateTimeInt(DateTime time)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
        long t = (time.Ticks - startTime.Ticks) / 10000;
        return t;
    }
}