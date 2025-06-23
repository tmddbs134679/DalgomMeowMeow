using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_ : MonoBehaviour
{
    public int ItemID; 
    public int ItmeCount; //아이템 개수
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetItem()
    {
        //콜라이더를 쓰던 트리거로 하던 건물에 가서 레이를 쏘던
        //other.getcomponent<ItemID> or 건물에서 캐릭터한테 값을 주는 형식으로
        //ItemID = other.ItemID;
        //ItmeCount = other.ItmeCount;
        //건물의 아이템 카운트 제거
        //창고로 가는 상태로 변경
    }

    public void StoreItem()
    {
        //저장 상태에서 setdestination으로 창고로 이동
        //창고에 도착하면 아이템을 저장
        //창고에는 기존 아이템 데이터가 있고, 플레이어가 가진 ItemID와 일치하는 ID를 가진 데이터에 Count만큼 값을 더해줌
        //아이템을 저장한 후에는 플레이어의 ItemID와 ItmeCount를 초기화
    }
}
