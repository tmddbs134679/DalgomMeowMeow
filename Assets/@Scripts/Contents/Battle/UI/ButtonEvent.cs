using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
    public void OnClickTitleBtn()
    {
        Debug.Log("Title Button Clicked");
        //SceneManager.LoadScene("Title");
    }
}
