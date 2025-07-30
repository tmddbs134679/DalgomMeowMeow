using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


public class UI_ChangePopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        Content,
        ContentGroup,
        ScrollView
    }

    enum Buttons
    {
        AcceptButton,
        Background,
    }
    enum Texts
    {
        
    }

    enum Images
    {
        
    }
    #endregion
    private List<(string UniqueId, bool InMainScene)> origin = new();
    Character _character;

    ScrollRect _scrollRect;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentGroup));
        origin = Managers.Game.Characters
       .Select(ch => (ch.UniqueId, ch.InMainScene))
       .ToList();
        GetCharacterInfo();
    }
    public override bool Init()
    {
        if (!base.Init()) return false;


        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        _scrollRect = GetObject((int)GameObjects.ScrollView).GetComponent<ScrollRect>();

        

        GetButton((int)Buttons.Background).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.AcceptButton).gameObject.BindEvent(OnClickAcceptButton);



        //GetCharacterInfo();

        return true;
    }

    private void OnClickBackgroundButton()
    {
        this.gameObject.SetActive(false);

        foreach (var saved in origin)
        {
            var ch = Managers.Game.Characters.FirstOrDefault(c => c.UniqueId == saved.UniqueId);
            if (ch != null)
            {
                ch.InMainScene = saved.InMainScene;
            }
        }
    }
    private void OnClickAcceptButton()
    {
        int i = Managers.Game.Characters.Count(ch => ch.InMainScene);

        if (i > Managers.Game._gameData.MaxCountInScene)
        {
            Managers.UI.ShowToast("메인 씬에 캐릭터는 최대 " + Managers.Game._gameData.MaxCountInScene + "명까지 가능합니다.");
            return;
        }
        

        Managers.Scene.LoadScene(Define.EScene.CharacterStoreScene, transform);

    
    }

    public void OnClickSlot(Character character)
    {
        _character = character;

        character.InMainScene = !character.InMainScene;
    }

    #region 캐릭터 정보 가져오기 및 삭제

    public void GetCharacterInfo()
    {
        Extension.DestroyChilds(GetObject((int)GameObjects.Content).gameObject);

        List<Character> characters = Managers.Game.Characters;
        foreach (Character ch in characters)
        {
            UI_ChangeCharacterSlot slot = Managers.UI.MakeSubItem<UI_ChangeCharacterSlot>(GetObject((int)GameObjects.Content).transform);
            slot.SetInfo(ch , _scrollRect);
        }
    }

    #endregion

}
