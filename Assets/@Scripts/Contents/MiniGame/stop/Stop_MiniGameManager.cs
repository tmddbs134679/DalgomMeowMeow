using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stop_MiniGameManager : MonoBehaviour
{
    public enum ForwardTime
    {
        Slowest = 1,
        Slow = 2,
        Moderate = 3,
        Fast = 4,
        Fastest = 5,
    }
    public bool IsLookBack = false;
    public float LookBackTime = 4f;


    private Image Image;
    private void Awake()
    {
        Image = GetComponent<Image>();
    }

    private void Update()
    {
        if (IsLookBack)
        {
            LookBackTime -= Time.deltaTime;
            if (LookBackTime < 0)
            {
                IsLookBack = false;
                //앞에봄
                GameStart(); //3으로 시작
            }
        }
    }

    public int RandomTiming()
    {
        int k = Random.Range(1, 6);     //1~5까지 랜덤
        return k;
    }

    public void GameStart()
    {

    }



    public void LookBack()
    {
        IsLookBack = true;
        LookBackTime = 4f;
    }



}
