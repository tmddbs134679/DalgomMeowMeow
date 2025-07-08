using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBuildTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        BuildingPlacer.OnBuildingAccepted += Check;
    }

    private void OnDisable()
    {
        BuildingPlacer.OnBuildingAccepted -= Check;
    }

    private void Check(BaseBuildingSO building)
    {
        if (!TutorialManager.Instance.IsStepActive("도로건설")) return;

        if (building.BuildingType == Define.BuildingType.Road)
        {
            TutorialManager.Instance.CompleteStep();
        }
    }
}
