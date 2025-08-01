using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }
    //Contents
    GameManager _game = new GameManager();
    EquipmentManager _equipment = new EquipmentManager();
    FoodManager _food = new FoodManager();
    TimeManager _time;
    RoomManager _room = new RoomManager();


    public static GameManager Game { get { return Instance?._game; } }
    public static EquipmentManager Equipment { get { return Instance?._equipment; } }
    public static FoodManager Food { get { return Instance?._food; } }
    public static TimeManager Time { get { return Instance?._time; } }
    public static RoomManager Room { get { return Instance?._room; } }

    //Core
    DataManager _data = new DataManager();
    ResourceManager _resource = new ResourceManager();
    MySceneManager _scene = new MySceneManager();
    UIManager _ui = new UIManager();
    ObjectManager _object = new ObjectManager();
    PoolManager _pool = new PoolManager();
    AIManager _ai = new AIManager();
    SoundManager _sound = new SoundManager();

    //Edit
    DebugManager _debug = new DebugManager();
    //AdsManager _ads = new AdsManager();

    public static DataManager Data { get { return Instance?._data; } }
    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static MySceneManager Scene { get { return Instance?._scene; } }
    public static UIManager UI { get { return Instance?._ui; } }
    public static AIManager AI { get { return Instance?._ai; } }
    //public static AdsManager Ads { get { return Instance?._ads; } }
    public static DebugManager Debug { get { return Instance?._debug; } }
    public static ObjectManager Object { get { return Instance?._object; } }
    public static PoolManager Pool { get { return Instance?._pool; } }
    public static SoundManager Sound { get { return Instance?._sound; } }


    [SerializeField] private DebugSettings debugSettingsSO;

    public void Awake()
    {
        _debug.debugSettings = debugSettingsSO;
    }
    public static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
            s_instance._time = go.AddComponent<TimeManager>();

            s_instance._sound.Init();
            //ds.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Object.Clear();
        Pool.Clear();
    }
}
