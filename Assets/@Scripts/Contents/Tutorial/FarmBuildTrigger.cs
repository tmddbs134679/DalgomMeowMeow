using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmBuildTrigger : MonoBehaviour
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
        if (!TutorialManager.Instance.IsStepActive("농장건설")) return;

        if (building.BuildingType == Define.BuildingType.Farm)
        {
            TutorialManager.Instance.CompleteStep();
        }
    }
}