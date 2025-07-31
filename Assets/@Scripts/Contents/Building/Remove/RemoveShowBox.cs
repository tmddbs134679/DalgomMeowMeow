using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveShowBox : MonoBehaviour
{
    public GameObject removebox;

    void Start()
    {
        removebox.SetActive(false);        
    }
    public void OnRemoveShowBox()
    {
        removebox.SetActive(true);
    }
        public void OffRemoveShowBox()
    {
        removebox.SetActive(false);
    }
}
