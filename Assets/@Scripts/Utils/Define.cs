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

    public static readonly int GOLD_TO_DIA_PRICE = 1000;
    public static readonly int DIA_TO_GOLD_PRICE = 100;
    public static readonly int DIA_TO_TICKET_PRICE = 100;

    #endregion

    public static readonly int FOOD_MAX_VALUE = 12;
    public static readonly int GOLD_PER_MINUTE = 100;

    public static readonly int TRAVEL_TIME = 5;

    public static readonly string COOK_TEXT = "휘적";
    public static readonly string FARM_TEXT = "툭";
    public static readonly string PLAYING_TEXT = "띠용";
    public static readonly string FISHING_TEXT = "첨벙";
    public static readonly string REST_TEXT = "z..";

    public static readonly string[] CAT_SOUNDS = { "Cat1", "Cat2" };
    public enum EUIEvent
    {
        None,
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
        CabbageFarm,
        OnionFarm,
        PotatoFarm,
        PumpkinFarm,
        CarrotFarm,
        Rest,
        Deliver,
        Collect,
        Build,
        Hello,
        Fishing,
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
        TutorialScene,
        CharacterStoreScene,
    }

    //BuildSo의 넘버링과 순서가 동일해야함,SO 닉네임을 정할 때 앞에 번호를 붙여주면 해결
    public enum EBuildingType
    {
        None,
        Cooking,
        CabbageFarm,
        Playing,
        Resting,
        Fishing,
        Storage,
        SlotMachine,
        Road,
        Shop,
        OnionFarm,
        PotatoFarm,
        PumpkinFarm,
        CarrotFarm,
        Pot,
        Travel,
        RemoveRoad,
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
        AD
    }

    public enum EMaterialType
    {
        None,
        Gold,
        Dia,
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
        Kill,
        Build
    }
    public enum ETargetType
    {
        None,
        Soup,
        Build,
        Farm,
        Animal,
        Forest,
        Fishing,
        Onion,
        Cabbage,
        Carrot,
        Potato,
        Pumpkin,
        OnionFarm,
        CabbageFarm,
        CarrotFarm,
        PotatoFarm,
        PumpkinFarm,
    }

    public static readonly float[] COMMON_GACHA_GRADE = new float[]
    {
            0,
            0.62f,   // Common 확률
            0.18f,   // Uncommon 확률
            0.15f,   // Rare 확률
            0.05f,  // Epic 확률
    };


    public enum EBuildPopUpType
    {
        None,
        PopUpButton,
        SlotButton,
    }


    public enum ESound
    {
        None,
        Bgm,
        SubBgm,
        Effect,
        Max,

    }

    public enum ECropType
    {
        None,
        Carrot,
        Pumpkin,
        Potato,
        Onion,
        Cabbage,
    }

    public enum EGachaType
    {
        None,
        Creature,
        Equipment
    }

}
