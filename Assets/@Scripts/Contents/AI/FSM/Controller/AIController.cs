using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : BaseController<AICharacter>
{
    protected AICharacter aiCharacter;

    public AIController(BaseState<AICharacter> initState, AICharacter aiCharacter)
        : base(initState, aiCharacter)
    {
        this.aiCharacter = aiCharacter;
        aiCharacter.characterAction.OnAction += OnActionPerformed;
    }

    private void OnActionPerformed(Define.EAIState action)
    {
        switch (action)
        {
            /*case Define.EAIState.Cooking:
                ChangeState(nameof(CharacterCookState));
                break;
            case Define.EAIState.Playing:
                ChangeState(nameof(CharacterPlayState));
                break;
            case Define.EAIState.Resting:
                ChangeState(nameof(CharacterRestState));
                break;
            case Define.EAIState.Farming:
                ChangeState(nameof(CharacterFarmingState));
                break;
            case Define.EAIState.Building:
                ChangeState(nameof(CharacterBuildingState));
                break;*/
        }
    }
    
    
    private Vector3 FindNearestBuilding(Define.EAIState action)
    {
        // 추후 BuildingManager 등에서 가져오면 됨
        if (action == Define.EAIState.Cooking)
            return GameObject.Find("CookBuilding").transform.position;

        return Vector3.zero; // fallback
    }
}

    
    
    
}
