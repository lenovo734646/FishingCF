
/******************************************************************************
 * 
 *  Title:  捕鱼项目
 *
 *  Version:  1.0版
 *
 *  Description:
 *
 *  Author:  monkey256
 *       
 *  Date:  2019
 * 
 ******************************************************************************/
 
using System;
using UnityEngine;

/// <summary>
/// 时间相关的辅助类
/// </summary>
public static class TimeHelper
{
    private static ulong serverTimestamp_;
    private static DateTime serverSettingTime_;

    /// <summary>
    /// 设置服务器毫秒时间戳
    /// </summary>
    /// <param name="timestamp">毫秒时间戳</param>
    public static void SetServerTimestamp(ulong timestamp)
    {
        serverTimestamp_ = timestamp;
        serverSettingTime_ = DateTime.Now;
    }

    /// <summary>
    /// 获取当前服务器毫秒时间戳
    /// </summary>
    /// <returns>毫秒时间戳</returns>
    public static ulong GetServerTimestamp()
    {
        return serverTimestamp_ + (ulong)(DateTime.Now - serverSettingTime_).TotalMilliseconds;
    }

    /// <summary>
    /// 获取服务器本地时间对象
    /// </summary>
    /// <returns>服务器本地时间对象</returns>
    public static DateTime GetServerDateTime()
    {
        return ConvertTimestampToLocalDateTime(GetServerTimestamp());
    }

    /// <summary>
    /// 将毫秒时间戳转换为本地时间对象
    /// </summary>
    /// <param name="timestamp">毫秒时间戳</param>
    /// <returns>本地时间对象</returns>
    public static DateTime ConvertTimestampToLocalDateTime(ulong timestamp)
    {
        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return dt.AddMilliseconds(timestamp).ToLocalTime();
    }

    /// <summary>
    /// 将毫秒时间戳转换为Utc时间对象
    /// </summary>
    /// <param name="timestamp">毫秒时间戳</param>
    /// <returns>Utc时间对象</returns>
    public static DateTime ConvertTimestampToUtcDateTime(ulong timestamp)
    {
        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return dt.AddMilliseconds(timestamp);
    }

    /// <summary>
    /// 将DateTime对象转换为毫秒时间戳
    /// </summary>
    /// <param name="time">DateTime对象，Kind必须为Local或Utc</param>
    /// <returns>毫秒时间戳</returns>
    public static ulong ConvertDateTimeToTimestamp(DateTime time)
    {
        if (time.Kind == DateTimeKind.Unspecified)
            throw new Exception("Can't convert timestamp by unspecified kind of DateTime!");

        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        if (time.Kind == DateTimeKind.Local)
            time = time.ToUniversalTime();

        return (ulong)(time - dt).TotalMilliseconds;
    }

    /// <summary>
    /// 将在线时间转换为文本
    /// </summary>
    /// <param name="time">秒</param>
    /// <returns>在线时间文本</returns>
    public static string ConvertOnlineTimeToTextDesc(int time)
    {
        var curTime = (int)(ConvertDateTimeToTimestamp(DateTime.UtcNow) * 0.001f);
        var t = curTime - time;
        string s;
        if (t < 3600)
        {
            s = "一小时内";
        }
        else if (t < 86400)
        {
            s = Mathf.CeilToInt(t / 3600) + "小时前";
        }
        else if (t < 2592000)
        {
            s = Mathf.CeilToInt(t / 86400) + "天前";
        }
        else
        {
            s = "一个月前";
        }
        return s;
    }

    /// <summary>
    /// 按照某种格式转换时间戳显示为日期
    /// </summary>
    /// <param name="timestamp">时间戳</param>
    /// <param name="format">需要转换的格式</param>
    /// <returns>显示的日期</returns>
    public static string ConvertToFormat(ulong timestamp, string format)
    {
        var dt = ConvertTimestampToLocalDateTime(timestamp);
        return dt.ToString(format);
    }
}
