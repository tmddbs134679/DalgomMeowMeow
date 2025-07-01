using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuestPopup : UI_Popup
{
    enum GameObjects { Content }
    enum Buttons { BackgroundButton , DailyButton, AchievementButton }

    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.DailyButton).gameObject.BindEvent(DailyButton);
        GetButton((int)Buttons.AchievementButton).gameObject.BindEvent(AchievementButton);
        CreateQuestSlots();

        return true;
    }

    private void DailyButton()
    {
        CreateQuestSlots();
    }

    private void AchievementButton()
    {
        CreateAchievementSlots();
    }

    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    void CreateQuestSlots()
    {
        Transform parent = GetObject((int)GameObjects.Content).transform;
        foreach (Transform child in parent)
            Destroy(child.gameObject);

        foreach (var quest in QuestManager.Instance.DailyQuests)
        {
            UI_QuestSlot slot = Managers.UI.MakeSubItem<UI_QuestSlot>(parent);
            slot.SetQuest(quest);
        }
    }
    
    void CreateAchievementSlots()
    {
        Transform parent = GetObject((int)GameObjects.Content).transform;
        foreach (Transform child in parent)
            Destroy(child.gameObject);

        foreach (var quest in QuestManager.Instance.AchievementQuests)
        {
            UI_QuestSlot slot = Managers.UI.MakeSubItem<UI_QuestSlot>(parent);
            slot.SetQuest(quest);
        }
    }
}

