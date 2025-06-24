using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TitleScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        Slider,
    }

    enum Buttons
    {
        StartButton
    }

    enum Texts
    {
        StartText
    }
    #endregion

    bool isPreload = false;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        
        GetObject((int)GameObjects.Slider).GetComponent<Slider>().value = 0;

        GetButton((int)Buttons.StartButton).gameObject.BindEvent(() =>
        {
            if (isPreload)
                Managers.Scene.LoadScene(Define.EScene.FrameworkTestScene, transform);
        });

        GetButton((int)Buttons.StartButton).gameObject.SetActive(false);


        return true;
    }
    private void Awake()
    {
        Init();
    }


    private void Start()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            GetObject((int)GameObjects.Slider).GetComponent<Slider>().value = (float)count / totalCount;
            if (count == totalCount)
            {
                isPreload = true;
                GetButton((int)Buttons.StartButton).gameObject.SetActive(true);
                Managers.Data.Init();
                Managers.Game.Init();
            }
        });
    }


}
