using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Define 스크립트 사용 규칙 예시
 *  1. 전역적 static 변수 (변수명은 대문자, 언더바 활용)
 *  ex) public static float POTION_COLLECT_DISTANCE = 2.6F;
 *  ex) public const string CHARACTER_ID = 51515;
 *  ex) public const string MONSTER_ID = "M05616";
 *  
 *  3. Enum의 첫 글자는 대문자 E를 사용, 0번은 None으로 일단 고정
 *    public enum EState
 *    {
 *          None,
 *    }
 */


public class Define 
{
    public enum EUIEvent
    {
        Click,
        Preseed,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag,
    }


    public enum EObjectType
    {
        None,
        Character,
        Building,
        Enemy,

    }
}
