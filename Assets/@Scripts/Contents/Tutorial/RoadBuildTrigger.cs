using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBuildTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        // 도로 건설이 일어나면 Check() 실행되도록 등록
        //.OnRoadBuilt += Check;
    }

    private void OnDisable()
    {
        // 해제 (안 하면 메모리 누수 위험)
        //RoadManager.OnRoadBuilt -= Check;
    }

    private void Check()
    {
        if (TutorialManager.Instance.IsStepActive("도로 건설"))
        {
            TutorialManager.Instance.CompleteStep(); // 다음 단계로 진행
        }
    }
}
