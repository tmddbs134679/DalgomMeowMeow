using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotResult
{
    public string Symbol;       
    public int RewardGold;
    public int Weight;
}

public class SlotMachineTestData
{
    public static List<SlotResult> TestResults = new()
    {
        new SlotResult { Symbol = "BEAR", RewardGold = 100 , Weight = 50 },
        new SlotResult { Symbol = "SHARK", RewardGold = -200 , Weight = 30 },
        new SlotResult { Symbol = "CAT",  RewardGold = 1000, Weight = 5  }
    };
}
public class SlotMachineBuilding : BaseObject
{
    private List<SlotResult> _results => SlotMachineTestData.TestResults;

    public string[] _currentResult = new string[3];


    private SlotResult GetRandomResult()
    {
        int totalWeight = 0;
        foreach (var result in _results)
            totalWeight += result.Weight;

        int rand = Random.Range(0, totalWeight);
        int current = 0;

        foreach (var result in _results)
        {
            current += result.Weight;
            if (rand < current)
                return result;
        }

        return _results[0]; // fallback
    }
    public void RollSlot()
    {
        for (int i = 0; i < 3; i++)
        {
            _currentResult[i] = GetRandomResult().Symbol;
        }


        CheckReward();
    }

    private void CheckReward()
    {
        if (_currentResult[0] == "고양이" && _currentResult[1] == "고양이" && _currentResult[2] == "고양이")
        {
            SlotResult match = _results.Find(r => r.Symbol == "고양이");

            if (match != null)
            {
                Debug.Log($"🎉 대박! 고양이 x3 → 보상: {match.RewardGold}골드");
                // Managers.Game.Gold += match.RewardGold;
            }
        }
        else
        {
            Debug.Log("꽝! 다시 도전하세요.");
        }
    }
    public (string[], int) RollSlotAndReturn()
    {
        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, _results.Count);
            _currentResult[i] = _results[index].Symbol;
        }

        int reward = 0;
        if (_currentResult[0] == "CAT" && _currentResult[1] == "CAT" && _currentResult[2] == "CAT")
        {
            reward = _results.Find(r => r.Symbol == "CAT")?.RewardGold ?? 0;
            Managers.Game.Gold += reward;
        }
        else if (_currentResult[0] == "SHARK" && _currentResult[1] == "SHARK" && _currentResult[2] == "SHARK")
        {
            reward = _results.Find(r => r.Symbol == "SHARK")?.RewardGold ?? 0;
            Managers.Game.Gold += reward;
        }

        return (_currentResult, reward);
    }
    
    public override void OnClick()
    {
        Managers.Debug.Log("슬롯팝업",Define.EDebugType.Building);
        UI_SlotMachinePopup popup = Managers.UI.ShowPopupUI<UI_SlotMachinePopup>();
        popup.SetTarget(this);
    }
}
