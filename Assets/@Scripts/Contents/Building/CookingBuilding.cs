using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class CookingBuilding : BuildingBase
{
    [SerializeField] private Renderer buildingRenderer;

    public GameObject collectIcon;
    
    // 채소 누적 수
    private int deliveredVegetableCount = 0;

     // 단계별 요리 이름들
    [SerializeField]
    private List<FoodData> upgradeDishes; // [ "수프", "야채수프", "특제 야채수프", "궁극 야채수프" ]


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

        deliveredVegetableCount = 0; // 생산 후 초기화
        
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
        
   
        //(Managers.UI.SceneUI as UI_GameScene).ResetCookItem();
        QuestManager.Instance.OnEvent(QuestConditionType.Collect, TargetType.Soup);
        
        
       // QuestManager.Instance.GiveReward("Soup_10");
        // collectIcon.SetActive(true);
        
    }
    public override void OnClick()
    {

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
}
