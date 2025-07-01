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
    void Start()
    {
                        uGUI.text = money.ToString();
    }
    public void UpdateMoneyText()
    {
        uGUI.text = money.ToString();
    }
}
