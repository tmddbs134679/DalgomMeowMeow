using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

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

    public DateTime LastQuitTime
    {
        get
        {
            string savedTimeStr = PlayerPrefs.GetString("LastQuitTime", string.Empty);
            if (!string.IsNullOrEmpty(savedTimeStr))
                return DateTime.Parse(savedTimeStr);
            else
                return DateTime.Now;
        }
        set
        {
            PlayerPrefs.SetString("LastQuitTime", value.ToString());
            PlayerPrefs.Save();
        }
    }
    //최고 오프라인 시간 15시간 

    public TimeSpan TimeSinceLastQuit
    {
        get
        {
            TimeSpan timeSpan = DateTime.Now - LastQuitTime;
            if (timeSpan > TimeSpan.FromHours(15))
                return TimeSpan.FromHours(15);
            return timeSpan;
        }
    }

    public DateTime TravelStartTime
    {
        get
        {
            string savedTimeStr = PlayerPrefs.GetString("TravelStartTime", string.Empty);
            if (!string.IsNullOrEmpty(savedTimeStr))
            {
                return DateTime.Parse(savedTimeStr);
            }
            else
            {
                return DateTime.MinValue; // 기본값: 여행 안 함
            }
        }
        set
        {
            PlayerPrefs.SetString("TravelStartTime", value.ToString());
            PlayerPrefs.Save();
        }
    }
    public TimeSpan TravelDuration
    {
        get
        {
            string savedDurationStr = PlayerPrefs.GetString("TravelDuration", string.Empty);
            if (!string.IsNullOrEmpty(savedDurationStr))
            {
                return TimeSpan.Parse(savedDurationStr);
            }
            else
            {
                return TimeSpan.Zero; // 기본값: 여행 시간 없음
            }
        }
        set
        {
            PlayerPrefs.SetString("TravelDuration", value.ToString());
            PlayerPrefs.Save();
        }
    }
    public TimeSpan TravelRemainingTime
    {
        get
        {
            if (TravelDuration == TimeSpan.Zero)
                return TimeSpan.Zero; // 여행 중 아님

            TimeSpan elapsed = DateTime.Now - TravelStartTime;
            TimeSpan remaining = TravelDuration - elapsed;

            // 이미 시간이 다 지났다면 0 반환
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }
    }

    #region 낮과밤
    private DateTime _lastDayNightCheckTime;

public DateTime LastDayNightCheckTime
{
    get
    {
        if (_lastDayNightCheckTime == default(DateTime))
        {
            string savedTimeStr = PlayerPrefs.GetString("LastDayNightCheckTime", string.Empty);
            if (!string.IsNullOrEmpty(savedTimeStr))
            {
                _lastDayNightCheckTime = DateTime.Parse(savedTimeStr);
            }
            else
            {
                _lastDayNightCheckTime = DateTime.Now;
            }
        }

        return _lastDayNightCheckTime;
    }
    set
    {
        _lastDayNightCheckTime = value;
        string timeStr = value.ToString();
        PlayerPrefs.SetString("LastDayNightCheckTime", timeStr);
        PlayerPrefs.Save();
    }
}
#endregion


    public bool _claimedThisSession;

    public bool IsTraveling
    {
        get
        {
            if (TravelDuration == TimeSpan.Zero)
                return false;

            TimeSpan elapsed = DateTime.Now - TravelStartTime;
            return elapsed < TravelDuration;
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
        CheckAttendance();
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

    //True이면 광고 보상
    public void GiveOfflineGold(bool IsReward = false)
    {
        int totalGold = CalculateOfflineGold();

        if (totalGold > 0)
        {
           if(IsReward)
                Managers.Game.Gold += (totalGold * 2);
           else
                Managers.Game.Gold += totalGold;


            LastRewardTime = DateTime.Now;
            _lastRewardTime = LastRewardTime;

            _claimedThisSession = true;
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
        _claimedThisSession = false;
    }

    public int CalculateOfflineGold()
    {
        // 마지막 보상 시각부터 현재까지 경과시간
        int totalMinutes = (int)TimeSinceLastQuit.TotalMinutes;

        // 골드 계산
        int totalGold = totalMinutes * Define.GOLD_PER_MINUTE;

        return totalGold;
    }


}
