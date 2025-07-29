using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FishingResult
{
    Miss,       // 놓침
    Normal,     // 평범
    Jackpot     // 월척
}
public class FishingBuilding : BuildingBase
{
    [SerializeField] private Renderer buildingRenderer;

    public GameObject collectIcon;
    
    [Header("낚시 확률 (0~1)")]
    [Range(0f, 1f)] public float JackpotProbability = 0.1f;
    [Range(0f, 1f)] public float NormalProbability = 0.7f;
    private void Awake()
    {
        _textAnim = Managers.UI.ShowPopupUI<UI_TextAnimation>();

    }

    private void OnDestroy()
    {
        if(_textAnim != null)
            _textAnim.gameObject.SetActive(false);
    }
    protected override void Start()
    {
        base.Start();

        _textAnim.gameObject.SetActive(false);
        _textAnim.SetInfo(Define.EBuildingType.Fishing, transform.position);
    }
    public override void ConnectToAnimal(AICharacter animal)
    {
        base.ConnectToAnimal(animal);

        _textAnim.gameObject.SetActive(true);
    }

    public override void DisconnectAnimal()
    {
        base.DisconnectAnimal();

        _textAnim.gameObject.SetActive(false);
    }
    public override bool Init()
    {
        base.Init();
        //collectIcon.SetActive(false);
        return true;
    }
    public override void Produce()
    {

        FishingResult result = GetFishingResult();

        switch (result)
        {
            case FishingResult.Jackpot:
                Debug.Log("월척 희귀한 물고기 획득!");
                StoredCount++; // 월척도 카운트에 포함
                break;

            case FishingResult.Normal:
                Debug.Log(" 평범한 물고기 획득");
                StoredCount++;
                break;

            case FishingResult.Miss:
                Debug.Log(" 놓침");
                break;
        }


        // if (StoredCount > 0) collectIcon.SetActive(true);

    }
    
    private FishingResult GetFishingResult()
    {
        float rand = Random.value; // 0.0 ~ 1.0

        if (rand < JackpotProbability)
            return FishingResult.Jackpot;
        else if (rand < JackpotProbability + NormalProbability)
            return FishingResult.Normal;
        else
            return FishingResult.Miss;
    }
    public void Collect()
    {
        if (StoredCount <= 0) return;

        Debug.Log($" {StoredCount}마리 물고기를 획득");

        StoredCount = 0;
        CurrentState = BuildingState.Producing;
        // collectIcon.SetActive(false);

    }
    public override void OnClick()
    {
        if (StoredCount > 0)
        {
            Collect();
        }
    }

}
