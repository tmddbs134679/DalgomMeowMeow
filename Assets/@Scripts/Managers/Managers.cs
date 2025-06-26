using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }
    //Contents
    GameManager _game = new GameManager();
    FoodManager _food = new FoodManager();
    BattleManager _battle = new BattleManager();
    public static GameManager Game { get { return Instance?._game; } }
    public static FoodManager Food { get { return Instance?._food; } }
    public static BattleManager Battle { get { return Instance?._battle; } }
    //Core
    DataManager _data = new DataManager();
    ResourceManager _resource = new ResourceManager();
    MySceneManager _scene = new MySceneManager();
    UIManager _ui = new UIManager();
    public static DataManager Data { get { return Instance?._data; } }
    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static MySceneManager Scene { get { return Instance?._scene; } }
    public static UIManager UI { get { return Instance?._ui; } }
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
        }
    }
}
