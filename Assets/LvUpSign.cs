using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LvUpSign : MonoBehaviour
{
    BuildingBase buildingBase;
    public TMP_Text LvText;
    void Start()
    {
        buildingBase = gameObject.GetComponent<BuildingBase>();
        buildingBase.OnLvUp += LvUpSignSet;
                LvText.text = buildingBase.CurrentLevel.ToString();
    }


    void LvUpSignSet()
    {
        LvText.text = buildingBase.CurrentLevel.ToString();
}
}
