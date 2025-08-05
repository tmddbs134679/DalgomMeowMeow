using UnityEngine;
using UnityEngine.UI;

public class UI_Whack_a_mole : UI_Popup
{
    #region Enums
    enum GameObjects
    {
        Mole_Manager,
        ScoreUI,
        TextUI,
    }
    enum Buttons
    {
        Start,
        CloseBtn,
    }
    #endregion
    private MoleManager moleManager;

    public Button Startbtn;
    public Button Closebtn;
    
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));

        Startbtn = GetButton((int)Buttons.Start);
        Startbtn.gameObject.BindEvent(StartGame);


        Closebtn = GetButton((int)Buttons.CloseBtn);
        Closebtn.gameObject.BindEvent(PopupClose);

        Startbtn.gameObject.SetActive(true);
        GetObject((int)GameObjects.ScoreUI).SetActive(false);
        GetObject((int)GameObjects.TextUI).SetActive(true);
        moleManager = GetObject((int)GameObjects.Mole_Manager).GetComponent<MoleManager>();

        return true;
    }


    private void StartGame()
    {
        Managers.Sound.PlayButtonClick();
        moleManager.StartGame();
        Startbtn.gameObject.SetActive(false);
    }

    
    public void PopupClose()
    {
        Managers.Game.Gold += moleManager.HighScore * 0.5f;
        Managers.Sound.PlayPopupClose();
        Managers.UI.ClosePopupUI(this);
    }
}
