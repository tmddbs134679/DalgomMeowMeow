
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class UI_ChangePopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        Content,
        ContentGroup,
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

        if (i > Managers.Game.MaxCountInScene)
        {
            Managers.UI.ShowToast("메인 씬에 캐릭터는 최대 " + Managers.Game.MaxCountInScene + "명까지 가능합니다.");
            return;
        }
        //foreach (var ch in Managers.Game.Characters.Where(c => !c.InMainScene))
        //{
        //    var pos = new Vector3(Random.Range(-5f, 5f), 0.616f, Random.Range(-5f, 5f));
        //    AICharacter ai = Managers.Object.Spawn<AICharacter>(pos, ch.DataId);

        //    ai.Init();
        //    ai.Data = ch;
        //    ai.ControllerRegister();

        //    Managers.Game.CharacterInMainScene[ch.UniqueId] = ai;
        //    Managers.AI.ValidateNavMeshPosition(ai);

        Managers.Scene.LoadScene(Define.EScene.CharacterStoreScene, transform);

        //this.gameObject.SetActive(false);
    
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
            slot.SetInfo(ch);
        }
    }

   


    

    #endregion 

}
