using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
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
}
