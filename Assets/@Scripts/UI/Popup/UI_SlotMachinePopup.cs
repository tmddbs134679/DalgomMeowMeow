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
    enum Texts {  Result }

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

        
        if (_targetBuilding == null)
        {
            Debug.LogError("TargetBuilding not set! 반드시 SetTarget(building)을 먼저 호출하세요.");
            return false;
        }
        
        _targetBuilding.Init();
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

        _targetBuilding.SpinAllSlots(); // ⭐ 이미지 회전 효과 실행

        yield return new WaitForSeconds(2.5f); // 대략적인 spin 시간 후 결과 처리

        FinalizeRewardText();
        _isSpinning = false;

    }


    private void FinalizeRewardText()
    {
        string[] result = _targetBuilding.CurrentResult;

        
        string a = result[0], b = result[1], c = result[2];
        string rewardText = "Try again!";
        Debug.Log($"결과: {a}, {b}, {c} → {a == b}, {b == c}");
        if (a == b && b == c)
        {
            var match = _targetBuilding.GetMatchResult(a); // 또는 _targetBuilding 내부에서 match를 리턴하는 메서드 만들기
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
        QuestManager.Instance.UpdateQuestProgress(Define.EQuestConditionType.Collect,Define.ETargetType.SlotMachine);
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
