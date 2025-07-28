using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill : UI_Popup   //어드레서블에 프리펩 넣기
{
    public RectTransform LeftTop;
    public RectTransform RightBottom;
    public RectTransform CharacterImage;
    public Image CharImg;

    private Vector2 leftInPos = new Vector2(-490f, 280f);
    private Vector2 rightInPos = new Vector2(850f, -410f);

    private Vector2 leftOutPos = new Vector2(-1490f, 1280f);
    private Vector2 rightOutPos = new Vector2(1850f, -1410f);

    private Vector2 characterOutPos = new Vector2(-1400f, 80f); // 왼쪽에서 시작
    private Vector2 characterInPos = new Vector2(-835f, 340f); // 중앙으로 이동
    private Vector2 characterEndPos = new Vector2(140f, 720f); // 나가기

    #region Enum
    enum Images
    {
        Character
    }
    #endregion
    private void Awake()
    {
        LeftTop = transform.Find("LeftTop").GetComponent<RectTransform>();
        RightBottom = transform.Find("RightBottom").GetComponent<RectTransform>();
        CharacterImage = transform.Find("Character").GetComponent<RectTransform>();
        CharImg = CharacterImage.GetComponent<Image>();

        Init();
    }

    private void OnEnable()
    {
        // 시작 위치
        LeftTop.anchoredPosition = leftOutPos;
        RightBottom.anchoredPosition = rightOutPos;
        CharacterImage.anchoredPosition = characterOutPos; // 왼쪽에서 시작

        Sequence cutSceneSequence = DOTween.Sequence().SetUpdate(true);

        // 들어오기
        cutSceneSequence
            .Append(LeftTop.DOAnchorPos(leftInPos, 0.3f).SetEase(Ease.OutExpo))
            .Join(RightBottom.DOAnchorPos(rightInPos, 0.3f).SetEase(Ease.OutExpo))
            .Join(CharacterImage.DOAnchorPos(characterInPos,0.4f).SetEase(Ease.OutBack))

            // 멈춤 (1초 기다림)
            .AppendInterval(0.3f)

            // 나가기
            .Append(CharacterImage.DOAnchorPos(characterEndPos, 0.3f).SetEase(Ease.InBack))
            .Join(LeftTop.DOAnchorPos(leftOutPos, 0.4f).SetEase(Ease.InExpo))
            .Join(RightBottom.DOAnchorPos(rightOutPos, 0.4f).SetEase(Ease.InExpo));
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        return true;
    }

    public void SetImage(Sprite image)
    {
        // 스킬 캐릭터 이미지 설정
        CharImg.sprite = image;
        // GetImage((int)Images.SkillCharacter).sprite = Managers.Resource.Load<Sprite>( cat sprite );
    }
}
