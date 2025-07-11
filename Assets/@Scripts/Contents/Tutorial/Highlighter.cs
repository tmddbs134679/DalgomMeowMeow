using UnityEngine;

public class Highlighter : MonoBehaviour
{
    public RectTransform HighlighterRect; 
    private RectTransform target;
    public RectTransform HoleMaskRect;    

    public void Highlight(RectTransform target)
    {
        this.target = target;
        UpdatePosition();
        gameObject.SetActive(true);
    }

    public void Follow(RectTransform targetTransform)
    {
        this.target = targetTransform;
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
        if (HoleMaskRect != null)
        {
            HoleMaskRect.position = target.position;
            HoleMaskRect.sizeDelta = target.sizeDelta + new Vector2(20, 20);
        }

        if (HighlighterRect != null)
        {
            HighlighterRect.position = target.position;
            HighlighterRect.sizeDelta = target.sizeDelta + new Vector2(20, 20);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        target = null;
    }
}