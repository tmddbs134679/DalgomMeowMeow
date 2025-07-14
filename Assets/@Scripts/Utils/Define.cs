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
    public static readonly int UI_GROUP_SPACING = 100;
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
        Cook,
        MoveTo,
        Play,
        Farm,
        Rest,
        Deliver,
        Collect,
        Build,
        Hello,
        None,

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
        BattleScene,
        TutorialScene
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
        ROAD
    }

    public enum BuildingType
    {
        Cooking,
        Farm,
        Playing,
        Resting,
        Fishing,
        Storage,
        SlotMachine,
        Road,
        Shop,
        UnLockStage,
    }

    public enum EEquipmentType
    {
        None,
        Hat,
        Accessory,
        Bag,
    }

    public enum EDebugType
    {
        None,
        Building,
        AI,
        UI,
        Drag,
    }

    public enum EMaterialType
    {
        None,
        Gold,
        Ticket,
    }

    public enum EEquipmentGrade
    {
        None,
        Common,
        Uncommon,
        Rare,
        Epic,
    }

    public enum EExchangeType
    {
        None,
        Gold,
        Ticket,
        Dia
    }
    
    public enum EQuestType
    {
        None,
        Daily,
        Achievement
    }
    
    public enum EQuestConditionType
    {
        None,
        Collect,
        Kill
    }
    public enum ETargetType
    {
        None,
        Soup,
        Build,
        Farm
    }

    public static readonly float[] COMMON_GACHA_GRADE = new float[]
{
        0,
        0.62f,   // Common 확률
        0.18f,   // Uncommon 확률
        0.15f,   // Rare 확률
        0.05f,  // Epic 확률
};

}
