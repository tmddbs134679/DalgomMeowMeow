using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotResult
{
    public string Symbol;       // 예: "🍒", "⭐", "7"
    public int RewardGold;
}

public class SlotMachineTestData
{
    public static List<SlotResult> TestResults = new()
    {
        new SlotResult { Symbol = "곰", RewardGold = 100 },
        new SlotResult { Symbol = "상어", RewardGold = 200 },
        new SlotResult { Symbol = "고양이",  RewardGold = 1000 }
    };
}
public class SlotMachineBuilding : MonoBehaviour
{
    private List<SlotResult> _results => SlotMachineTestData.TestResults;

    private string[] _currentResult = new string[3];

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("슬롯머신 테스트 시작!");
            RollSlot();
        }
    }

    private void RollSlot()
    {
        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, _results.Count);
            _currentResult[i] = _results[index].Symbol;
        }

        Debug.Log($"슬롯 결과: {_currentResult[0]} | {_currentResult[1]} | {_currentResult[2]}");

        CheckReward();
    }

    private void CheckReward()
    {
        if (_currentResult[0] == _currentResult[1] && _currentResult[1] == _currentResult[2])
        {
            string symbol = _currentResult[0];
            SlotResult match = _results.Find(r => r.Symbol == symbol);

            if (match != null)
            {
                Debug.Log($"당첨! {symbol} x3 → 보상: {match.RewardGold}골드");
                //Managers.Game.Gold += match.RewardGold;
            }
        }
        else
        {
            Debug.Log("꽝!");
        }
    }

    // public override void OnClick()
    // {
    //     
    // }
    //
    // public override void Produce()
    // {
    //     
    // }
}
