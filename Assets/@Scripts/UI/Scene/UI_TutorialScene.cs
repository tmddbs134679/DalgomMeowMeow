using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TutorialScene : MonoBehaviour
{
    public GameObject Panel;
    public TMP_Text TitleText;
    public TMP_Text DescriptionText;
    public void Show(string title, string desc)
    {
        Panel.SetActive(true);
        TitleText.text = title;
        DescriptionText.text = desc;
    }

    public void Hide()
    {
        Panel.SetActive(false);
    }
}
