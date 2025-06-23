using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Define ��ũ��Ʈ ��� ��Ģ ����
 *  1. ������ static ���� (�������� �빮��, ����� Ȱ��)
 *  ex) public static float POTION_COLLECT_DISTANCE = 2.6F;
 *  ex) public const string CHARACTER_ID = 51515;
 *  ex) public const string MONSTER_ID = "M05616";
 *  
 *  3. Enum�� ù ���ڴ� �빮�� E�� ���, 0���� None���� �ϴ� ����
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


    public enum CharacterState
    {
        Idle,
        Cooking,
        Playing,
        Resting,
        Collect,
        Farming,
        Building,
        
    }
}
