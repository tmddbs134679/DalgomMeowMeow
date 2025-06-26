using UnityEngine;

public class FishingRangeController : MonoBehaviour
{
    public RectTransform bar;
    public float speed = 10f;
    public float minY = -40, maxY = 40f;
    //
    void Update()
    {
        float input = Input.GetMouseButton(0) ? 0.5f : -0.5f;

        bar.anchoredPosition += Vector2.up * input * speed;
        // anchoredPosition은 RectTransform의 위치를 설정하는데 사용

        bar.anchoredPosition = new Vector2(bar.anchoredPosition.x,
            Mathf.Clamp(bar.anchoredPosition.y, minY, maxY));
    }

    public void StopFishing()
    {
        speed = 0f;
    }
}