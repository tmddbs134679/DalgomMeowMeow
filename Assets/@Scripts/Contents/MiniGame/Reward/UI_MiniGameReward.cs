using TMPro;
using UnityEngine;

public class UI_MiniGameReward : UI_Popup
{
    #region Enums
    enum GameObjects
    {
        Score,
        ScoreReward,
        Time,
        TimeReward,
    }
    enum Buttons
    {
        Close,
    }
    #endregion

    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _timeText;
    private TextMeshProUGUI _scoreRewardText;
    private TextMeshProUGUI _timeRewardText;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));

        _scoreText = GetObject((int)GameObjects.Score).GetComponent<TextMeshProUGUI>();
        _timeText = GetObject((int)GameObjects.Time).GetComponent<TextMeshProUGUI>();
        _scoreRewardText = GetObject((int)GameObjects.ScoreReward).GetComponent<TextMeshProUGUI>();
        _timeRewardText = GetObject((int)GameObjects.TimeReward).GetComponent<TextMeshProUGUI>();

        GetButton((int)Buttons.Close).gameObject.BindEvent(PopupClose);

        SetReward();
        return true;
    }



    public void SetReward()
    {
        float score = PlayerPrefs.GetFloat("HighScore", 0);
        float time = PlayerPrefs.GetFloat("FastestTime", 60f);

        _scoreText.text = $"Score\n{score}";
        _timeText.text = $"Time\n{time}s";
        _scoreRewardText.text = $"{(int)(score * 0.5f)}G";
        _timeRewardText.text = $"{(60 - (int)time) * 100}G";
    }


    public void PopupClose()
    {
        Managers.Sound.PlayPopupClose();
        Managers.UI.ClosePopupUI(this);
    }
}
