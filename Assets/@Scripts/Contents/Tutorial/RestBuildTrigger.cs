using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestBuildTrigger : MonoBehaviour
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
        if (!TutorialManager.Instance.IsStepActive("침대건설")) return;

        if (building.BuildingType == Define.BuildingType.Resting)
        {
            TutorialManager.Instance.CompleteStep();
        }
    }
}