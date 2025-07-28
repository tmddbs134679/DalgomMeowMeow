using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabbageBuildTrigger : MonoBehaviour
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
        if (!TutorialManager.Instance.IsStepActive("배추농장건설")) return;

        if (building.BuildingType == Define.EBuildingType.Road)
        {
            TutorialManager.Instance.CompleteStep();
        }
    }
}
