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
    enum Buttons { SkipButton,  }
    
    
    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        
        GetButton((int)Buttons.SkipButton).gameObject.BindEvent(OnClickSkip);
        
        
        SetInfo();
        return true;
    }

    private void SetInfo()
    {
        GetText((int)Texts.DescriptionText).text = "description";
        
    }

    private void OnClickSkip()
    {
        TutorialManager.Instance.SkipTutorial(); // 내부에서 모든 처리

        this.gameObject.SetActive(false);
    }

    public void Show(string title, string description)
    {
        GetText((int)Texts.DescriptionText).text = description;
    }
}
