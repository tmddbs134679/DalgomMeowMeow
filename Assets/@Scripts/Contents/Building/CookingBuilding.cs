using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class CookingBuilding : BuildingBase
{
    [SerializeField] private Renderer buildingRenderer;
    public int CurrentLevel { get; private set; } = 1; 
    
    private BuildingLevelData LevelData => 
        Managers.Data.BuildingLevelDic[(BuildingData.Id.ToString(), CurrentLevel)];

    public GameObject collectIcon;
    
    // 채소 누적 수
    private int deliveredVegetableCount = 0;

     // 단계별 요리 이름들
    [SerializeField]
    private List<FoodData> upgradeDishes; 

    private void Start()
    {
        Debug.Log($"현재 레벨: {CurrentLevel}");
        var key = (BuildingData.Id.ToString(), CurrentLevel + 1);
        if (Managers.Data.BuildingLevelDic.TryGetValue(key, out var levelData))
        {
            Debug.Log($"[UpgradeTest] 다음 레벨 정보 있음: {levelData.ProducedFoodId}, 비용: {levelData.UpgradeCost}");
        }
        else
        {
            Debug.LogWarning($"[UpgradeTest] 다음 레벨 정보 없음: {key}");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (Upgrade())
                Debug.Log("업그레이드 성공!");
            else
                Debug.Log("업그레이드 실패!");
        }
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
        
        int dishLevel = Mathf.Clamp(deliveredVegetableCount, 0, upgradeDishes.Count - 1);
        //FoodData finalDish = upgradeDishes[dishLevel];

        //Debug.Log($"{finalDish.Name} 요리 완성!");

        
        
        StoredCount++; //  생산 누적
        
        Debug.Log($"요리 완성! 누적 수량: {StoredCount}");

        if (deliveredVegetableCount == 0)
        {
            Managers.Food.MakeFood();
        }
        else if(deliveredVegetableCount == 1)
        {
            Managers.Food.MakeFood();
        }
        else if (deliveredVegetableCount == 2)
        {
            Managers.Food.MakeFood();
        }
        else if (deliveredVegetableCount >= 3)
        {
            Managers.Food.MakeFood();
        }
        
        deliveredVegetableCount = 0; // 생산 후 초기화
        //(Managers.UI.SceneUI as UI_GameScene).ResetCookItem();
        QuestManager.Instance.OnEvent(QuestConditionType.Collect, TargetType.Soup);
        
        
       // QuestManager.Instance.GiveReward("Soup_10");
        // collectIcon.SetActive(true);
        
    }
    public override void OnClick()
    {
        UI_BuildingPopup popup = Managers.UI.ShowPopupUI<UI_BuildingPopup>();
        popup.SetTarget(this); // 클릭한 CookingBuilding 인스턴스
    }
    public void Collect()
    {
        if (StoredCount <= 0) return;

        Debug.Log($" {StoredCount}개 요리를 수확했습니다!");

        
        StoredCount = 0;
        CurrentState = BuildingState.Producing;
        // collectIcon.SetActive(false);

    }
    

    
    public void DeliverIngredient(AICharacter animal)
    {
        if (CurrentState != BuildingState.Producing) return;

            deliveredVegetableCount++;
            Debug.Log($"[야채 도착] 누적 채소 수: {deliveredVegetableCount}");

    }
    
    private void OnTriggerEnter(Collider other)
    {
        AICharacter animal = other.GetComponent<AICharacter>();
        if (animal == null) return;

        if (animal.CurrentState == Define.EAIState.Delivery)
        {
            deliveredVegetableCount++;
            Debug.Log($"[야채 도착] 누적 채소 수: {deliveredVegetableCount}");

        }
    }
    
    public bool CanUpgrade()
    {
        return Managers.Data.BuildingLevelDic.ContainsKey((BuildingData.Id.ToString(), CurrentLevel + 1));
    }

    public bool Upgrade()
    {
        if (!CanUpgrade()) return false;

        var next = Managers.Data.BuildingLevelDic[(BuildingData.Id.ToString(), CurrentLevel + 1)];
        if (Managers.Game.Gold <= next.UpgradeCost)
            return false;

        CurrentLevel++;
        ApplyLevel();
        return true;
    }
    private void ApplyLevel()
    {
        Debug.Log($"[CookingBuilding] 업그레이드 완료 → Lv.{CurrentLevel}");
        //Debug.Log($"[CookingBuilding] 업그레이드 완료 → Lv.{CurrentLevel}, 생산 요리: {LevelData.ProducedFood.Name}");
        // 외형 변경, 사운드 등도 여기에
    }
}
