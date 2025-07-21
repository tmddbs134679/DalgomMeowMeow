using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingBuildTrigger : MonoBehaviour
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
        if (!TutorialManager.Instance.IsStepActive("요리건물건설")) return;

        if (building.BuildingType == Define.EBuildingType.Cooking)
        {
            TutorialManager.Instance.CompleteStep();
        }
    }
}
