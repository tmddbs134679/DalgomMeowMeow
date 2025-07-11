using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaButton : MonoBehaviour
{
    public void OnClick_Gacha()
    {
        
        Managers.Game.SpawnRandomGachaCharacter(new Vector3(38f, 0.616f,27f ));

    }
   
}
