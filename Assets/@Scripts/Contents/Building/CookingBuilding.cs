using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

public class IngredientSet
{
    public class IngredientInfo
    {
        public Define.ECropType Type;    // 재료 종류
        public int FieldLevel;     // 채소를 가져온 밭 건물 레벨
    }

    public List<IngredientInfo> Ingredients = new List<IngredientInfo>();
    public int TotalCount => Ingredients.Count;

    // 밭 건물 평균 레벨 → 가격 보정 계수 계산
    public float GetAvgFieldLevelMultiplier()
    {
        if (Ingredients.Count == 0) return 1.0f;

        float avgFieldLevel = (float)Ingredients.Average(i => i.FieldLevel);
        return 1.0f + ((avgFieldLevel - 1) * 0.1f); // Lv1=1.0, Lv2=1.1, Lv3=1.2
    }

    // 재료 추가
    public void AddIngredient(Define.ECropType type, int fieldLevel)
    {
        Ingredients.Add(new IngredientInfo { Type = type, FieldLevel = fieldLevel });
    }

    public void Reset()
    {
        Ingredients.Clear();
    }
}

public class CookingBuilding : BuildingBase
{
    [SerializeField] private Renderer buildingRenderer;

    private BuildingLevelData LevelData =>
        Managers.Data.BuildingLevelDic[(BuildingData.Id.ToString(), CurrentLevel)];

    public GameObject collectIcon;
    private IngredientSet _currentIngredients = new IngredientSet();

    // 채소 누적 수
    private int deliveredVegetableCount = 0;
    private Define.ECropType _cropType;

    // 단계별 요리 이름들
    [SerializeField] private List<FoodData> upgradeDishes;


    private void Awake()
    {
        _textAnim = Managers.UI.ShowPopupUI<UI_TextAnimation>();

    }

    private void OnDestroy()
    {
        _textAnim.gameObject.SetActive(false);
    }
    protected override void Start()
    {
        base.Start();
    
        _textAnim.gameObject.SetActive(false);
        _textAnim.SetInfo(Define.EBuildingType.Cooking, transform.position);

        Managers.Debug.Log($"현재 레벨: {CurrentLevel}", Define.EDebugType.Building);
        var key = (BuildingData.Id.ToString(), CurrentLevel + 1);
        if (Managers.Data.BuildingLevelDic.TryGetValue(key, out var levelData))
        {
            Managers.Debug.Log($"[UpgradeTest] 다음 레벨 정보 있음: {levelData.ProducedFoodId}, 비용: {levelData.UpgradeCost}",
                Define.EDebugType.Building);
        }
        else
        {
            Debug.LogWarning($"[UpgradeTest] 다음 레벨 정보 없음: {key}");
        }
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
        // collectIcon.SetActive(false);
        return true;
    }

    public void ConnectToDelevery(AICharacter animal)
    {
        if (animal == null) return;

        DisconnectDelevery();
        assignedAnimal = animal;
        assignedAnimal.AnimalDelivered += DeliverIngredient;
    }

    public void DisconnectDelevery()
    {
        if (assignedAnimal == null) return;

        assignedAnimal.AnimalDelivered -= DeliverIngredient;

        assignedAnimal = null;
    }

    public override void Produce()
    {
        //int dishLevel = Mathf.Clamp(deliveredVegetableCount, 0, upgradeDishes.Count - 1);
        //FoodData finalDish = upgradeDishes[dishLevel];

        //Debug.Log($"{finalDish.Name} 요리 완성!");


        StoredCount++; //  생산 누적

        Managers.Debug.Log($"요리 완성! 누적 수량: {StoredCount}", Define.EDebugType.Building);

        Managers.Food.MakeFood(_currentIngredients, CurrentLevel);


        // deliveredVegetableCount = 0; // 생산 후 초기화
        //(Managers.UI.SceneUI as UI_GameScene).ResetCookItem();

        _currentIngredients.Reset();

        QuestManager.Instance.UpdateQuestProgress(Define.EQuestConditionType.Collect, Define.ETargetType.Soup);


        // QuestManager.Instance.GiveReward("Soup_10");
        // collectIcon.SetActive(true);
    }

    public override void OnClick()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;
        UI_BuildContent popup = Managers.UI.ShowPopupUI<UI_BuildContent>();
        popup.SetTarget(gameObject);
        popup.SettingOnOff(Define.EBuildPopUpType.PopUpButton);
    }

    public void Collect()
    {
        if (StoredCount <= 0) return;


        StoredCount = 0;
        CurrentState = BuildingState.Producing;
        // collectIcon.SetActive(false);
    }


    public void DeliverIngredient(AICharacter animal)
    {
        if (CurrentState != BuildingState.Producing) return;


        Managers.Debug.Log($"[야채 도착] 누적 채소 수: {deliveredVegetableCount}", Define.EDebugType.Building);
    }

    private void OnTriggerEnter(Collider other)
    {
        AICharacter animal = other.GetComponent<AICharacter>();
        if (animal == null) return;

        if (animal.CurrentState == Define.EAIState.Deliver)
        {
 
            _currentIngredients.AddIngredient(animal._ecropType, CurrentLevel);

            Managers.Debug.Log($"[야채 도착] 누적 채소 수: {deliveredVegetableCount}", Define.EDebugType.Building);
        }
    }



    protected override void ApplyLevel()
    {
        Managers.Debug.Log($"[CookingBuilding] 업그레이드 완료 → Lv.{CurrentLevel}", Define.EDebugType.Building);
        //Debug.Log($"[CookingBuilding] 업그레이드 완료 → Lv.{CurrentLevel}, 생산 요리: {LevelData.ProducedFood.Name}");
        // 외형 변경, 사운드 등도 여기에
    }
}