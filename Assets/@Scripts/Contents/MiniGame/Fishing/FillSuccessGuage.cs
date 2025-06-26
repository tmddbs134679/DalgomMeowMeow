using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FillSuccessGuage : MonoBehaviour
{
    public RectTransform fish, bar;
    public Image guageBar;
    public float gauge = 30;
    public float successThreshold = 100f;
    public float fillSpeed = 20f;
    public float decaySpeed = 10f;
    public float SightRange = 10;
    private bool isFishing = true;
    public event Action Successed;
    public event Action Failed;

    void Update()
    {
        if (isFishing)
        {
            guageBar.fillAmount = gauge / successThreshold;
            float distance = Mathf.Abs(fish.anchoredPosition.y - bar.anchoredPosition.y);
            if (distance < SightRange)
            {
                gauge += Time.deltaTime * fillSpeed;
            }
            else
            {
                gauge -= Time.deltaTime * decaySpeed;
            }

            gauge = Mathf.Clamp(gauge, 0f, successThreshold);

            Success();
            Fail();


        }
    }

    private void Success()
    {
        if (gauge >= successThreshold)
        {
            Debug.Log("낚시 성공!");
            isFishing = false;
            FIshingManager.Instance.isFishing = false;
            Successed?.Invoke();

        }
    }

    private void Fail()
    {
        if (gauge <= 0f)
        {
            isFishing = false;
            FIshingManager.Instance.isFishing = false;
            Debug.Log("낚시 실패");
            Failed?.Invoke();
        }
    }

    
}