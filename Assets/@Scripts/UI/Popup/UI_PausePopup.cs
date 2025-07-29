using UnityEngine;

public class UI_PausePopup : UI_Popup   //어드레서블에 프리펩 넣기
{
    #region Enum

    enum Buttons
    {
        ResumeButton,   //버튼이름(알아서 찾아옴, 게임오브젝트랑 이름 똑같이)
        RetryButton,
        TitleButton
    }

    enum Images
    {
        Pause    
    }
    #endregion

    [SerializeField]Sprite[] Image;
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {

    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        
        GetButton((int)Buttons.ResumeButton).gameObject.BindEvent(OnClickResumeButton); //버튼 등록
        GetButton((int)Buttons.RetryButton).gameObject.BindEvent(OnClickRetryButton);
        GetButton((int)Buttons.TitleButton).gameObject.BindEvent(OnClickTitleButton);

        //getimage
        GetImage((int)Images.Pause).sprite = Image[Random.Range(0,3)];




        return true;
    }

    public void OnClickResumeButton()
    {
        Managers.UI.ClosePopupUI(this);
        Time.timeScale = 1f;
    }
    public void OnClickRetryButton()
    {
        Managers.UI.ClosePopupUI(this);
        Time.timeScale = 1f;
        Managers.Scene.LoadScene(Define.EScene.BattleScene);

    }
    public void OnClickTitleButton()
    {
        Managers.UI.ClosePopupUI(this);
        Time.timeScale = 1f; 
        Managers.Sound.PlayPopupClose();
        Managers.Scene.LoadScene(Define.EScene.GameScene);
    }

}
