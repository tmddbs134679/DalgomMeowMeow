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

    private void Awake()
    {
        _results = new()
        {
            new SlotResult { Symbol = "BEAR", RewardGold = 100, Weight = 50, Icon = bearIcon },
            new SlotResult { Symbol = "SHARK", RewardGold = -200, Weight = 30, Icon = sharkIcon },
            new SlotResult { Symbol = "CAT", RewardGold = 1000, Weight = 5, Icon = catIcon }
        };
    }

    public void Init()
    {
        slotContents = new RectTransform[3];
        slotContents[0] = GameObject.Find("Slot1").GetComponent<RectTransform>();
        slotContents[1] = GameObject.Find("Slot2").GetComponent<RectTransform>();
        slotContents[2] = GameObject.Find("Slot3").GetComponent<RectTransform>();
    }

    public IEnumerator LoadSlotIcons(List<SlotResult> results)
    {
        int loaded = 0;
        int total = results.Count;

        foreach (var result in results)
        {
            if (!string.IsNullOrEmpty(result.IconAddress))
            {
                bool isDone = false;

                Managers.Resource.LoadAsync<Sprite>(result.IconAddress, (sprite) =>
                {
                    result.Icon = sprite;
                    loaded++;
                    isDone = true;
                });

                yield return new WaitUntil(() => isDone);
            }
        }
    }

    public IEnumerator StartAllSlots()
    {
        _finishedCount = 0;
        _currentResult = new string[_slotCount];

        for (int i = 0; i < _slotCount; i++)
        {
            StartCoroutine(RollSingleSlot(i));
        }

        while (_finishedCount < _slotCount)
            yield return null;

        CheckReward();
    }


    private IEnumerator RollSingleSlot(int index)
    {
        float randomSpinTime = Random.Range(0.5f, 1.5f);
        float elapsed = 0f;

        while (elapsed < randomSpinTime)
        {
            SlotResult temp = _results[Random.Range(0, _results.Count)];
            _currentResult[index] = temp.Symbol;
            slotImages[index].sprite = temp.Icon; // 이미지 갱신
            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        SlotResult final = _results[Random.Range(0, _results.Count)];
        _currentResult[index] = final.Symbol;
        slotImages[index].sprite = final.Icon; // 최종 고정

        Managers.Debug.Log($"슬롯 {index + 1} 멈춤 → {final.Symbol}", Define.EDebugType.Building);
        _finishedCount++;
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

    public void RollSlot()
    {
        for (int i = 0; i < 3; i++)
        {
            _currentResult[i] = GetRandomResult().Symbol;
        }


        CheckReward();
    }

    private void CheckReward()
    {
        string a = _currentResult[0];
        string b = _currentResult[1];
        string c = _currentResult[2];

        if (a == b && b == c)
        {
            SlotResult match = _results.Find(r => r.Symbol == a);
            if (match != null)
                Managers.Debug.Log($"🎉 당첨! {a} x3 → 보상: {match.RewardGold}골드", Define.EDebugType.Building);
        }
        else
        {
            Managers.Debug.Log("😢 꽝!", Define.EDebugType.Building);
        }
        // if (_currentResult[0] == "고양이" && _currentResult[1] == "고양이" && _currentResult[2] == "고양이")
        // {
        //     SlotResult match = _results.Find(r => r.Symbol == "고양이");
        //
        //     if (match != null)
        //     {
        //         Debug.Log($"🎉 대박! 고양이 x3 → 보상: {match.RewardGold}골드");
        //         // Managers.Game.Gold += match.RewardGold;
        //     }
        // }
        // else
        // {
        //     Debug.Log("꽝! 다시 도전하세요.");
        // }
    }

    public (string[], int) RollSlotAndReturn()
    {
        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, _results.Count);
            _currentResult[i] = _results[index].Symbol;
        }

        int reward = 0;
        if (_currentResult[0] == "CAT" && _currentResult[1] == "CAT" && _currentResult[2] == "CAT")
        {
            reward = _results.Find(r => r.Symbol == "CAT")?.RewardGold ?? 0;
            Managers.Game.Gold += reward;
        }
        else if (_currentResult[0] == "SHARK" && _currentResult[1] == "SHARK" && _currentResult[2] == "SHARK")
        {
            reward = _results.Find(r => r.Symbol == "SHARK")?.RewardGold ?? 0;
            Managers.Game.Gold += reward;
        }

        return (_currentResult, reward);
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
            seq.Append(content.DOAnchorPosY(_originalPositions[index].y - moveDistance, duration).SetEase(Ease.Linear))
                .AppendCallback(() =>
                {
                    RectTransform last = content.GetChild(content.childCount - 1).GetComponent<RectTransform>();
                    Image image = last.GetComponent<Image>();

                    SlotResult randomResult = GetRandomResult();
                    image.sprite = randomResult.Icon;

                    // 마지막 반복에서 보여줄 아이콘 저장
                    if (i == spinCount - 1)
                        finalSprite = randomResult.Icon;

                    last.SetAsFirstSibling();
                    content.anchoredPosition = _originalPositions[index];
                });
        }

        seq.OnComplete(() =>
        {
            // 보여진 스프라이트를 기준으로 Symbol 찾아 저장
            SlotResult matched = _results.Find(r => r.Icon == finalSprite);
            _currentResult[index] = matched != null ? matched.Symbol : "";
            Debug.Log($"슬롯 {index + 1} 멈춤! 결과: {_currentResult[index]}");
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