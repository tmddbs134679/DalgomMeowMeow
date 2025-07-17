using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_Onclick : MonoBehaviour
{
    public bool isClicked = false;
    public void Clicked()
    {
        if (isClicked)
            return;

        isClicked = true;
    }
}
