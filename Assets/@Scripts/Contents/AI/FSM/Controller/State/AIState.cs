using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : BaseState<AICharacter>
{
    protected Define.EAIState state;
    protected AICharacter character;
    
    public override void Init(AICharacter owner)
    {
        this.character = owner;
    }

    public override void OnExit()
    {
        character.currentBuilding = null;
    }
    public Define.EAIState GetState() { return state; }
}
