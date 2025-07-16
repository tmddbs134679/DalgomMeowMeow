using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public float _minute = 60;
    public int AttendanceDay
    {
        get
        {
            int savedTime = PlayerPrefs.GetInt("AttendanceDay", 1);
            return savedTime;
        }
        set
        {
            PlayerPrefs.SetInt("AttendanceDay", value);
            PlayerPrefs.Save();
        }
    }

    public DateTime LastLoginTime
    {
        get
        {
            string savedTimeStr = PlayerPrefs.GetString("LastLoginTime", string.Empty);
            if (!string.IsNullOrEmpty(savedTimeStr))
            {
                return DateTime.Parse(savedTimeStr);
            }
            else
            {
                return DateTime.Now;
            }
        }
        set
        {
            string timeStr = value.ToString();
            PlayerPrefs.SetString("LastLoginTime", timeStr);
            PlayerPrefs.Save();
        }
    }
    private DateTime _lastRewardTime;

    public DateTime LastRewardTime
    {
        get
        {
            if (_lastRewardTime == default(DateTime))
            {
                string savedTimeStr = PlayerPrefs.GetString("LastRewardTime", string.Empty);
                if (!string.IsNullOrEmpty(savedTimeStr))
                {
                    _lastRewardTime = DateTime.Parse(savedTimeStr);
                }
                else
                {
                    _lastRewardTime = DateTime.Now;
                }
            }

            return _lastRewardTime;
        }
        set
        {
            _lastRewardTime = value;
            string timeStr = value.ToString();
            PlayerPrefs.SetString("LastRewardTime", timeStr);
            PlayerPrefs.Save();
        }
    }
    public DateTime LastResetTime
    {
        get
        {
            string savedTimeStr = PlayerPrefs.GetString("LastResetTime", string.Empty);
            if (!string.IsNullOrEmpty(savedTimeStr))
            {
                return DateTime.Parse(savedTimeStr);
            }
            else
            {
                return DateTime.MinValue; 
            }
        }
        set
        {
            PlayerPrefs.SetString("LastResetTime", value.ToString());
            PlayerPrefs.Save();
        }
    }



    public TimeSpan TimeSinceLastReward
    {
        get
        {
            TimeSpan timeSpan = DateTime.Now - LastRewardTime;
            if (timeSpan > TimeSpan.FromHours(24))
            {
                return TimeSpan.FromHours(24);
            }
            return timeSpan;
        }
    }
    public void Init()
    {
        CheckOfflineAttendance();
        CheckDailyReset();
        TimerStart();
    }

    public void TimerStart()
    {
        StartCoroutine(CoStartTimer());
    }
    IEnumerator CoStartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            _minute--;
            if (_minute == 0)
            {
                CheckAttendance();
                _minute = 60;
            }
        }
    }

    private bool IsSameDay(DateTime savedTime, DateTime currentTime)
    {
        if (LastLoginTime.Day == DateTime.Now.Day)
        {
            return true;
        }
        else
            return false;
    }

    public void CheckAttendance()
    {
        if (IsSameDay(LastLoginTime, DateTime.Now) == false)
        {
            AttendanceDay++;
            LastLoginTime = DateTime.Now;
            Managers.Game.SaveGame();
        }
    }

    private void CheckOfflineAttendance()
    {
        DateTime now = DateTime.Now;
        DateTime lastLogin = LastLoginTime;

        int daysPassed = (now.Date - lastLogin.Date).Days;

        if(daysPassed > 0)
        {
            AttendanceDay += daysPassed;

            LastLoginTime = now;
            Managers.Game.SaveGame();
        }
    }

    private void GiveOfflineGold()
    {
        TimeSpan offlineTime = DateTime.Now - LastLoginTime;

        int goldPerminute = 100;
        int totalMinutes = (int)offlineTime.TotalMinutes;
        int totalGold = totalMinutes * goldPerminute;

        if(totalGold > 0)
        {
            Managers.Game.Gold += totalGold;

            LastRewardTime = DateTime.Now;
            
        }
        else
        {
            Managers.UI.ShowToast("이미 보상을 받았습니다!");
        }
    }


    //광고 카운트 및 9시 리셋
    public void CheckDailyReset()
    {
        DateTime now = DateTime.Now;
        DateTime todayResetTime = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0); 
        DateTime lastReset = LastResetTime;

        if (lastReset < todayResetTime && now >= todayResetTime)
        {
        
            ResetDailyCounts();
            LastResetTime = now; 
        }

    }

    private void ResetDailyCounts()
    {
        Managers.Game.AdvancedGachaOpenCount = 3;
    }
}
