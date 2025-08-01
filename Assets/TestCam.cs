using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().targetTexture = Managers.Resource.Load<RenderTexture>("CharacterProfile_RT");
    }

}