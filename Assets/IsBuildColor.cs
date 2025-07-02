using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsBuildColor : MonoBehaviour
{
    public GameObject isbuild;
    public Material green;
    public Material red;
    public void SetIsBUildColor(bool istrue)
    {
        Renderer renderer = isbuild.GetComponent<Renderer>();
        if (renderer != null && green != null)
        {
            if (istrue == true)
            {
                renderer.material = green;
            }
            else
            {
                renderer.material = red;
            }
        }
    }
}
