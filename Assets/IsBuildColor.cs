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
        if (istrue = true && renderer != null && green != null)
        {
            renderer.material = green;
        }
        else if (istrue = false && renderer != null && green != null)
        {
            renderer.material = red;
        }
    }
}
