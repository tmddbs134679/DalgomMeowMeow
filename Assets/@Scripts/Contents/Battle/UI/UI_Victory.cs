public class UI_Victory : UI_Popup   //어드레서블에 프리펩 넣기
{
    #region Enum
    enum Buttons
    {
        TitleButton,
    }
    enum Texts
    {
        Gold,
        Exp,
    }
    #endregion

    private UI_BattleScene battleUI;

    private void Awake()
    {
        Init();
                                                             //배틀 UI 비활성화
    }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.TitleButton).gameObject.BindEvent(OnClickTitle);

        string gold = StageDataManager.Instance.SetStage().GoldReward.ToString();
        string exp = StageDataManager.Instance.SetStage().ExpReward.ToString();
        GetText((int)Texts.Gold).text = $"Gold +{gold}";
        GetText((int)Texts.Exp).text = $"Exp +{exp}";

        


        return true;
    }

    private void Start()
    {
        transform.parent.GetComponentInChildren<UI_BattleScene>().SetOFF(); //배틀 UI 가져오기
    }

    public void OnClickTitle()
    {
        Managers.UI.ClosePopupUI(this);
        Managers.Scene.LoadScene(Define.EScene.GameScene);
    }
}
