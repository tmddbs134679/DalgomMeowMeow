using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTriggerOn : MonoBehaviour
{

   public ArrayMapPos arrayMapPos;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("건물과 충돌");
        arrayMapPos.SetTile(true, (int)transform.position.x,(int) transform.position.z);
    }
}
