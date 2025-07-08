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

    public void Init()
    {
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
            Managers.Game.SaveGame();
        }
    }



}
