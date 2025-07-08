using UnityEngine;

public class Highlighter : MonoBehaviour
{
    public RectTransform HighlighterReact;
    private RectTransform target;

    public void Follow(RectTransform targetTransform)
    {
        target = targetTransform;
        UpdatePosition();
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (target != null)
            UpdatePosition();
    }

    void UpdatePosition()
    {
        HighlighterReact.position = target.position;
        HighlighterReact.sizeDelta = target.sizeDelta + new Vector2(20, 20);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}