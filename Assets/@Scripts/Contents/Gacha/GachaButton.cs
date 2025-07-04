using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaButton : MonoBehaviour
{
    public void OnClick_Gacha()
    {
        Vector3 spawnPos = GetRandomSpawnPosition();
        var newAI = Managers.Game.SpawnRandomGachaCharacter(spawnPos);

    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-5f, 5f);
        float z = Random.Range(-5f, 5f);
        return new Vector3(x, 0, z);
    }
}
