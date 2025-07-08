using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class UI_Skill : UI_Popup   //어드레서블에 프리펩 넣기
{
    private void Awake()
    {
        Init();
    }
    

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        return true;
    }
}
