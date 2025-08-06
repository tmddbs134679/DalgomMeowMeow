using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

[System.Serializable]
public class SlotResult
{
    public string Symbol;
    public int RewardGold;
    public int Weight;
    public string IconAddress; // Addressable 주소
    public Sprite Icon; // 실제 로딩된 스프라이트
}


public class SlotMachineBuilding : BuildingBase
{
    private string[] _currentResult = new string[3];
    public string[] CurrentResult => _currentResult;
    [SerializeField] private Image[] slotImages; // 슬롯 3칸 이미지 연결

    private int _slotCount = 3;

    private int _finishedCount = 0;

    public RectTransform[] slotContents;
    public float moveDistance = 200f;
    public float duration = 0.2f;
    public int spinCount = 10;

    private List<SlotResult> _results;

    [SerializeField] private Sprite bearIcon;
    [SerializeField] private Sprite sharkIcon;
    [SerializeField] private Sprite catIcon;
    
    private Vector2[] _originalPositions;
    public System.Action OnAllSlotFinished;

    private void Awake()
    {
        _results = new()
        {
            new SlotResult { Symbol = "UI_SlotBear", RewardGold = 100, Weight = 50, Icon = bearIcon },
            new SlotResult { Symbol = "UI_SlotShark", RewardGold = -200, Weight = 30, Icon = sharkIcon },
            new SlotResult { Symbol = "UI_SlotCat", RewardGold = 1000, Weight = 5, Icon = catIcon }
        };
    }

    public void Init()
    {
        slotContents = new RectTransform[3];
        slotContents[0] = GameObject.Find("Slot1").GetComponent<RectTransform>();
        slotContents[1] = GameObject.Find("Slot2").GetComponent<RectTransform>();
        slotContents[2] = GameObject.Find("Slot3").GetComponent<RectTransform>();
    }





    public SlotResult GetRandomResult()
    {
        int totalWeight = 0;
        foreach (var result in _results)
            totalWeight += result.Weight;

        int rand = Random.Range(0, totalWeight);
        int current = 0;

        foreach (var result in _results)
        {
            current += result.Weight;
            if (rand < current)
                return result;
        }

        return _results[0]; // fallback
    }


    

    public override void OnClick()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;
        UI_BuildContent popup = Managers.UI.ShowPopupUI<UI_BuildContent>();
        popup.SetTarget(gameObject);
        popup.SettingOnOff(Define.EBuildPopUpType.SlotButton);
    }

    private IEnumerator SpinSingleSlot(int index)
    {
        RectTransform content = slotContents[index];

        if (_originalPositions == null || _originalPositions.Length != slotContents.Length)
        {
            _originalPositions = new Vector2[slotContents.Length];
            for (int i = 0; i < slotContents.Length; i++)
                _originalPositions[i] = slotContents[i].anchoredPosition;
        }

        Sequence seq = DOTween.Sequence();
        Sprite finalSprite = null;

        for (int i = 0; i < spinCount; i++)
        {
            
            int currentIndex = i;
            seq.Append(content.DOAnchorPosY(_originalPositions[index].y - moveDistance, duration).SetEase(Ease.Linear))
                .AppendCallback(() =>
                {
                    RectTransform last = content.GetChild(content.childCount - 1).GetComponent<RectTransform>();
                    Image image = last.GetComponent<Image>();

                    SlotResult randomResult = GetRandomResult();
                    image.sprite = randomResult.Icon;

                    if (randomResult.Icon == null)
                        Debug.LogError($"[Slot Error] {randomResult.Symbol}의 Icon이 null입니다!");

                    // 마지막 반복에서 보여줄 아이콘 저장
                    if (currentIndex == spinCount - 1)
                        finalSprite = randomResult.Icon;

                    last.SetAsFirstSibling();
                    content.anchoredPosition = _originalPositions[index];
                });
        }

        seq.OnComplete(() =>
        {
            // 🔍 가장 anchoredPosition.y가 0에 가까운 이미지가 실제 가운데에 표시되는 슬롯
            Transform center = null;
            float closest = float.MaxValue;

            for (int i = 0; i < content.childCount; i++)
            {
                var child = content.GetChild(i);
                float dist = Mathf.Abs(child.localPosition.y); // 가운데는 0에 가까운 값
                if (dist < closest)
                {
                    closest = dist;
                    center = child;
                }
            }

            Sprite centerSprite = center.GetComponent<Image>().sprite;

            SlotResult matched = _results.Find(r => r.Icon.name == centerSprite.name);
            _currentResult[index] = matched != null ? matched.Symbol : "";
            Debug.Log($"슬롯 {index + 1} 멈춤! 결과: {_currentResult[index]}");

            _finishedCount++;
            if (_finishedCount >= _slotCount)
            {
                _finishedCount = 0;
                OnAllSlotFinished?.Invoke();
            }
        });
        
        yield return seq.WaitForCompletion();
    }

    public void SpinAllSlots()
    {
        for (int i = 0; i < slotContents.Length; i++)
        {
            StartCoroutine(DelayedSpin(i));
        }
    }
    private IEnumerator DelayedSpin(int index)
    {
        float delay = 0.1f * index; // 인덱스에 따라 약간의 지연
        yield return new WaitForSeconds(delay);
        StartCoroutine(SpinSingleSlot(index));
    }

    public SlotResult GetMatchResult(string symbol)
    {
        return _results.Find(r => r.Symbol == symbol);
    }

    public override void Produce()
    {
    }
}