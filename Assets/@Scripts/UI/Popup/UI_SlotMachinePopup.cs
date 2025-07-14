using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SlotMachinePopup : UI_Popup
{
    enum GameObjects
    {
        Pivot,
    }

    enum Buttons
    {
        SlotButton,
        Background
    }
    enum Texts { Slot1, Slot2, Slot3, Result }

    private SlotMachineBuilding _targetBuilding;
    private bool _isSpinning = false;
    private int _finishedCount = 0;
    public GameObject target;

    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.SlotButton).gameObject.BindEvent(() => StartCoroutine(OnClickSlotButton()));
        GetButton((int)Buttons.Background).gameObject.BindEvent(OnClickBackgroundButton);

        FocusCameraOnPivot();

        return true;
    }

    private void OnClickBackgroundButton()
    {
        // this.gameObject.SetActive(false);
        Managers.UI.ClosePopupUI(this);
    }

    private IEnumerator OnClickSlotButton()
    {
        if (_isSpinning) yield break;
        _isSpinning = true;

        Managers.Game.Gold -= 100;
        _finishedCount = 0;
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(RollSingleSlot(i));
        }

        while (_finishedCount < 3)
            yield return null;

        FinalizeRewardText();

        _isSpinning = false;
        // yield return StartCoroutine(_targetBuilding.StartAllSlots());
        //
        // string[] result = _targetBuilding.CurrentResult; // CurrentResult는 public으로 만들어야 함
        //
        // GetText((int)Texts.Slot1).text = result[0];
        // GetText((int)Texts.Slot2).text = result[1];
        // GetText((int)Texts.Slot3).text = result[2];
        //
        // string a = result[0], b = result[1], c = result[2];
        // string rewardText = "Try again!";
        // if (a == b && b == c)
        // {
        //     var match = SlotMachineTestData.TestResults.Find(r => r.Symbol == a);
        //     if (match != null)
        //     {
        //         if (match.RewardGold > 0)
        //             rewardText = $"🎉 {a} x3 → +{match.RewardGold} Gold";
        //         else
        //             rewardText = $"🦈 {a} x3 → {match.RewardGold} Gold";
        //     }
        // }
        //
        // GetText((int)Texts.Result).text = rewardText;

    }

    private IEnumerator RollSingleSlot(int index)
    {
        float duration = Random.Range(0.5f, 1.5f);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            string symbol = _targetBuilding.GetRandomResult().Symbol;
            _targetBuilding.CurrentResult[index] = symbol;

            // 슬롯 텍스트 즉시 반영
            GetText((int)Texts.Slot1 + index).text = symbol;

            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        // 멈출 때의 최종 결과
        string final = _targetBuilding.GetRandomResult().Symbol;
        _targetBuilding.CurrentResult[index] = final;
        GetText((int)Texts.Slot1 + index).text = final;

        _finishedCount++;
    }
    private void FinalizeRewardText()
    {
        string[] result = _targetBuilding.CurrentResult;

        string a = result[0], b = result[1], c = result[2];
        string rewardText = "Try again!";

        if (a == b && b == c)
        {
            var match = SlotMachineTestData.TestResults.Find(r => r.Symbol == a);
            if (match != null)
            {
                if (match.RewardGold > 0)
                {
                    rewardText = $" {a} x3 → +{match.RewardGold} Gold";
                    Managers.Game.Gold += match.RewardGold;
                }
                else
                {
                    rewardText = $" {a} x3 → {match.RewardGold} Gold";
                    Managers.Game.Gold += match.RewardGold;
                }
            }
        }

        GetText((int)Texts.Result).text = rewardText;
    }
    public void SetTarget(SlotMachineBuilding building)
    {
        _targetBuilding = building;
    }

    public void SetPivot(GameObject go)
    {
        target = go;
    }
        private void FocusCameraOnPivot()
    {
                Vector3 camForward = Camera.main.transform.forward;

        // 카메라와 타겟 사이 거리
        float distanceToTarget = Vector3.Dot(target.transform.position - Camera.main.transform.position, camForward);

        // 타겟 위치에서 카메라 방향으로 역산
        Vector3 newCamPos = target.transform.position - camForward * distanceToTarget;

        // 카메라 위치 이동
        Camera.main.transform.position = new Vector3(newCamPos.x + 6.05f, Camera.main.transform.position.y, newCamPos.z - 3.38f);
    }
}
