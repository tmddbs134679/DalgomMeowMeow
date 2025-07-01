using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUI : MonoBehaviour
{

    public GameObject BuildUi;
    public GameObject BuildTypeUI;
    public GameObject BuildActiontUI;
    public GameObject MoneyUI;
public MoneyPreview moneyPreview;

    void Start()
    {
        BuildingPlacer.Instance.buildUI = this;
    }

    public void OnBuild()
    {
        BuildTypeUI.SetActive(true);
        BuildUi.SetActive(false);
    }

    public void OnSelectBuilding()
    {
        BuildTypeUI.SetActive(false);
        BuildActiontUI.SetActive(true);
        MoneyUI.SetActive(true);
    }

    public void AcceptBuild()
    {

    }
    public void CancelBuild()
    {
        BuildActiontUI.SetActive(false);
        BuildTypeUI.SetActive(false);
        MoneyUI.SetActive(false);
        BuildUi.SetActive(true);
    }
    
    public void SelectBuildingType(int type)
    {
        OnSelectBuilding();
    }
public bool CheckMoneyEnough()
    {
        return moneyPreview.money > 0;
    }

    public void SpendMoney(int amount)
    {
        moneyPreview.money -= amount;
        moneyPreview.UpdateMoneyText();
    }

    private void Update()
    {
        if (BuildingPlacer.Instance.tempDraggleOBJ != null && BuildActiontUI.activeSelf)
        {
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, BuildingPlacer.Instance.tempDraggleOBJ.transform.position);
            BuildActiontUI.transform.position = screenPos;
        }
    }
}
