using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaGacha : MonoBehaviour
{
    [SerializeField]private int[] gachaItems; 

    //[SerializeField]private CharacterStatSo[] statPool; //기본스탯 풀 1,2,3,4,5성 차이두기
    //[SerializeField]private CharacterSkillSo[] skillPool; //기본스킬 풀 1,2,3,4,5성 차이두기

    //private List<newCat> gachaCats = new List<newCat>();

    private float[] weights = new float[] //합이 1
    {
        0.35f, //1성 35%
        0.3f,  //2성 30%
        0.2f,  //3성 20%
        0.1f,  //4성 10%
        0.05f  //5성 5%
    }; 

    public void RandomGacha()       //비중을 더해주면서 랜덤값을 추출해 어느 구간인지 나누는 작업
    {
        float totalWeight = 0f;

        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0f, totalWeight);

        float currentDum = 0f;
        for(int i = 0; i < weights.Length; i++)
        {
            currentDum += weights[i];
            if (randomValue <= currentDum)
            {
                Debug.Log("Gacha Item ID: " + gachaItems[i]);
                return;
            }
        }
    }
    /*
    public newCat RandomGacha2()
    {
        float totalWeight = 0f;
        foreach (var w in weights)
            totalWeight += w;

        float randomValue = Random.Range(0f, totalWeight);

        float currentDum = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            currentDum += weights[i];
            if (randomValue <= currentDum)
            {
                var stat = skillPool[i];   // 별 등급별 배열에서 랜덤 선택 함수 i 는 캐릭터의 등급
                var skill = skillPool[i];   // 별 등급별 배열에서 랜덤 선택 함수
                var catName = "CatStar" + (i + 1);   // 이름 임시 지정

                var newCatInstance = new newCat(catName, stat, skill);

                gachaCats.Add(newCatInstance);  //리스트에 추가
                Debug.Log($"뽑힌 캐릭터: {catName} 등급 {i + 1}성");
                return newCatInstance;
            }
        }
        return null; // 예외 처리
    }
    */
    public void Gacha_Tentimes()
    {
        for (int i = 0; i < 10; i++)
            RandomGacha();
    }
    

    //가챠시 고양이, 스킬 데이터 매칭한 객체 생성? 5성이면 스탯 좋고 스킬도 5성풀에서 랜덤으로 달아줄 수 있게끔 구성
    /*
    public class newCat
    {
        string name;
        CharacterStatSo data;
        characterSkillSo skill;
        public newCat(string name, CharacterStatSo data, characterSkillSo skill)
        {
            this.name = name;
            this.data = data;
            this.skill = skill;
        }
    }
    */
}
