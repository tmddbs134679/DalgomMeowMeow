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
    public bool Isblink = false; //깜빡임 여부
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
        StartCoroutine(GameStartCoroutine(RandomTiming()));

    }

    IEnumerator GameStartCoroutine(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            if (time < 1 && Isblink == false)
            {
                Isblink = true;
                StartCoroutine(Blink());
            }
            yield return null;
        }
        LookBack();
    }

    IEnumerator Blink()
    {
        //깜빡임
        yield return new WaitForSeconds(0.5f);
    }



    public void LookBack()
    {
        //뒤돌아보기
        Isblink = false;
        IsLookBack = true;
        LookBackTime = 4f;
    }



}
