using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundMove : MonoBehaviour
{
    public RawImage rawImg;
    Rect rect;

    public float speed = 1f;
    void Start()
    {
        rawImg = GetComponent<RawImage>();
    }


    void Update()
    {
        rect = rawImg.uvRect;
        rect.x += Time.deltaTime * speed;
        rect.y += Time.deltaTime * speed;
        rawImg.uvRect = rect;
    }
}
