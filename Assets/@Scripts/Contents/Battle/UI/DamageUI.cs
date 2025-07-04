using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    public TextMeshPro text;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
        if (text == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on DamageUI.");
        }
    }

    public void Show(float damage, Vector3 worldPos, int layer)
    {
        // 색상 처리
        if (damage <= 0)
            text.color = new Color(0f, 1f, 105f / 255f, 1f); // 힐
        else if (layer == LayerMask.NameToLayer("Player"))
            text.color = Color.red;
        else if (layer == LayerMask.NameToLayer("Enemy"))
            text.color = Color.yellow;

        text.text = Mathf.FloorToInt(damage).ToString();

        transform.position = worldPos + Vector3.up * 2f;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(transform.position.y + 1f, 1f).SetEase(Ease.OutCubic));
        seq.Join(text.DOFade(0f, 1f));
        seq.OnComplete(() => Destroy(gameObject));
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180f, 0); // 글자가 거꾸로 보이는 것 방지
    }
}
