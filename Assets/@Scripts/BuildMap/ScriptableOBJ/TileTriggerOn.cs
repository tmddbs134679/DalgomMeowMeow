using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObjectData : MonoBehaviour
{

   public ArrayMapPos arrayMapPos;
    public bool isCurrentbuild;

    public void SetTile()
    {
        arrayMapPos.SetTile(isCurrentbuild, (int)transform.position.x, (int)transform.position.z);
    }
}
