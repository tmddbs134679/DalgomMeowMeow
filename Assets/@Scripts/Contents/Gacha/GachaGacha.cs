using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaGacha : MonoBehaviour
{
    [SerializeField]private int[] gachaItems; // Array to hold the IDs of gacha items
    private float[] weights = new float[]
    {
        0.35f, //1성 35%
        0.3f,  //2성 30%
        0.2f,  //3성 20%
        0.1f,  //4성 10%
        0.05f  //5성 5%
    }; 

    public void RandomGacha()
    {
        float totalWeight = 0f;

        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0f, totalWeight);

        float currentDum = 0f;
        for(int i = 0; i < weights.Length; i++)
        {
            currentDum += weights[i];
            if (randomValue <= currentDum)
            {
                Debug.Log("Gacha Item ID: " + gachaItems[i]);
                return;
            }
        }
    }

    public void Gacha_Tentimes()
    {
        for (int i = 0; i < 10; i++)
            RandomGacha();
    }
}
