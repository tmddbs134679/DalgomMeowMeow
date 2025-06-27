using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    [SerializeField] private int StageNum;
    public bool Cleared = false;

    private void Update()
    {
        if(Managers.Game.CurrentStage >= StageNum && !Cleared)
        {
            Cleared = true;
        }
    }

    public void ShowStageInfo()
    {
        if (Managers.Game.CurrentStage >= StageNum && !Cleared)
        {
            //ui 켜주기
            Debug.Log("Stage " + StageNum + " is available.");
        }
        else if (Cleared)
        {
            Debug.Log("Stage " + StageNum + " is already cleared.");
        }
        else
        {
            Debug.Log("Stage " + StageNum + " is not available yet.");
        }
    }
    public void GoToBattle()
    {
        //켜진 ui에서 버튼 누르면 배틀로 간다
        Managers.Game.CurrentStageCleared = Cleared;
        Debug.Log($"현재 스테이지는 {Managers.Game.CurrentStage}스테이지이고 클리어 여부는 {Managers.Game.CurrentStageCleared} 입니다");
        //SceneManager.LoadScene("Battle");
    }
}
