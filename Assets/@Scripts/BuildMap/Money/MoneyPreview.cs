using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 임시 머니  UI
/// </summary>
public class MoneyPreview : MonoBehaviour
{
    public int money;

    public TextMeshProUGUI uGUI;
    void Update()
    {
        uGUI.text = money.ToString();
    }
}
