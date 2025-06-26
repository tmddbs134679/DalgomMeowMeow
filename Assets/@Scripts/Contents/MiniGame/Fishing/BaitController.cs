using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaitController : MonoBehaviour
{

    public RectTransform fish;
    public float moveSpeed = 10f;
    public float range = 40f;
    public Vector2 targetPosition;
    public float changeInterval = 2f;
    private float changeTimer;

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
        
        changeTimer += Time.deltaTime;
        if (changeTimer >= changeInterval)
        {
            PickNewTarget();
            changeTimer = 0f;
            changeInterval = Random.Range(0f, 1.5f);
        }

        fish.anchoredPosition = Vector2.Lerp(fish.anchoredPosition, targetPosition, Time.deltaTime * moveSpeed);
    }

    void PickNewTarget()
    {
        float y = Random.Range(-range, range);
        targetPosition = new Vector2(fish.anchoredPosition.x, y);
    }

    public void StopBait()
    {
        
        moveSpeed = 0f;
    }
   
}
