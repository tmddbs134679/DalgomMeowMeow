using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChapterPopup : UI_Popup
{
    enum GameObjects
    {
        Content
    }

    enum Buttons
    {
        BackgroundButton,
        UnlockButton,
        Chapter1Button,
        Chapter2Button,
        Chapter3Button,
        Chapter4Button,
    }

    enum Texts
    {
        UnlockText1,
        UnlockText2,
        UnlockText3,
    }

    enum Images
    {
        Chapter1SelectImage,
        Chapter2SelectImage,
        Chapter3SelectImage,
        Chapter4SelectImage,
    }

    private string _currentChapterId = "Chapter1";
    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.UnlockButton).gameObject.BindEvent(OnClickUnlockButton);
        GetButton((int)Buttons.Chapter1Button).gameObject.BindEvent(OnClickChapter1Button);
        GetButton((int)Buttons.Chapter2Button).gameObject.BindEvent(OnClickChapter2Button);
        GetButton((int)Buttons.Chapter3Button).gameObject.BindEvent(OnClickChapter3Button);
        GetButton((int)Buttons.Chapter4Button).gameObject.BindEvent(OnClickChapter4Button);


        GetImage((int)Images.Chapter2SelectImage).gameObject.SetActive(false);
        GetImage((int)Images.Chapter3SelectImage).gameObject.SetActive(false);
        GetImage((int)Images.Chapter4SelectImage).gameObject.SetActive(false);
        CreateChapterSlot("Chapter1");
        return true;
    }

    private void OnClickUnlockButton()
    {
        QuestManager.Instance.TryUnlockContent(_currentChapterId);
        Managers.UI.ShowToast("해금완료");
    }

    private void OnClickChapter4Button()
    {
        _currentChapterId = "Chapter4";
        GetImage((int)Images.Chapter1SelectImage).gameObject.SetActive(false);
        GetImage((int)Images.Chapter2SelectImage).gameObject.SetActive(false);
        GetImage((int)Images.Chapter3SelectImage).gameObject.SetActive(false);
        GetImage((int)Images.Chapter4SelectImage).gameObject.SetActive(true);
        CreateChapterSlot(_currentChapterId); 
    }

    private void OnClickChapter3Button()
    {
        _currentChapterId = "Chapter3";
        GetImage((int)Images.Chapter1SelectImage).gameObject.SetActive(false);
        GetImage((int)Images.Chapter2SelectImage).gameObject.SetActive(false);
        GetImage((int)Images.Chapter3SelectImage).gameObject.SetActive(true);
        GetImage((int)Images.Chapter4SelectImage).gameObject.SetActive(false);
        CreateChapterSlot(_currentChapterId);   
    }

    private void OnClickChapter2Button()
    {
        _currentChapterId = "Chapter2";
        GetImage((int)Images.Chapter1SelectImage).gameObject.SetActive(false);
        GetImage((int)Images.Chapter2SelectImage).gameObject.SetActive(true);
        GetImage((int)Images.Chapter3SelectImage).gameObject.SetActive(false);
        GetImage((int)Images.Chapter4SelectImage).gameObject.SetActive(false);
        CreateChapterSlot(_currentChapterId); 
    }

    private void OnClickChapter1Button()
    {
        _currentChapterId = "Chapter1";
        GetImage((int)Images.Chapter1SelectImage).gameObject.SetActive(true);
        GetImage((int)Images.Chapter2SelectImage).gameObject.SetActive(false);
        GetImage((int)Images.Chapter3SelectImage).gameObject.SetActive(false);
        GetImage((int)Images.Chapter4SelectImage).gameObject.SetActive(false);
        CreateChapterSlot(_currentChapterId); 
    }

    private void OnClickBackgroundButton()
    {
        Destroy(this);
        Managers.UI.ClosePopupUI(this);
    }
    
    
    private void CreateChapterSlot(string chapterId)
    {
        if (Managers.Data.UnlockContentDic.TryGetValue(chapterId, out UnlockContentsData data))
        {
            Transform contentParent = GetObject((int)GameObjects.Content).transform;
            foreach (Transform child in contentParent)
                Destroy(child.gameObject);
            foreach (var cond in data.Conditions)
            {
                var slot = Managers.UI.MakeSubItem<UI_ChapterSlot>(contentParent);
                slot.Init();
                slot.SetCondition(cond, chapterId);
            }
        }
        // 오른쪽 해금 콘텐츠 텍스트 설정
        for (int i = 0; i < 3; i++)
        {
            string desc = "";

            if (data.Items != null && i < data.Items.Count)
                desc = data.Items[i].Description;

            GetText((int)Texts.UnlockText1 + i).text = string.IsNullOrEmpty(desc) ? "" : $"- {desc}";
        }
    }
}
