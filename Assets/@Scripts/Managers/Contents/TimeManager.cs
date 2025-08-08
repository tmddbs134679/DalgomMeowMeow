using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TimeManager : MonoBehaviour
{
    public float _minute = 60f;

    private DateTime _now => DateTime.Now;

    private void Start()
    {
        Init();
    }

    public int AttendanceDay
    {
        get => PlayerPrefs.GetInt("AttendanceDay", 1);
        set
        {
            PlayerPrefs.SetInt("AttendanceDay", value);
            PlayerPrefs.Save();
        }
    }

    private DateTime ParseDateTime(string key, DateTime defaultValue)
    {
        string saved = PlayerPrefs.GetString(key, string.Empty);
        if (!string.IsNullOrEmpty(saved) && DateTime.TryParse(saved, null, System.Globalization.DateTimeStyles.RoundtripKind, out var parsed))
        {
            return parsed;
        }
        return defaultValue;
    }

    private void SaveDateTime(string key, DateTime value)
    {
        PlayerPrefs.SetString(key, value.ToString("o"));
        PlayerPrefs.Save();
    }

    public DateTime LastLoginTime
    {
        get => ParseDateTime("LastLoginTime", _now);
        set => SaveDateTime("LastLoginTime", value);
    }

    private DateTime _lastRewardTime;
    public DateTime LastRewardTime
    {
        get
        {
            if (_lastRewardTime == default)
                _lastRewardTime = ParseDateTime("LastRewardTime", _now);
            return _lastRewardTime;
        }
        set
        {
            _lastRewardTime = value;
            SaveDateTime("LastRewardTime", value);
        }
    }

    public DateTime LastResetTime
    {
        get => ParseDateTime("LastResetTime", DateTime.MinValue);
        set => SaveDateTime("LastResetTime", value);
    }

    public DateTime LastQuitTime
    {
        get => ParseDateTime("LastQuitTime", _now);
        set => SaveDateTime("LastQuitTime", value);
    }

    public TimeSpan TimeSinceLastQuit
    {
        get
        {
            var elapsed = _now - LastQuitTime;
            return elapsed > TimeSpan.FromHours(15) ? TimeSpan.FromHours(15) : elapsed;
        }
    }

    public DateTime TravelStartTime
    {
        get => ParseDateTime("TravelStartTime", DateTime.MinValue);
        set => SaveDateTime("TravelStartTime", value);
    }

    public TimeSpan TravelDuration
    {
        get
        {
            string str = PlayerPrefs.GetString("TravelDuration", string.Empty);
            return TimeSpan.TryParse(str, out var result) ? result : TimeSpan.Zero;
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
            if (TravelDuration == TimeSpan.Zero) return TimeSpan.Zero;
            var elapsed = _now - TravelStartTime;
            var remaining = TravelDuration - elapsed;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }
    }

    private DateTime _lastDayNightCheckTime;
    public DateTime LastDayNightCheckTime
    {
        get
        {
            if (_lastDayNightCheckTime == default)
                _lastDayNightCheckTime = ParseDateTime("LastDayNightCheckTime", _now);
            return _lastDayNightCheckTime;
        }
        set
        {
            _lastDayNightCheckTime = value;
            SaveDateTime("LastDayNightCheckTime", value);
        }
    }

    public bool _claimedThisSession;

    public bool IsTraveling => TravelDuration != TimeSpan.Zero && (_now - TravelStartTime) < TravelDuration;

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
            if (_minute <= 0)
            {
                CheckAttendance();
                _minute = 60f;
            }
        }
    }

    private bool IsSameDay(DateTime savedTime, DateTime currentTime) => savedTime.Date == currentTime.Date;

    public void CheckAttendance()
    {
        if (!IsSameDay(LastLoginTime, _now))
        {
            AttendanceDay++;
            LastLoginTime = _now;
            Managers.Game.SaveGame();
        }
    }

    private void CheckOfflineAttendance()
    {
        DateTime lastLogin = LastLoginTime;
        int daysPassed = (_now.Date - lastLogin.Date).Days;

        if (daysPassed > 0)
        {
            AttendanceDay += daysPassed;
            LastLoginTime = _now;
            Managers.Game.SaveGame();
        }
    }

    public void GiveOfflineGold(bool isReward = false)
    {
        int totalGold = CalculateOfflineGold();
        if (totalGold > 0)
        {
            Managers.Game.Gold += isReward ? totalGold * 2 : totalGold;
            LastRewardTime = _now;
            _lastRewardTime = LastRewardTime;
            _claimedThisSession = true;
        }
    }

    public void CheckDailyReset()
    {
        DateTime todayResetTime = new DateTime(_now.Year, _now.Month, _now.Day, 9, 0, 0);
        if (LastResetTime < todayResetTime && _now >= todayResetTime)
        {
            ResetDailyCounts();
            LastResetTime = _now;
        }
    }

    private void ResetDailyCounts()
    {
        Managers.Game.AdvancedGachaOpenCount = 3;
        _claimedThisSession = false;
        Managers.Game.RewardMinigame = true;
    }

    public int CalculateOfflineGold()
    {
        int totalMinutes = (int)TimeSinceLastQuit.TotalMinutes;
        return totalMinutes * GOLD_PER_MINUTE;
    }
}
