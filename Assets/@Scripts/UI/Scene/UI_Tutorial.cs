using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tutorial : UI_Popup
{
    enum GameObjects
    {
        Content
    }


    enum Texts { DescriptionText }
    
    
    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        
        BindText(typeof(Texts));
        
        SetInfo();
        return true;
    }

    private void SetInfo()
    {
        GetText((int)Texts.DescriptionText).text = "description";
    }

    public void Show(string title, string description)
    {
        GetText((int)Texts.DescriptionText).text = description;
    }
}
