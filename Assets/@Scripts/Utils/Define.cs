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
    #region UI 
    public static readonly int UI_GROUP_SPACING = 40;
    #endregion

    public static readonly int FOOD_MAX_VALUE = 12;

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

    public enum EAIState
    {
        Idle,
        Cooking,
        MoveTo,
        Playing,
        Farming,
        Resting,
        Delivery,
        Collecting,
        Building,

    }

    public enum EItemType
    {
        None,
        Vegetable,
        Fish
    }

    public enum EItemID
    {
        Cabbage,
        Carrot,
        Onion,
        Potato,
        Tomato,
    }

    public enum EScene
    {
        None,
        TitleScene,
        LobbyScene,
        GameScene,
        FrameworkTestScene,
        Test_Battle
    }

    public enum EBuildingType
    {
        NONE,
        COOK,
        FARM,
        FISHING,
        PLAYGROUND,
        REST,
        SHOP,
        STORAGE,
        LOAD
    }

    public enum BuildingType
    {
        Cooking,
        Fishing,
        Resting,
        Farm,
        Shop,
        STORAGE
    
}
}
